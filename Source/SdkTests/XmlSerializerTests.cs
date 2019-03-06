using System;
using System.Collections.Generic;
using System.Linq;
using AdobeConnectSDK.Common;
using AdobeConnectSDK.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SdkTests
{
    //TODO: implement tests for all data models
    [TestClass, Ignore]
    public class XmlSerializerTests
    {
        [TestMethod]
        public void QuotaEntity_CanSerializeAndDeserialize()
        {
            QuotaInfo qi = new QuotaInfo() {
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
            QuotaInfo qi = new QuotaInfo() {
                QuotaList = new List<Quota> {
                    new Quota { AclId = 838570243, Limit = 2, QuotaId = "tstquota", SoftLimit = 4, Used = 5, DateBegin = new DateTime(2009, 03, 31, 08, 07, 59, 180, DateTimeKind.Utc) },
                    new Quota { AclId = 5, Limit = 6, QuotaId = "tstquota2", SoftLimit = 7, Used = 8 }
                }
            };

            string s =
                "<report-quotas> <quota acl-id=\"838570243\" quota-id=\"download-quota\" used=\"0\" limit=\"10\" soft-limit=\"10\">  <date-begin>2009-03-31T10:07:59.180+02:00</date-begin>   <date-end>3000-01-01T01:00:00.537+01:00</date-end>   </quota></report-quotas>";

            QuotaInfo r = XmlSerializerHelpersGeneric.FromXML<QuotaInfo>(s);

            Assert.AreEqual(qi.QuotaList.First().AclId, r.QuotaList.First().AclId);
            Assert.AreEqual(qi.QuotaList.First().DateBegin, r.QuotaList.First().DateBegin);
        }
    }
}
