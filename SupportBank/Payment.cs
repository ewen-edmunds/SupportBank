using System;

namespace SupportBank
{
    public class Payment
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
    }
}