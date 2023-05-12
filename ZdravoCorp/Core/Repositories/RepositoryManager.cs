using System;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;

namespace ZdravoCorp.Core.Repositories;

public class RepositoryManager
{
    private DirectorRepository _directorRepository;
    private DoctorRepository _doctorRepository;
    private EquipmentRepository _equipmentRepository;
    private InventoryRepository _inventoryRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private NurseRepository _nurseRepository;
    private OrderRepository _orderRepository;
    private PatientRepository _patientRepository;
    private RoomRepository _roomRepository;
    private ScheduleRepository _scheduleRepository;
    private TransferRepository _transferRepository;
    private UserRepository _userRepository;

    public RepositoryManager()
    {
        _userRepository = new UserRepository();
        _directorRepository = new DirectorRepository();
        _patientRepository = new PatientRepository();
        _nurseRepository = new NurseRepository();
        _doctorRepository = new DoctorRepository();
        _equipmentRepository = new EquipmentRepository();
        _roomRepository = new RoomRepository();
        _inventoryRepository = new InventoryRepository(_roomRepository, _equipmentRepository);
        _medicalRecordRepository = new MedicalRecordRepository();
        _scheduleRepository = new ScheduleRepository();
        _orderRepository = new OrderRepository();
        _transferRepository = new TransferRepository();
    }

    public UserRepository UserRepository
    {
        get => _userRepository;
        set => _userRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DirectorRepository DirectorRepository
    {
        get => _directorRepository;
        set => _directorRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public PatientRepository PatientRepository
    {
        get => _patientRepository;
        set => _patientRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public NurseRepository NurseRepository
    {
        get => _nurseRepository;
        set => _nurseRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DoctorRepository DoctorRepository
    {
        get => _doctorRepository;
        set => _doctorRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public EquipmentRepository EquipmentRepository
    {
        get => _equipmentRepository;
        set => _equipmentRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public RoomRepository RoomRepository
    {
        get => _roomRepository;
        set => _roomRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public InventoryRepository InventoryRepository
    {
        get => _inventoryRepository;
        set => _inventoryRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public MedicalRecordRepository MedicalRecordRepository
    {
        get => _medicalRecordRepository;
        set => _medicalRecordRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ScheduleRepository ScheduleRepository
    {
        get => _scheduleRepository;
        set => _scheduleRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public OrderRepository OrderRepository
    {
        get => _orderRepository;
        set => _orderRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TransferRepository TransferRepository
    {
        get => _transferRepository;
        set => _transferRepository = value ?? throw new ArgumentNullException(nameof(value));
    }
}