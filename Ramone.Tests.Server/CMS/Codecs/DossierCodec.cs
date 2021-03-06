﻿using System.Xml;
using System.Xml.Serialization;
using OpenRasta.Codecs;
using Ramone.Tests.Common.CMS;
using Ramone.Tests.Server.Codecs;


namespace Ramone.Tests.Server.CMS.Codecs
{
  [MediaType(CMSConstants.CMSMediaTypeId)]
  [MediaType("application/xml")]
  public class DossierCodec : XmlCodecBase<Dossier>
  {
    XmlSerializer Serializer = new XmlSerializer(typeof(Dossier));


    protected override void WriteTo(Dossier d, XmlWriter writer)
    {
      Serializer.Serialize(writer, d);
    }


    protected override Dossier ReadFrom(XmlReader reader)
    {
      return (Dossier)Serializer.Deserialize(reader);
    }
  }
}