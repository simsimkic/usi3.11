using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.View.DirectorView;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class MoveDEquipmentViewModel : ViewModelBase
{
    private ObservableCollection<InventoryViewModel> _allInventory;
    private ObservableCollection<InventoryViewModel> _inventory;
    private readonly InventoryRepository _inventoryRepository;
    private readonly object _lock;
    private RoomRepository _roomRepository;
    private string _searchText = "";

    public MoveDEquipmentViewModel(InventoryRepository inventoryRepository, RoomRepository roomRepository)
    {
        _lock = new object();
        _roomRepository = roomRepository;
        _inventoryRepository = inventoryRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => UpdateTable(true);
        _allInventory = new ObservableCollection<InventoryViewModel>();
        MoveSelectedInventoryItem = new DelegateCommand(o => MoveInventoryItem(), o => IsInventoryItemSelected());
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
            if (inventoryItem.Quantity <= 5)
                _allInventory.Add(new InventoryViewModel(inventoryItem));

        _inventory = _allInventory;
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
    }

    public InventoryViewModel? SelectedInventoryItemVm { get; set; }


    public ICommand MoveSelectedInventoryItem { get; }


    public string SearchBox
    {
        get => _searchText;
        set
        {
            _searchText = value;
            UpdateTable();
            OnPropertyChanged("Search");
        }
    }


    public IEnumerable<InventoryViewModel> Inventory
    {
        get => _inventory;
        set
        {
            _inventory = new ObservableCollection<InventoryViewModel>(value);
            OnPropertyChanged();
        }
    }


    private void UpdateTable(bool newAdded = false)
    {
        lock (_lock)
        {
            if (newAdded)
            {
                _allInventory = new ObservableCollection<InventoryViewModel>();
                foreach (var inventoryItem in _inventoryRepository.GetDynamic())
                    if (inventoryItem.Quantity <= 5)
                        _allInventory.Add(new InventoryViewModel(inventoryItem));
            }

            Inventory = UpdateTableFromSearch();
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<InventoryViewModel>(Search(_searchText));
        return _allInventory;
    }

    public IEnumerable<InventoryViewModel> Search(string inputText)
    {
        return _allInventory.Where(item => item.ToString().Contains(inputText));
    }

    private void MoveInventoryItem()
    {
        if (SelectedInventoryItemVm != null)
        {
            var inventoryItemId = SelectedInventoryItemVm.Id;
            var roomId = SelectedInventoryItemVm.Room;
            var vm = new DEquipmentTransferWindowViewModel(inventoryItemId, roomId, SelectedInventoryItemVm.Quantity,
                _inventoryRepository);

            var transferWindow = new DynamicTransferWindowView { DataContext = vm };
            vm.OnRequestClose += (s, e) => transferWindow.Close();
            transferWindow.Show();
        }
    }

    private bool IsInventoryItemSelected()
    {
        return SelectedInventoryItemVm != null;
    }
}