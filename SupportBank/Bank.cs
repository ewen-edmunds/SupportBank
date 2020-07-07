using System;
using System.Collections.Generic;
using NLog;

namespace SupportBank
{
    public class Bank
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public readonly Dictionary<string, Person> nameAccountDictionary = new Dictionary<string, Person>();
        public List<Payment> payments = new List<Payment>();
        
        public void AddAccountsFromCSV(IEnumerable<string> CSVLines){
            HashSet<string> uniqueNames = new HashSet<string>();

            foreach (string line in CSVLines)
            {
                var values = line.Split(',');
                uniqueNames.Add(values[1]);
                uniqueNames.Add(values[2]);
            }
            
            foreach (string person in uniqueNames)
            {
                if (!nameAccountDictionary.ContainsKey(person))
                {
                    nameAccountDictionary.Add(person, new Person(person));
                }
            }
        }

        public void AddPaymentsFromCSV(IEnumerable<string> transactionLines, BankSystemDisplay display)
        {
            List<Payment> newPayments = new List<Payment>();

            var lineCounter = 0;
            foreach (string line in transactionLines)
            {
                lineCounter++;
                var values = line.Split(',');
                try
                {
                    newPayments.Add(new Payment(DateTime.Parse(values[0]), values[1], values[2], values[3], decimal.Parse(values[4])));
                }
                catch (Exception e)
                {
                    logger.Error($"Error in adding new payment, triggered on CSV line {lineCounter}. Error message: {e.Message}");
                    logger.Debug($"Failed payment had values: DATE {values[0]} FROM {values[1]} TO {values[2]} NARRATIVE {values[3]} AMOUNT {values[4]} ");
                    display.DisplayMessage($"Warning: There was an error importing data on line {lineCounter} of this CSV file.\nAs a result, this specific transaction has not been logged.");
                }
            }
            UpdateAccountPayments(newPayments);
        }

        private void UpdateAccountPayments(IEnumerable<Payment> payments)
        {
            foreach (Payment payment in payments)
            {
                nameAccountDictionary[payment.From].AddPayment(payment);
                nameAccountDictionary[payment.To].AddPayment(payment);
            }
        }
    }
}