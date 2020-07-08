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
            logger.Debug("Reading in XML payments.");
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(Filepath);
            }
            catch (XmlException e)
            {
                throw new FormatException("This XML document is not formatted correctly - the file cannot be opened.");
            }

            List<Payment> allNewPayments = new List<Payment>();
            
            XmlNode node = doc.DocumentElement.SelectSingleNode("/TransactionList");

            if (node == null)
            {
                throw new FormatException("No enclosing 'TransactionList' tag - the file cannot be opened.");
            }
            
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
                    Display.DisplayMessage($"Warning: There was an error importing data from transaction {lineCounter} of this XML file: this didn't have the correct format.\nAs a result, this specific transaction has not been read in.");
                    logger.Error($"Error in adding new payment, triggered on transaction number {lineCounter} in XML file. Error message: {e.Message}");
                }
            }
            
            logger.Debug("Finished reading in XML payments.");
            return allNewPayments;
        }
    }
}