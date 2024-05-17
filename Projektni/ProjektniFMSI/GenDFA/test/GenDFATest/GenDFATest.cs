using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GenDFA;
namespace GenDFATest
{
    [TestClass]
    public class GenDFATest
    {
        [TestMethod]
        public void GenerateTest()
        {   
            GenDFA.GenDFA.Generate("/home/pavle/Desktop/FAKS/II/FMSI/C#/Projektni/ProjektniFMSI/GenTest1.txt");
        }
    }
}