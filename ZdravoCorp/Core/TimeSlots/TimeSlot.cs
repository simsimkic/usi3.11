using System;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoCorp.Core.TimeSlots;

public class TimeSlot : IEquatable<TimeSlot> //will be some functions for time
{
    public TimeSlot(DateTime start, DateTime end)
    {
        this.start = start;
        this.end = end;
    }

    public DateTime start { get; set; }
    public DateTime end { get; set; }

    public bool Equals(TimeSlot? other)
    {
        if (other == null) return false;
        return start.Year == other.start.Year && start.Month == other.start.Month &&
               start.Day == other.start.Day && start.Hour == other.start.Hour &&
               start.Minute == other.start.Minute;
    }

    public bool Overlap(TimeSlot time)
    {
        return start >= time.end || end <= time.start;
    }

    public int GetTimeBeforeStart(DateTime time)
    {
        return (start - time).Days * 24 + (start - time).Hours;
    }

    public bool IsInsideSingleSlot(TimeSlot time)
    {
        return start >= time.start && end <= time.end;
    }

    public bool IsInsideListOfSlots(IEnumerable<TimeSlot> slots)
    {
        return slots.Any(IsInsideSingleSlot);
    }

    public List<TimeSlot> GiveSameTimeUntileSomeDay(DateTime lastDate)
    {
        var allSlots = new List<TimeSlot>();
        var current = this;
        while (current.start < lastDate)
        {
            allSlots.Add(current);
            current = new TimeSlot(current.start.AddDays(1), current.end.AddDays(1));
        }

        return allSlots;
    }

    public TimeSlot ExtendButStayOnSameDay(TimeSpan amount)
    {
        var adjustedStart = start.Add(-1 * amount);
        var adjustedEnd = end.Add(amount);

        if (adjustedStart.Date < start.Date) adjustedStart = start;

        if (adjustedEnd.Date > end.Date) adjustedEnd = end.Date.AddDays(1).AddSeconds(-1);

        return new TimeSlot(adjustedStart, adjustedEnd);
    }

    public override bool Equals(object? o)
    {
        if (o == null) return false;

        return o is TimeSlot && Equals(o);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + start.Year.GetHashCode();
            hash = hash * 23 + start.Month.GetHashCode();
            hash = hash * 23 + start.Day.GetHashCode();
            hash = hash * 23 + start.Hour.GetHashCode();
            hash = hash * 23 + start.Minute.GetHashCode();
            return hash;
        }
    }

    public bool IsNow()
    {
        var nowTime = DateTime.Now;
        var interval = start - nowTime;
        var notPassed = !(start.CompareTo(DateTime.Now) < 0);
        return interval.TotalMinutes < 15 && notPassed;
    }

    public static DateTime GiveFirstDevisibleBy15(DateTime time) //this should be somewhere else
    {
        var minutes = time.Minute;
        var minutesToAdd = minutes switch
        {
            < 15 => 15 - minutes,
            < 30 => 30 - minutes,
            < 45 => 45 - minutes,
            < 60 => 60 - minutes,
            _ => 0
        };
        return time.AddMinutes(minutesToAdd);
    }

    public bool IsBefore()
    {
        return start.CompareTo(start) < 0;
    }
}