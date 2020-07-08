using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SupportBank
{
    class CSVFileReader : FileReader
    {
        private string Filepath;
        private Bank BankSystem;
        private BankSystemDisplay Display;

        public CSVFileReader(string filepath, Bank bank, BankSystemDisplay display)
        {
            this.Filepath = filepath;
            this.BankSystem = bank;
            this.Display = display;
        }

        public override void ReadInPayments()
        {
            IEnumerable<string> transactionLines = System.IO.File.ReadAllLines(Filepath).Skip(1);
            
            logger.Debug("Adding CSV payments to bank.");
            List<Payment> newPayments = new List<Payment>();

            var lineCounter = 0;
            foreach (string line in transactionLines)
            {
                lineCounter++;
                var values = line.Split(',');
                if (values.Length != 5)
                {
                    logger.Error($"Error in adding new payment, triggered on CSV line {lineCounter}. Not the correct number of values.");
                    logger.Debug($"Failed payment had {values.Length} values, but needs exactly 5 values.");
                    Display.DisplayMessage($"Warning: There was an error importing data on line {lineCounter} of this CSV file: there should be exactly 5 entries per line; this line had {values.Length}.\nAs a result, this specific transaction has not been logged.");
                }
                else
                {
                    try
                    {
                        newPayments.Add(new Payment(DateTime.Parse(values[0]), values[1], values[2], values[3], decimal.Parse(values[4])));
                    }
                    catch (Exception e)
                    {
                        logger.Error($"Error in adding new payment, triggered on CSV line {lineCounter}. Error message: {e.Message}");
                        logger.Debug($"Failed payment had values: DATE {values[0]} FROM {values[1]} TO {values[2]} NARRATIVE {values[3]} AMOUNT {values[4]} ");
                        Display.DisplayMessage($"Warning: There was an error importing data on line {lineCounter} of this CSV file: something didn't have the correct format.\nAs a result, this specific transaction has not been logged.");
                    }
                }
            }
            
            BankSystem.UpdateAccountPayments(newPayments);
        }
    }
}