<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Session>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Session at <%= Html.ActionLink(Model.Event.Name, "Index", "SessionAdmin", new { id = Model.Event.EventID }, null) %></h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="EventID">Event:</label>
                <%= Html.Hidden("EventID", Model.EventID) %>
                <%= Model.Event.Name %>
            </p>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", Model.Name) %>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Start">Start:</label>
                <%= Html.TextBox("Start", String.Format("{0:g}", Model.Start)) %>
                <%= Html.ValidationMessage("Start", "*") %>
            </p>
            <p>
                <label for="Finish">Finish:</label>
                <%= Html.TextBox("Finish", String.Format("{0:g}", Model.Finish)) %>
                <%= Html.ValidationMessage("Finish", "*") %>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

