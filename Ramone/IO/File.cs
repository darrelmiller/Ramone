﻿using System;
using System.IO;


namespace Ramone.IO
{
  public class File : IFile
  {
    #region IFile Members

    public string Filename { get; protected set; }


    public Stream OpenStream()
    {
      return new FileStream(Filename, FileMode.Open);
    }

    #endregion


    public File(string filename)
    {
      Filename = filename;
    }
  }
}
