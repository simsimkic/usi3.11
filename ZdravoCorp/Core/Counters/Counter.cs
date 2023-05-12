using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZdravoCorp.Core.Counters;

public class Counter
{
    [JsonPropertyName("cancelations")] public List<DateTime> Cancelations;
    [JsonPropertyName("news")] public List<DateTime> News;

    public Counter()
    {
        Cancelations = new List<DateTime>();
        News = new List<DateTime>();
    }

    [JsonConstructor]
    public Counter(List<DateTime> clc, List<DateTime> chan)
    {
        Cancelations = clc;
        News = chan;
    }
}