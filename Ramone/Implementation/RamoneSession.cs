﻿using System;
using System.Net;
using System.Text;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Implementation
{
  public class RamoneSession : ISession
  {
    #region IRamoneSession Members

    public IService Service { get; protected set; }

    public string UserAgent { get; set; }
    
    public Uri BaseUri { get; protected set; }

    public Encoding DefaultEncoding { get; set; }

    public MediaType DefaultRequestMediaType { get; set; }

    public MediaType DefaultResponseMediaType { get; set; }

    public CookieContainer Cookies { get; protected set; }

    public IAuthorizationDispatcher AuthorizationDispatcher { get; protected set; }

    public IRequestInterceptorSet RequestInterceptors { get; protected set; }

    public ObjectSerializerSettings SerializerSettings { get; set; }

    #endregion


    public RamoneSession(IService service)
    {
      Service = service;
      UserAgent = service.UserAgent;
      BaseUri = Service.BaseUri;
      DefaultEncoding = service.DefaultEncoding;
      DefaultRequestMediaType = service.DefaultRequestMediaType;
      DefaultResponseMediaType = service.DefaultResponseMediaType;
      Cookies = new CookieContainer();
      AuthorizationDispatcher = service.AuthorizationDispatcher.Clone();
      RequestInterceptors = service.RequestInterceptors.Clone();
      SerializerSettings = new ObjectSerializerSettings(service.SerializerSettings);
    }
  }
}
