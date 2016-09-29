namespace SdkTests
{
  using System.Linq;
  using System.Net;
  using AdobeConnectSDK;
  using AdobeConnectSDK.Extensions;
  using AdobeConnectSDK.Model;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using SdkTests.Common;

  [TestClass]
  public class ScenarioTests : TestsBase
  {
    /// <summary>
    /// Scenario1:
    /// create user.
    /// create meeting.
    /// assign user to meeting.
    /// attend meeting as user.
    /// call and inspect result of "report-meeting-attendance".
    /// delete user
    /// delete meeting.
    /// </summary>
    [TestMethod]
    [Ignore]
    public void Scenario1()
    {
      ApiStatus result;

      Principal principal;

      PrincipalSetup pSetup = new PrincipalSetup();
      pSetup.PrincipalType = PrincipalTypes.user;
      pSetup.Login = "tst.sdk_tst";
      pSetup.Name = "tst.sdk_tst";
      pSetup.Password = "tst.sdk_tst";

      result = this.Api.PrincipalUpdate(pSetup, out principal);
      Assert.IsTrue(result.Code == StatusCodes.OK);

      MeetingDetail detailItem;
      var meetingUpdateItem = new MeetingUpdateItem();

      meetingUpdateItem.Name = "tst.SDK_debugging2";
      meetingUpdateItem.MeetingItemType = SCOtype.Meeting;
      meetingUpdateItem.FolderId = "838570252";      
      result = this.Api.MeetingCreate(meetingUpdateItem, out detailItem);
      Assert.IsTrue(result.Code == StatusCodes.OK);

      result = this.Api.ParticipantSubscribe(detailItem.ScoId, principal.PrincipalId);
      Assert.IsTrue(result.Code == StatusCodes.OK);

      WebClient client = new WebClient();
      var response = client.OpenRead(detailItem.FullUrl);
      response.Close();

      var reportResult = this.Api.Report_MeetingAttendance(detailItem.ScoId, string.Empty);
    }

    [TestMethod]
    [Ignore]
    public void CleanupAllTestUsers()
    {
      ApiStatus result;

      var principalList = this.Api.GetPrincipalList(string.Empty, "filter-like-name=tst.");
      result = this.Api.PrincipalDelete(principalList.Result.Select(item => item.PrincipalId).ToArray());

      Assert.IsTrue(result.Code == StatusCodes.OK);
    }

    [TestMethod] 
    [Ignore]
    public void CleanupAllTestMeetings()
    {
      ApiStatus result;

      var scoList = this.Api.GetAllMeetings();
      result = this.Api.ScoDelete(scoList.Result.Select(item => item.ScoId).ToArray());

      Assert.IsTrue(result.Code == StatusCodes.OK);
    }
  }
}
