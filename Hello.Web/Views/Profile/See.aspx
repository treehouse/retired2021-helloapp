<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.User>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/styles/Profile.css" rel="stylesheet" type="text/css" />    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $.getJSON('http://twitter.com/statuses/user_timeline/<%= Model.Username %>.json?count=1&callback=?',
                function(tweets) {
                    $('#latestTweet').text('Latest tweet: ' + tweets[0].text);
                }
            );
        });
    </script>
    
    <%
        var hiFivers = ViewData["hiFivers"] as List<String>;
        var hiFivees = ViewData["hiFivees"] as List<String>;

        var friends = ViewData["friends"] as List<Friendship>;
        var following = ViewData["following"] as List<String>;
        var followers = ViewData["followers"] as List<String>;
    %>    
    <div class="narrowContent">
        <div class="profileBox" style="width: 500px;">
            <div class="twitterProfile">
                <img height="73px" src="<%= Model.ImageURL %>" alt="<%= Model.Username %>"/>
                <div class="bio">
                    <p>@<a href="http://twitter.com/<%= Model.Username %>" class="twitterLink"><%= Model.Username %></a> has <%= ViewData["pointsTotal"] %> points (<a href="http://twitter.com/?status=%40HelloApp+met+%40<%= Model.Username %>">Im following <%= Model.Username %></a>)</p>
                    <p id="latestTweet"></p>
                </div>
            </div>                        
            <ul class="categories">
            <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
                <li class="<%= ut.UserTypeID %> <% if (ut.UserTypeID == Model.UserTypeID) { %>selected <%}; %>"><%= ut.Name %></li>
            <% } %>
            </ul>
            <ul class="badgeList">
                <% int currentPoints = (int)ViewData["pointsTotal"]; %>
                <li style="display: <% if (currentPoints >= Settings.Thresholds.Bronze) { %> list-item <% } else { %> none <% }%>;" class="bronze" title="Bronze Badge">Bronze Badge</li>
                <li style="display: <% if (currentPoints >= Settings.Thresholds.Silver) { %> list-item <% } else { %> none <% }%>;" class="silver" title="Silver Badge">Silver Badge</li>
			    <li style="display: <% if (currentPoints >= Settings.Thresholds.Gold) { %> list-item <% } else { %> none <% }%>;" class="gold" title="Gold Badge">Gold Badge</li>
			    <li style="display: <% if (currentPoints >= Settings.Thresholds.Diamond) { %> list-item <% } else { %> none <% }%>;" class="diamond" title="Diamond Badge">Diamond Badge</li>
			    <li style="display: <% if (hiFivers.Count > 0) { %> list-item <% } else { %> none <% }%>;" class="highFive" title="Hi5 Badge">Hi5 Badge</li>
			    <li style="display: <% if (currentPoints >= Settings.Thresholds.Smiley) { %> list-item <% } else { %> none <% }%>;" class="smiley" title="Smiley Badge">Smiley Badge</li>
			    
            </ul>
            <div id="tokenImages">
            <% foreach (Token t in ViewData["redeemedTokens"] as List<Token>) { %>
                <img class="tokenImage" src="/Content/images/tokens/<%= t.FileName %>" alt="<%= t.Text %>" title="<%= t.Text %>" />
            <% } %>
            </div>
            <p class="tagPara">
            <% foreach (String tag in ViewData["tags"] as List<String>) { %>
                #<%= Html.ActionLink(tag, "Index", new { controller = "Search", search = tag })%> 
            <% } %>
            </p>
        </div>
    </div>
    <div class="castWrapper">
        <h4 class="top">Friends</h4>
        <h5>@<%= Model.Username %> has met these people and they've returned the favor</h5>
    <% if (friends.Count > 0) { %>
        <ul class="friends">
        <% foreach (Friendship f in friends) { %>
            <li>
                <a href="/Profile/See/<%= f.User1.Username %>"><img src="<%= f.User1.ImageURL %>" alt="<%= f.User1.Username %>" height="24" width="24" /></a>
            </li>            
        <% } %>
        </ul>
    <% } else {%>
        <p class="nada">No one yet, they must be shy. Somebody give 'em a hug!</p>
    <% } %>
        <h4>People that met @<%= Model.Username %></h4>
        <h5>... but he hasn't returned the favor</h5>
    <% if (followers.Count > 0 ) {%>
        <ul class="subNav people">
        <% foreach (String username in followers) {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } else { %>
        <p class="nada">Nada, but hopefully it'll happen soon.</p>
    <% } %>
        <h4>People @<%= Model.Username %> has met</h4>
        <h5>... but they haven't returned the favor</h5>
    <% if (following.Count > 0) { %>
        <ul class="subNav people">
        <% foreach (String username in following)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } else {%>
        <p class="nada">... Nothing yet</p>
    <% } %>
    <% if (hiFivees.Count > 0) { %>
        <h4 class="border">People that @<%= Model.Username %> has given a High Five to</h4>
        <ul class="subNav people">
        <% foreach (String username in hiFivees)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } else { %>
        <h4 class="border">People that @<%= Model.Username %> has High Fived</h4>
        <p class="nada">Zippo. Seriously @<%= Model.Username %>, theres got to be someone :)</p>
    <% } %>
        <h4 class="border">People that have given @<%= Model.Username %> a High Five</h4>
    <% if (hiFivers.Count > 0) { %>
        <ul class="subNav people">
        <% foreach (String username in hiFivers)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } else { %>
        <p class="nada">Come on people. That's just mean. Get High Fivin!</p>
    <% } %>
    <h4>@<%= Model.Username %>'s latest message</h4>
    <h5>Silver badge required</h5>
        <% if (Model.Message != null) { %>
            <h6><%= Model.Message.Text%></h6>
        <% } else {%>
            <p class="nada">Nothing yet ... but watch this space!</p>
        <% } %>
    </div>
</asp:Content>