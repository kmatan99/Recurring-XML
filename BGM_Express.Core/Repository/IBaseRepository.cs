using BGM_Express.Core.Command;
using BGM_Express.Core.Dto;
using BGM_Express.Domain.Models;

namespace BGM_Express.Core.Repository
{
    public interface IBaseRepository
    {
        List<Buyer> GetAvailableBuyers();
        List<Car> GetAvailableCars();
        int GetProcessedOrdersAmount();
        List<OrderDto> GetUnprocessedOrders();
        int UpsertOrder(UpsertOrderCommand orders);
        bool CorrectOrderMismatch(OrderDto order);
    }
}
