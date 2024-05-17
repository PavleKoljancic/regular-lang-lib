namespace RegularLanguages
{
    /*
		Abstract class for the represntaion of Finate Automata.
		The class poses the following protected atributes:
		- Hashset<string> States which contains all the states of a Automate
			this field is infred from the delta transtion function.
		- Hashset<char> Alfabet which represents the alfabet of a Automata
		  it is a infred atribute from the transtion function.
		- HeshSet<String> FinalStates which is a sub set of states representing
		  all final states.
		- Dictoionary<(string,char), Hashset<string>> Delta which represents
		  the delta transition function of a automate
		- string statr state representing the start state of an Automata.
	*/
    public abstract class Automata
    {

        protected HashSet<string> States;
        protected HashSet<char> Alfabet;
        protected HashSet<string> FinalStates;

        protected Dictionary<(string, char), HashSet<string>> Delta;
        protected string startState;

        /*
		* Protected Constructor 
		* It is used by the derived classes to encapsulate the common
		* parts of a constructor shared by all automata.
		* It takes a string startState representing the start state
		* a HashSet of strings represnting the final states
		* and a Dictonary represnting the delta function
		* It infers the States HashSet and and Alfabet HashSet from the Delta
		* dictionary
		*/
        protected Automata(string startState, HashSet<string> FinalStates, Dictionary<(string, char), HashSet<string>> Delta)
        {
            this.FinalStates = FinalStates;
            this.startState = startState;
            this.Delta = Delta;
            States = new HashSet<string>();
            Alfabet = new HashSet<char>();
            ConstructInferedSets();
            StateConsitency();
        }
        /*
		 FinalStaesFromString takes in a string of the form "state1,state2,...,staten"
		 and it creats a HashSet from the string seperated bya comma.
		 It is used for  easier user input.
		*/
        public static HashSet<string> FinalStatesFromString(string FinalStates)
        {
            HashSet<string> Final = new HashSet<string>();
           if(FinalStates.Length>0) Final.UnionWith(FinalStates.Split(','));
            return Final;

        }

        /*
		 DeltaFromString takes in a string of the form "(state;symbol)>state,(state;symbol)>state,...,(state;symbol)>state"
		 where the (stae;symbol) represnts the key and >state represnts the value. All key value pairs
		 are seperated by a comma.
		It is used for easier user input.
		*/
        public static Dictionary<(string, char), HashSet<string>> DeltaFromString(string transitions)
        {
            Dictionary<(string, char), HashSet<string>> Delta = new Dictionary<(string, char), HashSet<string>>();
            string[] transtionArray;
            if (transitions.Length > 0) 
			{ 
				transtionArray = transitions.Split(",");
            foreach (string trans in transtionArray)
            {
                string key = trans.Substring(1, trans.LastIndexOf(')'));
                string[] KeyArray = key.Split(';');

                string value = trans.Substring(trans.LastIndexOf('{') + 1, trans.Length - trans.LastIndexOf('{') - 2);
                HashSet<string> values = new HashSet<string>();
                values.UnionWith(value.Split(';'));

                Delta.Add((KeyArray[0], KeyArray[1][0]), values);
            }
			}
            return Delta;

        }
        /*
			private function used for the construction and intilization of 
			the Infered Sets that being states and alfabet
		*/
        private void ConstructInferedSets()
        {	
			if(startState.Length>0)States.Add(startState);
            foreach (KeyValuePair<(string, char), HashSet<string>> entry in Delta)
            {
                Alfabet.Add(entry.Key.Item2);
                States.Add(entry.Key.Item1);
                States.UnionWith(entry.Value);
            }
        }

        /*
			private function that checks that the Final States are a sub set of States
			and that the start state is contained in the States set.
		*/
        private void StateConsitency()
        {
            if (!FinalStates.IsSubsetOf(States)) throw new Exception("The set of FinalStates has to be a sub set of all states!");
            if (!States.Contains(startState)) throw new Exception("Start state has to be member of states!");
        }

        /*
		 A protected function that takes in a char c 
		 and renames all states and excludes unreachable states.
		 It should be noted that this function does not return a Automate
		 but a triplet containg all the elements needed to construct a Automata.
         It is used by ExcludeUnReachableStatesAndRename functions in DFAs in NFAs.

         It does so by triversing all states that can be reached from the start state
         and renaming them.
		*/
        protected (string, HashSet<string>, Dictionary<(string, char), HashSet<string>>) RenameAndExcludeUnreachableStatesInner(char c)
        {
            Dictionary<string, string> Mapping = new Dictionary<string, string>();
            int stateCount = 0;
            Mapping.Add(this.startState, FormStateName(c, stateCount++));

            Dictionary<(string, char), HashSet<string>> reducedDelta = new Dictionary<(string, char), HashSet<string>>();
            Queue<string> uncheckedStates = new Queue<string>();
            uncheckedStates.Enqueue(startState);

            while (uncheckedStates.Count > 0)
            {
                string state = uncheckedStates.Dequeue();
                foreach (char symbol in Alfabet)
                {
                    if (Delta.ContainsKey((state, symbol)))
                    {
                        HashSet<string> results = Delta[(state, symbol)];
                        HashSet<string> renamedResults = new HashSet<string>();
                        foreach (string resultState in results)
                        {
                            if (!Mapping.ContainsKey(resultState))
                            {
                                Mapping.Add(resultState, FormStateName(c, stateCount++));
                                uncheckedStates.Enqueue(resultState);
                            }
                            renamedResults.Add(Mapping[resultState]);

                        }
                        reducedDelta.Add((Mapping[state], symbol), renamedResults);
                    }
                }
            }

            HashSet<string> renamedFinalStates = new HashSet<string>();
            foreach (string finalState in FinalStates)
                if (Mapping.ContainsKey(finalState))
                    renamedFinalStates.Add(Mapping[finalState]);

            return (Mapping[startState], renamedFinalStates, reducedDelta);
        }

        /*
			A helper function that creats a string representing a state name
			containg of a char c concated with an int i.
		*/
        protected static string FormStateName(char c, int i)
        {
            return c + i.ToString();
        }

        /*
			Exclude un reachable states is an public abstract function
			that is used for the removal of unreachable states i.e.
			states that you can not get to from the startState.
		*/
        public abstract Automata ExcludeUnReachableStatesAndRename(char c = 'q');

        /*
		 A function that takes in a string and constructs a HashSet<string>
		  containg only that state.
		*/
        protected static HashSet<string> OneMemberSet(string state)
        {
            HashSet<string> temp = new HashSet<string>();
            temp.Add(state);
            return temp;
        }
    }
}