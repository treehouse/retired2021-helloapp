<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Token>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Token for </h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="CampaignID">Campaign:</label>
                <%= Html.Hidden("CampaignID", Model.CampaignID)%>
                <%= Html.ValidationMessage("CampaignID", "*") %>
            </p>
            <p>
                <label for="Code">Code:</label>
                <%= Html.TextBox("Code", Model.Code) %>
                <%= Html.ValidationMessage("Code", "*") %>
            </p>
            <p>
                <label for="AllowedRedemptions">AllowedRedemptions:</label>
                <%= Html.TextBox("AllowedRedemptions", Model.AllowedRedemptions) %>
                <%= Html.ValidationMessage("AllowedRedemptions", "*") %>
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

