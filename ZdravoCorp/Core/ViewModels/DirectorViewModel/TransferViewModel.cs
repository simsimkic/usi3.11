using ZdravoCorp.Core.Models.Transfers;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class TransferViewModel : ViewModelBase
{
    private readonly Transfer _transfer;


    public TransferViewModel(Transfer transfer)
    {
        _transfer = transfer;
    }

    public string? Item => _transfer.InventoryItemName;
    public string From => _transfer.From.Id.ToString();
    public string To => _transfer.To.Id.ToString();
    public string When => _transfer.When.ToString();
    public int Quantity => _transfer.Quantity;
}