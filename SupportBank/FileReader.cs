using System.IO;
using System.Linq;
using System.Xml;
using NLog;

namespace SupportBank
{
    abstract class FileReader
    {
        protected static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public static FileReader GetFileReaderForInput(string filepath, Bank bank, BankSystemDisplay display)
        {
            if (filepath.EndsWith(".csv"))
            {
                return new CSVFileReader(filepath, bank, display);
            }
            else if (filepath.EndsWith(".json"))
            {
                return new JSONFileReader(filepath, bank, display);
            }
            else
            {
                throw new FileNotFoundException("File wasn't loaded: that file type isn't supported.");
            }
        }

        public abstract void ReadInPayments();
    }
}