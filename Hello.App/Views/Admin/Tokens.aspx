<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Token>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var campaign = (Campaign)ViewData["Campaign"]; %>

    <h2>Tokens for <%= Html.ActionLink(campaign.Name, "Campaigns") %></h2>

    <table>
        <tr>
            <th>
                TokenID
            </th>
            <th>
                Campaign
            </th>
            <th>
                Code
            </th>
            <th>
                Allowed Redemptions
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.TokenID) %>
            </td>
            <td>
                <%= Html.Encode(item.Campaign.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.Code) %>
            </td>
            <td>
                <%= Html.Encode(item.AllowedRedemptions) %>
            </td>
        </tr>
    
    <% } %>

    </table>
    
    <h3>Create a Token</h3>
    
    <% using (Html.BeginForm()) {%>

        <%= Html.Hidden("CampaignID", campaign.CampaignID)%>
        <p>
            <label for="Code">Code:</label>
            <%= Html.TextBox("Code", ViewData["Code"]) %>
            <%= Html.ActionLink("New Code", "Tokens") %>
        </p>
        <p>
            <label for="AllowedRedemptions">Allowed Redemptions:</label>
            <%= Html.TextBox("AllowedRedemptions") %>
        </p>
        <p>
            <input type="submit" value="Create" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

