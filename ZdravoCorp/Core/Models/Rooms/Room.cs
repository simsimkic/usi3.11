using System;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Models.Rooms;

public class Room
{
    [JsonConstructor]
    public Room(int id, RoomType type)
    {
        Id = id;
        Type = type;
    }

    public int Id { get; set; }
    public RoomType Type { get; set; }

    protected bool Equals(Room other)
    {
        return Id == other.Id && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Room)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, (int)Type);
    }
}

public enum RoomType
{
    StockRoom,
    OperationRoom,
    ExaminationRoom,
    PatientRoom,
    WaitingRoom
}