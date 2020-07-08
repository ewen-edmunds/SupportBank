using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using NLog;

namespace SupportBank
{
    public class Bank
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public readonly Dictionary<string, Person> nameAccountDictionary = new Dictionary<string, Person>();
        public List<Payment> payments = new List<Payment>();

        public void InputDataFrom(string Filepath, BankSystemDisplay display)
        {
            if (Filepath.EndsWith(".csv"))
            {
                AddPaymentsFromCSV(System.IO.File.ReadAllLines(Filepath).Skip(1), display);
            }
            else if (Filepath.EndsWith(".json"))
            {
                AddPaymentsFromJSON(System.IO.File.ReadAllText(Filepath));
            }
            else
            {
                throw new FileNotFoundException("File wasn't loaded: that file isn't supported.");
            }
        }

        public void AddPaymentsFromJSON(string JSONLines)
        {
            logger.Debug("Adding JSON payments to bank.");
            var newPayments = JsonConvert.DeserializeObject<List<Payment>>(JSONLines);

            foreach (var payment in newPayments)
            {
                if (payment.FromAccount == null || payment.ToAccount == null || payment.Narrative == null || payment.Date == null || payment.Amount == null)
                {
                    throw new FormatException("One of the JSON records was not in the correct format. Import cancelled.");
                }
            }
            
            payments.Concat(newPayments);
            UpdateAccountPayments(newPayments);
        }

        public void AddPaymentsFromCSV(IEnumerable<string> transactionLines, BankSystemDisplay display)
        {
            logger.Debug("Adding CSV payments to bank.");
            List<Payment> newPayments = new List<Payment>();

            var lineCounter = 0;
            foreach (string line in transactionLines)
            {
                lineCounter++;
                var values = line.Split(',');
                if (values.Length != 5)
                {
                    logger.Error($"Error in adding new payment, triggered on CSV line {lineCounter}. Not the correct number of values.");
                    logger.Debug($"Failed payment had {values.Length} values, but needs exactly 5 values.");
                    display.DisplayMessage($"Warning: There was an error importing data on line {lineCounter} of this CSV file: there should be exactly 5 entries per line; this line had {values.Length}.\nAs a result, this specific transaction has not been logged.");
                }
                else
                {
                    try
                    {
                        newPayments.Add(new Payment(DateTime.Parse(values[0]), values[1], values[2], values[3], decimal.Parse(values[4])));
                    }
                    catch (Exception e)
                    {
                        logger.Error($"Error in adding new payment, triggered on CSV line {lineCounter}. Error message: {e.Message}");
                        logger.Debug($"Failed payment had values: DATE {values[0]} FROM {values[1]} TO {values[2]} NARRATIVE {values[3]} AMOUNT {values[4]} ");
                        display.DisplayMessage($"Warning: There was an error importing data on line {lineCounter} of this CSV file: something didn't have the correct format.\nAs a result, this specific transaction has not been logged.");
                    }
                }
            }

            payments.Concat(newPayments);
            UpdateAccountPayments(newPayments);
        }

        private void UpdateAccountPayments(IEnumerable<Payment> newPayments)
        {
            logger.Debug("Updating Bank Accounts");
            HashSet<string> uniqueNames = new HashSet<string>();

            foreach (Payment payment in newPayments)
            {
                uniqueNames.Add(payment.FromAccount);
                uniqueNames.Add(payment.ToAccount);
            }
            foreach (string person in uniqueNames)
            {
                if (!nameAccountDictionary.ContainsKey(person))
                {
                    nameAccountDictionary.Add(person, new Person(person));
                }
            }
            
            logger.Debug("Adding payments to the accounts in the bank.");
            foreach (Payment payment in newPayments)
            {
                nameAccountDictionary[payment.FromAccount].AddPayment(payment);
                nameAccountDictionary[payment.ToAccount].AddPayment(payment);
            }
        }
    }
}