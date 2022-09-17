namespace GreedyAlgorithms
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    static class Program
    {
        public static void Main()
        {
            Print(FindShortestSum(129));
            Print(FindShortestSum(1041));

            var events = new List<Event>()
            {
                new Event(1, 4),
                new Event(3, 5),
                new Event(0, 6),
                new Event(5, 7),
                new Event(3, 8),
                new Event(5, 9),
                new Event(6, 10),
                new Event(8, 11),
                new Event(8, 12),
                new Event(2, 13),
                new Event(12, 14),
            };

            Print(SelectMaxEvents(events));
        }

        public static List<Event> SelectMaxEvents(List<Event> events)
        {
            var result = new List<Event>();

            while (events.Any())
            {
                Event next = events.First(ev => ev.End == events.Min(e => e.End));

                events.RemoveAll(ev => ev.Start < next.End);

                result.Add(next);
            }

            return result;
        }

        public static List<int> FindShortestSum(int total)
        {
            int[] values = { 1, 5, 10, 25, 50, 100 };

            var result = new List<int>();

            while (total > 0)
            {
                int value = values.Last(v => v <= total);

                result.Add(value);

                total -= value;
            }

            result.Reverse();

            return result;
        }

        public static void Print<T>(IEnumerable<T> items)
        {
            Console.WriteLine(string.Join(" ", items));
        }
    }

    class Event
    {
        public Event(int start, int end)
        {
            this.Start = start;
            this.End = end;
        }

        public int Start { get; }

        public int End { get; }

        public override string ToString()
        {
            return $"Start: {this.Start}, End: {this.End}";
        }
    }
}