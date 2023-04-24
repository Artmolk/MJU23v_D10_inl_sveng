using System;
using System.Collections.Generic;
using System.IO;

namespace MJU23v_D10_inl_sveng
{
    public static class ErrorMessage
    {
        public const string FileNotFound = "Filen kunde inte hittas.";
        public const string InvalidArguments = "Ogiltiga argument angivna.";
        // Fler felmeddelande kan läggas här
    }
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string SwedishWord, EnglishWord;
            public SweEngGloss(string swedishWord, string englishWord)
            {
                SwedishWord = swedishWord;
                EnglishWord = englishWord;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                SwedishWord = words[0];
                EnglishWord = words[1];
            }
        }

        static string defaultFile = "default.txt";
        static void LoadDictionary(string fileName)
        {
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    dictionary = new List<SweEngGloss>();
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        SweEngGloss gloss = new SweEngGloss(line);
                        dictionary.Add(gloss);
                        line = reader.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // Använder det centraliserade felmeddelandet för filer som inte hittas
                Console.WriteLine(ErrorMessage.FileNotFound);
            }
        }

        static int FindGlossIndex(string swedishWord, string englishWord)
        {
            for (int i = 0; i < dictionary.Count; i++)
            {
                SweEngGloss gloss = dictionary[i];
                if (gloss.SwedishWord == swedishWord && gloss.EnglishWord == englishWord)
                    return i;
            }
            return -1;
        }

        static void AddGloss(string swedishWord, string englishWord)
        {
            int index = FindGlossIndex(swedishWord, englishWord);
            if (index == -1)
            {
                SweEngGloss gloss = new SweEngGloss(swedishWord, englishWord);
                dictionary.Add(gloss);
                Console.WriteLine($"Lagt till: {swedishWord} - {englishWord}");
            }
            else
            {
                Console.WriteLine("Ordparet finns redan i ordlistan.");
            }
        }

        static bool Run(TextReader input, TextWriter output, string defaultFile)
        {
            output.WriteLine("Welcome");

            do
            {
                output.Write("> ");
                string[] arguments = input.ReadLine().Split();
                string command = arguments[0];
                if (command == "quit")
                {
                    output.WriteLine("Hejdå!");
                    return true;
                }
                else if (command == "hjälp")
                {
                    output.WriteLine("Tillgängliga kommandon:");
                    output.WriteLine("Ladda [filnamn]");
                    output.WriteLine("Lista - Visar hela ordlistan.");
                    output.WriteLine("Ny [svenskt ord] [engelskt ord]");
                    output.WriteLine("Radera [svenskt ord] [engelskt ord]");
                    output.WriteLine("Översätt [ord]");
                    output.WriteLine("hjälp");
                    output.WriteLine("Avsluta");
                }
                else if (command == "load")
                {
                    string fileName = arguments.Length == 2 ? arguments[1] : defaultFile;
                    LoadDictionary(@"C:\Users\A.T\Desktop\Mölk\Sveng\MJU23v_D10_inl_sveng\dict\computing.lis");

                }
                else if (command == "list")
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        output.WriteLine($"{gloss.SwedishWord,-10} - {gloss.EnglishWord,-10}");
                    }
                }
                else if (command == "delete")
                {
                    if (arguments.Length == 3)
                    {
                        string swedishWord = arguments[1];
                        string englishWord = arguments[2];
                        int index = FindGlossIndex(swedishWord, englishWord);
                        if (index != -1)
                        {
                            dictionary.RemoveAt(index);

                            output.WriteLine($"Raderat: {swedishWord} - {englishWord}");
                        }
                        else
                        {
                            output.WriteLine("Ordparet kunde inte hittas.");
                        }
                    }
                    else
                    {
                        output.WriteLine("För att radera, ange både svenskt och engelskt ord: Radera [svenskt ord] [engelskt ord]");
                    }
                }
                else if (command == "new")
                {
                    if (arguments.Length == 3)
                    {
                        string swedishWord = arguments[1];
                        string englishWord = arguments[2];
                        AddGloss(swedishWord, englishWord);
                    }
                    else
                    {
                        output.WriteLine("För att lägga till, ange både svenskt och engelskt ord: Ny [svenskt ord] [engelskt ord]");
                    }
                }
                else
                {
                    output.WriteLine($"Okänt kommando: '{command}'");
                }
            } while (true);
        }

        static void Main(string[] args)
        {
            string defaultFile = "computing.lis";
            dictionary = new List<SweEngGloss>(); // dictionary here
            RunTests(defaultFile);

            while (!Run(Console.In, Console.Out, defaultFile))
            {
            }
        }
        static void RunTests(string defaultFile)
        {
            // Test 1
            using (var input = new StringReader("list\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 1: {result}");

                if (result.Contains("Welcome") && !result.Contains("-"))
                {
                    Console.WriteLine("Test 1: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 1: MISSLYCKAD");
                }
            }
            // Test 2
            using (var input = new StringReader("load\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 2: {result}");

                if (result.Contains("Welcome") && result.Contains("-"))
                {
                    Console.WriteLine("Test 2: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 2: MISSLYCKAD");
                }
            }

            // Test 3
            using (var input = new StringReader("load computing.lis\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 3: {result}");

                if (result.Contains("Welcome") && result.Contains("-"))
                {
                    Console.WriteLine("Test 3: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 3: MISSLYCKAD");
                }
            }

            // Test 4 - Misslyckad
            using (var input = new StringReader("load finns.ej\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 4: {result}");

                if (result.Contains("Welcome") && result.Contains("Kunde inte hitta filen"))
                {
                    Console.WriteLine("Test 4: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 4: MISSLYCKAD");
                }
            }

            // Test 5 - misslyckad
            using (var input = new StringReader("new\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 5: {result}");

                if (result.Contains("Welcome") && !result.Contains("-"))
                {
                    Console.WriteLine("Test 5: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 5: MISSLYCKAD");
                }
            }

            // Test 6
            using (var input = new StringReader("load\nlist\nnew\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 6: {result}");

                if (result.Contains("Welcome") && result.Contains("-"))
                {
                    Console.WriteLine("Test 6: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 6: MISSLYCKAD");
                }
            }

            /// Test 7 - misslyckad
            using (var input = new StringReader("load\nlist\nnew sol sun\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 7: {result}");

                if (result.Contains("Welcome") && result.Contains("sol") && result.Contains("sun"))
                {

                    Console.WriteLine("Test 7: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 7: MISSLYCKAD");
                }
            }

            // Test 8
            using (var input = new StringReader("load\nlist\nnew sol\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 8: {result}");

                if (result.Contains("Welcome") && !result.Contains("sol"))
                {
                    Console.WriteLine("Test 8: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 8: MISSLYCKAD"); //Även test 4-8 tillagd, glömde committa
                }
            }
            // Test 9 - misslyckad
            using (var input = new StringReader("delete\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 9: {result}");

                if (result.Contains("Welcome") && result.Contains("Fel antal argument"))
                {
                    Console.WriteLine("Test 9: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 9: MISSLYCKAD");
                }
            }

            // Test 10 - misslyckad
            using (var input = new StringReader("load\nlist\ndelete\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 10: {result}");

                if (result.Contains("Welcome") && result.Contains("Fel antal argument"))
                {
                    Console.WriteLine("Test 10: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 10: MISSLYCKAD");
                }
            }

            // Test 11 - > Ordparet kunde inte hittas.
            using (var input = new StringReader("load\nlist\ndelete ost\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 11: {result}");

                if (result.Contains("Welcome") && result.Contains("Fel antal argument"))
                {
                    Console.WriteLine("Test 11: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 11: MISSLYCKAD");
                }
            }

            // Test 12
            using (var input = new StringReader("load\nlist\ndelete ost cheese\nlist\nquit\n"))
            using (var output = new StringWriter())
            {
                Run(input, output, defaultFile);
                string result = output.ToString();
                Console.WriteLine($"Resultat för test 12: {result}");

                if (result.Contains("Welcome") && !result.Contains("ost") && !result.Contains("cheese"))
                {
                    Console.WriteLine("Test 12: GODKÄND");
                }
                else
                {
                    Console.WriteLine("Test 12: MISSLYCKAD");
                }
            }
        }
    }
}
