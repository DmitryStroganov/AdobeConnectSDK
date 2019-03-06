/*
Copyright 2007-2014 Dmitry Stroganov (dmitrystroganov.dk), 
Copyright 2014-* Public domain.
Redistributions of any form must retain the above copyright notice.
*/

using System;
using System.Xml.Serialization;

namespace AdobeConnectSDK.Model
{
  /// <summary>
  /// UserInfo structure
  /// </summary>
  [Serializable]
  [XmlRoot("user")]
  public class UserInfo
  {
    [XmlAttribute("user-id")]
    public string UserId;

    [XmlElement("name")]
    public string Name;

    [XmlElement("login")]
    public string Login;
  }
}
