using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NLog;

namespace SupportBank
{
    abstract class FileReader
    {
        protected static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static FileReader GetFileReaderForInput(string filepath, BankSystemDisplay display)
        {
            if (filepath.EndsWith(".csv"))
            {
                return new CSVFileReader(filepath, display);
            }
            else if (filepath.EndsWith(".json"))
            {
                return new JSONFileReader(filepath, display);
            }
            else if (filepath.EndsWith(".xml"))
            {
                return new JSONFileReader(filepath, display);
            }
            else
            {
                throw new FileNotFoundException("File wasn't loaded: that file type isn't supported.");
            }
        }

        public abstract List<Payment> GetPayments();
    }

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
            
            return new List<Payment>();
        }
    }
}