using System.Linq;

namespace SupportBank
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] allLines = System.IO.File.ReadAllLines(@"..\..\..\..\Transactions2014.csv");
            
            Bank SupportBank = new Bank();
            SupportBank.AddAccountsFromCSV(allLines.Skip(1));
            SupportBank.AddPaymentsFromCSV(allLines.Skip(1));
            
            BankSystemDisplay display = new ConsoleBankSystemDisplay(SupportBank);
            display.DisplayWelcome();
            
            BankSystemInput input = new ConsoleBankSystemInput(display);
            input.TakeUserInputs();
        }
    }
}