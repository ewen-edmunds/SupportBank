using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SupportBank
{
    class JSONFileReader : FileReader
    {
        private string Filepath;
        private Bank BankSystem;
        private BankSystemDisplay Display;
        
        public JSONFileReader(string filepath, Bank bank, BankSystemDisplay display)
        {
            this.Filepath = filepath;
            this.BankSystem = bank;
            this.Display = display;
        }
        public override void ReadInPayments()
        {
            var JSONLines = (System.IO.File.ReadAllText(Filepath));
            List<Payment> correctFormatPayments = new List<Payment>();
            
            logger.Debug("Adding JSON payments to bank.");
            var allNewPayments = JsonConvert.DeserializeObject<List<Payment>>(JSONLines);

            var lineCounter = 0;
            foreach (var payment in allNewPayments)
            {
                lineCounter++;
                if (payment.FromAccount == null || payment.ToAccount == null || payment.Narrative == null || payment.Date == null)
                {
                    Display.DisplayMessage($"Warning: There was an error importing data from object {lineCounter} of this JSON file: something didn't have the correct format.\nAs a result, this specific transaction has not been logged.");
                }
                else
                {
                    correctFormatPayments.Add(payment);
                }
            }
            
            BankSystem.UpdateAccountPayments(correctFormatPayments);
        }
    }
}