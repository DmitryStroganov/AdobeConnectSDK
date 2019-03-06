namespace SdkTests
{
  using System.Linq;
  using AdobeConnectSDK;
  using AdobeConnectSDK.Extensions;
  using AdobeConnectSDK.Model;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using SdkTests.Common;

  [TestClass]
  public class MeetingTests : TestsBase
  {
    [TestMethod]
    public void GetAllMeetings()
    {
      var result = this.Api.GetAllMeetings();
      Assert.IsTrue(result.Result.Any());
      Assert.AreEqual(7, result.Result.Count());
    }

    [TestMethod]
    public void GetMyMeetings()
    {
      //this.Api.Login("tst.sdk_tst", "tst.sdk_tst");
      var result = this.Api.GetMyMeetings();
      Assert.IsTrue(result.Result.Any());
    }

    [TestMethod]
    public void GetSCOshotcuts()
    {
      var result = MeetingManagement.GetSCOshortcuts(this.Api);
      Assert.IsTrue(result.Result.Any());
    }

    [TestMethod]
    public void GetMeetingDetail()
    {
      var param = "1640856066";
      var result = this.Api.GetMeetingDetail(param);
      Assert.IsNotNull(result.Result);
    }

    [TestMethod]
    public void CreateMeeting()
    {
      MeetingDetail detailItem;
      var item = new MeetingUpdateItem();

      item.Name = "tst.SDK_debugging";
      item.MeetingItemType = SCOtype.Meeting;
      item.FolderId = "838570252";
      var result = this.Api.MeetingCreate(item, out detailItem);
      Assert.IsTrue(result.Code == StatusCodes.OK);
      Assert.AreEqual(item.Name, detailItem.Name);
    }

    [TestMethod]
    [Ignore]
    public void UpdateMeeting()
    {
      var item = new MeetingUpdateItem();

      item.Name = "tst.SDK_debugging";
      var result = this.Api.MeetingUpdate(item);
      Assert.IsTrue(result.Code == StatusCodes.OK);
    }
  }
}
