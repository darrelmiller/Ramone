﻿using NUnit.Framework;
using Ramone.HyperMedia;
using Ramone.Tests.Common.CMS;
using System.Collections.Generic;
using System;


namespace Ramone.Tests
{
  [TestFixture]
  public class SubmitTests : TestHelper
  {
    Request DossierReq;


    protected override void SetUp()
    {
      base.SetUp();
      DossierReq = Session.Bind(VerifiedMethodDossierTemplate, new { method = "GET", id = 8 });
    }

    
    [Test]
    public void CanRememberGetForNextSubmit_generic()
    {
      // Act
      using (var r = DossierReq.Method("Get").Submit<Dossier>())
      {
        // Assert
        Assert.AreEqual(8, r.Body.Id);
      }
    }


    [Test]
    public void CanRememberGetForNextSubmit_generic_async()
    {
      // Act
      TestAsync(wh =>
        {
          DossierReq.Method("GET").Async()
            .OnError(e => Assert.Fail())
            .OnComplete(() => wh.Set())
            .Submit<Dossier>(r => 
            {
              Assert.AreEqual(8, r.Body.Id);
            });
        });
    }


    [Test]
    public void CanRememberGetForNextSubmitWithEmptyHandler_generic_async()
    {
      // Act
      TestAsync(wh =>
      {
        DossierReq.Method("GET").Async()
          .OnError(e => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Submit<Dossier>();
      });
    }


    [Test]
    public void CanRememberGetForNextSubmitWithEmptyHandler_untyped_async()
    {
      // Act
      TestAsync(wh =>
      {
        DossierReq.Method("GET").Async()
          .OnError(e => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Submit();
      });
    }


    [Test]
    public void WhenNoMethodIsSetThenSubmitThrows_generic()
    {
      // Arrange
      Request dossierReq = Session.Bind(DossierTemplate, new { id = 8 });

      // Act
      AssertThrows<InvalidOperationException>(() => dossierReq.Submit<Dossier>());
    }


    [Test]
    public void CanRememberGetForNextSubmit_untyped()
    {
      // Act
      using (var r = DossierReq.Method("Get").Submit())
      {
        Dossier dossier = r.Decode<Dossier>();

        // Assert
        Assert.AreEqual(8, dossier.Id);
      }
    }


    [Test]
    public void CanRememberGetForNextSubmit_untyped_async()
    {
      // Act
      TestAsync(wh =>
      {
        DossierReq.Method("GET").Async()
          .OnError(e => Assert.Fail())
          .OnComplete(() => wh.Set())
          .Submit(r =>
          {
            Dossier dossier = r.Decode<Dossier>();
            Assert.AreEqual(8, dossier.Id);
          });
      });
    }


    [Test]
    public void WhenNoMethodIsSetThenSubmitThrows_untyped()
    {
      // Act
      AssertThrows<InvalidOperationException>(() => DossierReq.Submit());
    }
  }
}
