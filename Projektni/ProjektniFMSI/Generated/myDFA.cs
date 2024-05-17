using RegularLanguages;
using GenDFA;
public class myDFA
{
    class myDFASubscriber : Subscriber
    {
        private string Name;
        private int count;
        private char? trans;
        public myDFASubscriber(EventDFA Publisher) : base(Publisher)
        {
            Name = "Pero";
            count = 0;
            trans = null;
        }
        override protected void OnTransition(object sender, (char, string) SymbolState)
        {
            if (SymbolState == ('a', "q0"))
            {
                trans = SymbolState.Item1;
                Console.WriteLine($"Otisli ste iz pocetnog stanja koriscenjem {SymbolState.Item1}");
            }
            if (SymbolState == ('b', "q0"))
            {
                trans = SymbolState.Item1;
                Console.WriteLine($"Otisli ste iz pocetnog stanja koriscenjem {SymbolState.Item1}");
            }
        }
        override protected void OnStateEntered(object sender, string state)
        {
            if (state == "q0")
            {
                Console.WriteLine("Moj macak je crn.");
                Console.WriteLine($"Ja sam {Name} usli ste u ${state}");
                Console.ReadLine();
            }
            if (state == "q4")
            {
                Console.WriteLine("Usili ste u zavrsno stanje nadam se da necete otici");
            }
            if (state == "q3")
            {
                Console.WriteLine("Usili ste u zavrsno stanje nadam se da necete otici");
            }
        }
        override protected void OnStateLeft(object sender, string state)
        {
            if (state == "q2")
            {
                if (trans == 'a')
                    Console.WriteLine("Pero je imao predhodno a!!!");
            }
        }
    }
    public static void Main()
    {
        EventDFA myDFA = new("q0", Automata.FinalStatesFromString("q3,q4"), Automata.DeltaFromString("(q0;a)>{q1},(q0;b)>{q2},(q1;b)>{q1},(q1;a)>{q3},(q2;b)>{q4},(q2;a)>{q2},(q3;b)>{q1},(q3;a)>{q3},(q4;b)>{q4},(q4;a)>{q2}"));
        myDFASubscriber submyDFA = new(myDFA);
    }
}
