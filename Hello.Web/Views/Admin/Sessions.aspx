<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Session>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var theEvent = (Event)ViewData["Event"]; %>

    <h2>Sessions for <%= Html.ActionLink(theEvent.Name, "Events") %></h2>

    <table>
        <tr>
            <th>
                SessionID
            </th>
            <th>
                Name
            </th>
            <th>
                Start
            </th>
            <th>
                Finish
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.SessionID) %>
            </td>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.Start)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.Finish)) %>
            </td>
            <td>
                <% using (Html.BeginForm("DeleteSession", "Admin", new { id = item.SessionID })) { %>
                    <input type="submit" value="Delete" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>
    
    <h3>Create a Session</h3>
    
    <% using (Html.BeginForm()) { %>
    
        <%= Html.Hidden("EventID", theEvent.EventID)%>
        <p>
            <label for="Name">Name:</label>
            <%= Html.TextBox("Name") %>
        </p>
        <p>
            <label for="Start">Start:</label>
            <%= Html.TextBox("Start") %>
        </p>
        <p>
            <label for="Finish">Finish:</label>
            <%= Html.TextBox("Finish") %>
        </p>
        <p>
            <input type="submit" value="Create" />
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

