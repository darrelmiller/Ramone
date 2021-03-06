﻿using System;
using System.IO;
using System.Text;
using Ramone.Utility.ObjectSerialization;


namespace Ramone.Utility
{
  public class MultipartFormDataSerializer
  {
    protected ObjectSerializer Serializer;


    public MultipartFormDataSerializer(Type t)
    {
      Serializer = new ObjectSerializer(t);
    }


    public void Serialize(Stream s, object data, Encoding encoding = null, string boundary = null, ObjectSerializerSettings settings = null)
    {
      MultipartFormDataPropertyVisitor v = new MultipartFormDataPropertyVisitor(s, encoding, boundary, settings);
      Serializer.Serialize(data, v, settings);
    }
  }
}
