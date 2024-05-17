using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myLib;

namespace myLibTest;

[TestClass]
public class Class1Test
{
    [TestMethod]
    public void PavleTest()
    {
        Assert.AreEqual(Class1.Pavle(),"Ja sam Pavle!");
    }
}