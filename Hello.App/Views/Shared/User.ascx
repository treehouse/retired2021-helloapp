<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Hello.Repo.User>" %>

<div class="profileBox<% if (ViewData["Stripe"] != null) { %><%= (bool)ViewData["Stripe"] ? " right" : " left" %><% } %>">
    <div class="twitterProfile">
        <img height="73px" src="<%= Model.ImageURL %>" alt="<%= Model.Username %>" />
        <div class="bio">
            <p>@<a class="username" href="http://twitter.com/<%= Model.Username %>"><%= Model.Username %></a></p>
            <p id="latestTweet<%= Model.Username %>">Loading...</p>
        </div>
    </div>
    <ul class="categories">
        <% foreach (var ut in ViewData["UserTypes"] as List<UserType>)
           { %>
            <li class="<%= ut.UserTypeID == Model.UserTypeID ? "selected" : "" %>"><%= ut.Name %></li>
        <% } %>
    </ul>
    <p>
        <% foreach (var tag in Model.Tags.OrderByDescending(t => t.Created).Take(3)) { %>
            #<%= Html.ActionLink(tag.Name, "Index", new { search = tag.Name }) %>
        <% } %>
    </p>
</div>
