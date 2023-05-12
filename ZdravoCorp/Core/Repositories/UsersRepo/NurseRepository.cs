using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.UsersRepo;

public class NurseRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\nurses.json";
    private List<Nurse?> _nurses;


    public NurseRepository()
    {
        _nurses = new List<Nurse?>();
        Serializer.Load(this);
    }


    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _nurses;
    }

    public void Import(JToken token)
    {
        _nurses = token.ToObject<List<Nurse>>();
    }

    public void Add(Nurse? nurse)
    {
        _nurses.Add(nurse);
    }

    public Nurse? GetNurseByEmail(string email)
    {
        return _nurses.FirstOrDefault(nurse => nurse.Email == email);
    }
}