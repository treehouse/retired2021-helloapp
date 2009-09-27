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
        <% if (Model.Message != null) { %>
            <h4><%= Model.Message.Text%></h4>
        <% } %>
        <h5 class="top">friends</h5>
    <% if (friends.Count > 0) { %>
        <ul class="friends">
        <% foreach (Friendship f in friends) { %>
            <li>
                <a href="/Profile/See/<%= f.User1.Username %>"><img src="<%= f.User1.ImageURL %>" alt="<%= f.User1.Username %>" height="24" width="24" /></a>
            </li>            
        <% } %>
        </ul>
    <% } else {%>
        <p>I have not made any friends yet.</p>
    <% } %>
    <% if (followers.Count > 0 ) {%>
        <h5>people that met me</h5>
        <ul class="subNav people">
        <% foreach (String username in followers)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } %>
    <% if (following.Count > 0) { %>
        <h5>people I've met</h5>
        <ul class="subNav people">
        <% foreach (String username in following)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% }%>
        <h5>HiFivers</h5>
    <% if (hiFivers.Count > 0) { %>
        <ul class="subNav people">
        <% foreach (String username in hiFivers)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } else { %>
        <p>I have not been HiFived</p>
    <% } %>
        <h5>HiFived</h5>
    <% if (hiFivees.Count > 0) { %>
        <ul class="subNav people">
        <% foreach (String username in hiFivees)
           {%>
        <li><%= Html.ActionLink(username, "See", new { id = username })%></li>
        <% } %>
        </ul>
    <% } else { %>
        <p>I have not HiFived anyone</p>
    <% } %>    
    </div>
</asp:Content>