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
    <ul class="profilebadges">
        <% var points = Model.Points.Sum(p => p.Amount); %>
        <% if (points >= Settings.Thresholds.Bronze) { %>
            <li>
                <img src="<%= Url.Content("~/Content/images/content/bronze.png") %>" alt="Bronze Badge" />
            </li>
        <% } %>
        <% if (points >= Settings.Thresholds.Silver) { %>
            <li>
                <img src="<%= Url.Content("~/Content/images/content/silver.png") %>" alt="Silver Badge" />
            </li>
        <% } %>
        <% if (points >= Settings.Thresholds.Gold) { %>
            <li>
                <img src="<%= Url.Content("~/Content/images/content/gold.png") %>" alt="Gold Badge" />
            </li>
        <% } %>
        <% if (points >= Settings.Thresholds.Diamond) { %>
            <li>
                <img src="<%= Url.Content("~/Content/images/content/diamond.png") %>" alt="Diamond Badge" />
            </li>
        <% } %>
        <% var hiFives = Model.HiFivees.Count(); %>
        <% if (hiFives > 0) { %>
            <li>
                <img src="<%= Url.Content("~/Content/images/content/high_five.png") %>" alt="Hi5" />
                <% if (hiFives > 1) { %>
                    <span>x <%= hiFives %></span>
                <% } %>
            </li>
        <% } %>
        <% if (Model.Friendships().Count() >= Settings.Thresholds.Smiley) { %>
            <li>
                <img src="<%= Url.Content("~/Content/images/content/smiley.png") %>" alt="Smiley" />
            </li>
        <% } %>
    </ul>
    <ul class="categories">
        <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
            <li class="<%= ut.UserTypeID == Model.UserTypeID ? "selected" : "" %>"><%= ut.Name%></li>
        <% } %>
    </ul>
    <p>
        <% foreach (var tag in Model.Tags.OrderByDescending(t => t.Created).Take(3)) { %>
            #<%= Html.ActionLink(tag.Name, "Index", new { search = tag.Name })%>
        <% } %>
    </p>
</div>
