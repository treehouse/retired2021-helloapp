<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Log In</h1>

    <% if ((bool?)ViewData["lastLoginFailed"] == true) { %>
        <div class="Message">
            Sorry, your login attempt failed. Please try again.
        </div>
    <% } %>
    
    <p>Please log in to access the administrative area:</p>
    <% using (Html.BeginForm()) { %>
        <p>
            <label for="name">Login name:</label>
            <%= Html.TextBox("name") %>
        </p>
        <p>
            <label for="password">Password:</label>
            <%= Html.Password("password") %>
        </p>
        <p><input type="submit" value="Log in" /></p>
    <% } %>
</asp:Content>
