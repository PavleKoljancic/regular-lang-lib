namespace RegularLanguages
{
    /*Public Interface for the represntaion of regular languages
      All the possible operations on Regular Languages are
      are required through this interface such as:
      Accapets Equals Union Intersect Difference
      Complement Concat and Kleens Star.
      It also has aditional functions
      such as taking a Regeular Ecpresion string
      and converting it to an IRegular(NFA)
      and it ensurse that all the IRegular 
      members can be converted to a DFA
    */
    public interface IRegular
    {
        /*
        * REtoIR converts a regular expresion string
        * to IRegular(NFA) it does so 
        * by creating a parsing tree 
        * and then converting it to a NFA 
        * using the Star Union and Concat operations.
        * It can result ina very large NFA 
        * Due to the fact that every charcter is made into
        * a single char language 
        * It throws an Error if the string is empty.
        */
        public static IRegular REtoIR(string RE)
        {   
          if(!(RE.Length>0)) throw new ArgumentException("RE string cannot be empty!");
          if(RE.Contains(';')||RE.Contains(',')) throw new ArgumentException("RE cannot contain the ; or , symbols!");
            Node tree = Node.ParsingTree(RE);
            return ConstructNFA(tree);
        }
        /*
          private static helper function that
          creats an IRegular(NFA) from a 
          parsing tree
          It does so by creating an NFA from every
          symbol that is not an operator
          If the operator is | it creats a Union of the given NFAs
          IF the operator is Kleens Star it does a Star operation
          on the NFA
          IF the operation is concation it concats the given NFAS 
          
        */
        private static NFA ConstructNFA(Node root)
        {
            if (root == null)
                throw new Exception("Improperly parsed root cannot be null");
            NFA l, r;
            switch (root.getData())
            {
                case "|":
                    l = ConstructNFA(root.getLeft());
                    r = ConstructNFA(root.getRight());
                    return l.Union(r).toDFA().toNFA();


                case "•":
                    l = ConstructNFA(root.getLeft());
                    r = ConstructNFA(root.getRight());
                    return l.Concat(r).toDFA().toNFA();
                case "*":
                    l = ConstructNFA(root.getLeft());
                    return l.Star().toDFA().toNFA();
                default:
                    return new NFA("q0", Automata.FinalStatesFromString("q1"), Automata.DeltaFromString("(q0;" + root.getData() + ")>{q1}"));

            }
        }
        /*
         A virtual function that ensuers that every IRegular
         represntion has a an Accepts function. It is implemnted
         trough the abstract class Automata.
        */
        public Boolean Accepts(string word);
        /*
         A virtual function that ensuers that every IRegular
         represntion can be compred it does so by using the
         compersion of Minimized DFAs
        */
        public Boolean isEqual(IRegular L);
        /*
          A virtual function that ensuers
          that every IRegular representaion
          can be converted to DFA.
        */
        public DFA toDFA();
        /*
          A virtual function that ensuers
          that every IRegular implemention
          has the Kleens Star operator.
          It does so by using NFA's.
        */
        public IRegular Star();

        /*
          A virtual function that ensuers
          that every IRegular implemention
          has the Concat operator.
          It does so by using NFA's.
        */
        public IRegular Concat(IRegular L);

        /*
          A virtual function that ensuers
          that every IRegular implemention
          has the Concat operator.
          It does so by using NFA's.
        */
        public IRegular Union(IRegular L);


        /*
          A virtual function that ensuers
          that every IRegular implemention
          has the Intersect operator.
          It does so by using DFA's.
        */
        public IRegular Intersect(IRegular L);

        /*
          A virtual function that ensuers
          that every IRegular implemention
          has the Difference operator.
          It does so by using DFA's.
        */
        public IRegular Diff(IRegular L);

        /*
          A virtual function that ensuers
          that every IRegular implemention
          has the Complement operator.
          It does so by using DFA's.
        */
        public IRegular Complement();
        /*
          Public method that
          returns a bool true if the language is Finite
          returns false if the langauge is Finite
          It returns false also for the empty language.
        */
        public Boolean isFinite();

        /*
         Public method
         returns the shortest word in a non-Empty language.
         If the empty language is given it throws an Exception.
        */
        public string ShortestWord();

        /*
         Public method
         returns the longest word in a non-Empty finite language.
         If the empty language is given it throws an Exception.
         If the language is non-Finite it throws an excpetion
        */

        public string LongestWord();
        
    }
}
