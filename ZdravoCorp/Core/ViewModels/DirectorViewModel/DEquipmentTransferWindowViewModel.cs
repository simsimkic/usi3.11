using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.InventoryRepo;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DEquipmentTransferWindowViewModel : ViewModelBase

{
    private readonly InventoryRepository _inventoryRepository;
    private int _maxMoveQuantity;
    private int _moveQuantity;
    private ObservableCollection<SourceRoomViewModel> _rooms;


    public DEquipmentTransferWindowViewModel(int inventoryItemId, int roomId, int quantity,
        InventoryRepository inventoryRepository)
    {
        _rooms = new ObservableCollection<SourceRoomViewModel>();
        _inventoryRepository = inventoryRepository;
        InventoryItemId = inventoryItemId;
        DestinationRoom = roomId;
        Quantity = quantity;
        MoveQuantity = 0;
        MaxMoveQuantity = -1;
        var inventoryItem = _inventoryRepository.GetInventoryById(inventoryItemId);
        RoomType = inventoryItem?.Room.Type.ToString();
        Item = inventoryItem.Equipment.Name;

        foreach (var item in _inventoryRepository.LocateItem(inventoryItem)) _rooms.Add(new SourceRoomViewModel(item));
        ConfirmTransfer = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelTransfer = new DelegateCommand(o => Cancel());
    }

    public ICommand ConfirmTransfer { get; set; }

    public ICommand CancelTransfer { get; }
    public SourceRoomViewModel? SelectedRoom { get; set; }
    public int Quantity { get; set; }
    public int InventoryItemId { get; set; }

    public int MoveQuantity
    {
        get => _moveQuantity;
        set
        {
            if (SelectedRoom != null)
                _maxMoveQuantity = SelectedRoom.Quantity;
            else
                _maxMoveQuantity = -1;

            _moveQuantity = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }


    public int MaxMoveQuantity { get; set; }
    public int DestinationRoom { get; set; }
    public string RoomType { get; set; }
    public string? Item { get; set; }

    public IEnumerable<SourceRoomViewModel> Rooms
    {
        get => _rooms;
        set
        {
            _rooms = new ObservableCollection<SourceRoomViewModel>(value);
            CommandManager.InvalidateRequerySuggested();
            OnPropertyChanged();
        }
    }

    public event EventHandler OnRequestClose;


    private bool CanConfirm()
    {
        return SelectedRoom != null && MoveQuantity <= _maxMoveQuantity;
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()

    {
        _inventoryRepository.UpdateDestinationInventoryItem(SelectedRoom.ItemId, InventoryItemId, _moveQuantity);
        OnRequestClose(this, new EventArgs());
        _inventoryRepository.OnRequestUpdate.Invoke(this, new EventArgs());
    }
}