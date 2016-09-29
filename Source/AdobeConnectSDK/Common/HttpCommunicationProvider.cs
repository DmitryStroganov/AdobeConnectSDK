namespace AdobeConnectSDK.Common
{
  using System;
  using System.IO;
  using System.Net;
  using System.Text;
  using System.Xml;
  using AdobeConnectSDK.Interfaces;
  using AdobeConnectSDK.Model;

  public class HttpCommunicationProvider : ICommunicationProvider
  {
    private string m_SessionInfo = string.Empty;
    private string m_SessionDomain = string.Empty;

    public ISdkSettings Settings { get; set; }

    public ApiStatus ProcessRequest(string pAction, string qParams)
    {
      if (this.Settings == null)
      {
        throw new InvalidOperationException("This provider is not configured.");
      }

      ApiStatus operationApiStatus = new ApiStatus();
      operationApiStatus.Code = StatusCodes.NotSet;

      if (qParams == null)
        qParams = string.Empty;

      HttpWebRequest HttpWReq = WebRequest.Create(this.Settings.ServiceURL + string.Format(@"?action={0}&{1}", pAction, qParams)) as HttpWebRequest;
      if (HttpWReq == null)
        return null;

      try
      {
        if (!string.IsNullOrEmpty(this.Settings.ProxyUrl))
        {
          if (!string.IsNullOrEmpty(this.Settings.ProxyUser) && !string.IsNullOrEmpty(this.Settings.ProxyPassword))
          {
            HttpWReq.Proxy = new WebProxy(this.Settings.ProxyUrl, true);
            HttpWReq.Proxy.Credentials = new NetworkCredential(this.Settings.ProxyUser, this.Settings.ProxyPassword, this.Settings.ProxyDomain);
          }

        }
      }
      catch (Exception ex)
      {
        throw ex.InnerException;
      }

      //20 sec. timeout: A Domain Name System (DNS) query may take up to 15 seconds to return or time out.
      HttpWReq.Timeout = 20000 * 60;
      HttpWReq.Accept = "*/*";
      HttpWReq.KeepAlive = false;
      HttpWReq.CookieContainer = new CookieContainer();

      if (!this.Settings.UseSessionParam)
      {
        if (!string.IsNullOrEmpty(m_SessionInfo) && !string.IsNullOrEmpty(m_SessionDomain))
          HttpWReq.CookieContainer.Add(new Cookie("BREEZESESSION", this.m_SessionInfo, "/", this.m_SessionDomain));
      }

      HttpWebResponse HttpWResp = null;

      try
      {
        //FIX: Invalid SSL passing behavior
        //(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        ServicePointManager.ServerCertificateValidationCallback = delegate
        {
          return true;
        };

        HttpWResp = HttpWReq.GetResponse() as HttpWebResponse;

        if (this.Settings.UseSessionParam)
        {
          if (HttpWResp.Cookies["BREEZESESSION"] != null)
          {
            this.m_SessionInfo = HttpWResp.Cookies["BREEZESESSION"].Value;
            this.m_SessionDomain = HttpWResp.Cookies["BREEZESESSION"].Domain;            
          }
        }

        Stream receiveStream = HttpWResp.GetResponseStream();
        if (receiveStream == null)
          return null;

        using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
        {
#if DEBUG
          string buf = readStream.ReadToEnd();
          File.WriteAllText("httpproviderdump.txt", buf);
          operationApiStatus = Helpers.ResolveOperationStatusFlags(new XmlTextReader(new StringReader(buf)));
#else
          operationApiStatus = Helpers.ResolveOperationStatusFlags(new XmlTextReader(readStream));
#endif
        }

        if (this.Settings.UseSessionParam)
        {
          operationApiStatus.SessionInfo = this.m_SessionInfo;
        }

        return operationApiStatus;
      }
      catch (Exception ex)
      {
        HttpWReq.Abort();
        throw ex.InnerException;
      }
      finally
      {
        if (HttpWResp != null)
          HttpWResp.Close();
      }

      return null;
    }
  }
}