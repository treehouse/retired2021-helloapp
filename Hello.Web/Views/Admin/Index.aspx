<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <p>
        <%= Html.ActionLink("Events", "Index", "EventAdmin") %>
    </p>
    
    <p>
        <%= Html.ActionLink("Messages", "Messages") %>
    </p>
    
    <p>
        <%= Html.ActionLink("Campaigns", "Index", "CampaignAdmin") %>
    </p>
    
    <p>
        <%= Html.ActionLink("Status", "Status") %>
    </p>
    
</asp:Content>
