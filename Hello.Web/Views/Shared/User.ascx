<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Hello.Repo.User>" %>

<div class="profileBox<% if (ViewData["Stripe"] != null) { %><%= (bool)ViewData["Stripe"] ? " right" : " left" %><% } %>">
    <div class="twitterProfile">
        <img height="73px" src="<%= Model.ImageURL %>" alt="<%= Model.Username %>" />
        <div class="bio">
            <p>@<a class="username" href="http://twitter.com/<%= Model.Username %>"><%= Model.Username%></a></p>
            <% if (ViewData["DelayTweetLoad"] == null || (bool)ViewData["DelayTweetLoad"] == false) { %>
            <p id="latestTweet<%= Model.Username %>">Loading...</p>
            <% } else { %>
            <p id="latestTweet<%= Model.Username %>" style="display: none">Loading...</p>
            <p id="showTweet<%= Model.Username %>" style="cursor:pointer; text-decoration: underline" onclick="$('#latestTweet<%= Model.Username %>').show(); $('#showTweet<%= Model.Username %>').hide(); fetchTweets()">View latest tweet</p>
            <% } %>
        </div>
    </div>
    <ul class="categories">
        <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
            <li class="<%= ut.UserTypeID == Model.UserTypeID ? "selected" : "" %>"><%= ut.Name%></li>
        <% } %>
    </ul>
    <ul class="badgeList">
        <% var points = Model.Points.Sum(p => p.Amount); %>
        <% if (points >= Settings.Thresholds.Bronze) { %>
            <li title="Bronze Badge" class="bronze">Bronze Badge</li>
        <% } %>
        <% if (points >= Settings.Thresholds.Silver) { %>
            <li title="Silver Badge" class="silver">Silver Badge</li>
        <% } %>
        <% if (points >= Settings.Thresholds.Gold) { %>
			<li title="Gold Badge" class="gold">Gold Badge</li>
        <% } %>
        <% if (points >= Settings.Thresholds.Diamond) { %>
			<li title="Diamond Badge" class="diamond">Diamond Badge</li>
        <% } %>
        <% var hiFives = Model.HiFivees.Count(); %>
        <% if (hiFives > 0) { %>
			<li title="High Five Badge<% if (hiFives > 1) { %> x <%= hiFives %><% } %>" class="highFive">High Five Badge</li>
        <% } %>
        <% if (Model.Friends.Count() >= Settings.Thresholds.Smiley) { %>
			<li title="Smiley Badge" class="smiley">Smiley Badge</li>
        <% } %>
    </ul>
    <p class="tagPara">
        <% foreach (var tag in Model.Tags.OrderByDescending(t => t.Created).Take(3)) { %>
            #<%= Html.ActionLink(tag.Name, "Index", new { search = tag.Name })%>
        <% } %>
    </p>
</div>
