using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegularLanguages;

namespace RegularLanguagesTest
{
	[TestClass]
	public class DFATest
	{


		[TestMethod]
		public void Empty()
		{
			DFA EmptyLang = new DFA("q0", new HashSet<string>(), Automata.DeltaFromString("(q0;a)>{q0}"));


			static void Accepts(DFA EmptyLang)
			{
				Assert.IsFalse(EmptyLang.Accepts("aaaaa"));
				Assert.IsFalse(EmptyLang.Accepts(""));
			}

			Accepts(EmptyLang);
			Accepts(EmptyLang.ExcludeUnReachableStatesAndRename());
			Accepts(EmptyLang.Minimize());





		}
		[TestMethod]
		public void Mod2()
		{
			DFA mod2 = new DFA("q0", Automata.FinalStatesFromString("q0"), Automata.DeltaFromString("(q0;1)>{q1},(q0;0)>{q0},(q1;1)>{q0},(q1;0)>{q1}"));





			static void Accepts(DFA mod2)
			{
				Assert.IsTrue(mod2.Accepts(""));
				Assert.IsTrue(mod2.Accepts("101101"));
				Assert.IsTrue(mod2.Accepts("000"));

				Assert.IsFalse(mod2.Accepts("00000110001"));
			}

			Accepts(mod2);
			Accepts(mod2.ExcludeUnReachableStatesAndRename());
			Accepts(mod2.Minimize());

		
		}

		[TestMethod]
		public void L2()
		{
			DFA l2 = new DFA("q0", Automata.FinalStatesFromString("q2"), Automata.DeltaFromString("(q0;1)>{q1},(q0;0)>{q1},(q1;1)>{q2},(q1;0)>{q2},(q2;1)>{q3},(q2;0)>{q3},(q3;1)>{q3},(q3;0)>{q3}"));



			static void Accepts(DFA l2)
			{
				Assert.IsFalse(l2.Accepts(""));
				Assert.IsFalse(l2.Accepts("101101"));
				Assert.IsFalse(l2.Accepts("000"));
				Assert.IsFalse(l2.Accepts("00000110001"));

				Assert.IsTrue(l2.Accepts("11"));
				Assert.IsTrue(l2.Accepts("10"));
				Assert.IsTrue(l2.Accepts("00"));
			}

			Accepts(l2);
			Accepts(l2.ExcludeUnReachableStatesAndRename());
			Accepts(l2.Minimize());


		}
		[TestMethod]
		public void Mod2Or3()
		{
			DFA mod20r3 = new DFA("q00", Automata.FinalStatesFromString("q00,q02,q10,q01"), Automata.DeltaFromString("(q00;1)>{q11},(q00;0)>{q00},(q11;1)>{q02},(q11;0)>{q11},(q02;1)>{q10},(q02;0)>{q02},(q10;1)>{q01},(q10;0)>{q10},(q01;1)>{q12},(q01;0)>{q01},(q12;1)>{q00},(q12;0)>{q12}"));
			static void Accepts(DFA mod20r3)
			{
				Assert.IsFalse(mod20r3.Accepts("0000001"));
				Assert.IsFalse(mod20r3.Accepts("1011101"));
				Assert.IsFalse(mod20r3.Accepts("10101011"));

				Assert.IsTrue(mod20r3.Accepts("00000110001"));
				Assert.IsTrue(mod20r3.Accepts("111"));
				Assert.IsTrue(mod20r3.Accepts("101"));
				Assert.IsTrue(mod20r3.Accepts(""));
			}

			Accepts(mod20r3);
			Accepts(mod20r3.ExcludeUnReachableStatesAndRename());
			Accepts(mod20r3.Minimize());


		}
		[TestMethod]
		public void XwX()
		{

			DFA xWx = new DFA("q0", Automata.FinalStatesFromString("q3,q4"), Automata.DeltaFromString("(q0;a)>{q1},(q0;b)>{q2},(q1;b)>{q1},(q1;a)>{q3},(q2;b)>{q4},(q2;a)>{q2},(q3;b)>{q1},(q3;a)>{q3},(q4;b)>{q4},(q4;a)>{q2}"));
			static void Accepts(DFA xWx)
			{
				Assert.IsTrue(xWx.Accepts("aa"));

				Assert.IsTrue(xWx.Accepts("bb"));

				Assert.IsTrue(xWx.Accepts("aaaaabbbbbabababababababababaaaaaaaaabba"));

				Assert.IsTrue(xWx.Accepts("bbbbbbbbab"));


				Assert.IsFalse(xWx.Accepts(""));

				Assert.IsFalse(xWx.Accepts("bba"));
				Assert.IsFalse(xWx.Accepts("a"));
				Assert.IsFalse(xWx.Accepts("b"));

				Assert.IsFalse(xWx.Accepts("aaaaabbbbbabababababababababaaaaaaaaabbab"));

				Assert.IsFalse(xWx.Accepts("bbbbbbbbaa"));
			}

			Accepts(xWx);
			Accepts(xWx.ExcludeUnReachableStatesAndRename());

			Accepts(xWx.Minimize());
		}


		[TestMethod]
		public void MinTest()
		{
			DFA A = new DFA("A", Automata.FinalStatesFromString("C,D,E"), Automata.DeltaFromString("(A;0)>{B},(A;1)>{C},(B;0)>{A},(B;1)>{D},(C;0)>{E},(C;1)>{F},(D;0)>{E},(D;1)>{F},(E;1)>{F},(E;0)>{E},(F;0)>{F},(F;1)>{F}"));
			Assert.IsTrue(A.Minimize().CountOfFinalStates() == 1);
			Assert.IsTrue(A.Minimize().CountOfStates() == 3);
			DFA B = new DFA("A", Automata.FinalStatesFromString("E"), Automata.DeltaFromString("(A;0)>{B},(A;1)>{C},(B;0)>{B},(B;1)>{D},(C;0)>{B},(C;1)>{C},(D;0)>{B},(D;1)>{E},(E;0)>{B},(E;1)>{C}"));
			Assert.IsTrue(B.Minimize().CountOfFinalStates() == 1);
			Assert.IsTrue(B.Minimize().CountOfStates() == 4);
		}

		[TestMethod]
		public void path() 
		{	
			IRegular L = IRegular.REtoIR("asfa|asfsafasf|aaasfasfa|a");
			string res = L.toDFA().ShortestWord();
			IRegular L1 = IRegular.REtoIR("bbb(aa|aba)|a(c|aaa)").toDFA().Minimize();
			string res2 = L1.toDFA().ShortestWord();
			IRegular L3 = IRegular.REtoIR("abc(ac|db)*");
			Assert.IsFalse(L3.isFinite());
			string res3 = L3.toDFA().Minimize().ShortestWord();
			L1.toDFA().LongestWord();
			L.toDFA().LongestWord();
		}
	}
}