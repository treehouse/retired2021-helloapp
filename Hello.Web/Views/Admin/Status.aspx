<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.TideMark>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Status</h2>
    
    <h3>Queued Tweets</h3>
    
    <table>
        <tr>
            <td>
                Processed:
            </td>
            <td>
                <%= ViewData["ProcessedTweets"] %>
            </td>
        </tr>
        <tr>
            <td>
                Unprocessed:
            </td>
            <td>
                <%= ViewData["UnprocessedTweets"] %>
            </td>
        </tr>
    </table>


    <h3>Tide Marks</h3>

    <table>
        <tr>
            <th>
                Name
            </th>
            <th>
                LastID
            </th>
            <th>
                TimeStamp
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.LastID) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.TimeStamp)) %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

