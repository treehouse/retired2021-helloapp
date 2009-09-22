<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Event>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Events</h2>

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
            <th></th>
            <th></th>
            <th></th>
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
            <td>
                <%= Html.ActionLink("Sessions", "Index", "SessionAdmin", new { id = item.EventID }, null)%>
            </td>
            <td>
                <%= Html.ActionLink("Seating", "Seating", new { id = item.EventID }) %>
            </td>
            <td>
                <% if (item.Start > DateTime.Now) { %>
                    <% using (Html.BeginForm("Delete", "EventAdmin", new { id = item.EventID })) { %>
                        <input type="submit" value="Delete" />
                    <% } %>
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create an Event", "Create")%>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

