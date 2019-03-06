namespace SdkTests.Common
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using AdobeConnectSDK.Interfaces;
    using AdobeConnectSDK.Model;

    internal class TestCommunicationProvider : ICommunicationProvider
    {
        public ISdkSettings Settings { get; set; }

        public ApiStatus ProcessRequest(string pAction, string qParams, ISdkSettings settings)
        {
            ApiStatus apiStatus = new ApiStatus();

            switch (pAction)
            {
                case "login":
                    apiStatus = AdobeConnectSDK.Common.Helpers.ResolveOperationStatusFlags(new XmlTextReader(new StringReader(Helpers.GetAssemblyResource("login.xml"))));
                    apiStatus.SessionInfo = Guid.NewGuid().ToString();
                    break;
                case "testAction.ResolveOperationStatusFlags":
                    var sourceXml = new XmlTextReader(new StringReader(Helpers.GetAssemblyResource("testAction.ResolveOperationStatusFlags.xml")));
                    var xDoc = XDocument.Load(sourceXml);

                    if (qParams == "invalid-missing")
                    {
                        var elementInvalid = new XElement("invalid",
                            new XAttribute("subcode", AdobeConnectSDK.Common.Helpers.EnumToString(StatusSubCodes.Missing)),
                            new XAttribute("field", "testField"));
                        var elementStatus = xDoc.XPathSelectElement("//status");
                        elementStatus.Attribute("code").SetValue(AdobeConnectSDK.Common.Helpers.EnumToString(StatusCodes.Invalid));
                        elementStatus.Add(elementInvalid);

                        apiStatus = AdobeConnectSDK.Common.Helpers.ResolveOperationStatusFlags(xDoc.CreateReader());
                    }

                    if (qParams == "no-access-Denied")
                    {
                        var elementInvalid = new XElement("invalid", new XAttribute("subcode", AdobeConnectSDK.Common.Helpers.EnumToString(StatusSubCodes.Denied)));
                        var elementStatus = xDoc.XPathSelectElement("//status");
                        elementStatus.Attribute("code").SetValue(AdobeConnectSDK.Common.Helpers.EnumToString(StatusCodes.NoAccess));
                        elementStatus.Add(elementInvalid);

                        apiStatus = AdobeConnectSDK.Common.Helpers.ResolveOperationStatusFlags(xDoc.CreateReader());
                    }

                    if (qParams == "no-results")
                    {
                        apiStatus = AdobeConnectSDK.Common.Helpers.ResolveOperationStatusFlags(xDoc.CreateReader());
                    }

                    break;
                default:
                    apiStatus = AdobeConnectSDK.Common.Helpers.ResolveOperationStatusFlags(new XmlTextReader(new StringReader(Helpers.GetAssemblyResource(pAction + ".xml"))));
                    break;
            }

            return apiStatus;
        }
    }
}
