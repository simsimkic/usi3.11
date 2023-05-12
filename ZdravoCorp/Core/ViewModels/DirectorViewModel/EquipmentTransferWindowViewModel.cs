using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Utilities.CronJobs;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class EquipmentTransferWindowViewModel : ViewModelBase

{
    private int _inputQuantity;
    private readonly InventoryItem _inventoryItem;
    private readonly InventoryRepository _inventoryRepository;

    private readonly int _quantity;

    private readonly RoomRepository _roomRepository;
    private ObservableCollection<RoomViewModel> _rooms;
    private readonly int _sourceRoomId;
    private readonly TransferRepository _transferRepository;


    public EquipmentTransferWindowViewModel(int inventoryItemId, int roomId, int quantity,
        RoomRepository roomRepository, InventoryRepository inventoryRepository, TransferRepository transferRepository)
    {
        _rooms = new ObservableCollection<RoomViewModel>();
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _transferRepository = transferRepository;
        InventoryItemId = inventoryItemId;
        _sourceRoomId = roomId;
        _quantity = quantity;
        Quantity = 0;
        MaxQuantity = "Quantity(max " + _quantity + "):";
        _inventoryItem = _inventoryRepository.GetInventoryById(inventoryItemId);
        ConfirmTransfer = new DelegateCommand(o => Confirm(), o => CanConfirm());
        CancelTransfer = new DelegateCommand(o => Cancel());
        foreach (var room in _roomRepository.GetAllExcept(_inventoryItem.RoomId)) _rooms.Add(new RoomViewModel(room));

        InitComboBoxes();
    }

    public ICommand ConfirmTransfer { get; }
    public ICommand CancelTransfer { get; }
    public RoomViewModel? SelectedRoom { get; set; }

    public int[]? Hour { get; set; }
    public int[]? Minute { get; set; }

    public int? SelectedHour { get; set; }
    public int? SelectedMinute { get; set; }
    public DateTime? SelectedDate { get; set; }
    public string MaxQuantity { get; }

    public int Quantity
    {
        get => _inputQuantity;
        set
        {
            _inputQuantity = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public IEnumerable<RoomViewModel> Rooms
    {
        get => _rooms;
        set
        {
            _rooms = new ObservableCollection<RoomViewModel>(value);
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public int InventoryItemId { get; set; }
    public event EventHandler? OnRequestClose;
    public event EventHandler? OnRequestUpdate;

    private void InitComboBoxes()
    {
        Hour = new int[24];
        Minute = new int[60];
        for (var i = 0; i < 24; i++) Hour[i] = i;

        for (var i = 0; i < 60; i++) Minute[i] = i;
    }

    private bool CanConfirm()
    {
        return SelectedDate != null && SelectedHour != null && SelectedMinute != null && SelectedRoom != null &&
               Quantity != 0 && Quantity < _quantity;
    }

    private void Cancel()
    {
        OnRequestClose(this, new EventArgs());
    }

    private void Confirm()
    {
        var tempDate = (DateTime)SelectedDate;
        var when = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, (int)SelectedHour, (int)SelectedMinute, 0);

        var newTransfer = new Transfer(IDGenerator.GetId(), _roomRepository.GetById(_sourceRoomId),
            _roomRepository.GetById(SelectedRoom.Id), when, Quantity, InventoryItemId, _inventoryItem.Equipment.Name);
        _transferRepository.Add(newTransfer);
        Serializer.Save(_transferRepository);
        JobScheduler.TransferRequestTaskScheduler(newTransfer);
        OnRequestUpdate?.Invoke(this, new EventArgs());
        OnRequestClose?.Invoke(this, new EventArgs());
    }
}