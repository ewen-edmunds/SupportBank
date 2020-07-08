using System.Collections.Generic;
using System.Linq;
using NLog;

namespace SupportBank
{
    public class Bank
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        public readonly Dictionary<string, Person> nameAccountDictionary = new Dictionary<string, Person>();
        public List<Payment> payments = new List<Payment>();
        
        public void UpdateAccountPayments(IEnumerable<Payment> newPayments)
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

            payments.Concat(newPayments);
        }
    }
}