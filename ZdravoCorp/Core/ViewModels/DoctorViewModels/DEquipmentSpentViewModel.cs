using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.ViewModels.DirectorViewModel;

namespace ZdravoCorp.Core.ViewModels.DoctorViewModels;

public class DEquipmentSpentViewModel : ViewModelBase
{
    private ObservableCollection<DynamicInventoryViewModel> _dynamicInventory;
    private readonly InventoryRepository _inventoryRepository;
    private readonly int _roomId;

    public DEquipmentSpentViewModel(InventoryRepository inventoryRepository, int roomId)
    {
        _roomId = roomId;
        _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>();
        _inventoryRepository = inventoryRepository;
        foreach (var inventoryItem in _inventoryRepository.GetDynamic())
            if (inventoryItem.RoomId == _roomId)
                _dynamicInventory.Add(new DynamicInventoryViewModel(inventoryItem));
        ConfirmSpentQuantity = new DelegateCommand(o => ConfirmChanges());
    }

    public IEnumerable<DynamicInventoryViewModel> DynamicInventory

    {
        get => _dynamicInventory;
        set
        {
            _dynamicInventory = new ObservableCollection<DynamicInventoryViewModel>(value);
            OnPropertyChanged();
        }
    }

    public ICommand ConfirmSpentQuantity { get; }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void ConfirmChanges()
    {
        var correctnessCheck = true;
        foreach (var inventoryItem in _dynamicInventory)
        {
            if (!inventoryItem.IsChecked) continue;
            if (inventoryItem.OrderQuantity <= 0 || inventoryItem.OrderQuantity > inventoryItem.Quantity)
            {
                correctnessCheck = false;
                break;
            }

            _inventoryRepository.GetInventoryById(inventoryItem.Id).Quantity -= inventoryItem.OrderQuantity;
            Serializer.Save(_inventoryRepository);
        }

        if (correctnessCheck) CloseWindow();
        else MessageBox.Show("Invalid data", "Error", MessageBoxButton.OK);
    }
}