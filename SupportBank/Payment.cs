using System;

namespace SupportBank
{
    class Payment
    {
        public string Date;
        public string From;
        public string To;
        public string Narrative;
        public decimal Amount;

        public Payment(string date, string from, string to, string narrative, decimal amount)
        {
            this.Date = date;
            this.From = from;
            this.To = to;
            this.Narrative = narrative;
            this.Amount = amount;
        }

        public void PrintToConsole()
        {
            Console.WriteLine($"On {Date}, {From} paid {To} £{Amount.ToString()}, with a narrative of: {Narrative}");
        }
    }
}