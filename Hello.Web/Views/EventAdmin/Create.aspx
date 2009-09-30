<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Event>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create an Event</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>

        <fieldset>
            <legend>Fields</legend>
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
                <label for="HiFiveLimit">HiFiveLimit:</label>
                <%= Html.TextBox("HiFiveLimit") %>
                <%= Html.ValidationMessage("HiFiveLimit", "*") %>
            </p>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%= Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

