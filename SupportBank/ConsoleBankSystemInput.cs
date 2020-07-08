using System;
using System.Collections.Generic;
using System.IO;
using NLog.Targets;

namespace SupportBank
{
    public class ConsoleBankSystemInput : BankSystemInput
    {
        private Bank BankSystem;
        private BankSystemDisplay Display;

        public ConsoleBankSystemInput(Bank bank, BankSystemDisplay display)
        {
            this.BankSystem = bank;
            this.Display = display;
        }
        
        public override void TakeUserInputs()
        {
            var isRunning = true;
            while (isRunning)
            {
                Console.Write("\nOptions: \n: List All \n: List [Account] \n: Import File [Filepath] \n: Quit\n> ");
                
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
                else if (userInput.ToLower().StartsWith("import file "))
                {
                    try
                    {
                        string inputtedFilepath = userInput.Remove(0, 12);

                        FileReader fileReader = FileReader.GetFileReaderForInput(inputtedFilepath, Display);
                        List<Payment> successfulFilePayments = fileReader.GetPayments();
                        
                        BankSystem.UpdateAccountPayments(successfulFilePayments);
                        
                        Display.DisplaySuccessfulImport(inputtedFilepath);
                    }
                    catch (FileNotFoundException e)
                    {
                        Display.DisplayMessage(e.Message);
                    }
                    catch (FormatException e)
                    {
                        Display.DisplayMessage(e.Message);
                    }
                }
            } 
        }
    }
}