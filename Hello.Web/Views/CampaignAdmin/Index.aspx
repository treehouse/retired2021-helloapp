<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Campaign>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Campaigns</h2>

    <table>
        <tr>
            <th></th>
            <th>
                CampaignID
            </th>
            <th>
                Name
            </th>
            <th>
                Value
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.CampaignID }) %>
            </td>
            <td>
                <%= Html.Encode(item.CampaignID) %>
            </td>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.Value) %>
            </td>
            <td>
                <%= Html.ActionLink("Tokens", "Index", "TokenAdmin", new { id = item.CampaignID }, null) %>
            </td>
            <td>
                <% using (Html.BeginForm("Delete", "CampaignAdmin", new { id = item.CampaignID })) { %>
                    <input type="submit" value="Delete" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create a Campaign", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

