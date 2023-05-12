using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.Models.Transfers;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.TransfersRepo;

public class TransferRepository : ISerializable
{
    private readonly string _fileName = @".\..\..\..\Data\transfers.json";
    private List<Transfer>? _transfers;


    public EventHandler OnRequestUpdate;

    public TransferRepository()
    {
        _transfers = new List<Transfer>();
        Serializer.Load(this);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _transfers;
    }

    public void Import(JToken token)
    {
        _transfers = token.ToObject<List<Transfer>>();
    }

    public List<Transfer> GetAll()
    {
        return _transfers;
    }

    public void Add(Transfer transfer)
    {
        _transfers.Add(transfer);
    }

    public void Remove(Transfer transfer)
    {
        _transfers.Remove(transfer);
        Serializer.Save(this);
    }
}