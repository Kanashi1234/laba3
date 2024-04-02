using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        // Створюємо список подій
        List<Event> calendar = new List<Event>();

        // Додаємо деякі події до календаря
        calendar.Add(new SingleEvent("Подія 1", new DateTime(2024, 4, 1)));
        calendar.Add(new SingleEvent("Подія 2", new DateTime(2024, 4, 5)));
        calendar.Add(new PeriodicEvent("Подія 3", new DateTime(2024, 4, 10), TimeSpan.FromDays(7)));

        // Отримуємо наступну подію
        Event nextEvent = GetNextEvent(calendar);
        if (nextEvent != null)
        {
            Console.WriteLine($"Наступна подія: {nextEvent.Name}, Дата: {nextEvent.GetDate().Value.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine("Немає наступних подій в календарі.");
        }
    }

    public static Event GetNextEvent(List<Event> calendar)
    {
        Event nextEvent = null;
        DateTime? earliestDate = null;

        foreach (var ev in calendar)
        {
            DateTime? eventDate = ev.GetDate();
            if (eventDate.HasValue && (!earliestDate.HasValue || eventDate.Value < earliestDate.Value))
            {
                earliestDate = eventDate;
                nextEvent = ev;
            }
        }

        return nextEvent;
    }
}

// Базовий клас події
public abstract class Event
{
    public string Name { get; protected set; }

    public abstract DateTime? GetDate();
}

// Клас події, яка відбувається лише один раз
public class SingleEvent : Event
{
    private DateTime eventDate;

    public SingleEvent(string name, DateTime date)
    {
        this.Name = name;
        this.eventDate = date;
    }

    public override DateTime? GetDate()
    {
        if (DateTime.Now < eventDate)
        {
            return eventDate;
        }
        else
        {
            return null;
        }
    }
}

// Клас події, яка відбувається періодично
public class PeriodicEvent : Event
{
    private DateTime startDate;
    private TimeSpan interval;

    public PeriodicEvent(string name, DateTime startDate, TimeSpan interval)
    {
        this.Name = name;
        this.startDate = startDate;
        this.interval = interval;
    }

    public override DateTime? GetDate()
    {
        DateTime nextOccurrence = startDate;
        while (nextOccurrence < DateTime.Now)
        {
            nextOccurrence += interval;
        }

        return nextOccurrence;
    }
}
