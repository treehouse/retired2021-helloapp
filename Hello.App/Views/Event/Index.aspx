<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Event>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="TopContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.Encode(Model.Name) %></h2>

</asp:Content>
