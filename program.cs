using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.IO;

namespace JSON_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Ticket tick1 = new Ticket(1, "09-04-2020", 1, "A", 15);
            Ticket tick2 = new Ticket(1, "09-04-2020", 1, "A", 16);
            Ticket tick3 = new Ticket(1, "09-04-2020", 1, "A", 17);

            var jsonString = JsonSerializer.Serialize(new Ticket[] { tick1, tick2, tick3 });

            Console.WriteLine(jsonString);
            File.WriteAllText("data.json", jsonString);
            
        }
    }

    public class Ticket
    {
        public int Price { get; set; }
        public string Date { get; set; }
        public int TheaterNumber { get; set; }
        public SeatInfo SeatNumber { get; set; }

        public Ticket(int price, string date, int theaterNumber, string row, int seat)
        {
            Price = price;
            Date = date;
            TheaterNumber = theaterNumber;
            SeatNumber = new SeatInfo(row, seat);
        }

    }

    public class SeatInfo
    {
        public string Row { get; set; }
        public int Seat { get; set; }

        public SeatInfo(string row, int seat)
        {
            Row = row;
            Seat = seat;
        }
    }

}