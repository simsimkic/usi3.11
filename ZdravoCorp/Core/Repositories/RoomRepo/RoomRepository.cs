using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Rooms;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.RoomRepo;

public class RoomRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\rooms.json";


    public RoomRepository()
    {
        Rooms = new List<Room>();
        Serializer.Load(this);
    }

    public List<Room>? Rooms { get; private set; }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return Rooms;
    }

    public void Import(JToken token)
    {
        Rooms = token.ToObject<List<Room>>();
    }

    public void Add(Room newRoom)
    {
        Rooms.Add(newRoom);
    }

    public IEnumerable<Room> GetAllExcept(int roomId)
    {
        return Rooms.Where(room => room.Id != roomId);
    }


    public Room? GetById(int id)
    {
        return Rooms.FirstOrDefault(room => room.Id == id);
    }
}