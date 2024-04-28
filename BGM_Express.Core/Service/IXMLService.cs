namespace BGM_Express.Core.Service
{
    public interface IXMLService
    {
        bool UpsertOrders();
        void DispatchOrder(int orderNumber);
        int GetProcessedOrdersAmount();
        void CorrectOrderMismatch();
    }
}
