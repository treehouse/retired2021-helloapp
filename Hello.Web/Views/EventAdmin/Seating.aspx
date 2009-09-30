<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var theEvent = (Event)ViewData["Event"]; %>
    <% var seatingPlan = (string)ViewData["SeatingPlan"]; %>

    <h2>Seating for <%= Html.ActionLink(theEvent.Name, "Index", "EventAdmin") %></h2>
    
    <% if (seatingPlan.Length > 0) { %>
    
        <h3>Current Seating Plan</h3>
        
        <%= Html.ActionLink("Seat Codes", "SeatCodes", new { id = theEvent.EventID }) %>
        
        <pre><%= seatingPlan %></pre>
    
    <% } %>
    
    <h3>New Seating Plan</h3>
    
    <h4>Key</h4>
    
    <p>
        <code>x</code> = seat
        <br />
        <code>.</code> = gap (e.g. aisle)
    </p>
    
    <% using (Html.BeginForm()) { %>
    
        <p>
            <%= Html.TextArea("seating", seatingPlan, 30, 120, null) %>
        </p>
        <p>
            <input type="submit" value="Ok" />
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
