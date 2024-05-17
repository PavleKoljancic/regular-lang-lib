using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenDFA
{
    public abstract class Subscriber
    {   
        private EventDFA Publisher;
        public Subscriber(EventDFA Publisher)
        {
            this.Publisher = Publisher;
            this.Publisher.Transtion += OnTransition;

            this.Publisher.StateEntered += OnStateEntered;

            this.Publisher.StateLeft += OnStateLeft;
        }

        abstract protected void OnTransition(object sender, (char ,string) SymbolState);
        abstract protected void OnStateEntered(object sender, string state);

        abstract protected  void OnStateLeft(object sender, string state);
    }
}