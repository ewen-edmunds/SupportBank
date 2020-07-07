using System;

namespace SupportBank
{
    public class Payment
    {
        public DateTime Date;
        public string FromAccount;
        public string ToAccount;
        public string Narrative;
        public decimal Amount;

        public Payment(DateTime date, string fromAccount, string toAccount, string narrative, decimal amount)
        {
            this.Date = date;
            this.FromAccount = fromAccount;
            this.ToAccount = toAccount;
            this.Narrative = narrative;
            this.Amount = amount;
        }
    }
}