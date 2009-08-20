<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Seating for <%= Html.ActionLink(((Event)ViewData["Event"]).Name, "Events") %></h2>
    
    <% using (Html.BeginForm()) { %>
    
        <p>
            <%= Html.TextArea("seating", String.Empty, 30, 120, null) %>
        </p>
        <p>
            <input type="submit" value="Ok" />
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
