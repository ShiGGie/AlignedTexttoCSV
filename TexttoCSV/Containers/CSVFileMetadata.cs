using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVConverterConsole.Containers
{
    public class CSVFileMetadata : IFileContainer
    {
        public CSVFileMetadata(string fileName, int numHeaderLines, int dataFormat, int primaryKey, string[] delim)
        {
            this.filePath = fileName;
            this.numHeaderLines = numHeaderLines;
            this.dataFormat = dataFormat;
            this.primaryKey = primaryKey;
            this.delim = delim;
        }
        public string filePath{get; private set;}
        public int numHeaderLines { get; private set; }
        public int dataFormat { get; private set; }
        public int primaryKey { get; private set; }
        public string[] delim { get; private set; }
    }
}
