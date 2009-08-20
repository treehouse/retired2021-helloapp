<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Message>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Messages</h2>

    <table>
        <tr>
            <th>
                Username
            </th>
            <th>
                Text
            </th>
            <th>
                Offensive
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Username) %>
            </td>
            <td>
                <%= Html.Encode(item.Text) %>
            </td>
            <td>
                <%= Html.Encode(item.Offensive) %>
            </td>
            <td>
                <% using (Html.BeginForm()) { %>
                    <%= Html.Hidden("username", item.Username) %>
                    <input type="submit" value="Toggle" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

