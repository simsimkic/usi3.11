using System;
using Newtonsoft.Json;
using ZdravoCorp.Core.Models.Equipments;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Inventory;

public class InventoryItem
{
    public InventoryItem(int id, int quantity, Room? room, Equipment? equipment)
    {
        Id = id;
        Quantity = quantity;
        Room = room;
        Equipment = equipment;
        EquipmentId = equipment.Id;
        RoomId = room.Id;
    }

    [JsonConstructor]
    public InventoryItem(int id, int quantity, int roomid, int equipmentid)
    {
        Id = id;
        Quantity = quantity;
        EquipmentId = equipmentid;
        RoomId = roomid;
    }

    public InventoryItem(InventoryItem other)
    {
        Id = other.Id;
        Quantity = other.Quantity;
        Room = other.Room;
        Equipment = other.Equipment;
        EquipmentId = other.EquipmentId;
        RoomId = other.RoomId;
    }

    public int Id { get; set; }
    public int Quantity { get; set; }
    [JsonIgnore] public Room? Room { get; set; }
    [JsonIgnore] public Equipment? Equipment { get; set; }

    public int EquipmentId { get; }
    public int RoomId { get; set; }

    public void UpdateRoom(Room room)
    {
        Room = room;
    }

    protected bool Equals(InventoryItem other)
    {
        return Id == other.Id && Quantity == other.Quantity && Equals(Room, other.Room) &&
               Equals(Equipment, other.Equipment) && EquipmentId == other.EquipmentId && RoomId == other.RoomId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((InventoryItem)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Quantity, Room, Equipment, EquipmentId, RoomId);
    }
}