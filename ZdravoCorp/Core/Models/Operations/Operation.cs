using System.Collections.Generic;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.Models.Operations;

public class Operation
{
    public readonly bool IsCanceled;

    public Operation(int id, TimeSlot time, Doctor doctor, MedicalRecord medicalRecord)
    {
        Id = id;
        Time = time;
        Doctor = doctor;
        MedicalRecord = medicalRecord;
        Doctor = doctor;
        MedicalRecord = medicalRecord;
        Room = null;
        Equipment = null;
        IsCanceled = false;
    }

    public int Id { get; set; }
    public TimeSlot Time { get; set; }
    public Doctor Doctor { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public Room? Room { get; set; }
    public List<Equipment>? Equipment { get; set; }
}