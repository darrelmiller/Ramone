﻿using System.ServiceModel.Syndication;
using NUnit.Framework;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Common;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class PostTests : TestHelper
  {
    Dossier MyDossier = new Dossier
    {
      Title = "A new dossier"
    };

    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { method = "POST", id = 8 });
    }


    [Test]
    public void CanPostAndIgnoreReturnedBody()
    {
      // Act
      using (Response response = DossierReq.Post(MyDossier))
      {
        // Assert
        Assert.IsNotNull(response);
      }
    }


    [Test]
    public void CanPostAndGetResult()
    {
      // Act
      using (Response<Dossier> response = DossierReq.Post<Dossier>(MyDossier))
      {
        Dossier newDossier = response.Body;

        // Assert
        Assert.IsNotNull(newDossier);
      }
    }


    [Test]
    public void WhenPostingEmptyDataAsyncTheRequestIsInFactAsync()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath);
      TimeSpan asyncTime = TimeSpan.MaxValue;
      TimeSpan syncTime = TimeSpan.MinValue;
      SlowResource result = null;

      TestAsync(wh =>
      {
        DateTime t1 = DateTime.Now;

        // Act
        request.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Post(response =>
          {
            syncTime = DateTime.Now - t1;
            result = response.Decode<SlowResource>();
          });

        asyncTime = DateTime.Now - t1;
      });

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(4, result.Time);
      Assert.Greater(syncTime, TimeSpan.FromSeconds(3), "Request takes at least 4 seconds - 3 should be a safe test");
      Assert.Less(asyncTime, TimeSpan.FromSeconds(1), "Async should be instantaneous - 1 second should be safe");
    }


    [Test]
    public void WhenPostingAsyncTheRequestIsInFactAsync()
    {
      // Arrange
      Request request = Session.Bind(Constants.SlowPath).AsJson();
      TimeSpan asyncTime = TimeSpan.MaxValue;
      TimeSpan syncTime = TimeSpan.MinValue;
      SlowResource input = new SlowResource { Time = 10 };
      SlowResource result = null;

      TestAsync(wh =>
      {
        DateTime t1 = DateTime.Now;

        // Act
        request.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Post<SlowResource>(input, response =>
          {
            syncTime = DateTime.Now - t1;
            result = response.Body;
          });

        asyncTime = DateTime.Now - t1;
      });

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual(input.Time, result.Time);
      Assert.Greater(syncTime, TimeSpan.FromSeconds(3), "Request takes at least 4 seconds - 3 should be a safe test");
      Assert.Less(asyncTime, TimeSpan.FromSeconds(1), "Async should be instantaneous - 1 second should be safe");
    }


    [Test]
    public void CanPostAndGetResultWithAccept()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>().Post(MyDossier))
      {
        // Assert
        Assert.IsNotNull(newDossier.Body);
      }
    }


    [Test]
    public void CanPostAndGetResultWithAcceptMediaType()
    {
      // Act
      using (var newDossier = DossierReq.Accept<Dossier>(CMSConstants.CMSMediaType).Post(MyDossier))
      {
        // Assert
        Assert.IsNotNull(newDossier.Body);
      }
    }


    [Test]
    public void CanPostEmptyBody_Typed()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Accept("text/plain").ContentType("application/octet-stream").Post<string>())
      {
        // Assert
        Assert.IsNull(response.Body);
      }
    }


    [Test]
    public void CanPostEmptyBody_Typed_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").Async()
          .OnError(error => Assert.Fail())
          .Post<string>(r =>
          {
            wh.Set();
          });
      });
    }


    [Test]
    public void CanPostEmptyBodyWhenNoDefaultMediaTypeIsSpecified_Typed()
    {
      // Arrange
      Session.DefaultRequestMediaType = null;
      Session.DefaultResponseMediaType = null;
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response<string> response = request.Post<string>())
      {
        // Assert
        Assert.IsNull(response.Body);
      }
    }


    [Test]
    public void CanPostEmptyBody_Untyped()
    {
      // Arrange
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Accept("text/plain").ContentType("application/octet-stream").Post())
      {
        // Assert
        Assert.IsNull(response.Body);
      }
    }


    [Test]
    public void CanPostEmptyBody_Untyped_Async()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.ContentType("application/octet-stream").Async()
          .OnError(error => Assert.Fail())
          .Post(r =>
          {
            wh.Set();
          });
      });
    }


    [Test]
    public void CanPostEmptyBodyWhenNoDefaultMediaTypeIsSpecified_Untyped()
    {
      // Arrange
      Session.DefaultRequestMediaType = null;
      Session.DefaultResponseMediaType = null;
      Request request = Session.Bind(AnyEchoTemplate);

      // Act
      using (Response response = request.Post())
      {
        // Assert
        Assert.IsNull(response.Body);
      }
    }

    #region POST with null/empty callback handlers

    [Test]
    public void CanPostAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(MyDossier);
      });
    }


    [Test]
    public void CanPostEmptyAsyncWithoutHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post();
      });
    }


    [Test]
    public void CanPostAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>(MyDossier);
      });
    }


    [Test]
    public void CanPostEmptyAsyncWithoutHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>();
      });
    }


    [Test]
    public void CanPostAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(MyDossier, null);
      });
    }


    [Test]
    public void CanPostEmptyAsyncWithNullHandler()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post(null);
      });
    }


    [Test]
    public void CanPostAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>(MyDossier, null);
      });
    }


    [Test]
    public void CanPostEmptyAsyncWithNullHandler_Typed()
    {
      TestAsync(wh =>
      {
        // Act
        DossierReq.Async()
          .OnError(error => Assert.Fail())
          .OnComplete(() =>
          {
            wh.Set();
          }).Post<Dossier>(null);
      });
    }

    #endregion
  }
}
