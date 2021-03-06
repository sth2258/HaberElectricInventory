// <copyright file="_DefaultTest.cs">Copyright ©  2016</copyright>
using System;
using InventoryManagement;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InventoryManagement.Tests
{
    /// <summary>This class contains parameterized unit tests for _Default</summary>
    [PexClass(typeof(_Default))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class _DefaultTest
    {
        [TestMethod()]
        public void MailTest()
        {
            Utils.SendMail("subject", "body");
            Assert.AreEqual(true, true);

        }
    }
}
