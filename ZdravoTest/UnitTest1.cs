using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Models.Inventory;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Operations;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestDirector()
    {
        Director director1 = new Director("email", "ime", "prezime");
        DirectorRepository directorRepository = new DirectorRepository(director1 );
        //directorRepository.SaveToFile();

        DirectorRepository directorRepository2 = new DirectorRepository();
        Assert.AreEqual(directorRepository2.Director, director1);
    }

    [TestMethod]
    public void TestDoctor()
    {
        Doctor? doctor1 = new Doctor("email", "ime", "pre", Doctor.SpecializationType.Neurologist);
        DoctorRepository doctorRepository = new DoctorRepository();
        //doctorRepository.Add(doctor1);
        //doctorRepository.SaveToFile();

        DoctorRepository doctorRepository2 = new DoctorRepository();
        Doctor? doctor2 = doctorRepository2.GetDoctorByEmail("email");
        Assert.AreEqual(doctor1, doctor2);
    }

    [TestMethod]
    public void TestNurse()
    {
        Nurse? nurse1 = new Nurse("email", "ime", "prezime");
        NurseRepository nurseRepository = new NurseRepository();
        nurseRepository.Add(nurse1);
        //nurseRepository.SaveToFile();

        NurseRepository nurseRepository2 = new NurseRepository();
        Nurse? nurse2 = nurseRepository.GetNurseByEmail("email");
        Assert.AreEqual(nurse1, nurse2);
    }

    [TestMethod]
    public void TestPatient()
    {
        Patient patient1 = new Patient("email", "ime", "prezime");
        PatientRepository patientRepository = new PatientRepository();
        patientRepository.Add(patient1);
        //patientRepository.SaveToFile();

        PatientRepository patientRepository2 = new PatientRepository();
        Patient? patient2 = patientRepository2.GetPatientByEmail("email");
        Assert.AreEqual(patient1, patient2);
        
    }

    [TestMethod]
    public void TestUser()
    {
        User user = new User("pass", "email", User.UserType.Director, User.State.NotBlocked);
        UserRepository userRepository = new UserRepository();
        userRepository.AddUser(user);
        //userRepository.SaveToFile();

        UserRepository userRepository2 = new UserRepository();
        User user2 = userRepository2.GetUserByEmail("email");
        Assert.AreEqual(user, user2);
    }

    [TestMethod]
    public void TestRoom()
    {
        Room room = new Room(12, RoomType.ExaminationRoom);
        RoomRepository roomRepository = new RoomRepository();
        roomRepository.Add(room);
        //roomRepository.SaveToFile();

        RoomRepository roomRepository2= new RoomRepository();
        Room room2 = roomRepository2.GetById(12);
        Assert.AreEqual(room, room2);
    }
    [TestMethod]
    public void TestOperation()
    {
        Operation operation1 = new Operation(12, new TimeSlot(DateTime.Now, DateTime.Now),
            new Doctor("email", "ime", "prezime", Doctor.SpecializationType.Anesthesiologist),
            new MedicalRecord(new Patient("email", "ime", "prezime"), 20, 120));
        ScheduleRepository scheduleRepository = new ScheduleRepository();
        scheduleRepository.AddOperation(operation1);
        //scheduleRepository.SaveOperations();

        ScheduleRepository scheduleRepository2 = new ScheduleRepository();
        //scheduleRepository2.LoadOperations();
        Operation ?operation2 = scheduleRepository2.GetOperationById(12);
        Assert.AreEqual(operation1, operation2);

    }
    [TestMethod]
    public void TestEquipment()
    {
        Equipment equipment1 = new Equipment(23, "stolica", Equipment.EquipmentType.Operation, false);
        EquipmentRepository equipmentRepository = new EquipmentRepository();
        equipmentRepository.Add(equipment1);
        //equipmentRepository.SaveToFile();

        EquipmentRepository equipmentRepository2 = new EquipmentRepository();
        Equipment? equipment2 = equipmentRepository2.GetById(23);
        Assert.AreEqual(equipment1, equipment2);

    }

    [TestMethod]
    public void TestInventory()
    {
        Room room = new Room(22, RoomType.ExaminationRoom);
        Equipment equipment = new Equipment(33, "sto", Equipment.EquipmentType.Examination, false);
        RoomRepository roomRepository = new RoomRepository();
        roomRepository.Add(room);
        EquipmentRepository equipmentRepository = new EquipmentRepository();
        equipmentRepository.Add(equipment);
        InventoryItem inventoryItem1 = new InventoryItem(22, 44, room, equipment);
        InventoryRepository inventoryRepository = new InventoryRepository(roomRepository, equipmentRepository);
        inventoryRepository.AddItem(inventoryItem1);
        //inventoryRepository.SaveToFile();

        InventoryRepository inventoryRepository2 = new InventoryRepository(roomRepository, equipmentRepository);
        InventoryItem ?inventoryItem2= inventoryRepository.GetInventoryById(22);
        
        Assert.AreEqual(inventoryItem1, inventoryItem2);
    }





}