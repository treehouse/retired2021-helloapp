<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Token>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% var campaign = (Campaign)ViewData["Campaign"]; %>
    
    <h2>Create a Token for <%= Html.ActionLink(campaign.Name, "Index", "CampaignAdmin") %></h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm("Create", "TokenAdmin", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Code">Code:</label>
                <%= Html.TextBox("Code", ViewData["Code"]) %>
                <%= Html.ValidationMessage("Code", "*") %>
                <%= Html.ActionLink("New Code", "Create") %>
            </p>
            <p>
                <label for="AllowedRedemptions">Allowed Redemptions:</label>
                <%= Html.TextBox("AllowedRedemptions") %>
                <%= Html.ValidationMessage("AllowedRedemptions", "*") %>
            </p>
            <p>
                <label for="Text">Token Text:</label>
                <%= Html.TextBox("Text") %>
                <%= Html.ValidationMessage("Text", "*") %>
            </p>
            <p>
                <label for="image">Token Image:</label>
                <input type="file" name="image" id="image" />
            </p>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

