using RegularLanguages;

namespace GenDFA
{
    public class GenDFA
    {
        private static string[] RequiredUsing = { "RegularLanguages", "GenDFA" };
        private static string  path = "/home/pavle/Desktop/FAKS/II/FMSI/C#/Projektni/ProjektniFMSI/Generated/";
        public static void Main(string[] args)
        {

            foreach (string filename in args)
            {
                Generate(filename);
            }

        }
        public static void Generate(string filename)
        {
            string[] Lines = File.ReadAllLines(filename);
            int i = 0;
            //Genral Data
            string Name = Lines[i++];
            if (Name.Length == 0)
                throw new FormatException("Name of DFA cannot be empty");
            string[] Using = Lines[i].Split(',');
            
            //DFA Data
            var DataDFA = ReadAutomata(Lines, ref i);
            new DFA(DataDFA.Item1, Automata.FinalStatesFromString(DataDFA.Item2),Automata.DeltaFromString(DataDFA.Item3));
            
            //Subscriber Data
            var Data = AditionalData(Lines, ref i);
            var Entry = LoadEntery(Lines, ref i);
            var Left = LoadLeave(Lines, ref i);
            var Transtion = LoadTranstion(Lines, ref i);
            
            if (File.Exists($"{path}{Name}.cs"))
                File.Delete($"{path}{Name}.cs");

            using (StreamWriter writer = new StreamWriter($"{path}{Name}.cs"))
            {   
                //Writing use directives
                WriteUsing(Using,writer);

                //Writing DFA class
                writer.WriteLine($"public class {Name}");
                OpenBracket(writer);
                //Write Custom Subscriber
                WriteSubscriber(writer,Name,Data,Entry,Left,Transtion);
                
                //Write Main
                WriteMain(writer, Name ,DataDFA);
                
                //Closing DFA class
                CloseBracket(writer);
                writer.Close();
            }

        }

        private static void WriteSubscriber(TextWriter writer,string Name, Dictionary<string,(string,string)> Data,Dictionary<string,List<string>>? Entery,Dictionary<string,List<string>>? Leave,Dictionary<(string,char),List<string>>? Transition) 
        {
            writer.WriteLine($"class {Name}Subscriber:Subscriber");
            OpenBracket(writer);
            foreach(string name in Data.Keys)
                writer.WriteLine($"private {Data[name].Item1} {name};");

            WriteConstructor(writer,Name,Data);

            WriteTransition(writer,Transition);

            WriteEnterOrLeave(writer,"override protected void OnStateEntered(object sender, string state)",Entery);

            WriteEnterOrLeave(writer,"override  protected void OnStateLeft(object sender, string state)",Leave);

            CloseBracket(writer);
        
        }

        private static void WriteTransition(TextWriter writer,Dictionary<(string,char),List<string>>? Transition)
        {

            writer.WriteLine("override protected void OnTransition(object sender, (char ,string) SymbolState)");
            OpenBracket(writer);
            if(Transition!=null) 
            {
                foreach((string,char) Key in Transition.Keys)
                    {
                        writer.WriteLine($"if(SymbolState==('{Key.Item2}',\"{Key.Item1}\"))");
                        OpenBracket(writer);
                        WriteCode(writer,Transition[Key]);
                        CloseBracket(writer);
                    }
            }
            CloseBracket(writer);
        }
       
        private static void WriteEnterOrLeave(TextWriter writer, string Signature, Dictionary<string,List<string>>? LeaveOrEntery) 
        {
            writer.WriteLine(Signature);
            OpenBracket(writer);
            if(LeaveOrEntery!=null) 
            {
                foreach(string Key in LeaveOrEntery.Keys)
                    {
                        writer.WriteLine($"if(state==\"{Key}\")");
                        OpenBracket(writer);
                        WriteCode(writer,LeaveOrEntery[Key]);
                        CloseBracket(writer);
                    }
            }
            CloseBracket(writer);
        }
       
       
        private static void WriteCode(TextWriter writer, List<string> Code) 
        {
            foreach(string line in Code)
                writer.WriteLine(line);
        }
        private static void WriteConstructor(TextWriter writer, string Name, Dictionary<string,(string,string)> Data)
        {
            
            writer.WriteLine($"public {Name}Subscriber(EventDFA Publisher):base(Publisher)");
            OpenBracket(writer);
            foreach(string name in Data.Keys)
                writer.WriteLine($"{name} = {Data[name].Item2};");
        
            CloseBracket(writer);
        
        }
        private static void WriteMain(TextWriter writer, string Name, (string,string,string) DataDFA)
        {
                //Writing Main
                writer.WriteLine("public static void Main()");
                OpenBracket(writer);
                writer.WriteLine($"EventDFA {Name} = new (\"{DataDFA.Item1}\",Automata.FinalStatesFromString(\"{DataDFA.Item2}\"),Automata.DeltaFromString(\"{DataDFA.Item3}\"));");
                writer.WriteLine($"{Name}Subscriber sub{Name} = new({Name});");
                //Closing Main
                CloseBracket(writer);
        }
        private static void CloseBracket( TextWriter writer) 
        {
         writer.WriteLine("}");   
        }

        private static void OpenBracket( TextWriter writer) 
        {
         writer.WriteLine("{");   
        }
        private static void WriteUsing(string [] Using, StreamWriter writer) 
        {
            foreach ( string use in RequiredUsing)
               if(use.Length>0) writer.WriteLine("using " + use+";");

            foreach ( string use in Using)
               if(use.Length>0) writer.WriteLine("using " + use+";");

        }

        public static (string,string,string)  ReadAutomata(string [] Lines, ref int i)
        {   
            
            string startState;
            string FinalStates;
            string Delta;

            startState = Lines[++i];
            FinalStates =Lines[++i];
            Automata.FinalStatesFromString(FinalStates);
            Delta = Lines[++i];
            Automata.DeltaFromString(Delta);
            return (startState,FinalStates,Delta);
        }
        private static Dictionary<string, (string, string)> AditionalData(string[] Lines, ref int i)
        {
            string[] Defs = Lines[++i].Split(',');
            string[] Init = Lines[++i].Split(',');

            Dictionary<string, (string, string)> Data = new();

            foreach (string definition in Defs)
            {
                string[] TypeAndData = definition.Split(' ');
                if (TypeAndData.Length != 2)
                    throw new FormatException("Definition of aditional data is inproper. It must contain  2 elements type then name");
                Data[TypeAndData[1]] = (TypeAndData[0], "");
            }
            foreach (string initilization in Init)
            {
                var NameAndValue = initilization.Split("=");
                Data[NameAndValue[0]] = (Data[NameAndValue[0]].Item1, NameAndValue[1]);
            }
            foreach (KeyValuePair<string, (string, string)> Item in Data)
            {
                if (Item.Key.Length == 0)
                {
                    throw new FormatException("Name of variable cannot be empty");
                }

                if (Item.Value.Item1.Length == 0)
                {
                    throw new FormatException("Type of variable cannot be empty");
                }

                if (Item.Value.Item2.Length == 0)
                {
                    throw new FormatException("Itial value of variable cannot be empty");
                }
            }



            return Data;
        }

        private static Dictionary<string, List<string>> Load(string[] Lines, ref int i)
        {
            Dictionary<string, List<string>> Code = new();
            while (Lines[++i] != "END")
            {
                string LoadState = Lines[i++].Split(',')[1];

                Code[LoadState] = LoadCode(Lines, ref i);
            }
            return Code;
        }


        private static Dictionary<(string, char), List<string>>? LoadTranstion(string[] Lines, ref int i)
        {
            if (Lines[++i] != "TRANSITION") return null;
            Dictionary<(string, char), List<string>> Code = new();
            while (Lines[++i] != "END")
            {
                string LoadState = Lines[i].Split(',')[1];
                char LoadSymbol = Lines[i++].Split(',')[2][0];
                Code[(LoadState, LoadSymbol)] = LoadCode(Lines, ref i);
            }
            return Code;
        }
        private static List<string> LoadCode(string[] Lines, ref int i)
        {
            List<string> StateCode = new();
            for (; i < Lines.Length && Lines[i] != "END"; i++)
                StateCode.Add(Lines[i]);
            return StateCode;
        }
        private static Dictionary<string, List<string>>? LoadEntery(string[] Lines, ref int i)
        {
            if (Lines[++i] == "ENTERY")
                return Load(Lines, ref i);
            --i;
            return null;
        }
        private static Dictionary<string, List<string>>? LoadLeave(string[] Lines, ref int i)
        {
            if (Lines[++i] == "LEFT")
                return Load(Lines, ref i);
            --i;
            return null;
        }
    }

}