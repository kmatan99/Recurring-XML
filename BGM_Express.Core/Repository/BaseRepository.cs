using BGM_Express.Core.Command;
using BGM_Express.Core.Dto;
using BGM_Express.Domain;
using BGM_Express.Domain.Models;

namespace BGM_Express.Core.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly BGM_DbContext _dbContext;
        public BaseRepository(BGM_DbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public List<Buyer> GetAvailableBuyers()
        {
            return _dbContext.Buyers.ToList();
        }
        public List<Car> GetAvailableCars()
        {
            return _dbContext.Cars.ToList();
        }

        public int GetProcessedOrdersAmount()
        {
            return _dbContext.Orders.Count();
        }

        public List<OrderDto> GetUnprocessedOrders()
        {
            return _dbContext.Orders.Where(o => !o.IsProcessed).Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                IsProcessed = o.IsProcessed,
                OrderDate = o.OrderDate,
                Price = o.Price
            }).ToList();
        }

        public int UpsertOrder(UpsertOrderCommand order)
        {
            var newOrder = new Order
            {
                OrderDate = order.OrderDate,
                IsProcessed = true,
                Price = order.Price,
                CarId = order.Car.CarId,
                BuyerId = order.Buyer.BuyerId
            };

            _dbContext.Orders.Add(newOrder);
            _dbContext.SaveChanges();

            return newOrder.OrderId;
        }

        public bool CorrectOrderMismatch(OrderDto orderDto)
        {
            var order = new Order
            {
                OrderId = orderDto.OrderId,
                OrderDate = orderDto.OrderDate,
                Price = orderDto.Price,
                IsProcessed = true
            };

            _dbContext.Orders.Update(order);

            return _dbContext.SaveChanges() > 0;
        }
    }
}
