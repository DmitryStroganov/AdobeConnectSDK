/*
Copyright 2007-2014 Dmitry Stroganov (dmitrystroganov.dk), 
Copyright 2014-* Public domain.
Redistributions of any form must retain the above copyright notice.
*/

namespace AdobeConnectSDK.Interfaces
{
  using AdobeConnectSDK.Model;

  /// <summary>
  /// Communication provider.
  /// </summary>
  public interface ICommunicationProvider
  {
    ApiStatus ProcessRequest(string pAction, string qParams, ISdkSettings settings);
  }
}
