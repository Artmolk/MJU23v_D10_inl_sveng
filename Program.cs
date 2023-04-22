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
                    Console.WriteLine("load [filnamn]");
                    Console.WriteLine("list - Visar hela ordlistan.");
                    Console.WriteLine("new [svenskt ord] [engelskt ord]");
                    Console.WriteLine("delete [svenskt ord] [engelskt ord]");
                    Console.WriteLine("translate [ord]");
                    Console.WriteLine("hjälp");
                    Console.WriteLine("quit");
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
                    if (arguments.Length == 3)
                    {
                        dictionary.Add(new SweEngGloss(arguments[1], arguments[2]));
                    }
                    else if (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");
                        string s = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string e = Console.ReadLine();
                        dictionary.Add(new SweEngGloss(s, e));
                    }
                }
                else if (command == "delete")
                {
                    if (arguments.Length == 3)
                    {
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.SwedishWord == arguments[1] && gloss.EnglishWord == arguments[2])
                                index = i;
                        }
                        if (index >= 0)
                        {
                            dictionary.RemoveAt(index);
                        }
                    }
                    else if (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word in Swedish: ");

                        string s = Console.ReadLine();
                        Console.Write("Write word in English: ");
                        string e = Console.ReadLine();
                        int index = -1;
                        for (int i = 0; i < dictionary.Count; i++)
                        {
                            SweEngGloss gloss = dictionary[i];
                            if (gloss.SwedishWord == s && gloss.EnglishWord == e)
                                index = i;
                        }
                        if (index >= 0)
                        {
                            dictionary.RemoveAt(index);
                        }
                    }
                }
                else if (command == "translate")
                {
                    if (arguments.Length == 2)
                    {
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            if (gloss.SwedishWord == arguments[1])
                                Console.WriteLine($"English for {gloss.SwedishWord} is {gloss.EnglishWord}");
                            if (gloss.EnglishWord == arguments[1])
                                Console.WriteLine($"Swedish for {gloss.EnglishWord} is {gloss.SwedishWord}");
                        }
                    }
                    else if (arguments.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        string s = Console.ReadLine();
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            if (gloss.SwedishWord == s)
                                Console.WriteLine($"English for {gloss.SwedishWord} is {gloss.EnglishWord}");
                            if (gloss.EnglishWord == s)
                                Console.WriteLine($"Swedish for {gloss.EnglishWord} is {gloss.SwedishWord}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }
    }
}