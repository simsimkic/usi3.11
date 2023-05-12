using System;
using Quartz;
using Quartz.Impl;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class JobScheduler
{
    private static ISchedulerFactory _builder;
    private static IScheduler _scheduler;
    private static InventoryRepository _inventoryRepository;
    private static OrderRepository _orderRepository;
    private static TransferRepository _transferRepository;

    public JobScheduler(InventoryRepository inventoryRepository, TransferRepository transferRepository,
        OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
        _inventoryRepository = inventoryRepository;
        _transferRepository = transferRepository;
        _builder = new StdSchedulerFactory();
        _scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        _scheduler.Start();
        LoadScheduledTasks();
    }

    private void LoadScheduledTasks()
    {
        foreach (var order in _orderRepository.GetOrders())
            if (order.Status == Order.OrderStatus.Pending)
                DEquipmentTaskScheduler(order);

        foreach (var transfer in _transferRepository.GetAll()) TransferRequestTaskScheduler(transfer);
    }

    // dynamic equipment order task
    public static void DEquipmentTaskScheduler(Order order)
    {
        var job = JobBuilder.Create<DEquipmentExecuteOrder>()
            .WithIdentity("DEquipmentTask" + order.OrderTime, "Orders").Build();
        job.JobDataMap["order"] = order;
        job.JobDataMap["invrepo"] = _inventoryRepository;
        job.JobDataMap["ordrepo"] = _orderRepository;
        ITrigger trigger;
        if (order.ArrivalTime < DateTime.Now)
            trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, "OrderTriggers")
                .StartNow().ForJob(job).Build();
        else
            trigger = TriggerBuilder.Create().WithIdentity("trigger" + order.ArrivalTime, "OrderTriggers")
                .WithCronSchedule(
                    "0 " + order.ArrivalTime.Minute + " " + order.ArrivalTime.Hour + " " + order.ArrivalTime.Day + " " +
                    order.ArrivalTime.Month + " ? " + order.ArrivalTime.Year,
                    x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed()).ForJob(job)
                .Build();

        _scheduler.ScheduleJob(job, trigger);
    }

    // equipment transfer task

    public static void TransferRequestTaskScheduler(Transfer transfer)
    {
        var job = JobBuilder.Create<TransferRequestTask>()
            .WithIdentity("TrasferRequest" + transfer.Id, "Transfers").Build();
        job.JobDataMap["transfer"] = transfer;
        job.JobDataMap["invrepo"] = _inventoryRepository;
        job.JobDataMap["transrepo"] = _transferRepository;
        ITrigger trigger;
        if (transfer.When < DateTime.Now)
            trigger = TriggerBuilder.Create()
                .WithIdentity("transfer trigger" + transfer.When, "TransferTriggers").StartNow().ForJob(job)
                .Build();
        else
            trigger = TriggerBuilder.Create()
                .WithIdentity("transfer trigger" + transfer.When, "TransferTriggers").WithCronSchedule(
                    "0 " + transfer.When.Minute + " " + transfer.When.Hour + " " + transfer.When.Day + " " +
                    transfer.When.Month + " ? " + transfer.When.Year,
                    x => x.InTimeZone(TimeZoneInfo.Local).WithMisfireHandlingInstructionFireAndProceed()).ForJob(job)
                .Build();

        _scheduler.ScheduleJob(job, trigger);
    }
}