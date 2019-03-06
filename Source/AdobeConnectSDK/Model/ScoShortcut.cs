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
  /// ScoShortcut structure
  /// </summary>
  [Serializable]
  [XmlRoot("sco")]
  public class ScoShortcut
  {
    [XmlAttribute("tree-id")]
    public int TreeId;

    [XmlAttribute("sco-id")]
    public string ScoId;

    [XmlAttribute("type")]
    public string Type;

    [XmlElement("domain-name")]
    public string DomainName;

  }
}
