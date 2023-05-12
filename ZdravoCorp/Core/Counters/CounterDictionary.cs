using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;

namespace ZdravoCorp.Core.Counters;

public class CounterDictionary
{
    private readonly string _fileName = @".\..\..\..\Data\counters.json";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        //WriteIndented = true
        PropertyNameCaseInsensitive = true
    };

    public Dictionary<string, Counter>? AllCounters;

    public CounterDictionary()
    {
        AllCounters = new Dictionary<string, Counter>();
        LoadFromFile();
    }

    public void AddCancelation(string email, DateTime date)
    {
        if (AllCounters.ContainsKey(email))
        {
            AllCounters[email].Cancelations.Add(date);
            foreach (var d in AllCounters[email].Cancelations)
            {
                var monthAgo = DateTime.Now - TimeSpan.FromDays(30);
                if (d < monthAgo)
                    AllCounters[email].Cancelations.Remove(d);
            }
        }
        else
        {
            var c = new Counter();
            c.Cancelations = new List<DateTime> { date };
            AllCounters.Add(email, c);
        }

        //AllCounters[email].Cancelations =
        SaveToFile();
    }

    public void AddNews(string email, DateTime date)
    {
        if (AllCounters.ContainsKey(email))
        {
            AllCounters[email].Cancelations.Add(date);
            foreach (var d in AllCounters[email].Cancelations)
            {
                var monthAgo = DateTime.Now - TimeSpan.FromDays(30);
                if (d < monthAgo)
                    AllCounters[email].Cancelations.Remove(d);
            }
        }

        else
        {
            var c = new Counter();
            c.News = new List<DateTime> { date };
            AllCounters.Add(email, c);
        }

        SaveToFile();
    }

    public bool IsForBlock(string email)
    {
        try
        {
            return AllCounters[email].Cancelations.Count >= 50 || AllCounters[email].News.Count >= 80;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public void LoadFromFile()
    {
        var text = File.ReadAllText(_fileName);
        if (text == "") return;
        var users = JsonConvert.DeserializeObject<Dictionary<string, Counter>>(text);

        AllCounters = users;
    }

    public void SaveToFile()
    {
        var counters = JsonConvert.SerializeObject(AllCounters, Formatting.Indented);
        File.WriteAllText(_fileName, counters);
    }
}