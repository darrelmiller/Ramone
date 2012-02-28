﻿<%@ Import Namespace="Ramone.Tests.Server.Blog.Resources" %>
<%@ Page Language="C#" Inherits="OpenRasta.Codecs.WebForms.ResourceView<Author>" %>
<html>
  <body>
    <h1>Author: <span class="name"><%=Resource.Name %></span></h1>
    <p>E-mail: <a rel="email" href="<%=Resource.EMail %>"><%=Resource.EMail %></a></p>
    <p><a href="<%=Resource.SelfLink %>" rel="self">Permalink</a></p>
  </body>
</html>