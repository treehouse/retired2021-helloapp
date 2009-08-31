<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Session>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var theEvent = (Event)ViewData["Event"]; %>

    <h2>Sessions for <%= Html.ActionLink(theEvent.Name, "Index", "EventAdmin") %></h2>

    <table>
        <tr>
            <th></th>
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
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.SessionID }) %>
            </td>
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
                <% using (Html.BeginForm("Delete", "SessionAdmin", new { id = item.SessionID })) { %>
                    <input type="submit" value="Delete" />
                <% } %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create a Session", "Create", new { id = theEvent.EventID })%>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

