using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegularLanguages;

namespace RegularLanguagesTest
{
    [TestClass]
    public class IRegularTest
    {
        [TestMethod]
        public void aPlus()
        {   
            IRegular L1 = IRegular.REtoIR("a(a)*");
    
            Assert.IsTrue(L1.Accepts("a"));
            Assert.IsTrue(L1.Accepts("aaa"));
            Assert.IsTrue(L1.Accepts("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
            Assert.IsFalse(L1.Accepts(""));
            Assert.IsTrue(L1.Complement().isEqual(new DFA("q0",Automata.FinalStatesFromString("q0"),Automata.DeltaFromString("(q0;a)>{q1},(q1;a)>{q1}"))));
            
            IRegular L2 = L1.toDFA().Minimize();
            Assert.IsTrue(L2.Complement().isEqual(new DFA("q0",Automata.FinalStatesFromString("q0"),Automata.DeltaFromString("(q0;a)>{q1},(q1;a)>{q1}"))));
            


            IRegular empty = IRegular.REtoIR("ε");
    
            IRegular aStar1 = empty.Union(L1).toDFA().Minimize();

            Assert.IsTrue(aStar1.isEqual(IRegular.REtoIR("a*")));
    
        }

        [TestMethod]
        public void UnionTest() 
        {
            IRegular L1 = IRegular.REtoIR("aa|bb|(a|b|c)*");
            IRegular L2 = IRegular.REtoIR("(aa)|(bb)");

            Assert.IsTrue(L1.isEqual(L1.Union(L2)));

            Assert.IsTrue(L2.Complement().toDFA().Minimize().Complement().isEqual(L2));

            IRegular L3 = IRegular.REtoIR("(a|b|c)*");

            IRegular L4 = IRegular.REtoIR("a*|b*|c*");

            Assert.IsFalse(L3.isEqual(L4));

            IRegular Empty = new NFA("q0",Automata.FinalStatesFromString(""),Automata.DeltaFromString(""));

            Assert.IsTrue(L1.isEqual(L1.Union(Empty)));
            Assert.IsTrue(L2.isEqual(L2.Union(Empty)));
            Assert.IsTrue(L3.isEqual(L3.Union(Empty)));
            Assert.IsTrue(L4.isEqual(L4.Union(Empty)));

            Assert.IsTrue(L1.isEqual(Empty.Union(L1)));
            Assert.IsTrue(L2.isEqual(Empty.Union(L2)));
            Assert.IsTrue(L3.isEqual(Empty.Union(L3)));
            Assert.IsTrue(L4.isEqual(Empty.Union(L4))); 
        
        }

        [TestMethod]
        public void AccapetsTest() 
        {
            IRegular L1 = IRegular.REtoIR("aa|bb|(a|b|c)*");
   
            IRegular L2 = IRegular.REtoIR("(aa)|(bb)");

            IRegular L3 = IRegular.REtoIR("(a|b|c)*");

            IRegular L4 = IRegular.REtoIR("a*|b*|c*");


            Assert.IsTrue(L1.Accepts("aa")&&L1.Accepts("bb")&&L1.Accepts("abbc")&&L1.Accepts("aaaaaaaaaabbbbbbbbbbbbbbbcccccccccccccccccccccc"));
            Assert.IsTrue(L2.Accepts("aa")&&L2.Accepts("bb")&&!(L2.Accepts("a"))&&!(L2.Accepts("aaa"))&&!(L2.Accepts("b")));
        }

        [TestMethod]
        public void isFiniteTets() 
        {
            IRegular L1, L2 , L3, L4;

            L1 = IRegular.REtoIR("ab|ac|aaaaaaaaaaaaaaaaaaaaaaaaa|df|(asfasfsafas)");
            Assert.IsTrue(L1.isFinite());

            L2 = IRegular.REtoIR("h");
            Assert.IsTrue(L2.isFinite());

            L3 =  new NFA("q0",Automata.FinalStatesFromString(""),Automata.DeltaFromString(""));

            Assert.IsTrue(L3.isFinite());

            L4 = IRegular.REtoIR("a*");


            Assert.IsFalse(L4.isFinite());

             NFA mod20r3 = new("q00", Automata.FinalStatesFromString("q00,q02,q10,q01"), Automata.DeltaFromString("(q00;1)>{q11},(q00;0)>{q00},(q11;1)>{q02},(q11;0)>{q11},(q02;1)>{q10},(q02;0)>{q02},(q10;1)>{q01},(q10;0)>{q10},(q01;1)>{q12},(q01;0)>{q01},(q12;1)>{q00},(q12;0)>{q12}"));

            Assert.IsFalse(mod20r3.isFinite());

            IRegular L5 = IRegular.REtoIR("ε");
            Assert.IsTrue(L5.Accepts(""));
            L5 =L5.toDFA().Minimize(); 
            
            Assert.IsTrue(L5.Accepts(""));

            IRegular L6 = IRegular.REtoIR("ab*c");
            
            Assert.IsFalse(L6.isFinite());
            }
         [TestMethod]
        public void DekiTest() 
        {


 
            IRegular L1 = IRegular.REtoIR("(aa|bb)|(a|b|c)*cd|a(bcd)*");
            DFA dfa1 = L1.toDFA().Minimize();
            IRegular L2 = IRegular.REtoIR("(((a)(b))(c))(((c)(c)*(a|b)|(a|b))((c)(c)*(a|b)|(a|b))*(((c)(c)*(d))(ε))|(((c)(c)*(d))(ε)|((((d)(b))(c))((d)(ε))|((d)(ε)))))|(((a)(b))((a|b)((c)(c)*(a|b)|(a|b))*(((c)(c)*(d))(ε)))|(((b)(b)|((a)(a)))(((c)(c)*(a|b)|(a|b))((c)(c)*(a|b)|(a|b))*(((c)(c)*(d))(ε))|(((c)(c)*(d))(ε)|(ε)))|(((b)((c)(c)*(a|b)|(a))|((a)((c)(c)*(a|b))|((c)(c)*(a|b))))((c)(c)*(a|b)|(a|b))*(((c)(c)*(d))(ε))|(((b)((c)(c)*(d))|((a)((c)(c)*(d))|((c)(c)*(d))))(ε)|((a)(ε))))))");
            DFA dfa2 =  L2.toDFA().Minimize();
            
            Assert.IsTrue(L1.isEqual(L2));
            }
    }
}