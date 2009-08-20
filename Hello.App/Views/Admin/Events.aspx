<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Event>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Events</h2>

    <table>
        <tr>
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
                Hi5 Limit
            </th>
            <th></th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
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
                <%= Html.ActionLink("Sessions", "Sessions", new { id = item.EventID }) %>
            </td>
            <td>
                <%= Html.ActionLink("Seating", "Seating", new { id = item.EventID })%>
            </td>
            <td>
                <% using (Html.BeginForm("DeleteEvent", "Admin", new { id = item.EventID })) { %>
                    <input type="submit" value="Delete" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>
    
    <h3>Create an Event</h3>
    
    <% using (Html.BeginForm()) {%>

        <p>
            <label for="Name">Name:</label>
            <%= Html.TextBox("Name") %>
            <%= Html.ValidationMessage("Name", "*") %>
        </p>
        <p>
            <label for="Slug">Slug:</label>
            <%= Html.TextBox("Slug") %>
            <%= Html.ValidationMessage("Slug", "*") %>
        </p>
        <p>
            <label for="Start">Start:</label>
            <%= Html.TextBox("Start") %>
            <%= Html.ValidationMessage("Start", "*") %>
        </p>
        <p>
            <label for="End">End:</label>
            <%= Html.TextBox("End") %>
            <%= Html.ValidationMessage("End", "*") %>
        </p>
        <p>
            <label for="HiFiveLimit">Hi5 Limit:</label>
            <%= Html.TextBox("HiFiveLimit") %>
            <%= Html.ValidationMessage("HiFiveLimit", "*") %>
        </p>
        <p>
            <input type="submit" value="Create" />
        </p>

    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
