using BGM_Express.Core.Command;
using BGM_Express.Core.Dto;
using BGM_Express.Core.Repository;
using BGM_Express.Core.Utils;
using Rebex.Net;

namespace BGM_Express.Core.Service
{
    public class XMLService : IXMLService
    {
        private readonly IBaseRepository _baseRepository;
        private static readonly string FilePath = "C:\\Users\\KarloMatan\\Desktop\\Dev\\BGM_Orders";

        public XMLService(IBaseRepository baseRepository) 
        {
            _baseRepository = baseRepository;
            Rebex.Licensing.Key = "==ASi5DAiPy8DS+z/mtkojJWtEb0HebYWTFCNCiA0Tja1I==";
        }

        public bool UpsertOrders() 
        {
            var rebexConnection = RebexUtils.CreateRebexConnection();

            var availableFiles = rebexConnection.GetList();

            if (availableFiles.Count == 0) {
                return false;
            }

            foreach(SftpItem file in availableFiles)
            {
                var stream = new MemoryStream();
                UpsertOrderCommand deserializedOrder;

                rebexConnection.GetFile(file.Name, stream);
                stream.Seek(0, SeekOrigin.Begin);
                deserializedOrder = XMLUtils.DeserializeXML<UpsertOrderCommand>(stream);

                if(!deserializedOrder.IsProcessed)
                {
                    var orderId = _baseRepository.UpsertOrder(deserializedOrder);
                    deserializedOrder.OrderId = orderId;

                    //Ideally (this came to me when it was too late), we would move the processed files to a separate directory on Rebex
                    //So that on fetching the available orders, we don't have to work with the already processed ones at all
                    MarkOrderAsProcessed(deserializedOrder, file.Name, rebexConnection);
                }
            }
            
            rebexConnection.Disconnect();

            return true;
        }

        public void DispatchOrder(int orderNumber)
        {
            var rebexConnection = RebexUtils.CreateRebexConnection();

            var buyer = _baseRepository.GetAvailableBuyers().Select(b => new BuyerDto
            {
                BuyerId = b.BuyerId,
                FirstName = b.FirstName,
                LastName = b.LastName,
                CompanyName = b.CompanyName
            }).FirstOrDefault();

            var car = _baseRepository.GetAvailableCars().Select(c => new CarDto 
            { 
                CarId = c.CarId,
                Make = c.Make,
                Model = c.Model,
                Color = c.Color,
                Year = c.Year
            }).FirstOrDefault();

            var dispatchOrder = new DispatchOrderDto()
            {
                OrderDate = DateTime.Now,
                Price = 35000,
                IsProcessed = false,
                Buyer = buyer,
                Car = car
            };

            var fileName = $"Order-{orderNumber}";
            var generatedXml = XMLUtils.SerializeXML<DispatchOrderDto>(dispatchOrder);

            XMLUtils.SaveXmlToFile(generatedXml, $"{fileName}.xml", FilePath);

            rebexConnection.PutFile($"{FilePath}/{fileName}.xml", $"{fileName}.xml");
            rebexConnection.Disconnect();
        }

        public int GetProcessedOrdersAmount()
        {
            return _baseRepository.GetProcessedOrdersAmount();
        }

        //Handling edge cases if the service stops before the orders are saved into the DB
        public void CorrectOrderMismatch() 
        {
            var rebexConnection = RebexUtils.CreateRebexConnection();

            var availableFiles = rebexConnection.GetList();
            var unprocessedOrders = _baseRepository.GetUnprocessedOrders();

            if (availableFiles.Count > 0 && unprocessedOrders.Count > 0)
            {
                foreach (SftpItem file in availableFiles)
                {
                    var stream = new MemoryStream();
                    UpsertOrderCommand deserializedOrder;

                    rebexConnection.GetFile(file.Name, stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    deserializedOrder = XMLUtils.DeserializeXML<UpsertOrderCommand>(stream);

                    var matchingOrder = unprocessedOrders.Find(uo => uo.OrderId == deserializedOrder.OrderId);

                    if (matchingOrder != null)
                    {
                        matchingOrder.IsProcessed = true;
                        var result = _baseRepository.CorrectOrderMismatch(matchingOrder);

                        if(result)
                            MarkOrderAsProcessed(deserializedOrder, file.Name, rebexConnection);
                    }
                }
            }
        }

        //On successful save to DB, mark the remote files in Rebex as processed and populate OrderId
        //This is used to have a source of truth whether or not we should process an order which is on the remote
        private void MarkOrderAsProcessed(UpsertOrderCommand order, string fileName, Sftp rebexConnection)
        {
            var dispatchOrder = new DispatchOrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Price = order.Price,
                IsProcessed = true,
                Buyer = order.Buyer,
                Car = order.Car
            };

            var generatedXml = XMLUtils.SerializeXML<DispatchOrderDto>(dispatchOrder);

            XMLUtils.SaveXmlToFile(generatedXml, fileName, FilePath);

            rebexConnection.PutFile($"{FilePath}/{fileName}", $"{fileName}");
        }
    }
}
