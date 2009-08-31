<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Event>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index</h2>

    <table>
        <tr>
            <th></th>
            <th>
                EventID
            </th>
            <th>
                Name
            </th>
            <th>
                Slug
            </th>
            <th>
                Start
            </th>
            <th>
                End
            </th>
            <th>
                HiFiveLimit
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.EventID }) %>
            </td>
            <td>
                <%= Html.Encode(item.EventID) %>
            </td>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.Slug) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.Start)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.End)) %>
            </td>
            <td>
                <%= Html.Encode(item.HiFiveLimit) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

