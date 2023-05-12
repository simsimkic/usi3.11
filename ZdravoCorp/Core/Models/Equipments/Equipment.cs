using System;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Models.Equipments;

public class Equipment
{
    public enum EquipmentType
    {
        Operation,
        Examination,
        Room,
        Hallway
    }


    [JsonConstructor]
    public Equipment(int id, string? name, EquipmentType type, bool isDynamic)
    {
        Id = id;
        Name = name;
        Type = type;
        IsDynamic = isDynamic;
    }

    public int Id { get; set; }
    public string? Name { get; set; }
    public EquipmentType Type { get; set; }
    public bool IsDynamic { get; set; }

    protected bool Equals(Equipment other)
    {
        return Id == other.Id && Name == other.Name && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Equipment)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, (int)Type);
    }
}