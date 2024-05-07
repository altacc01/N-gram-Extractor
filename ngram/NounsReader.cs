using System.Text;

namespace ngram
{
    public class NounsReader
    {
        private const string file_path = "./NounsText/";
        private Dictionary<string, string> nouns_dictionary;
        public NounsReader()
        {
            nouns_dictionary = new Dictionary<string, string>();
        }
        /*
        * Method:readNouns
        * Purpose: reads the nouns data and index from text files and adds them into dictionary
        * Arguments: string 
        * Returns: void
        */
        public void readNouns(string file)
        {
            try
            {
                const int buffer_size = 128;
                var utf8_encoding = Encoding.UTF8; 

                using(var file_stream = File.OpenRead(file_path + file))
                using(var stream_reader = new StreamReader(file_stream, utf8_encoding, true, buffer_size))
                {
                    string line;
                    string[] nouns;
                    
                    while((line = stream_reader.ReadLine()) != null)
                    {
                        nouns = line.Split('|');
                        nouns_dictionary.Add(nouns[0], nouns[1]);
                    }
                }
            }catch(Exception)
            {
                throw;
            }
        }
        /*
        * Method:getNounsDictionary
        * Purpose: getter method for dictionary
        * Arguments: none
        * Returns: Dictionary<string, string>
        */
        public Dictionary<string, string> getNounsDictionary()
        {
            return this.nouns_dictionary;
        }
    }
}
