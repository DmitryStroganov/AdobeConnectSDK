//#define realapi

namespace SdkTests.Common
{
  using System;
  using AdobeConnectSDK;
  using AdobeConnectSDK.Common;
  using AdobeConnectSDK.Model;
  using Microsoft.VisualStudio.TestTools.UnitTesting;

  [TestClass]
  public class TestsBase
  {
    public AdobeConnectXmlAPI Api { get; private set; }

    [TestInitialize]
    public void Init()
    {      
#if realapi
      this.Api = new AdobeConnectXmlAPI();
      this.Login();
#else
      this.Api = new AdobeConnectXmlAPI(new TestCommunicationProvider(), new SdkSettings{ServiceURL = @"tst://tstURL"});
#endif
    }

    internal void Login()
    {
      ApiStatus iApiStatus;
      if (!this.Api.Login().Result)
        throw new Exception("API login failed.");
    }

    [TestCleanup]
    public void Cleanup()
    {
#if realapi
      this.Api.Logout();
#endif
      Api = null;
    }
  }
}