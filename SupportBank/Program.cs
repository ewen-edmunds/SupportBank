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
            string[] lines = System.IO.File.ReadAllLines("C:\\Users\\eweedm\\Documents\\Work\\Training\\SupportBank\\Transactions2014.csv");
            var newLines = lines.Skip(1);
            
            var people = CreateUniqueUsers(newLines);
            
            
        }
        
        static HashSet<string> CreateUniqueUsers(IEnumerable<string> lines){
            HashSet<string> people = new HashSet<string>();
            
            foreach (string line in lines)
            {
                var values = line.Split(',');
                people.Add(values[1]);
                people.Add(values[2]);
            }

            Console.WriteLine("Unique People:");
            foreach (string person in people)
            {
                Console.WriteLine(person);
            }

            return people;
        }
    }
    
    

    class Person
    {
        public string name;
        public float balance;

        public Person(string newName)
        {
            this.name = newName;
            this.balance = 0;
        }
    }

    class Payment
    {
        public string date;
        public string from;
        public string to;
        public string narrative;
        public float amount;
    }
}