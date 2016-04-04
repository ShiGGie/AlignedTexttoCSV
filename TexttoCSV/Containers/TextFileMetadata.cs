using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVConverterConsole.Containers
{
    public class TextFileMetadata : IFileContainer
    {
        public TextFileMetadata(string fileName)
        {
            this.filePath = fileName;
        }
        public string filePath { get; private set; }
    }
}
