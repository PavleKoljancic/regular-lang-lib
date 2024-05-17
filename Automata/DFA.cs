using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Automata
{
	public class DFA
	{
        //Fields
		private readonly string startState;
		private readonly Dictionary<(string, string), string> Delta;
		private readonly HashSet<string> Alfabet;
		private bool isComplete;

        //Methods
		private void CheckISAutomataComplete()
		{


			isComplete = true;
			SortedSet<string> Checked = new();
			foreach (var pair in Delta)
			{
				var temp = pair.Key.Item1;
				if (!Checked.Contains(temp) && isComplete == true)
					foreach (var input in Alfabet)
						if (!Delta.ContainsKey((temp, input)))
							isComplete = false;
				Checked.Add(temp);
			}


		}

        public DFA(string startState, Dictionary<(string, string), string> Delta)
		{
			this.startState = startState;
			this.Delta = new(Delta);
			Alfabet = new HashSet<string>();


			Dictionary<(string, string), string>.KeyCollection keys = Delta.Keys;
			foreach (var item in keys)
				Alfabet.Add(item.Item2);
		
            CheckISAutomataComplete();
		}

		public bool IsAutomataComplete()
		{
			return isComplete;

		}
   
    }
}