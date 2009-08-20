<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Session>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Sessions for <%= Html.ActionLink(((Event)ViewData["Event"]).Name, "Events") %></h2>

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
    
    <h3>Create a new Session</h3>
    
    <% using (Html.BeginForm()) { %>
    
        <%= Html.Hidden("EventID", ((Event)ViewData["Event"]).EventID) %>
        <p>
            <label for="Name">Name:</label>
            <%= Html.TextBox("Name") %>
            <%= Html.ValidationMessage("Name", "*") %>
        </p>
        <p>
            <label for="Start">Start:</label>
            <%= Html.TextBox("Start") %>
            <%= Html.ValidationMessage("Start", "*") %>
        </p>
        <p>
            <label for="Finish">Finish:</label>
            <%= Html.TextBox("Finish") %>
            <%= Html.ValidationMessage("Finish", "*") %>
        </p>
        <p>
            <input type="submit" value="Create" />
        </p>
    
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

