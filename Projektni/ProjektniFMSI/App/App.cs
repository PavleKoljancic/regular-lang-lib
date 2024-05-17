using System;
using RegularLanguages;
namespace App
{
   public class App
    {

        private static string TYPE = "TYPE";
        public static void Main(string[] args)
        {

            foreach (string filename in args)
            {
                ParseFile(filename);
            }

        }

        public static void ParseFile(string filename)
        {
            string[] Lines = File.ReadAllLines(filename);
            for (int i = 0; i < Lines.Length; i++)
            {

                if(Lines[i].Length==0)
                    continue;

                string[] ComandAndArgument = Lines[i].Split(':', 2, StringSplitOptions.None);
                if (ComandAndArgument.Length != 2)
                    throw new FormatException(
                        $"Error in file:{filename} on line:{i} \n The first line of the specification must follow TYPE:NAME_TYPE!");

                if (ComandAndArgument[0] != "TYPE")
                    throw new FormatException(
                    $"Error in file:{filename} on line:{i} \n Expected was {TYPE} gotten {ComandAndArgument[0]}.");

                else
                {
                    ComandAndArgument[1] = ComandAndArgument[1].Substring(1);
                    switch (ComandAndArgument[1])
                    {
                        case "DFA":
                            DFA(Lines,ref i);
                            break;


                        case "NFA":
                            DFA(Lines,ref i);
                            break;


                        case "RE":
                            RE(Lines, ref i);
                            break;
                        default:
                            throw new Exception($"Unknown type {ComandAndArgument[0]}");
                    }

                }

            }
        }
        
        private static void RE(string[] Lines , ref int i) 
        {
            
            Console.WriteLine("\nRE:\n");
            IRegular Expresion = IRegular.REtoIR(Lines[++i]);
            CheckWords(Expresion,Lines,ref i);
        }
        private static void DFA(string[] Lines, ref int i)
        {
            
            Console.WriteLine("\nDFA:\n");
            (string,HashSet<string>, Dictionary<(string,char),HashSet<string>>) result = ReadAutomata(Lines,ref i);

            DFA temp = new DFA(result.Item1, result.Item2, result.Item3);

            CheckWords(temp,Lines, ref i);

        }


        private static void NFA(string[] Lines, ref int i)
        {
            Console.WriteLine("\nNFA:\n");
            (string,HashSet<string>, Dictionary<(string,char),HashSet<string>>) result = ReadAutomata(Lines,ref i);

            NFA temp = new NFA(result.Item1, result.Item2, result.Item3);

            CheckWords(temp,Lines, ref i);

        }

        public static (string,HashSet<string>, Dictionary<(string,char),HashSet<string>>)  ReadAutomata(string [] Lines, ref int i)
        {
            string startState;
            HashSet<string> FinalStates;
            Dictionary<(string, char), HashSet<string>> Delta;

            startState = Lines[++i];
            FinalStates = Automata.FinalStatesFromString(Lines[++i]);
            Delta = Automata.DeltaFromString(Lines[++i]);
            return (startState,FinalStates,Delta);
        }

        private static void CheckWords(IRegular temp, string[] Lines, ref int i)
        {
            if (Lines[++i].Length == 0)
                Console.WriteLine("NO words to check.");
            else
            {
                string[] WordsToCheck = Lines[i].Split(',');
                foreach (string word in WordsToCheck)
                {
                    string W = word.Substring(1, word.Length - 2);
                    Console.WriteLine($"Word:{W} {temp.Accepts(W)}");
                }
            }
        }

        private static bool EndOfLines(string[] Lines, int i)
        {
            return i >= Lines.Length;
        }

    }

}