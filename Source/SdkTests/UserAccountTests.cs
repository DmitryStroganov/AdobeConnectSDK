namespace SdkTests
{
  using System.Linq;
  using AdobeConnectSDK.Extensions;
  using AdobeConnectSDK.Model;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using SdkTests.Common;

  [TestClass]
  public class UserAccountTests : TestsBase
  {
    [TestMethod]
    public void Login()
    {
      var result = this.Api.Login();
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Result);
    }

    [TestMethod]
    public void GetUserInfo()
    {
      var result = this.Api.GetUserInfo();
      Assert.IsNotNull(result.Result);
    }

    [TestMethod]
    public void GetPrincipalInfo()
    {
      ApiStatus s;
      var result = this.Api.GetPrincipalInfo("1603700505");
      Assert.IsNotNull(result);
      Assert.IsNotNull(result.Result.Contact);
      Assert.IsNotNull(result.Result.Preferences);
      Assert.IsNotNull(result.Result.PrincipalData);
    }

    [TestMethod]
    public void GetPrincipalList()
    {
      ApiStatus s;
      var result = this.Api.GetPrincipalList();
      Assert.IsTrue(result.Result.Any());
      Assert.AreEqual(2, result.Result.Count());
    }

    [TestMethod]
    public void PrincipalDelete()
    {
      ApiStatus result = this.Api.PrincipalDelete(new[] { "123456" });
      Assert.AreEqual(StatusCodes.OK, result.Code);
    }

    [TestMethod]
    public void PrincipalUpdate()
    {
      PrincipalSetup pSetup = new PrincipalSetup();
      Principal principal;

      ApiStatus result = this.Api.PrincipalUpdate(pSetup, out principal);
      Assert.AreEqual(StatusCodes.OK, result.Code);
      Assert.IsNotNull(principal);
    }

    [TestMethod]
    [Ignore]
    public void PrincipalUpdatePwd()
    {
      string userId = null;
      string passwordOld = null;
      string password = null;

      ApiStatus result = this.Api.PrincipalUpdatePwd(userId, passwordOld, password);
      Assert.AreEqual(StatusCodes.OK, result.Code);
    }

    [TestMethod]
    public void PrincipalGroupMembershipUpdate()
    {
      string groupId = null;
      string principalId = null;
      bool isMember = false;

      ApiStatus result = this.Api.PrincipalGroupMembershipUpdate(groupId, principalId, isMember);
      Assert.AreEqual(StatusCodes.OK, result.Code);
    }

    [TestMethod]
    public void GetPermissionsInfo()
    {
      string aclId = null;
      string principalID = null;
      string filter = null;

      var result = this.Api.GetPermissionsInfo(aclId, principalID, filter);
      Assert.IsTrue(result.Result.Any());
    }

    [TestMethod]
    public void PermissionsReset()
    {
      string aclId = null;

      ApiStatus result = this.Api.PermissionsReset(aclId);
      Assert.AreEqual(StatusCodes.OK, result.Code);
    }

    [TestMethod]
    [Ignore]
    public void SpecialPermissionsUpdate()
    {
      string aclId = null;

      ApiStatus result = this.Api.SpecialPermissionsUpdate(aclId, SpecialPermissionId.Denied);
      Assert.AreEqual(StatusCodes.OK, result.Code);
    }

    [TestMethod]
    [Ignore]
    public void PermissionsUpdate()
    {
      string aclId = null;
      string principalID = null;

      ApiStatus result = this.Api.PermissionsUpdate(aclId, principalID, PermissionId.View);
      Assert.AreEqual(StatusCodes.OK, result.Code);
    }
  }
}
