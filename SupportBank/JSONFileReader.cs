using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SupportBank
{
    class JSONFileReader : FileReader
    {
        private string Filepath;
        private BankSystemDisplay Display;
        
        public JSONFileReader(string filepath, BankSystemDisplay display)
        {
            this.Filepath = filepath;
            this.Display = display;
        }

        public override List<Payment> GetPayments()
        {
            logger.Debug("Reading in JSON payments.");
            
            var JSONLines = (System.IO.File.ReadAllText(Filepath));
            List<Payment> correctFormatPayments = new List<Payment>();

            var allNewPayments = JsonConvert.DeserializeObject<List<Payment>>(JSONLines);

            var lineCounter = 0;
            foreach (var payment in allNewPayments)
            {
                lineCounter++;
                if (payment.FromAccount == null || payment.ToAccount == null || payment.Narrative == null || payment.Date == null)
                {
                    Display.DisplayMessage($"Warning: There was an error importing data from object {lineCounter} of this JSON file: this didn't have the correct format.\nAs a result, this specific transaction has not been read in.");
                }
                else
                {
                    correctFormatPayments.Add(payment);
                }
            }

            logger.Debug("Finished reading in JSON payments.");
            return correctFormatPayments;
        }
    }
}