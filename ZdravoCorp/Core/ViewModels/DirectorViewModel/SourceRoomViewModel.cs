using ZdravoCorp.Core.Models.Inventory;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class SourceRoomViewModel : ViewModelBase
{
    private readonly InventoryItem _inventoryItem;

    public SourceRoomViewModel(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
    }

    public int ItemId => _inventoryItem.Id;
    public int Id => _inventoryItem.RoomId;
    public int Quantity => _inventoryItem.Quantity;
}