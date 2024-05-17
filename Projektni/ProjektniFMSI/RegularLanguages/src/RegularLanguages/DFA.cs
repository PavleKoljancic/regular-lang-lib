namespace RegularLanguages
{   /*
        Class DFA representing 
        Deterministic Finite Automata
        All its fileds are inherited from
        the abstract class Automata.
        It implements the IRegular Interface
        as every DFA is a representaio of a 
        Regular language.
    */
    public class DFA : Automata, IRegular
    {
        /*
        * Public Constructor.
        * It delgates all the veriable intilization to the
        * Abstract parent class Automata.
        * it should be noted that the transtion function
        * meaning the Dictonary Delta
        * returns a Hashset<string> this is only done so
        * that the DFA can be easily converted to a NFA
        * as it reprents a stricter version of a NFA.
        * 
        * To ensure that the DFA is deterministic i.e.
        * that it has no epsilon trnastions and that
        * the Delta function returns a single state
        * the HashSet it returns can only contain one member
        * Also every state needs to have a transtion for
        * every character in the Alfabet.
        */
        public DFA(string startState, HashSet<string> FinalStates, Dictionary<(string, char), HashSet<string>> Delta) : base(startState, FinalStates, Delta)
        {
            this.IsDetermenistic();

        }
        /*
         public function Accepts 
         returns true if the given string is accapted
         and false if the string is not epected
         
         It is accpated if the end state is a final state.

         It throws an exception of a symbol in the string is not part of the alfabet
        */
        virtual public Boolean Accepts(string word)
        {
            string currentState = startState;
            foreach (char symbol in word)
                if (Alfabet.Contains(symbol)) currentState = Delta[(currentState, symbol)].Single();
                else return false;
            return FinalStates.Contains(currentState);
        }
        /*
        * private helper function ensuring that the given 
          automata is deterministic it does so by ensuring 
          there are no epsilon transtion
          That every HashSet<string> that is Value in the
          Delta functioon dictionary has a count of 1.
          Thus that each transition returns only one state.
          That every state has a transtion for every symbol
          i.e. that the automata cannot hang.
          
        */
        private void IsDetermenistic()
        {
            if (Alfabet.Contains('ε')) throw new Exception("The Alfabet of DFA can not contain ε");
            foreach (HashSet<string> returnStates in Delta.Values)
                if (returnStates.Count != 1)
                    throw new Exception("The transition function for a DFA must return exactly 1 state fore a given input!");
            foreach (char symbol in Alfabet)
                foreach (string state in States)
                    if (!Delta.ContainsKey((state, symbol)))
                        throw new Exception("A transtion must be defined for every symbol in the alfabet for a DFA! (" + state + "," + symbol + ")");
        }

        /*
         Function returning a DFA excluding all unreachable states
         and renaming the reachable ones.
         It does so by usnig the protected RenameAndExcludeUnreachableStatesInner
         function implemented in Automata.
        */

        public override DFA ExcludeUnReachableStatesAndRename(char c = 'q')
        {
            (string, HashSet<string>, Dictionary<(string, char), HashSet<string>>) result = this.RenameAndExcludeUnreachableStatesInner(c);
            return new DFA(result.Item1, result.Item2, result.Item3);
        }
        /*
        * returns a DFA representing a complement of a language.
        * It does so by making a new DFA whos new FinalStates
        * are the non final states of the original DFA.
        */
        public IRegular Complement()
        {
            HashSet<string> newFinal = new HashSet<string>(States);
            newFinal.ExceptWith(FinalStates);
            return new DFA(this.startState, newFinal, this.Delta);
        }
        /*
        * Minimize of DFA
        * 1.First it creates a new DFA whos states are 
          renamed and unreachable ones are excluded.
          It does so by using ExcludeUnReachableStatesAndRename.
          2.It creats a dictionary where the keys are states
            and the values are bools. True if both sates
            are Final or nonFinal false otherwise.
          3.It iterets trough the Dictionary for all 
            Keys(state pairs) whos value is false.
            Then it itterts through all the symbols in the alfabet
            checking if the result states of the symbol are true
            or false if they are trough it sets the value 
            of the original state pair to true. It does
            so until one iteration where no values have changed.
          4.It creats a list of partions for all the Keys(state pairs)
            whos value in the dictionary was false. Thus creating a list of all 
            partions that have more than one member.
          5.Each partion becomes equated with this first state in the partion
            the transtions are ajusted correctly i.e. it is an auto transtion
            if for the symbol the states stay within the partion or its 
            a transion to a new partion which is again equated by the same rules
            with its first state.
          6. The last step is Renaming the newly created DFA so that 
             a better naming convetion is established.
        */
        public DFA Minimize()
        {
            DFA temp = this.ExcludeUnReachableStatesAndRename();
            HashSet<string> nonFinal = new HashSet<string>(temp.States);
            nonFinal.ExceptWith(temp.FinalStates);

            List<string> allStates = new List<string>(temp.States);
            Dictionary<(string, string), Boolean> Table = new Dictionary<(string, string), bool>();

            for (int i = 0; i < allStates.Count; i++)
                for (int j = i + 1; j < allStates.Count; j++)
                    if ((temp.FinalStates.Contains(allStates[i]) && nonFinal.Contains(allStates[j])) || (temp.FinalStates.Contains(allStates[j]) && nonFinal.Contains(allStates[i])))
                        Table.Add((allStates[i], allStates[j]), true);
                    else
                        Table.Add((allStates[i], allStates[j]), false);

            //This runs until it has one iteraion with no changes.
            bool change = true;
            while (change)
            {
                change = false;
                foreach (KeyValuePair<(string, string), Boolean> entry in Table)
                    if (entry.Value == false)
                    {
                        foreach (char c in temp.Alfabet)
                        {
                            string result1 = temp.Delta[(entry.Key.Item1, c)].Single();
                            string result2 = temp.Delta[(entry.Key.Item2, c)].Single();

                            if (Table.ContainsKey((result1, result2)))

                                if (Table[(result1, result2)])
                                {
                                    Table[entry.Key] = true;
                                    change = true;
                                }

                            if (Table.ContainsKey((result2, result1)))
                                if (Table[(result2, result1)])
                                {
                                    Table[entry.Key] = true;
                                    change = true;
                                }

                        }
                    }

            }
            List<HashSet<string>> partions = new List<HashSet<string>>();
            foreach (KeyValuePair<(string, string), Boolean> entry in Table)
                if (entry.Value == false)
                {
                    bool found = false;
                    for (int i = 0; i < partions.Count && !found; i++)
                        if (partions[i].Contains(entry.Key.Item1) || partions[i].Contains(entry.Key.Item2))
                        {
                            found = true;
                            partions[i].Add(entry.Key.Item1);
                            partions[i].Add(entry.Key.Item2);
                        }
                    if (!found)
                    {
                        HashSet<string> partion = new HashSet<string>();

                        partion.Add(entry.Key.Item1);
                        partion.Add(entry.Key.Item2);
                        partions.Add(partion);
                    }
                }

            string newStart = temp.startState;
            HashSet<string> newFinal = new HashSet<string>(temp.FinalStates);
            Dictionary<(string, char), HashSet<string>> newDelta = new Dictionary<(string, char), HashSet<string>>();
            HashSet<string> nonPartionStates = new HashSet<string>(temp.States);
            foreach (HashSet<string> partion in partions)
            {
                string state = partion.First();
                PartionAddToDelta(temp, state, newDelta, partions);
                if (partion.Contains(newStart)) newStart = partion.First();
                if (newFinal.Contains(state))
                {
                    newFinal.ExceptWith(partion);
                    newFinal.Add(state);
                }
                nonPartionStates.ExceptWith(partion);
            }
            foreach (string state in nonPartionStates)
                PartionAddToDelta(temp, state, newDelta, partions);

            DFA result = new DFA(newStart, newFinal, newDelta);

            result = result.ExcludeUnReachableStatesAndRename();

            return result;
        }

        /*
            private helper function for the public minimize function
            it takes the temporary DFA, the list of All partions, a state and the 
            Minimized DFA newDelta Dictonary.
            Then it checks for each symbol if it leads to one of the partions
            or not of it doesnt lead to a partion it means the state it returns
            is the only element in its partion.

        */

        private static void PartionAddToDelta(DFA temp, string state, Dictionary<(string, char), HashSet<string>> newDelta, List<HashSet<string>> partions)
        {
            foreach (char c in temp.Alfabet)
            {
                string result = temp.Delta[(state, c)].Single();
                foreach (HashSet<string> p in partions)
                    if (p.Contains(result)) result = p.First();
                newDelta.Add((state, c), Automata.OneMemberSet(result));
            }
        }
        /*
        * Public function returning the number of states in the DFA. 
          
        */
        public int CountOfStates() { return this.States.Count; }

        /*
        * Public function returning the number of final states in the DFA. 
          
        */
        public int CountOfFinalStates() { return this.FinalStates.Count; }
        /*
         iSEqual function checks the equality of 2 regular languages(IRegulars)
         it does so by turning the argument into a DFA thus the toDFA function
         in IRegular. It minimizes both of them.
         Then it checks that they have the same Alfabet, the same number of states
         and the same number of final states.
         Then it renames both of them with the same convention and checks 
         if they both return the same state for every state symbol pair
         in their respective delta function.
         If any of the above are false it returns false otherwise it
         returns true.
        */
        public bool isEqual(IRegular L)
        {
            DFA A1 = L.toDFA().Minimize();
            DFA A2 = this.Minimize();
            if (!(A1.Alfabet.IsSubsetOf(A2.Alfabet) && A2.Alfabet.IsSubsetOf(A1.Alfabet)))
                return false;
            if (A1.CountOfFinalStates() != A2.CountOfFinalStates())
                return false;
            if (A1.CountOfStates() != A2.CountOfStates())
                return false;


            A2.Alfabet = A1.Alfabet;
            A2.ExcludeUnReachableStatesAndRename();
            foreach (KeyValuePair<(string, char), HashSet<string>> entry in A1.Delta)
            {
                if (A2.Delta.ContainsKey(entry.Key))
                {
                    string r2 = A2.Delta[entry.Key].Single();
                    string r1 = entry.Value.Single();
                    if (r1 != r2)
                        return false;
                }
                else return false;
            }

            return true;

        }
        /*
            privet Combine function it 
            creates a DFA where it asings a
            state to every reachable state state pair 
            of both DFAs coreleting a new name with every state pair.
            While doing this it also creats a new set of FinalStates.
            For each pair by aplying a filter to each state pair.
            This function used by the intersect and Diff functions.
        */

        private DFA Combine(DFA A, Func<HashSet<string>, HashSet<string>, (string, string), bool> Filter)
        {
            if (!(A.Alfabet.IsProperSubsetOf(this.Alfabet) && this.Alfabet.IsProperSubsetOf(A.Alfabet)))
                throw new Exception("For operations on regular langauges the alfabet has to be the same!");

            DFA A1 = this;
            DFA A2 = A;


            Queue<(string, string)> uncheckedStatePairs = new Queue<(string, string)>();
            uncheckedStatePairs.Enqueue((A1.startState, A2.startState));


            Dictionary<(string, string), string> newName = new Dictionary<(string, string), string>();
            HashSet<(string, string)> checkedSatePairs = new HashSet<(string, string)>();

            Dictionary<(string, char), HashSet<string>> combinedDelta = new Dictionary<(string, char), HashSet<string>>();
            HashSet<string> CombinedFinal = new HashSet<string>();

            int count = 0;

            newName.Add((A1.startState, A2.startState), Automata.FormStateName('q', count++));

            if (Filter(A1.FinalStates, A2.FinalStates, (A1.startState, A2.startState)))
                CombinedFinal.Add(newName[(A1.startState, A2.startState)]);

            while (uncheckedStatePairs.Count > 0)
            {
                (string, string) statePair = uncheckedStatePairs.Dequeue();
                checkedSatePairs.Add(statePair);
                foreach (char symbol in Alfabet)
                {
                    (string, string) resultPair = (A1.Delta[(statePair.Item1, symbol)].Single(), A1.Delta[(statePair.Item1, symbol)].Single());
                    if (!checkedSatePairs.Contains(resultPair))
                    {
                        uncheckedStatePairs.Enqueue(resultPair);
                    }
                    if (!newName.ContainsKey(resultPair))
                    {
                        newName.Add(resultPair, Automata.FormStateName('q', count++));
                        if (Filter(A1.FinalStates, A2.FinalStates, resultPair))
                            CombinedFinal.Add(newName[resultPair]);


                    }
                    combinedDelta.Add((newName[statePair], symbol), Automata.OneMemberSet(newName[resultPair]));
                }
            }

            return new DFA(newName[(A1.startState, A2.startState)], CombinedFinal, combinedDelta);
        }

        /*
        * Intersects to IRegular with IRegular(DFA)
           it does so by apllying the itersect filter 
           meaning that a new state is a Final if and only if
           both coresponding states are final.
        */
        public IRegular Intersect(IRegular L)
        {
            DFA A = L.toDFA();
            Func<HashSet<string>, HashSet<string>, (string, string), bool> intersect = (HashSet<string> F1, HashSet<string> F2, (string, string) pair) =>
           {
               return F1.Contains(pair.Item1) && F2.Contains(pair.Item2);
           };
            return Combine(A, intersect);
        }

        /*
        * Intersects to IRegular with IRegular(DFA)
           it does so by apllying the difference filter 
           meaning that a new state is a Final if and only if
           its first corespondig pair is final and the second
           is non final.
        */
        public IRegular Diff(IRegular L)
        {
            DFA A = L.toDFA();
            Func<HashSet<string>, HashSet<string>, (string, string), bool> diff = (HashSet<string> F1, HashSet<string> F2, (string, string) pair) =>
            {
                return F1.Contains(pair.Item1) && (!F2.Contains(pair.Item2));
            };
            return Combine(A, diff);
        }
        /*
         Returns a NFA from DFA
         As the NFA reprents a less
         strict DFA every DFA can be turned
         into a NFA by just passing the same
         fileds as the DFA.
        */
        public NFA toNFA()
        {
            return new NFA(this.startState, this.FinalStates, this.Delta);
        }
        /*
         This function ensurs that every IRegular can
         be turned into a DFA since a DFA is alread 
         a DFA it returns the current object.
        */
        public DFA toDFA()
        {
            return this;
        }
        /*
         Returns the IRegular(NFA) for a given 
         DFA it does so by using the toNFA function
         and applying the Star function which
         is implemented in the NFA.
         This way we ensure that every IRegular
         Representaion has a Star operator.
        */
        public IRegular Star()
        {
            return this.toNFA().Star();
        }
        /*
         Returns the IRegular(NFA) concatination for a given 
         DFA  and a IRegular represention
         it does so by using the toNFA function
         and applying the Concat function which
         is implemented in the NFA.
         This way we ensure that every IRegular
         Representaion has a Concat operator.
        */
        public IRegular Concat(IRegular L)
        {

            return this.toNFA().Concat(L);
        }
        /*
         Returns the IRegular(NFA) Union for a given 
         DFA  and a IRegular represention
         it does so by using the toNFA function
         and applying the Union function which
         is implemented in the NFA.
         This way we ensure that every IRegular
         Representaion has a Union operator.
        */
        public IRegular Union(IRegular L)
        {
            return this.toNFA().Union(L);
        }

        /*
         returns true if the Language is finate
         first it checks if the language has any final
         states if not it returns true since its
         the empty language.
         Then it creates a temp DFA which is the Minimazation of the
         current DFA it does so because it has less states to check.
         Each other represntaion uses this function because
         an DFA represtion of a language has no cycles in its graph
         thus this function checks if there are any cycles in the language
         meaning it can be pumped if so it returns false.
         If no cycles are found it returns true.
        */
        public bool isFinite()
        {
            DFA temp = this.Minimize();
            if (temp.FinalStates.Count == 0)
                return true; //The empty Language
            HashSet<string> toCheck = new(temp.FinalStates);
            foreach (string state in toCheck)
            {
                HashSet<string> checkedStates = new();
                Queue<string> Q = new();
                Q.Enqueue(state);

                while (Q.Count > 0)
                {
                    string currentState = Q.Dequeue();
                    checkedStates.Add(currentState);
                    foreach (char symbol in temp.Alfabet)
                    {
                        string resultState = temp.Delta[(currentState, symbol)].First();

                        if (resultState == state) return false;

                        if (!Q.Contains(resultState) && !checkedStates.Contains(resultState))
                            Q.Enqueue(resultState);
                    }

                }

            }
            return true;
        }
        private static bool Greater(string str1, string str2) { return str1.Length > str2.Length; }

        /*
            returns a string representing the shortest
            word in a non-empty language.
            If the Language is empty it throws and exception.

            It does so by using Dijkstra's Algorithm
        
        */
        public string ShortestWord()
        {
            DFA temp = this.Minimize();
            Dictionary<(string, string), string> StateToState = temp.Transitions();
            if (temp.FinalStates.Count == 0)
                throw new ArgumentException("Cannot find words belonging to empty Lanuage!");
            if (temp.FinalStates.Contains(temp.startState))
                return "";
            Queue<string> Q = new();

            foreach (string state in temp.States)
                if (state != temp.startState && StateToState.ContainsKey((temp.startState, state)))
                    Q.Enqueue(state);

            HashSet<string> checkedStates = new();
            while (Q.Count > 0)
            {
                string currentState = Q.Dequeue();
                checkedStates.Add(currentState);
                foreach (string state in temp.States)
                {
                    if (state != currentState && StateToState.ContainsKey((currentState, state)))
                        if (((!StateToState.ContainsKey((temp.startState, state))) ||
                        Greater(StateToState[(temp.startState, state)], StateToState[(temp.startState, currentState)] + StateToState[(currentState, state)])))
                        {

                            StateToState[(temp.startState, state)] = StateToState[(temp.startState, currentState)] + StateToState[(currentState, state)];

                            if (!Q.Contains(state) && !checkedStates.Contains(state))
                                Q.Enqueue(state);

                        }

                }




            }

            Dictionary<(string, string), string> StartToAny = new();
            string res = "";
            foreach (string FinalState in temp.FinalStates)
                if (res == "" || res.Length > StateToState[(temp.startState, FinalState)].Length)
                    res = StateToState[(temp.startState, FinalState)];

            return res;

        }

        /*
            returns a string representing the longest
            word in a non-empty finite language.
            It does so by checking all possible 
            paths from the start node using a stack and
            saving the Longest Accapted word.
            If the Language is empty it throws and exception.
            If the Language is nonFinite it throws an exception.

        */
        public string LongestWord()
        {
            DFA temp = this.Minimize();
            if (temp.FinalStates.Count == 0)
                throw new ArgumentException("Cannot find words belonging to empty Lanuage!");

            if (!temp.isFinite())
                throw new ArgumentException("Cannot find longest word in nonfinite a lanuage!");

            Dictionary<(string, string), string> StateToState = temp.Transitions();

            Stack<(string, string)> WordStack = new();
            string Longest = "";
            WordStack.Push((temp.startState, ""));
            while (WordStack.Count > 0)
            {
                (string, string) StateAndWord = WordStack.Pop();

                foreach (string state in temp.States)
                    if (state != StateAndWord.Item1 && StateToState.ContainsKey((StateAndWord.Item1, state)))
                    {
                        string newWord = StateAndWord.Item2 + StateToState[(StateAndWord.Item1, state)];
                        WordStack.Push((state, newWord));
                        if (temp.FinalStates.Contains(state) && newWord.Length > Longest.Length)
                            Longest = newWord;
                    }
            }
            return Longest;
        }

        /*
         Private helper function returning a dictionary
         where the keys are state pairs and the values
         are symobol saved as string for the transtion.
         If there is no Transition for two states
         it has no key value pair.
         The transition from state to itself is represented
         with an empty string.
        */
        private Dictionary<(string, string), string> Transitions()
        {
            Dictionary<(string, string), string> StateToState = new Dictionary<(string, string), string>();

            foreach (KeyValuePair<(string, char), HashSet<string>> transition in this.Delta)
                StateToState[(transition.Key.Item1, transition.Value.First())] = transition.Key.Item2.ToString();
            foreach (string state in this.States)
                StateToState[(state, state)] = "";


            return StateToState;
        }
    }
}
