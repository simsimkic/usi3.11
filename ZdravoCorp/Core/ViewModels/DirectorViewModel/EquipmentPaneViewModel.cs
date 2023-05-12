using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Repositories.InventoryRepo;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class EquipmentPaneViewModel : ViewModelBase
{
    private readonly ObservableCollection<InventoryViewModel> _allInventory;
    private ObservableCollection<InventoryViewModel> _filteredInventory;
    private ObservableCollection<InventoryViewModel> _inventory;
    private readonly InventoryRepository _inventoryRepository;
    private readonly object _lock;
    private string _searchText = "";
    private string _selectedEquipmentType = "None";
    private string _selectedQuantity = "None";
    private string _selectedRoomType = "None";
    private bool _warehouseChecked;

    public EquipmentPaneViewModel(InventoryRepository inventoryRepository)
    {
        _lock = new object();
        _inventoryRepository = inventoryRepository;
        _inventoryRepository.OnRequestUpdate += (s, e) => UpdateTable();
        _allInventory = new ObservableCollection<InventoryViewModel>();
        foreach (var inventoryItem in _inventoryRepository.GetAll())
            _allInventory.Add(new InventoryViewModel(inventoryItem));
        _inventory = _allInventory;
        EquipmentTypes = new ObservableCollection<string>();
        RoomTypes = new ObservableCollection<string>();
        Quantities = new ObservableCollection<string>();
        BindingOperations.EnableCollectionSynchronization(_inventory, _lock);
        LoadComboBoxCollections();
    }

    public bool IsWarehouseChecked
    {
        get => _warehouseChecked;
        set
        {
            _warehouseChecked = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

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

    public string SelectedRoomType
    {
        get => _selectedRoomType;
        set
        {
            _selectedRoomType = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public string SelectedEquipmentType
    {
        get => _selectedEquipmentType;
        set
        {
            _selectedEquipmentType = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public string SelectedQuantity
    {
        get => _selectedQuantity;
        set
        {
            _selectedQuantity = value;
            UpdateTable();
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> Quantities { get; }

    public ObservableCollection<string> RoomTypes { get; }

    public ObservableCollection<string> EquipmentTypes { get; }

    public IEnumerable<InventoryViewModel> Inventory
    {
        get => _inventory;
        set
        {
            _inventory = new ObservableCollection<InventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    private void UpdateTable()
    {
        lock (_lock)
        {
            _filteredInventory = _allInventory;
            var wh = _filteredInventory.Intersect(UpdateTableFromWarehouseChecked());
            var f1 = wh.Intersect(UpdateTableFromSearch());
            var f2 = f1.Intersect(UpdateTableFromEquipmentType());
            var f3 = f2.Intersect(UpdateTableFromRoomType());
            var f4 = f3.Intersect(UpdateTableFromQuantity());

            Inventory = f4;
        }
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromSearch()
    {
        if (_searchText != "")
            return new ObservableCollection<InventoryViewModel>(Search(_searchText));
        return _allInventory;
    }

    private IEnumerable<InventoryViewModel> UpdateTableFromRoomType()
    {
        if (_selectedRoomType != "None")
            return new ObservableCollection<InventoryViewModel>(FilterByRoomType(_selectedRoomType));
        return _allInventory;
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromEquipmentType()
    {
        if (_selectedEquipmentType != "None")
            return new ObservableCollection<InventoryViewModel>(FilterRoomByEquipmentType(_selectedEquipmentType));
        return _allInventory;
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromQuantity()
    {
        if (_selectedQuantity != "None")
            return new ObservableCollection<InventoryViewModel>(FilterByQuantity(_selectedQuantity));
        return _allInventory;
    }

    private ObservableCollection<InventoryViewModel> UpdateTableFromWarehouseChecked()
    {
        if (_warehouseChecked == false)
            return new ObservableCollection<InventoryViewModel>(FilterFromWarehouse());
        return _allInventory;
    }


    public IEnumerable<InventoryViewModel> Search(string inputText)
    {
        return _allInventory.Where(item => item.ToString().Contains(inputText));
    }

    public IEnumerable<InventoryViewModel> FilterFromWarehouse()
    {
        return _allInventory.Where(item => item.RoomType != RoomType.StockRoom.ToString());
    }

    public IEnumerable<InventoryViewModel> FilterByRoomType(string roomType)
    {
        return _allInventory.Where(item => item.RoomType == roomType);
    }

    public IEnumerable<InventoryViewModel> FilterRoomByEquipmentType(string equipmentType)
    {
        return _allInventory.Where(item => item.Type == equipmentType);
    }

    public IEnumerable<InventoryViewModel> FilterByQuantity(string quantity)
    {
        if (quantity == "not in stock") return _allInventory.Where(item => item.Quantity == 0);

        if (quantity == "0-10") return _allInventory.Where(item => item.Quantity < 10);

        if (quantity == "10+") return _allInventory.Where(item => item.Quantity >= 10);

        return null;
    }


    public void LoadComboBoxCollections()
    {
        RoomTypes.Add("None");
        RoomTypes.Add(RoomType.OperationRoom.ToString());
        RoomTypes.Add(RoomType.WaitingRoom.ToString());
        RoomTypes.Add(RoomType.StockRoom.ToString());
        RoomTypes.Add(RoomType.PatientRoom.ToString());
        RoomTypes.Add(RoomType.ExaminationRoom.ToString());

        EquipmentTypes.Add("None");
        EquipmentTypes.Add(Equipment.EquipmentType.Room.ToString());
        EquipmentTypes.Add(Equipment.EquipmentType.Examination.ToString());
        EquipmentTypes.Add(Equipment.EquipmentType.Hallway.ToString());
        EquipmentTypes.Add(Equipment.EquipmentType.Operation.ToString());

        Quantities.Add("None");
        Quantities.Add("not in stock");
        Quantities.Add("0-10");
        Quantities.Add("10+");
    }
}