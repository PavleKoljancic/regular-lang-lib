using RegularLanguages;
namespace GenDFA
{
    public class EventDFA:DFA
    {   

        public event EventHandler<string>? StateEntered;
        public event EventHandler <(char,string)>?  Transtion;
        public event EventHandler<string>? StateLeft;
        public EventDFA(string startState, HashSet<string> FinalStates, Dictionary<(string, char), HashSet<string>> Delta) : base(startState, FinalStates, Delta)
        {
        }
        override public Boolean Accepts(string word)
        {
            string currentState = startState;
            OnStateEntered(startState);
            foreach (char symbol in word)
            {   
                if (Alfabet.Contains(symbol)) 
                    {   
                        string oldState = currentState;
                        OnTransition(symbol,currentState);
                        currentState = Delta[(currentState, symbol)].Single();
                        OnStateLeft(oldState);
                    }
                else throw new ArgumentException($"Unknown symbol {symbol}");
                OnStateEntered(currentState);
            }
            return FinalStates.Contains(currentState);
        }

        protected virtual void OnTransition(char c, string state)
        {
            EventHandler<(char,string)>? raiseEvent = Transtion;

            // Event will be null if there are no subscribers
            if (raiseEvent != null)
            {
             
                raiseEvent(this, (c,state));
            }
        }

        protected virtual void OnStateEntered(string state) 
        {
             EventHandler<string>? raiseEvent = StateEntered;

            // Event will be null if there are no subscribers
            if (raiseEvent != null)
            {
             
                raiseEvent(this, state);
            }
        }   

        protected virtual void OnStateLeft(string state) 
        {
             EventHandler<string>? raiseEvent = StateLeft;

            // Event will be null if there are no subscribers
            if (raiseEvent != null)
            {
             
                raiseEvent(this, state);
            }
        }   
    }
}