<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Event>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Seat Codes for <%= Html.ActionLink(Model.Name, "Seating", new { id = Model.EventID }) %></h2>

    <table>
        <tr>
            <th>
                SeatID
            </th>
            <th>
                Row
            </th>
            <th>
                Column
            </th>
            <th>
                EventID
            </th>
            <th>
                Code
            </th>
        </tr>

    <% foreach (var item in Model.Seats) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.SeatID) %>
            </td>
            <td>
                <%= Html.Encode(item.Row) %>
            </td>
            <td>
                <%= Html.Encode(item.Column) %>
            </td>
            <td>
                <%= Html.Encode(item.EventID) %>
            </td>
            <td>
                <%= Html.Encode(item.Code) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

