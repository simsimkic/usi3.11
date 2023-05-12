using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.EquipmentRepo;

public class EquipmentRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\equipment.json";
    private List<Equipment>? _equipment;

    public EquipmentRepository()

    {
        _equipment = new List<Equipment>();
        Serializer.Load(this);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _equipment;
    }

    public void Import(JToken token)
    {
        _equipment = token.ToObject<List<Equipment>>();
    }


    public void Add(Equipment newEquipment)
    {
        _equipment?.Add(newEquipment);
    }

    public Equipment? GetById(int id)
    {
        return _equipment.FirstOrDefault(eq => eq.Id == id);
    }
}