namespace SdkTests
{
    using AdobeConnectSDK.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SdkTests.Common;
    using System.Linq;

  [TestClass]
  public class ReportingTests : TestsBase
  {
    [TestMethod]
    public void Report_Quotas()
    {
      var result = Reporting.Report_Quotas(this.Api);
      Assert.IsNotNull(result.Result);
    }

    [TestMethod]
    public void Report_ConsolidatedTransactions()
    {
      var result = Reporting.Report_ConsolidatedTransactions(this.Api, string.Empty);
      Assert.IsTrue(result.Result.Any());
    }

    [TestMethod]
    [Ignore]
    public void ReportMyEvents()
    {
      //this.Api.Login("tst.sdk_tst", "tst.sdk_tst");
      var result = Reporting.ReportMyEvents(this.Api);
      Assert.IsTrue(result.Result.Any());
    }
  }
}
