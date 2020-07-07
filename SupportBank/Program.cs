using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SupportBank
{
    class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget { FileName = @"C:\Work\Logs\SupportBank.log", Layout = @"${longdate} ${level} - ${logger}: ${message}" };
            config.AddTarget("File Logger", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));
            LogManager.Configuration = config;
            
            logger.Info("The program has just started up.");

            string[] allLinesFrom2014 = System.IO.File.ReadAllLines(@"..\..\..\..\Transactions2014.csv");
            string[] allLinesFrom2015 = System.IO.File.ReadAllLines(@"..\..\..\..\DodgyTransactions2015.csv");
            
            Bank SupportBank = new Bank();
            BankSystemDisplay display = new ConsoleBankSystemDisplay(SupportBank);
            BankSystemInput input = new ConsoleBankSystemInput(display);
            
            display.DisplayWelcome();
            
            logger.Info("Finished initialising Bank/Display/Input, now inputting data.");
            
            SupportBank.AddAccountsFromCSV(allLinesFrom2014.Skip(1));
            SupportBank.AddAccountsFromCSV(allLinesFrom2015.Skip(1));
            SupportBank.AddPaymentsFromCSV(allLinesFrom2014.Skip(1), display);
            SupportBank.AddPaymentsFromCSV(allLinesFrom2015.Skip(1), display);
            
            logger.Info("Finished inputting data, now taking user inputs.");
            
            input.TakeUserInputs();
        }
    }
}