using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public class DoctorRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\doctors.json";


    public DoctorRepository()
    {
        Doctors = new List<Doctor>();
        Serializer.Load(this);
    }

    public List<Doctor>? Doctors { get; private set; }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return Doctors;
    }

    public void Import(JToken token)
    {
        Doctors = token.ToObject<List<Doctor>>();
    }


    public Doctor? GetDoctorByEmail(string email)
    {
        return Doctors.FirstOrDefault(doctor => doctor.Email == email);
    }

    public List<Doctor> GetAll()
    {
        return Doctors;
    }

    public List<Doctor> GetAllWithCertainSpecialization(Doctor.SpecializationType specialization)
    {
        var wantedDoctors = new List<Doctor>();
        foreach (var doctor in Doctors)
            if (doctor.Specialization == specialization)
                wantedDoctors.Add(doctor);
        return wantedDoctors;
    }

    public List<Doctor> GetAllSpecialized(Doctor.SpecializationType specializationType)
    {
        var suitableDoctors = Doctors.FindAll(doctor => doctor.Specialization == specializationType);
        return suitableDoctors;
    }
}