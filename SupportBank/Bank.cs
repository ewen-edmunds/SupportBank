using System.Collections.Generic;

namespace SupportBank
{
    public class Bank
    {
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

        public void AddPaymentsFromCSV(IEnumerable<string> transactionLines)
        {
            List<Payment> newPayments = new List<Payment>();
            
            foreach (string line in transactionLines)
            {
                var values = line.Split(','); 
                newPayments.Add(new Payment(values[0], values[1], values[2], values[3], decimal.Parse(values[4])));
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