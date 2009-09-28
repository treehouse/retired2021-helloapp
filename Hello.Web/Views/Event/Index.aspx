<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Event>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function() {
            var seats = $('.seating td:has(form)');
            var popup = $('.avatarPopup');
            var popupSize = popup.size();
            seats.click(function() {
                var $this = $(this);

                var username = $('input[name=Username]', $this).val();

                // Image
                var img = $('img', popup);
                // HACK: to stop image glitching
                img.attr('src', '');
                img.attr('src', $('input[name=ImageURL]', $this).val());
                img.attr('alt', username);

                // Username
                var twitterLink = $('.twitterLink', popup);
                twitterLink.attr('href', '/Profile/See/' + username);
                twitterLink.text(username);

                // UserType
                var userType = $('input[name=UserType]', $this).val();
                $('.categories li').removeClass('selected');
                if (userType != '') {
                    $('.' + userType, popup).addClass('selected');
                }

                // Tags
                var tags = $('input[name=Tag]', $this);
                var tagHtml = '';
                tags.each(function(i, tagInput) {
                    var tag = $(tagInput).val();
                    tagHtml += '#<a href="<%= Url.Action("Search", "Event", new { searchTerm = " " }) %>' + tag + '">' + tag + '</a> ';
                });
                $('.tagPara', popup).html(tagHtml);

                // Badges
                $('.badgeList li', popup).hide();
                var badges = $('input[name=Badge]', $this);
                badges.each(function(i, badgeInput) {
                    var badge = $(badgeInput).val();
                    var badgeLi = $('.badgeList .' + badge, popup);
                    badgeLi.show();
                    if (badge == 'highFive') {
                        var hiFives = $('input[name=HiFives]', $this).val();
                        badgeLi.attr('title', badgeLi.text() + (hiFives > 1 ? ' x ' + hiFives : ''));
                    }
                });

                //Points
                var points = $('input[name=Points]', $this).val();
                $('.points', popup).text(points);                
                
                // Positioning
                var pos = $this.position();
                var height = popup.height();
                popup.css({
                    top: pos.top - height - 29,
                    left: (pos.left < 980 / 2) ? pos.left + 14 : pos.left - 428
                });

                // Latest Tweet
                var latestTweetInput = $('input[name=LatestTweet]', $this);
                if (latestTweetInput.length == 0) {
                    $('#latestTweet', popup).text('Loading...');
                    $.getJSON('http://twitter.com/statuses/user_timeline/' +
                        username +
                        '.json?count=1&callback=?',
                        function(tweets) {
                            $('form#' + tweets[0].user.screen_name.toLowerCase()).append('<input name="LatestTweet" value="' + tweets[0].text + '" type="hidden" />');
                            $('#latestTweet', popup).html(tweets[0].text);
                        }
                    );
                } else {
                    $('#latestTweet', popup).html(latestTweetInput.val());
                }

                // Display
                popup.fadeIn();
            });

            $('.close').click(function() {
                popup.fadeOut();
                return false;
            });

            popup.hide();
        });
	</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="TopContent" runat="server">

    <% if (ViewData["Message"] != null) { %>
        <% Html.RenderPartial("Message", ViewData["Message"]); %>
    <% } %>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="checkInBar">
    <h2>Grab your seat!</h2>
    <h2 class="right">Grab your seat!</h2>
    <a href="http://twitter.com/?status=%40HelloApp+sat+" class="checkIn">Check In</a>
    </div>
    
    <div class="generalContent">
    
    <% using (Html.BeginForm("Search", "Event", FormMethod.Get)) { %>
    
        <div id="filter">
            <select onchange="this.form.submit()" id="viewBy" name="viewBy">
                <option <% if (!String.IsNullOrEmpty((string)ViewData["ViewBy"]) && (string)ViewData["ViewBy"] == "twitter") { %>selected<% } %> value="twitter">View Twitter Icons</option>
                <option <% if (!String.IsNullOrEmpty((string)ViewData["ViewBy"]) && (string)ViewData["ViewBy"] == "category") { %>selected<% } %> value="category">View Categories</option>
                <option <% if (!String.IsNullOrEmpty((string)ViewData["ViewBy"]) && (string)ViewData["ViewBy"] == "follower") { %>selected<% } %> value="follower">View Follower Heat Map</option>
            </select>
        </div>

        <ul class="tagCloud">
        
            <% if (ViewData["Tags"] != null && ((Dictionary<string, string>)ViewData["Tags"]).Any()) { %>
                
                <% var tagKeys = (IEnumerable<string>)ViewData["TagsKeys"]; %>
                <% var tags = (Dictionary<string, string>)ViewData["Tags"]; %>
                <% foreach (var key in tagKeys) { %>
                
                    <li class="<%= tags[key] %>">
                        <%= Html.ActionLink(key, "Search", "Event", new { searchTerm = key }, null) %>
                    </li>
                
                <% } %>
            
            <% } else { %>
            
                <li class="large">
                    <%= Html.ActionLink("Nobody has checked in yet, why don't you?", "Faq", "Home", null, null, "instructionscheckin", null, null)%>
                </li>
            
            <% } %>
            
        </ul>

        <div id="search">
        <p><label>Search:</label><input type="text" id="searchTerm" name="searchTerm" value="<%= Html.Encode(ViewData["SearchTerm"]) %>" /></p>
        <p><input type="submit" id="submitBtn" value="Submit" /></p>
        </div>
    
    <% } %>
    
    </div>
    
    <br clear="all" />
    
    <div class="seating">
    
    <table class="seatingPlan" cellpadding="0" cellspacing="0" border="0">
    
        <% var sats = (IList<Sat>)ViewData["Sats"]; %>
        
        <% foreach (var row in Model.Seats.GroupBy(s => s.Row)) { %>
        
            <tr>
            
                <% foreach (var seat in row) { %>
                
                    <td>
                    
                        <% if (seat.Code == null) { %>
                            <img width="24" height="24" src="<%= Url.Content("~/Content/images/presentation/spacer.gif") %>" />
                        <% } else { %>
                            <% var sat = sats.SingleOrDefault(s => s.SeatID == seat.SeatID); %>
                            <% if (sat == null) { %>
                                <img width="24" height="24" src="<%= Url.Content("~/Content/images/presentation/smiley.jpg") %>" />
                            <% } else { %>
                                <% if (!String.IsNullOrEmpty((string)ViewData["ViewBy"]) && (string)ViewData["ViewBy"] == "category") { %>
                                    <% var colour = sat.User.UserType == null ? "B7B4A3" : sat.User.UserType.DefaultColour; %>
                                    <div class="user" style="background-color: #<%= colour %>;"></div>
                                <% } else if (!String.IsNullOrEmpty((string)ViewData["ViewBy"]) && (string)ViewData["ViewBy"] == "follower") { %>
                                    <% var colour = Settings.GetHeatColour(sat.User.Followers); %>
                                    <div class="user" style="background-color: #<%= colour %>;"></div>
                                <% } else if (!String.IsNullOrEmpty((string)ViewData["searchTerm"]) && !sat.User.HasTag((string)ViewData["searchTerm"])) { %>
                                    <img class="user" width="24" height="24" style="opacity:0.4; filter:alpha(opacity=40);" src="<%= sat.User.ImageURL %>" />
                                <% } else { %>
                                    <img class="user" width="24" height="24" src="<%= sat.User.ImageURL %>" />
                                <% } %>
                                <form id="<%= sat.User.Username %>">
                                    <%= Html.Hidden("Username", sat.User.Username) %>
                                    <%= Html.Hidden("ImageURL", sat.User.ImageURL) %>
                                    <%= Html.Hidden("UserType", sat.User.UserTypeID) %>
                                    <% foreach (var tag in sat.User.Tags.OrderByDescending(t => t.Created).Take(3)) { %>
                                        <%= Html.Hidden("Tag", tag.Name) %>
                                    <% } %>
                                    <% var points = sat.User.Points.Sum(p => p.Amount); %>
                                        <%= Html.Hidden("Points", points) %>
                                    <% if (points >= Settings.Thresholds.Bronze) { %>
                                        <%= Html.Hidden("Badge", "bronze") %>
                                    <% } %>
                                    <% if (points >= Settings.Thresholds.Silver) { %>
                                        <%= Html.Hidden("Badge", "silver") %>
                                    <% } %>
                                    <% if (points >= Settings.Thresholds.Gold) { %>
                                        <%= Html.Hidden("Badge", "gold") %>
                                    <% } %>
                                    <% if (points >= Settings.Thresholds.Diamond) { %>
                                        <%= Html.Hidden("Badge", "diamond") %>
                                    <% } %>
                                    <% var hiFives = sat.User.HiFivees.Count(); %>
                                    <% if (hiFives > 0) { %>
                                        <%= Html.Hidden("Badge", "highFive") %>
                                    <% } %>
                                    <% if (hiFives > 1) { %>
                                        <%= Html.Hidden("HiFives", hiFives) %>
                                    <% } %>
                                    <% if (sat.User.Friends.Count() >= Settings.Thresholds.Smiley) { %>
                                        <%= Html.Hidden("Badge", "smiley") %>
                                    <% } %>
                                </form>
                            <% } %>
                        <% } %>
                    
                    </td>
                
                <% } %>
            
            </tr>
        
        <% } %>
    
    </table>
        
        <div class="profileBox avatarPopup" style="display: none;">
            <div class="twitterProfile">
                <img height="73px" />
                <div class="bio">
                    <p><a class="twitterLink" href="/Profile/View/ryancarson">Ryancarson</a><b class="points"></b></p>
                    <p id="latestTweet">Loading...</p>
                </div>
                <a href="#" class="close">Close</a>
            </div>
            <ul class="categories">
                <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
                    <li class="<%= ut.UserTypeID %>"><%= ut.Name %></li>
                <% } %>
            </ul>
            <ul class="badgeList">
                <li title="Bronze Badge" class="bronze" style="display:none;">Bronze Badge</li>
                <li title="Silver Badge" class="silver" style="display:none;">Silver Badge</li>
			    <li title="Gold Badge" class="gold" style="display:none;">Gold Badge</li>
			    <li title="Diamond Badge" class="diamond" style="display:none;">Diamond Badge</li>
			    <li title="Hi5 Badge" class="highFive" style="display:none;">Hi5 Badge</li>
			    <li title="Smiley Badge" class="smiley" style="display:none;">Smiley Badge</li>
            </ul>
            <p class="tagPara">#<a href="#">Fake</a> #<a href="#">Fake</a> #<a href="#">Fake</a></p>
        </div>
    
    </div><!-- /.seating -->
    
    <% if (ViewData["Messages"] != null && ((IQueryable<Message>)ViewData["Messages"]).Any()) { %>

        <h2 id="allmessages">Messages:</h2>
    
        <% foreach (var message in (IQueryable<Message>)ViewData["Messages"]) { %>
            
            <div class="message">
                <blockquote><p>"<%= message.Text %>"</p></blockquote>
                <p class="author">By @<a href="http://twitter.com/<%= message.Username %>"><%= message.Username %></a></p>
            </div><!-- /.message -->
            
        <% } %>
        
    <% } %>
    
</asp:Content>
