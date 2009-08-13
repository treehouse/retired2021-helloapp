<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="joinOrSearch">
    <%= Html.ActionLink("Join In", "Join", "Home", null, new { @class = "joinInSticker" })%>
    <% using (Html.BeginForm("Index", "Search", FormMethod.Get, new { id = "search" })) { %>
        <p><label><span>or</span> Search:</label><input type="text" id="searchTerm" name="search" value="<%= Html.Encode(ViewData["SearchTerm"]) %>" /></p>
        <p><input type="submit" id="submit" /></p>
    <% } %>
</div>