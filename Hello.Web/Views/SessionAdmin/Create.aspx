<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Session>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var theEvent = (Event)ViewData["Event"]; %>

    <h2>Create a Session for <%= Html.ActionLink(theEvent.Name, "Index", "SessionAdmin", new { id = theEvent.EventID }, null) %></h2>

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
        </fieldset>

    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

