using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Support Bank!");
            Console.WriteLine("============================");
            
            string[] allLines = System.IO.File.ReadAllLines(@"..\..\..\..\Transactions2014.csv");
            var transactionLines = allLines.Skip(1);
            
            Dictionary<string, Person> accountDictionary = CreateAccountDictionaryFromCSV(transactionLines);

            List<Payment> payments = CreatePaymentsFromCSV(transactionLines);
            
            UpdateAccountPayments(accountDictionary, payments);

            TakeUserInputsForAccounts(accountDictionary);
        }
        
        static Dictionary<string, Person> CreateAccountDictionaryFromCSV(IEnumerable<string> CSVLines){
            HashSet<string> uniqueNames = new HashSet<string>();
            Dictionary<string, Person> nameAccountDict = new Dictionary<string, Person>();
            
            foreach (string line in CSVLines)
            {
                var values = line.Split(',');
                uniqueNames.Add(values[1]);
                uniqueNames.Add(values[2]);
            }
            
            foreach (string person in uniqueNames)
            {
                nameAccountDict.Add(person, new Person(person));
            }

            return nameAccountDict;
        }

        static List<Payment> CreatePaymentsFromCSV(IEnumerable<string> transactionLines)
        {
            List<Payment> payments = new List<Payment>();
            foreach (string line in transactionLines)
            {
                var values = line.Split(',');
                payments.Add(new Payment(values[0], values[1], values[2], values[3], decimal.Parse(values[4])));
            }

            return payments;
        }

        static void UpdateAccountPayments(Dictionary<string, Person> nameAccountDict, IEnumerable<Payment> payments)
        {
            foreach (Payment payment in payments)
            {
                nameAccountDict[payment.From].AddPayment(payment);
                nameAccountDict[payment.To].AddPayment(payment);
            }
        }

        static void TakeUserInputsForAccounts(Dictionary<string, Person> nameAccountDict)
        {
            var isRunning = true;
            do
            {
                Console.WriteLine("\nPlease enter 'List All', 'List [Account]', or 'Quit'\n>");
                var userInput = Console.ReadLine();
                if (userInput == "" || userInput.ToLower() == "quit" || userInput.ToLower() == "q")
                {
                    isRunning = false;
                }
                else if (userInput.ToLower() == "list all")
                {
                    DisplayAllInformation(nameAccountDict); 
                }
                else if (userInput.ToLower().StartsWith("list "))
                {
                    try
                    {
                        string inputtedName = userInput.Remove(0, 5);
                        DisplaySpecificUserInformation(inputtedName, nameAccountDict);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            } while (isRunning);
        }

        static void DisplayAllInformation(Dictionary<string, Person> nameAccountDict)
        {
            Console.WriteLine("\nDisplaying Information for all Accounts:");
            foreach (var pair in nameAccountDict)
            {
                pair.Value.PrintToConsole();
            }
        }

        static void DisplaySpecificUserInformation(string user, Dictionary<string, Person> nameAccountDict)
        {
            if (nameAccountDict.ContainsKey(user))
            {
                Console.WriteLine($"\nDisplaying Information for User: {user}");
                nameAccountDict[user].PrintToConsole();
                
                Console.WriteLine("\nIndividual Payments:");
                foreach (var payment in nameAccountDict[user].Payments)
                {
                    payment.PrintToConsole();
                }
            }
            else
            {
                throw new ArgumentException("No user by that name exists.");
            }
        }
    }
}