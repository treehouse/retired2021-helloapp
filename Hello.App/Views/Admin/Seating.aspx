<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Seating</h2>
    
    <%= Model %>
    
    <% using (Html.BeginForm()) { %>

        <%= Html.TextArea("seating", String.Empty, 30, 120, null) %>
        
        <input type="submit" value="ok" />
    
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
