<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Seating for <%= Html.ActionLink(((Event)ViewData["Event"]).Name, "Events") %></h2>
    
    <h3>Current Seating Plan</h3>
    
    <pre><%= (string)ViewData["SeatingPlan"] %></pre>
    
    <h3>New Seating Plan</h3>
    
    <% using (Html.BeginForm()) { %>
    
        <p>
            <%= Html.TextArea("seating", (string)ViewData["SeatingPlan"], 30, 120, null) %>
        </p>
        <p>
            <input type="submit" value="Ok" />
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
