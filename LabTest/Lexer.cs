using System.Data.Common;
using System.Runtime.InteropServices;

namespace LabTest
{
    public class Lexer
    {   
       // private Stack<(string,string)> EvaluationStack = new Stack<(string,string)>();        
        private readonly string source;
        private int i;
        public Lexer(string source)
        {
            this.source = source;
            i=0;
        }

        public Token Next() 
        {


            for(; i <source.Length;i++) 
            {   
                
                Token t;
                while(char.IsWhiteSpace(source[i])) i++;
                if(char.IsLetter(source[i]))
                {   
                    string text = "";
                    for(;i<source.Length&&char.IsLetter(source[i]);i++)
                        text = text + source[i];
                    if(text=="print") //gutaj sve
                        t = new Token(Token.op,"print");
                    else
                        t = new Token(Token.id, text);

     
                    return t;
                }
                
                else if(char.IsDigit(source[i])) 
                {
                    string literal = "";
                    for(;i<source.Length&&char.IsDigit(source[i]);i++)
                    literal = literal + source[i];
                    t = new Token(Token.lit,literal);

                

                    return t;
                    
                }
                else if(Token.isOperator(source[i]+"")) 
                {
                    t = new Token(Token.op,source[i]+"");
                    i++;
                    return t;
                }
                else if(source[i]==Token.ExpresionDeliminater)
                {
                   t = new Token("EOE");
                   i++;
                   return t;
                }

                
            }
            return new Token("EOF");



        }
    }
}