namespace ngram
{
    /* 
     * Project: N-gram Extractor
     * Purpose: Using a subset of the WordNet Lexical database to experiment with n-gram definitions
     * Date: 27th October, 2022
     * Coder: Haris 
     */
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Welcome to N-gram Extractor - v1.0\n");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("            List of commands            ");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("1. Enter name of text file");
                Console.WriteLine("2. Help Info");
                Console.WriteLine("3. End the program");
                Console.WriteLine("----------------------------------------\n");
                bool flag = false;

                while (!flag)
                {
                    Console.Write("\nPlease enter a command: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            Console.Write("Enter name of text file (please include '.txt' extension at the end): ");
                            string txtFile = Console.ReadLine();
                            try
                            {
                                NounsExtractor nouns_extractor = new NounsExtractor();
                                nouns_extractor.defaultNGramSeed(txtFile);
                            }
                            catch(Exception)
                            {
                                Console.WriteLine("Error: File not found, please try again");
                            }
                            break;
                        case "2":
                            Console.WriteLine("Welcome to N-gram Extractor - v1.0\n");
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine("            List of commands            ");
                            Console.WriteLine("----------------------------------------");
                            Console.WriteLine("1. Enter name of text file");
                            Console.WriteLine("2. Help Info");
                            Console.WriteLine("3. End the program");
                            Console.WriteLine("----------------------------------------\n");
                            break;
                        case "3":
                            Console.Write("Program terminated..");
                            flag = true;
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Write("Error: Please enter valid command and try again\n");
                            break;
                    }
                }
            }
            else if(args.Length == 1)
            {
                Console.WriteLine("Welcome to N-gram Extractor - v1.0\n");
                NounsExtractor nouns_extractor = new NounsExtractor();
                nouns_extractor.defaultNGramSeed(args[0]);
            }
        }
    }
}
