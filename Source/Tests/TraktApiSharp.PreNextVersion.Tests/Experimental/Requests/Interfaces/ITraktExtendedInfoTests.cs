﻿namespace TraktApiSharp.Tests.Experimental.Requests.Interfaces
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;
    using TraktApiSharp.Experimental.Requests.Interfaces;
    using TraktApiSharp.Requests.Params;

    [TestClass]
    public class ITraktExtendedInfoTests
    {
        [TestMethod, TestCategory("Requests"), TestCategory("Interfaces")]
        public void TestITraktExtendedInfoIsInterface()
        {
            typeof(ITraktSupportsExtendedInfo).IsInterface.Should().BeTrue();
        }

        [TestMethod, TestCategory("Requests"), TestCategory("Interfaces")]
        public void TestITraktExtendedInfoHasExtendedOptionProperty()
        {
            var extendedOptionPropertyInfo = typeof(ITraktSupportsExtendedInfo).GetProperties()
                                                                       .Where(p => p.Name == "ExtendedInfo")
                                                                       .FirstOrDefault();

            extendedOptionPropertyInfo.CanRead.Should().BeTrue();
            extendedOptionPropertyInfo.CanWrite.Should().BeTrue();
            extendedOptionPropertyInfo.PropertyType.Should().Be(typeof(TraktExtendedInfo));
        }
    }
}
