<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Campaign>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Campaigns</h2>

    <table>
        <tr>
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
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
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
                <%= Html.ActionLink("Tokens", "Tokens", new { id = item.CampaignID }) %>
            </td>
            <td>
                <% using (Html.BeginForm("Delete", "Admin")) { %>
                    <%= Html.Hidden("campaignID", item.CampaignID)%>
                    <input type="submit" value="delete" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>
    
    <h3>Create a new Campaign</h3>

    <% using (Html.BeginForm()) {%>

        <p>
            <label for="Name">Name:</label>
            <%= Html.TextBox("Name") %>
        </p>
        <p>
            <label for="Value">Value:</label>
            <%= Html.TextBox("Value") %>
        </p>
        <p>
            <input type="submit" value="Create" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

