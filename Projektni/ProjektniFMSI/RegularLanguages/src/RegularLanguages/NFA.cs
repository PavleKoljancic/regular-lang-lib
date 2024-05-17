namespace RegularLanguages
{	 /*
        Class NFA representing 
        Non-Deterministic Finite Automata
        All its fileds are inherited from
        the abstract class Automata.
        It implements the IRegular Interface
        as every NFA is a representaio of a 
        Regular language.
    */
    public class NFA : Automata, IRegular
    {   /*
			Public Constructor for a NFA
			it has no Aditional fileds
			or requirmeds than those
			imposed by the Abstract class
			Automata.
			All its Fileds are inherted
			from the abstract class automata.
		*/
        public NFA(string startState, HashSet<string> FinalStates, Dictionary<(string, char), HashSet<string>> Delta) : base(startState, FinalStates, Delta)
        {
                

        }

        /*
         public function Accepts 
         returns true if the given string is accapted
         and false if the string is not epected
		 
		 A string is accapted if after the word has been procesed
		 at least one of the end states is a final state.
         It throws an exception of a symbol in the string is not part of the alfabet
        */
        public Boolean Accepts(string word)
        {
            HashSet<string> currentStates = new HashSet<string>();
            currentStates.Add(startState);
            foreach (char symbol in word)
            {
                currentStates = Closure(currentStates);

                HashSet<string> resultStates = new HashSet<string>();

                foreach (string state in currentStates)
                    if (Delta.ContainsKey((state, symbol)))
                        resultStates.UnionWith(Delta[(state, symbol)]);


                currentStates = resultStates;
            }

            currentStates = Closure(currentStates);

            return FinalStates.Intersect(currentStates).Count() > 0;
        }

        /*
			Closure funcion returns a set of states a state can 
			go to only using the epsilon transition.
		*/
        private HashSet<string> Closure(string state)
        {

            HashSet<string> C = new HashSet<string>();
            C.Add(state);
            Queue<string> uncheckedStates = new Queue<string>(C); ;
            HashSet<string> checkedStates = new HashSet<string>();
            while (uncheckedStates.Count > 0)
            {
                string currentState = uncheckedStates.Dequeue();
                checkedStates.Add(currentState);
                if (Delta.ContainsKey((currentState, 'ε')))
                {
                    C.UnionWith(Delta[(currentState, 'ε')]);
                    foreach (string s in Delta[(currentState, 'ε')].Except(checkedStates))
                        uncheckedStates.Enqueue(s);
                }
            }
            return C;

        }

        /*
         Function returning a NFA excluding all unreachable states
         and renaming the reachable ones.
         It does so by usnig the protected RenameAndExcludeUnreachableStatesInner
         function implemented in Automata.
        */

        public override NFA ExcludeUnReachableStatesAndRename(char c = 'q')
        {
            (string, HashSet<string>, Dictionary<(string, char), HashSet<string>>) result = this.RenameAndExcludeUnreachableStatesInner(c);
            return new NFA(result.Item1, result.Item2, result.Item3);
        }
        /*
		 Closure function for a set of states 
		 returns a set of states one can go to
		 only using the epsilon transtion
		 on any of the sets in the input set.

		 It does so by using the Closure function for 
		 a state.
		*/
        private HashSet<string> Closure(HashSet<string> States)
        {

            HashSet<string> ColsureStates = new HashSet<string>();
            foreach (string state in States)
                ColsureStates.UnionWith(Closure(state));

            return ColsureStates;
        }
        /*
			Star function returns
			A NFA representing the
			given language with the
			use of  Kleens Star operator
			on the language all other 
			Star operators are impleted
			trough the Star function in
			NFA by converting the given 
			IRegular Representaion to a NFA

			It does this by adding a new start state 
			which only has an Epsilon transtion
			to the original start state
			and A new finalState to which all original final
			states transition to using a epsilon transtion
			then it also implements an epsilon transtion
			from the newFinal to newStart state using 
			a epsilon transtion. 
			
		*/
        public IRegular Star()
        {
            Dictionary<(string, char), HashSet<String>> StarDelta = new Dictionary<(string, char), HashSet<string>>();

            string newStartState = "newStart";
            HashSet<string> oldStartState = Automata.OneMemberSet(startState);

            StarDelta.Add((newStartState, 'ε'), oldStartState);
            foreach (KeyValuePair<(string, char), HashSet<string>> entry in Delta)
                StarDelta.Add(entry.Key, entry.Value);

            HashSet<string> newFinal = new HashSet<string>();
            newFinal.Add(newStartState);

            foreach (string state in FinalStates)
                StarDelta.Add((state, 'ε'), newFinal);

            return new NFA(newStartState, newFinal, StarDelta);
        }
        /*
			Conccat function  takes in a
			IRegular represention of a language
			converts the given represntaion to a DFA 
			then NFA. then it adds a new start state
			whish has an epsilion to the start state
			of the NFA this function was called on.
			It adds a middle state conecting all the
			final states of this NFA to the middle 
			state  using a epsilon transtion each.

			Then it adds an epsilon transition fro the middle
			state to the start state of the NFA representing
			the IRegular that was passed as an argument

			the final states of the second NFA are the new
			NFAs final states.

			All other Concats are implemented trough this one.
			
			
		*/
        public IRegular Concat(IRegular L)
        {
            NFA A1 = this.ExcludeUnReachableStatesAndRename('q');

            NFA A2 = L.toDFA().toNFA().ExcludeUnReachableStatesAndRename('p');

            string newStartState = "newStart";

            Dictionary<(string, char), HashSet<string>> newDelta = new Dictionary<(string, char), HashSet<string>>(A1.Delta);

            HashSet<string> A1Start = Automata.OneMemberSet(A1.startState);
            newDelta.Add((newStartState, 'ε'), A1Start);


            string mState = "Middle";
            HashSet<string> middleState = Automata.OneMemberSet(mState);


            foreach (string finalState in A1.FinalStates)
                newDelta.Add((finalState, 'ε'), middleState);

            HashSet<string> A2Start = Automata.OneMemberSet(A2.startState);
            newDelta.Add((mState, 'ε'), A2Start);

            foreach (KeyValuePair<(string, char), HashSet<string>> entry in A2.Delta)
                newDelta.Add(entry.Key, entry.Value);

            NFA temp = new NFA(newStartState, A2.FinalStates, newDelta);

            return temp.ExcludeUnReachableStatesAndRename();
        }
		/*
			Union function  takes in a
			IRegular represention of a language
			converts the given represntaion to a DFA 
			then NFA. It Renames both NFAs
			so that no name colsion occurs.
			then it adds a new start state
			whish has an epsilion to the start state
			of both NFAs.
			
			the new Final states are the union of both 
			final states

			All other Unions are implemented trough this one.
			
			
		*/
        public IRegular Union(IRegular L)
        {
            NFA A1 = this.ExcludeUnReachableStatesAndRename('q');

            NFA A2 = L.toDFA().toNFA().ExcludeUnReachableStatesAndRename('p');

            string newStartState = "newStart";

            Dictionary<(string, char), HashSet<string>> newDelta = new Dictionary<(string, char), HashSet<string>>(A1.Delta);

            HashSet<string> StartUnion = Automata.OneMemberSet(A1.startState);
            StartUnion.Add(A2.startState);

            newDelta.Add((newStartState, 'ε'), StartUnion);

            foreach (KeyValuePair<(string, char), HashSet<string>> entry in A2.Delta)
                newDelta.Add(entry.Key, entry.Value);

            HashSet<string> newFinalStates = new HashSet<string>(A1.FinalStates);
            newFinalStates.UnionWith(A2.FinalStates);

            NFA temp = new NFA(newStartState, newFinalStates, newDelta);

            return temp.ExcludeUnReachableStatesAndRename();
        }


		/*
			Conversion Function from NFA to DFA
			returns a DFA it is also a virtual
			function in the IRegular interface
			Ensuring the conversion of all 
			representions of Regular languages to a DFA.

			It does so by creating a new Alfabet excluding epsilon.

			Queues the start state.
			Then Enques all the sets of States 
			one can go to using the current set
			of staes.
			Then it assignes to each set a new name 
			representing the Delta function of the DFA.

			The new Set of Final states are all the renamed
			states whos correspoding set contains at least
			on original final state of the NFA.

			This method implicitly removes all un reachable states.
		*/
        public DFA toDFA()
        {
            HashSet<string> newStart = this.Closure(this.startState);

            Queue<HashSet<string>> uncheckedSets = new Queue<HashSet<string>>();
            uncheckedSets.Enqueue(newStart);


            int count = 0;
            Dictionary<HashSet<string>, string> newName = new Dictionary<HashSet<string>, string>();
            newName.Add(newStart, Automata.FormStateName('p', count++));

            HashSet<string> newFinal = new HashSet<string>();
            if (newStart.Intersect(this.FinalStates).Any())
                newFinal.Add(newName[newStart]);

            HashSet<string> checkedStates = new HashSet<string>();

            Dictionary<(string, char), HashSet<string>> newDelta = new Dictionary<(string, char), HashSet<string>>();

            HashSet<char> nonEpsilonAlfabet = new HashSet<char>(this.Alfabet);
            nonEpsilonAlfabet.Remove('ε');
            while (uncheckedSets.Count > 0)
            {
                HashSet<string> states = uncheckedSets.Dequeue();
                checkedStates.Add(newName[states]);
                foreach (char symbol in nonEpsilonAlfabet)
                {
                    HashSet<string> resultStates = new HashSet<string>();
                    foreach (string state in states)
                        if (Delta.ContainsKey((state, symbol))) resultStates.UnionWith(this.Delta[(state, symbol)]);

                    resultStates = Closure(resultStates);

                    string statename = "";
                    foreach (HashSet<string> key in newName.Keys)
                        if (key.SetEquals(resultStates))
                            statename = newName[key];

                    if (statename.Length == 0)
                    {
                        newName.Add(resultStates, statename = Automata.FormStateName('p', count++));
                        uncheckedSets.Enqueue(resultStates);

                    }
                    if (resultStates.Intersect(this.FinalStates).Any())
                        newFinal.Add(statename);


                    newDelta.Add((newName[states], symbol), Automata.OneMemberSet(statename));
                }
            }

            return new DFA(newName[newStart], newFinal, newDelta);

        }
		/*
			isEqual returns true if the 
			two represntaions of regular languages
			are the same it does so by converting the
			NFA to a DFA and then calling the 
			implementaion of the isEqual function
			of the DFA with the arrgument being passed on.
		*/
        public Boolean isEqual(IRegular L)
        {
            return this.toDFA().isEqual(L);
        }

		/*
			Intersect returns IRegular(DFA) 
			represnting the intersect
			two represntaions of regular languages
			it does so by converting the
			NFA to a DFA and then calling the 
			implementaion of the Intersecr function
			of the DFA with the arrgument being passed on.
		*/
        public IRegular Intersect(IRegular L)
        {
            return this.toDFA().Intersect(L);
        }


		/*
			Diff returns IRegular(DFA) 
			represnting the Difference of
			two represntaions of regular languages
			it does so by converting the
			NFA to a DFA and then calling the 
			implementaion of the Diff function
			of the DFA with the arrgument being passed on.
		*/
        public IRegular Diff(IRegular L)
        {
            return this.toDFA().Diff(L);
        }

        public IRegular Complement()
        {
            return this.toDFA().Complement();
        }


		/*
			isFinate returns true
			if the Language is finite
			it returns false if the language 
			is nonfinite 

			It does so by converting the NFA to
			DFA and calling the DFA's implementaion
			of this function.

			Note the empty languge also returns true.
		*/
        public bool isFinite()
        {
            return this.toDFA().isFinite();
        }


		/*
		 returns a string of the shortest word 
		 accapted by a language.
		 It throws an exception for the Empty Language.

		 It does so by converting the NFA to a DFA
		 and calling the DFA's implementaion of 
		 this function.
		*/
        public string ShortestWord()
        {
            return this.toDFA().ShortestWord();

        }


		/*
		 returns a string of the longest word 
		 accapted by a language.
		 It throws an exception for the Empty Language.
		 It throws a exception if the language is nonfinite.
		*/
        public string LongestWord()
        {
            return this.toDFA().LongestWord();

        }
    }
}
  