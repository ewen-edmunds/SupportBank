using System;
using System.Collections.Generic;

namespace SupportBank
{
    public class ConsoleBankSystemDisplay : BankSystemDisplay
    {
        private Bank BankSystem;

        public ConsoleBankSystemDisplay(Bank bank)
        {
            this.BankSystem = bank;
        }
        
        public override void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the Support Bank!");
            Console.WriteLine("============================");
        }

        private void DisplayPaymentInformation(Payment payment)
        {
            Console.WriteLine($"On {payment.Date.ToShortDateString()}, {payment.FromAccount} paid {payment.ToAccount} £{payment.Amount.ToString()}, with a narrative of: {payment.Narrative}");
        }

        private void DisplayPersonInformation(Person person)
        {
            Console.WriteLine($"\nUser: {person.Name} \nBalance: {person.Balance.ToString()}");
        }

        public override void DisplayAllInformation()
        {
            Console.WriteLine("\nDisplaying Information for all Accounts:");
            foreach (var pair in BankSystem.nameAccountDictionary)
            {
                DisplayPersonInformation(pair.Value);
            }
        }

        public override void DisplaySpecificPersonInformation(string user)
        {
            if (BankSystem.nameAccountDictionary.ContainsKey(user))
            {
                Console.WriteLine($"\nDisplaying Information for User: {user}");
                DisplayPersonInformation(BankSystem.nameAccountDictionary[user]);
                
                Console.WriteLine("\nIndividual Payments:");
                foreach (var payment in BankSystem.nameAccountDictionary[user].Payments)
                {
                    DisplayPaymentInformation(payment);
                }
            }
            else
            {
                throw new ArgumentException("No user by that name exists.");
            }
        }

        public override void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}