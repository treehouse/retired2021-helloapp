<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <p>
        <%= Html.ActionLink("Events", "Events") %>
    </p>
    
    <p>
        <%= Html.ActionLink("Sessions", "Sessions")%>
    </p>
    
    <p>
        <%= Html.ActionLink("Messages", "Messages")%>
    </p>
    
    <p>
        <%= Html.ActionLink("Campaigns", "Campaigns")%>
    </p>
    
</asp:Content>
