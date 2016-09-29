namespace SdkTests
{
  using AdobeConnectSDK.Model;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using SdkTests.Common;

  [TestClass]
  public class CommunicationProviderTests : TestsBase
  {
    [TestMethod]
    public void DirtyData_StatusAndResultsAreReturned()
    {
      ApiStatus s;
      s = this.Api.ProcessApiRequest("dirtydata", string.Empty);

      Assert.AreEqual(StatusCodes.OK, s.Code);
      Assert.AreEqual(StatusSubCodes.NotSet, s.SubCode);
    }

    [TestMethod]
    public void InternalError_StatusAndExceptionAreReturned()
    {
      ApiStatus s;
      s = this.Api.ProcessApiRequest("InternalError", string.Empty);

      Assert.AreEqual(StatusCodes.InternalError, s.Code);
      Assert.AreEqual(StatusSubCodes.NotSet, s.SubCode);
      Assert.IsNotNull(s.Exception);
    }

    [TestMethod]
    public void ResolveOperationStatusFlags_InvalidMissing()
    {
      ApiStatus s;
      s = this.Api.ProcessApiRequest("testAction.ResolveOperationStatusFlags", "invalid-missing");

      Assert.AreEqual(StatusCodes.Invalid, s.Code);
      Assert.AreEqual(StatusSubCodes.Missing, s.SubCode);
      Assert.AreEqual("testField", s.InvalidField);
    }

    [TestMethod]
    public void ResolveOperationStatusFlags_AccessDenied()
    {
      ApiStatus s;
      s = this.Api.ProcessApiRequest("testAction.ResolveOperationStatusFlags", "no-access-Denied");

      Assert.AreEqual(StatusCodes.NoAccess, s.Code);
      Assert.AreEqual(StatusSubCodes.Denied, s.SubCode);
      Assert.IsNull(s.InvalidField);
    }

    [TestMethod]
    public void ResolveOperationStatusFlags_NoResults()
    {
      ApiStatus s;
      s = this.Api.ProcessApiRequest("testAction.ResolveOperationStatusFlags", "no-results");

      Assert.IsNull(s.ResultDocument);
    }
  }
}
