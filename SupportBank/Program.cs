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
            Console.WriteLine("============================\n");
            
            //todo: Make this work for any user, not just me
            string[] allLines = System.IO.File.ReadAllLines("C:\\Users\\eweedm\\Documents\\Work\\Training\\SupportBank\\Transactions2014.csv");
            var transactionLines = allLines.Skip(1);
            
            var accountDictionary = CreateUniqueUsers(transactionLines);

            List<Payment> payments = ConvertFromCSV(transactionLines);
            
            UpdateTransactions(accountDictionary, payments);

            TakeUserInputsForAccounts(accountDictionary);
        }
        
        static Dictionary<string, Person> CreateUniqueUsers(IEnumerable<string> lines){
            HashSet<string> uniqueNames = new HashSet<string>();
            Dictionary<string, Person> nameAccountDict = new Dictionary<string, Person>();
            
            foreach (string line in lines)
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

        static List<Payment> ConvertFromCSV(IEnumerable<string> transactionLines)
        {
            List<Payment> payments = new List<Payment>();
            foreach (string line in transactionLines)
            {
                var values = line.Split(',');
                payments.Add(new Payment(values[0], values[1], values[2], values[3], decimal.Parse(values[4])));
            }

            return payments;
        }

        static void UpdateTransactions(Dictionary<string, Person> nameAccountDict, IEnumerable<Payment> payments)
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
                Console.WriteLine("\nPlease enter 'List All', or 'List [Account]'");
                Console.WriteLine("============================================\n>");
                var userInput = Console.ReadLine();
                if (userInput == "" || userInput.ToLower() == "quit" || userInput.ToLower() == "q")
                {
                    isRunning = false;
                }
                else if (userInput == "List All")
                {
                    DisplayAllInformation(nameAccountDict); 
                }
                else if (userInput.StartsWith("List "))
                {
                    try
                    {
                        DisplaySpecificUserInformation(userInput.Remove(0, 5), nameAccountDict);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            } while (isRunning);
        }

        static void DisplayAllInformation(Dictionary<string, Person> nameAccountDict)
        {
            Console.WriteLine("\nDisplaying All Accounts:");
            Console.WriteLine("========================");
            foreach (var pair in nameAccountDict)
            {
                pair.Value.PrintToConsole();
            }
        }

        static void DisplaySpecificUserInformation(string user, Dictionary<string, Person> nameAccountDict)
        {
            if (nameAccountDict.ContainsKey(user))
            {
                Console.WriteLine($"\nDisplaying All Information for user: {user}");
                nameAccountDict[user].PrintToConsole();
                
                Console.WriteLine("\nIndividual Records:");
                foreach (var payment in nameAccountDict[user].Payments)
                {
                    payment.PrintToConsole();
                }
            }
            else
            {
                throw new Exception("No user by that name exists.");
            }
        }
    }
}