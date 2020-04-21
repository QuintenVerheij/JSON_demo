using System;


using System.Text.Json;
using System.Text.Json.Serialization;

using System.Collections.Generic;

using System.IO;

namespace JSON_demo
{
    public class CustomNamingPolicy: JsonNamingPolicy 
    {
        public override string ConvertName(string name){
            return "ALLNAMESARENOWTHISEXACTSTRING. PROBABLY NOT A GREAT IDEA";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Ticket tick1 = new Ticket(1, "09-04-2020", 1, "A", 15);
            Ticket tick2 = new Ticket(1, "09-04-2020", 1, "A", 16);
            Ticket tick3 = new Ticket(1, "09-04-2020", 1, "A", 17);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true,
                IgnoreReadOnlyProperties = true
            };


            //STORE JSON OBJECT
            var jsonString = JsonSerializer.Serialize(new Ticket[] { tick1, tick2, tick3 }, options);

            Console.WriteLine(jsonString);
            File.WriteAllText("data.json", jsonString);


            //READ AND UPDATE JSON OBJECT
            string jsonText = File.ReadAllText("data.json");
            List<Ticket> tickets = new List<Ticket>();
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                JsonElement root = document.RootElement;
                JsonElement ticketsArrayElement = root;
                
                foreach (JsonElement ticket in ticketsArrayElement.EnumerateArray())
                {
                    
                    //Make .NET object is values are present in JSON
                    if (ticket.TryGetProperty("Price", out JsonElement PriceElement) &&
                    ticket.TryGetProperty("Date", out JsonElement DateElement) &&
                    ticket.TryGetProperty("TheaterNumber", out JsonElement TheaterElement) &&
                    ticket.TryGetProperty("SeatNumber", out JsonElement SeatElement) &&
                        SeatElement.TryGetProperty("Row", out var RowElement) && SeatElement.TryGetProperty("Seat", out var SeatNumElement)
                    ){
                        int price = PriceElement.GetInt32();
                        string date = DateElement.GetString();
                        int theaterNumber = TheaterElement.GetInt32();
                        string row = RowElement.GetString();
                        int seat = SeatNumElement.GetInt32();
                        
                        tickets.Add(new Ticket(price, date, theaterNumber, row, seat));
                    }
                }
            }

            tickets[0].Date = "DATE HAS BEEN MODIFIED";

            jsonText = JsonSerializer.Serialize(tickets, options);
            File.WriteAllText("data.json", jsonText);

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
