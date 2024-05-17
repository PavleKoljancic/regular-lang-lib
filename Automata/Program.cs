
using System;
using System.Collections.Generic;

using Automata;
Dictionary< (string,char),string> Delta = new (); 

Delta.Add(("q0",'a'),"q0");
Delta.Add(("q0",'b'),"q1");


Delta.Add(("q1",'a'),"q0");
Delta.Add(("q1",'b'),"q2");


Delta.Add(("q2",'a'),"q2");
Delta.Add(("q2",'b'),"q2");


Delta.Add(("q3",'a'),"q0");
Delta.Add(("q3",'b'),"q0");

