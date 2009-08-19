<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.Event>>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Events</title>
</head>
<body>

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
                HiFiveLimit
            </th>
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
        </tr>
    
    <% } %>

    </table>

</body>
</html>

