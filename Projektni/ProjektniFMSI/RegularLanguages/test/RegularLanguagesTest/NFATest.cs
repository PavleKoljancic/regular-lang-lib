using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RegularLanguages;
namespace RegularLanguagesTest
{
	[TestClass]
	public class NFATest
	{
		[TestMethod]
		public void Empty()
		{
			NFA EmptyLang = new("q0", new HashSet<string>(), Automata.DeltaFromString("(q0;a)>{q0}"));
			Assert.IsFalse(EmptyLang.Accepts("aaaaa"));
			Assert.IsFalse(EmptyLang.Accepts(""));

			static void Accepts(IRegular EmptyLang)
			{
				Assert.IsFalse(EmptyLang.Accepts("aaaaa"));
				Assert.IsFalse(EmptyLang.Accepts(""));
			}

			Accepts(EmptyLang);
			Accepts(EmptyLang.ExcludeUnReachableStatesAndRename());
			Accepts(EmptyLang.toDFA().Minimize());
		}
		[TestMethod]
		public void Mod2()
		{
			NFA mod2 = new("q0", Automata.FinalStatesFromString("q0"), Automata.DeltaFromString("(q0;1)>{q1},(q0;0)>{q0},(q1;1)>{q0},(q1;0)>{q1}"));




			static void Accepts(IRegular mod2)
			{
				Assert.IsTrue(mod2.Accepts(""));
				Assert.IsTrue(mod2.Accepts("101101"));
				Assert.IsTrue(mod2.Accepts("000"));

				Assert.IsFalse(mod2.Accepts("00000110001"));
			}

			Accepts(mod2);
			Accepts(mod2.ExcludeUnReachableStatesAndRename());
			Accepts(mod2.toDFA());

		}

		[TestMethod]
		public void L2()
		{
			NFA l2 = new("q0", Automata.FinalStatesFromString("q2"), Automata.DeltaFromString("(q0;1)>{q1},(q0;0)>{q1},(q1;1)>{q2},(q1;0)>{q2},(q2;1)>{q3},(q2;0)>{q3},(q3;1)>{q3},(q3;0)>{q3}"));



			static void Accepts(IRegular l2)
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
			Accepts(l2.toDFA());


		}
		[TestMethod]
		public void Mod2Or3()
		{
			NFA mod20r3 = new("q00", Automata.FinalStatesFromString("q00,q02,q10,q01"), Automata.DeltaFromString("(q00;1)>{q11},(q00;0)>{q00},(q11;1)>{q02},(q11;0)>{q11},(q02;1)>{q10},(q02;0)>{q02},(q10;1)>{q01},(q10;0)>{q10},(q01;1)>{q12},(q01;0)>{q01},(q12;1)>{q00},(q12;0)>{q12}"));



			static void Accepts(IRegular mod20r3)
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
			Accepts(mod20r3.toDFA());



		}

		[TestMethod]
		public void XwX()
		{

			NFA xWx = new("q0", Automata.FinalStatesFromString("q3,q4"), Automata.DeltaFromString("(q0;a)>{q1},(q0;b)>{q2},(q1;b)>{q1},(q1;a)>{q3},(q2;b)>{q4},(q2;a)>{q2},(q3;b)>{q1},(q3;a)>{q3},(q4;b)>{q4},(q4;a)>{q2}"));


			static void Accepts(IRegular xWx)
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
			Accepts(xWx.toDFA());

		}

		[TestMethod]
		public void A()
		{

			NFA A = new("q0", Automata.FinalStatesFromString("q3,q2"), Automata.DeltaFromString("(q0;a)>{q1;q2},(q1;a)>{q3},(q2;a)>{q4},(q3;a)>{q2}"));




			static void Accepts(IRegular A)
			{
				Assert.IsTrue(A.Accepts("aaa"));
				Assert.IsTrue(A.Accepts("aa"));
				Assert.IsTrue(A.Accepts("a"));

				Assert.IsFalse(A.Accepts("aaaa"));

				Assert.IsFalse(A.Accepts("aaaaaaaaaaaaaaaaaaaaaaaaaa"));
			}

			Accepts(A);
			Accepts(A.ExcludeUnReachableStatesAndRename());
			Accepts(A.toDFA());

		}
		
		[TestMethod]
		public void EpsilonToDFA() 
		{
			NFA epsilon = new("q0",Automata.FinalStatesFromString("q0"),
			Automata.DeltaFromString("(q0;b)>{q1},(q1;b)>{q2},(q1;a)>{q2},(q1;ε)>{q5},(q2;a)>{q5},(q2;b)>{q3},(q2;ε)>{q6},(q3;a)>{q6},(q4;b)>{q0},(q5;a)>{q0;q4},(q6;ε)>{q5}"));
			epsilon.toDFA();
		}
		[TestMethod]
		public void Zero()
		{

			NFA Zero = new("q0", Automata.FinalStatesFromString("q1,q2"), Automata.DeltaFromString("(q0;ε)>{q1;q2},(q1;0)>{q3},(q2;0)>{q4},(q3;0)>{q1},(q4;0)>{q5},(q5;0)>{q2}"));




			static void Accepts(IRegular Zero)
			{
				Assert.IsTrue(Zero.Accepts(""));
				Assert.IsTrue(Zero.Accepts("00"));
				Assert.IsTrue(Zero.Accepts("000"));
				Assert.IsTrue(Zero.Accepts("0000000000"));
				Assert.IsTrue(Zero.Accepts("000000"));
				Assert.IsTrue(Zero.Accepts("000000000"));

				Assert.IsFalse(Zero.Accepts("0"));
				Assert.IsFalse(Zero.Accepts("00000"));
				Assert.IsFalse(Zero.Accepts("0000000"));
				Assert.IsFalse(Zero.Accepts("00000000000"));
			}

			Accepts(Zero);
			Accepts(Zero.ExcludeUnReachableStatesAndRename());
			Accepts(Zero.toDFA());
		}


		[TestMethod]
		public void Multiple()
		{

			NFA Multiple = new("q1", Automata.FinalStatesFromString("q4"),
			Automata.DeltaFromString("(q1;1)>{q1;q2},(q1;0)>{q1},(q2;0)>{q3},(q2;ε)>{q3},(q3;1)>{q4},(q4;0)>{q4},(q4;1)>{q4}"));


			static void Accepts(IRegular Multiple)
			{

				Assert.IsTrue(Multiple.Accepts("010110"));
				Assert.IsTrue(Multiple.Accepts("11"));
				Assert.IsTrue(Multiple.Accepts("1111"));
				Assert.IsTrue(Multiple.Accepts("11111"));

				Assert.IsFalse(Multiple.Accepts(""));

				Assert.IsFalse(Multiple.Accepts("000000000"));

				Assert.IsFalse(Multiple.Accepts("00000000000001000000000"));
			}

			Accepts(Multiple);
			Accepts(Multiple.ExcludeUnReachableStatesAndRename());
			Accepts(Multiple.toDFA());

		}

		[TestMethod]
		public void StarTestAA()
		{

			NFA AA = new("q0", Automata.FinalStatesFromString("q2"), Automata.DeltaFromString("(q0;a)>{q1},(q1;a)>{q2},(q2;a)>{q3},(q3;a)>{q3}"));
			NFA StarAA = AA.Star().toDFA().toNFA();


			static void Accepts(IRegular StarAA)
			{

				Assert.IsTrue(StarAA.Accepts(""));

				Assert.IsTrue(StarAA.Accepts("aa"));

				Assert.IsTrue(StarAA.Accepts("aaaa"));


				Assert.IsTrue(StarAA.Accepts("aaaaaa"));



				Assert.IsFalse(StarAA.Accepts("aaaaa"));

				Assert.IsFalse(StarAA.Accepts("aaa"));


				Assert.IsFalse(StarAA.Accepts("a"));
			}

			Accepts(StarAA);
			Accepts(StarAA.ExcludeUnReachableStatesAndRename());
			Accepts(StarAA.toDFA());

		}
		[TestMethod]
		public void ClosureTest()
		{
			
			NFA C = new("1", Automata.FinalStatesFromString("4"), Automata.DeltaFromString("(1;ε)>{2;5},(2;a)>{3},(3;c)>{4},(5;ε)>{6;7},(6;a)>{8},(7;b)>{8},(8;ε)>{1}"));



			static void Accepts(IRegular C)
			{
				Assert.IsTrue(C.Accepts("ac"));
				Assert.IsTrue(C.Accepts("aababbaababbbbbbbbbbbbbbbaac"));
				Assert.IsTrue(C.Accepts("aac"));
				Assert.IsTrue(C.Accepts("bac"));

				Assert.IsFalse(C.Accepts(""));
			}

			Accepts(C);
			Accepts(C.ExcludeUnReachableStatesAndRename());
			Accepts(C.toDFA());

		}
	}
}
