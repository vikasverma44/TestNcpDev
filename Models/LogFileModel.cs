using System.Text;

namespace SQLDataMaskingConfigurator.Models
{
    class LogFileModel
    {
        public LogFileModel(string filePath, bool isAppend, Encoding encoding, string textMessage)
        {
            this.FilePath = filePath;
            this.IsAppend = isAppend;
            this.EncodingType = encoding;
            this.TextString = textMessage;
        }
        public string FilePath { get; set; }
        public bool IsAppend { get; set; }
        public Encoding EncodingType { get; set; }
        public string TextString { get; set; }
    }
}
