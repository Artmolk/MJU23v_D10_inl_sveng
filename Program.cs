using System;
using System.Collections.Generic;
using System.IO;

namespace MJU23v_D10_inl_sveng
{
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

        static void LoadDictionary(string fileName)
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

        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            do
            {
                Console.Write("> ");
                string[] arguments = Console.ReadLine().Split();
                string command = arguments[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                else if (command == "hjälp")
                {
                    Console.WriteLine("Tillgängliga kommandon:");
                    Console.WriteLine("Ladda [filnamn]");
                    Console.WriteLine("Lista - Visar hela ordlistan.");
                    Console.WriteLine("Ny [svenskt ord] [engelskt ord]");
                    Console.WriteLine("Radera [svenskt ord] [engelskt ord]");
                    Console.WriteLine("Översätt [ord]");
                    Console.WriteLine("hjälp");
                    Console.WriteLine("Avsluta");
                }
                else if (command == "load")
                {
                    string fileName = arguments.Length == 2 ? arguments[1] : defaultFile;
                    LoadDictionary(fileName);
                }
                else if (command == "list")
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.SwedishWord,-10}  - {gloss.EnglishWord,-10}");
                    }
                }
                else if (command == "new")
                {
                    string swedishWord, englishWord;
                    if (arguments.Length == 3)
                    {
                        swedishWord = arguments[1];
                        englishWord = arguments[2];
                    }
                    else  //  1: (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        swedishWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        englishWord = Console.ReadLine();
                    }
                    dictionary.Add(new SweEngGloss(swedishWord, englishWord));
                }
                else if (command == "delete")
                {
                    string swedishWord, englishWord;
                    if (arguments.Length == 3)
                    {
                        swedishWord = arguments[1];
                        englishWord = arguments[2];
                    }
                    else // 2. (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        swedishWord = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        englishWord = Console.ReadLine();
                    }
                    int index = FindGlossIndex(swedishWord, englishWord);
                    if (index >= 0)
                    {
                        dictionary.RemoveAt(index);
                    }
                }
                else if (command == "translate")
                {
                    string wordToTranslate;
                    if (arguments.Length == 2)
                    {
                        wordToTranslate = arguments[1];
                    }
                    else
                    {
                        Console.WriteLine("Write word to be translated: ");
                        wordToTranslate = Console.ReadLine();
                    }
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        if (gloss.SwedishWord == wordToTranslate)
                            Console.WriteLine($"English for {gloss.SwedishWord} is {gloss.EnglishWord}");
                        if (gloss.EnglishWord == wordToTranslate)
                            Console.WriteLine($"Swedish for {gloss.EnglishWord} is {gloss.SwedishWord}");
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            } while (true);
        }
    }
}

