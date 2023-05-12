using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;

namespace ZdravoCorp.Core.ViewModels.DirectorViewModel;

public class DirectorViewModel : ViewModelBase

{
    private object _currentView;
    private readonly EquipmentRepository _equipmentRepository;
    private readonly InventoryRepository _inventoryRepository;
    private readonly OrderRepository _orderRepository;
    private readonly RoomRepository _roomRepository;
    private readonly TransferRepository _transferRepository;

    public DirectorViewModel(RepositoryManager _repositoryManager)
    {
        _inventoryRepository = _repositoryManager.InventoryRepository;
        _orderRepository = _repositoryManager.OrderRepository;
        _equipmentRepository = _repositoryManager.EquipmentRepository;
        _roomRepository = _repositoryManager.RoomRepository;
        _transferRepository = _repositoryManager.TransferRepository;
        ViewEquipmentCommand = new DelegateCommand(o => EquipmentView());
        MoveEquipmentCommand = new DelegateCommand(o => MoveEquipmentView());
        ViewDynamicEquipmentCommand = new DelegateCommand(o => DynamicEquipmentView());
        MoveDynamicEquipmentCommand = new DelegateCommand(o => MoveDynamicEquipmentView());
        _currentView = new EquipmentPaneViewModel(_inventoryRepository);
    }

    public ICommand ViewEquipmentCommand { get; private set; }
    public ICommand ViewDynamicEquipmentCommand { get; private set; }
    public ICommand MoveDynamicEquipmentCommand { get; private set; }
    public ICommand MoveEquipmentCommand { get; private set; }


    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public void EquipmentView()
    {
        CurrentView = new EquipmentPaneViewModel(_inventoryRepository);
    }

    public void DynamicEquipmentView()
    {
        CurrentView = new DEquipmentPaneViewModel(_inventoryRepository, _orderRepository, _equipmentRepository);
    }

    public void MoveDynamicEquipmentView()
    {
        CurrentView = new MoveDEquipmentViewModel(_inventoryRepository, _roomRepository);
    }

    public void MoveEquipmentView()
    {
        CurrentView = new MoveEquipmentViewModel(_inventoryRepository, _roomRepository, _transferRepository);
    }
}