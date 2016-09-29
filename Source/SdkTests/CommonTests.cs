using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using AdobeConnectSDK.Common;

namespace SdkTests
{
  [TestClass]
  public class CommonTests
  {
    [TestMethod]
    public void DateTime_Parse()
    {
      var s = new DateTime(2009, 03, 31, 10, 07, 59, 180, DateTimeKind.Local);
      var r = DateTime.ParseExact("2009-03-31T10:07:59.180+02:00", Constants.DateFormatString, CultureInfo.InvariantCulture);
      Assert.AreEqual(s, r);
    }

    [TestMethod]
    public void DateTime_ToString()
    {
      var s = new DateTime(2009, 03, 31, 10, 07, 59, 180, DateTimeKind.Local);
      var r = s.ToString(Constants.DateFormatString, CultureInfo.InvariantCulture);
      Assert.AreEqual("2009-03-31T10:07:59.180+02:00", r);
    }
  }
}
