namespace SdkTests
{
  using System;
  using AdobeConnectSDK;
  using AdobeConnectSDK.Common;
  using AdobeConnectSDK.Extensions;
  using AdobeConnectSDK.Model;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using SdkTests.Common;
  using System.Collections.Generic;
  using System.Linq;

  [TestClass]
  public class ReportingTests : TestsBase
  {
    [TestMethod]
    public void QuotaEntity_CanSerializeAndDeserialize()
    {
      QuotaInfo qi = new QuotaInfo()
      {
        QuotaList = new List<Quota> { 
          new Quota { AclId = 1, Limit = 2, QuotaId = "tstquota", SoftLimit = 4, Used = 5 },
          new Quota { AclId = 5, Limit = 6, QuotaId = "tstquota2", SoftLimit = 7, Used = 8 } 
        }
      };

      string s = XmlSerializerHelpersGeneric.ToXML(qi);

      QuotaInfo r = XmlSerializerHelpersGeneric.FromXML<QuotaInfo>(s);

      Assert.AreEqual(qi.QuotaList.First().AclId, r.QuotaList.First().AclId);
    }

    [TestMethod]
    public void QuotaEntity_CanDeserialize()
    {
      QuotaInfo qi = new QuotaInfo()
      {
        QuotaList = new List<Quota> { 
          new Quota { AclId = 838570243, Limit = 2, QuotaId = "tstquota", SoftLimit = 4, Used = 5, DateBegin = new DateTime(2009, 03, 31, 08, 07, 59, 180, DateTimeKind.Utc) },
          new Quota { AclId = 5, Limit = 6, QuotaId = "tstquota2", SoftLimit = 7, Used = 8 } 
        }
      };

      string s = "<report-quotas> <quota acl-id=\"838570243\" quota-id=\"download-quota\" used=\"0\" limit=\"10\" soft-limit=\"10\">  <date-begin>2009-03-31T10:07:59.180+02:00</date-begin>   <date-end>3000-01-01T01:00:00.537+01:00</date-end>   </quota></report-quotas>";

      QuotaInfo r = XmlSerializerHelpersGeneric.FromXML<QuotaInfo>(s);

      Assert.AreEqual(qi.QuotaList.First().AclId, r.QuotaList.First().AclId);
      Assert.AreEqual(qi.QuotaList.First().DateBegin, r.QuotaList.First().DateBegin);
    }

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
