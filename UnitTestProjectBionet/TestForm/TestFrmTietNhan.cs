using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using UnitTestProjectBionet;
using BioNetSangLocSoSinh.Entry;

namespace UnitTestProjectBionet.TestForm
{
    /// <summary>
    /// Summary description for TestFrmTietNhan
    /// </summary>
    [TestClass]
    public class TestFrmTietNhan
    {
        public TestFrmTietNhan()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private NUnit.Framework.TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public NUnit.Framework.TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
        private BioNetDAL.DataObjects a;
        [TestMethod]
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }
        [SetUp]
        public void TestTiepNhanPhieu()
        {
           
        }
    }
}
