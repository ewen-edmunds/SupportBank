using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json;
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

            Bank SupportBank = new Bank();
            BankSystemDisplay display = new ConsoleBankSystemDisplay(SupportBank);
            BankSystemInput input = new ConsoleBankSystemInput(SupportBank, display);
            
            display.DisplayWelcome();
            
            logger.Info("Finished initialising Bank/Display/Input, now inputting data.");

            SupportBank.InputDataFrom(@"..\..\..\..\Transactions2013.json", display);
            SupportBank.InputDataFrom(@"..\..\..\..\Transactions2014.csv", display);
            SupportBank.InputDataFrom(@"..\..\..\..\DodgyTransactions2015.csv", display);
            logger.Info("Finished inputting data, now taking user inputs.");

            input.TakeUserInputs();
        }
    }
}