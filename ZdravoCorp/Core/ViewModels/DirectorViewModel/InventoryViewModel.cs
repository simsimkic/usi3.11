using ZdravoCorp.Core.Models.Inventory;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class InventoryViewModel : ViewModelBase
{
    private readonly InventoryItem _inventoryItem;

    public InventoryViewModel(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
    }

    public int Id => _inventoryItem.Id;
    public string? Name => _inventoryItem.Equipment?.Name;
    public int Room => _inventoryItem.RoomId;
    public string? Type => _inventoryItem.Equipment?.Type.ToString();
    public string? RoomType => _inventoryItem.Room?.Type.ToString();
    public int Quantity => _inventoryItem.Quantity;

    public override string ToString()
    {
        return
            $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Room)}: {Room}, {nameof(Type)}: {Type}, {nameof(RoomType)}: {RoomType}, {nameof(Quantity)}: {Quantity}";
    }
}