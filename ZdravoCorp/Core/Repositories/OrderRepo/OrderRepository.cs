using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Orders;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.OrderRepo;

public class OrderRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\orders.json";
    private List<Order>? _orders;
    public EventHandler OnRequestUpdate;

    public OrderRepository()
    {
        _orders = new List<Order>();
        Serializer.Load(this);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _orders;
    }

    public void Import(JToken token)
    {
        _orders = token.ToObject<List<Order>>();
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    public List<Order>? GetOrders()
    {
        return _orders;
    }
}