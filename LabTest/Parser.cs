using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml;

namespace LabTest
{
    public class Parser
    {
        private readonly Lexer lexer;
        public Parser(string source) 
        {
            lexer = new Lexer(source);
        }

        public void Evaluation() 
        {   
            List<Token> LT = new List<Token>();
            Token t = lexer.Next();
            while(t.type!="EOF")
            {
                LT.Add(t);
                t= lexer.Next();
            }
            List<Node> Tree = new List<Node>();
            Node Temp;
            for(int i =0;i<LT.Count;i++) 
            {
                Temp = new Node(LT[i]);
                if(LT[i].value=="print"||LT[i].value=="-"||LT[i].value=="(")
            }
        }
    }
}
}