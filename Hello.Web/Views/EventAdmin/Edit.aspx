<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Event>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="EventID">EventID:</label>
                <%= Html.Hidden("EventID", Model.EventID) %>
                <%= Model.EventID %>
            </p>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", Model.Name) %>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Slug">Slug:</label>
                <%= Html.TextBox("Slug", Model.Slug) %>
                <%= Html.ValidationMessage("Slug", "*") %>
            </p>
            <p>
                <label for="Start">Start:</label>
                <%= Html.TextBox("Start", String.Format("{0:g}", Model.Start)) %>
                <%= Html.ValidationMessage("Start", "*") %>
            </p>
            <p>
                <label for="End">End:</label>
                <%= Html.TextBox("End", String.Format("{0:g}", Model.End)) %>
                <%= Html.ValidationMessage("End", "*") %>
            </p>
            <p>
                <label for="HiFiveLimit">HiFiveLimit:</label>
                <%= Html.TextBox("HiFiveLimit", Model.HiFiveLimit) %>
                <%= Html.ValidationMessage("HiFiveLimit", "*") %>
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

