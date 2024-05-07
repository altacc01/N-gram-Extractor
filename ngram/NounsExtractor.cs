using System.Text;

namespace ngram
{
    public class NounsExtractor
    {

        private const string file_path = "./";
        private const string textfile_extension = ".txt";
        private string output_data = "";
        private int ngram_level { get; set; } = default;
        private const int max_ngram_level = 4;
        private Dictionary<string, string> nouns_index, nouns_data;
        private List<string> ngram_seeds { get; set; } = default;

        public NounsExtractor()
        {
            ngram_level = 2; // default n-gram level
        }
        public NounsExtractor(int level)
        {
            ngram_level = level;
        }
        /*
        * Method:defaultNGramSeed
        * Purpose: sets the default seed for n-gram nouns index and data
        * Arguments: string
        * Returns: void
        */
        public void defaultNGramSeed(string file)
        {
            NounsReader nouns_reader = new NounsReader();
            // nouns index
            nouns_reader.readNouns("NounsIndex" + textfile_extension);
            nouns_index = new Dictionary<string, string>();
            nouns_index = nouns_reader.getNounsDictionary();
            // nouns data
            nouns_reader.readNouns("NounsData" + textfile_extension);
            nouns_data = new Dictionary<string, string>();
            nouns_data = nouns_reader.getNounsDictionary();

            try
            {
                const Int32 buffer_size = 128;
                var utf8_encoding = Encoding.UTF8;
                string line, seeds = "";
                string[] nouns = null;
                string[] line_data = null;
                int counter = 1;
                ngram_seeds = new List<string>();

                using var file_stream = File.OpenRead(file_path + file);
                using var stream_reader = new StreamReader(file_stream, utf8_encoding, true, buffer_size);
                while((line = stream_reader.ReadLine()) != null)
                {
                    Console.WriteLine("\n" + line);
                    this.output_data += line + "\n";
                    nouns = line.Split(new char[] { '.', '?', '!' }, StringSplitOptions.None);
                }
                foreach(var word in nouns)
                {
                    if(word != "")
                    {
                        Console.WriteLine("\n" + word.Trim());
                        this.output_data += "\n" + word.Trim() + "\n";
                        line_data = word.Trim().Split(' ');

                        while(ngram_level <= max_ngram_level)
                        {
                            for (int i = 0; i < line_data.Length; i++)
                            {
                                if (i <= line_data.Length - ngram_level)
                                {
                                    for (int j = i; j < i + ngram_level; j++)
                                    {
                                        if (counter == ngram_level)
                                        {
                                            seeds += line_data[j].Trim();
                                            ngram_seeds.Add(seeds);
                                            seeds = "";
                                            counter = 1;
                                        }
                                        else
                                        {
                                            seeds += line_data[j].Trim() + "_";
                                            counter++;
                                        }
                                    }
                                }
                            }
                            seekWordAndPrint(ngram_level, ngram_seeds, nouns_index, nouns_data);
                            ngram_seeds = new List<string>();
                            ngram_level++;

                        }
                        ngram_level = 2; // resets ngram level back to 2
                    }
                }
                // debug output
                debugOutput(this.output_data);
            }
            catch(Exception) 
            {
                throw;
            }
        }
        /*
        * Method:seekWordAndPrint
        * Purpose: seeks words from nouns index and data and prints them altogether
        * Arguments: int, List<string>, Dictionary<string, string>, Dictionary<string, string>
        * Returns: void
        */
        public void seekWordAndPrint(int ngram_level, List<string> ngram_seeds, Dictionary<string, string> nouns_index, Dictionary<string, string> nouns_data)
        {
            try
            {
                StringBuilder string_builder = new StringBuilder();
                string output = "";
                string[] output_values = null;

                foreach (var word in ngram_seeds)
                {
                    if (nouns_index.ContainsKey(word.ToLower()))
                    {
                       string[] index_values = nouns_index[word.ToLower()].Split(',');
                        foreach(var index in index_values)
                        {
                            if (nouns_data.ContainsKey(index.Trim()))
                            {
                                string noun_data_values = nouns_data[index];
                                if (noun_data_values.Contains(';'))
                                {
                                    output_values = noun_data_values.Split(';');
                                    for (int i = 0; i < output_values.Length; i++)
                                    {
                                        if(i != output_values.Length - 1)
                                        {
                                            output += output_values[i] + "<< and >> ";
                                        }
                                        else
                                        {
                                            output += output_values[i];
                                        }
                                    }
                                }
                                else
                                {
                                    output += noun_data_values;
                                }
                            }
                        }
                    }
                    string_builder.Append(String.Format("{0}{1}\n", word + ", ", output));
                    output = "";
                }
                Console.WriteLine("\n" + ngram_level + " level N-gram\n");
                Console.WriteLine(string_builder);
                this.output_data += "\n" + ngram_level + " level N-gram\n";
                this.output_data += "\n" + string_builder + "\n";

            }
            catch (Exception)
            {
                throw;
            }
        }
        /*
        * Method:debugOutput
        * Purpose: creates a debug text file
        * Arguments: string
        * Returns: void
        */
        private static void debugOutput(string output_data)
        {
            try
            {
                if (File.Exists("debug.txt"))
                {
                    File.Delete("debug.txt");
                }
                using(FileStream file_stream = File.Create("debug.txt"))
                {
                    // write into text file
                    byte[] info = new UTF8Encoding(true).GetBytes(output_data);
                    file_stream.Write(info, 0, info.Length);
                    Console.WriteLine("'debug.txt' text file has been created!");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: 'debug.txt' file creation failed");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
