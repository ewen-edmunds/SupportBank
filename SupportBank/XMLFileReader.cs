using System;
using System.Collections.Generic;
using System.Xml;

namespace SupportBank
{
    class XMLFileReader : FileReader
    {
        private string Filepath;
        private BankSystemDisplay Display;
        
        public XMLFileReader(string filepath, BankSystemDisplay display)
        {
            this.Filepath = filepath;
            this.Display = display;
        }
        
        public override List<Payment> GetPayments()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Filepath);
         
            List<Payment> allNewPayments = new List<Payment>();
            
            XmlNode node = doc.DocumentElement.SelectSingleNode("/TransactionList");
            
            var lineCounter = 0;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                lineCounter++;
                try
                {
                    Payment newPayment = new Payment(
                        DateTime.FromOADate(double.Parse(childNode.Attributes["Date"]?.InnerText)),
                        childNode["Parties"]["From"].InnerText, childNode["Parties"]["To"].InnerText,
                        childNode["Description"].InnerText, decimal.Parse(childNode["Value"].InnerText));
                    allNewPayments.Add(newPayment);
                }
                catch (Exception e)
                {
                    Display.DisplayMessage($"Warning: There was an error importing data from transaction {lineCounter} of this XML file: something didn't have the correct format.\nAs a result, this specific transaction has not been read in.");
                    logger.Error($"Error in adding new payment, triggered on transaction number {lineCounter} in XML file. Error message: {e.Message}");
                }
            }

            return allNewPayments;
        }
    }
}