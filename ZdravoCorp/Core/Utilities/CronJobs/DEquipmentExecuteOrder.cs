using System;
using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class DEquipmentExecuteOrder : IJob
{
    private InventoryRepository _inventoryRepository;
    private Order _order;
    private OrderRepository _orderRepository;

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _order = (Order)dataMap["order"];
        _inventoryRepository = (InventoryRepository)dataMap["invrepo"];
        _orderRepository = (OrderRepository)dataMap["ordrepo"];
        foreach (var item in _order.Items)
            _inventoryRepository.AddItem(new InventoryItem(IDGenerator.GetId(), item.Value, 999, item.Key));

        _order.Status = Order.OrderStatus.Completed;
        _inventoryRepository.LoadRoomsAndEquipment();
        _inventoryRepository.OnRequestUpdate?.Invoke(this, new EventArgs());
        _orderRepository.OnRequestUpdate?.Invoke(this, new EventArgs());
        Serializer.Save(_inventoryRepository);
        Serializer.Save(_orderRepository);


        return Task.CompletedTask;
    }
}