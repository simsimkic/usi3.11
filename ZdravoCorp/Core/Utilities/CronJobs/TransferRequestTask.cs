using System;
using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class TransferRequestTask : IJob
{
    private InventoryRepository _inventoryRepository;
    private Transfer _transfer;
    private TransferRepository _transferRepository;

    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _transfer = (Transfer)dataMap["transfer"];
        _inventoryRepository = (InventoryRepository)dataMap["invrepo"];
        _transferRepository = (TransferRepository)dataMap["transrepo"];
        _inventoryRepository.UpdateInventoryItem(_transfer);


        _transferRepository.Remove(_transfer);
        _transferRepository.OnRequestUpdate(this, new EventArgs());
        _inventoryRepository.OnRequestUpdate(this, new EventArgs());

        return Task.CompletedTask;
    }
}