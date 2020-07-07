using System;

namespace SupportBank
{
    public class ConsoleBankSystemInput : BankSystemInput
    {
        private BankSystemDisplay Display;

        public ConsoleBankSystemInput(BankSystemDisplay display)
        {
            this.Display = display;
        }
        
        public override void TakeUserInputs()
        {
            var isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\nPlease enter 'List All', 'List [Account]', or 'Quit'\n>");
                
                var userInput = Console.ReadLine();
                if (userInput == "" || userInput.ToLower() == "quit" || userInput.ToLower() == "q")
                {
                    isRunning = false;
                }
                else if (userInput.ToLower() == "list all")
                {
                    Display.DisplayAllInformation(); 
                }
                else if (userInput.ToLower().StartsWith("list "))
                {
                    try
                    {
                        string inputtedName = userInput.Remove(0, 5);
                        Display.DisplaySpecificPersonInformation(inputtedName);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            } 
        }
    }
}