<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Token>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var campaign = (Campaign)ViewData["Campaign"]; %>
    
    <h2>Tokens for <%= Html.ActionLink(campaign.Name, "Index", "CampaignAdmin") %></h2>

    <table>
        <tr>
            <th>
                TokenID
            </th>
            <th>
                Code
            </th>
            <th>
                Allowed Redemptions
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.TokenID) %>
            </td>
            <td>
                <%= Html.Encode(item.Code) %>
            </td>
            <td>
                <%= Html.Encode(item.AllowedRedemptions) %>
            </td>
            <td>
                <% using (Html.BeginForm("Delete", "TokenAdmin", new { id = item.TokenID })) { %>
                    <input type="submit" value="Delete" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create a Token", "Create", new { id = campaign.CampaignID }) %>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

