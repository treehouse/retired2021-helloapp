<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.User>>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function() {
            var avatar = $('.roundAvatars');
            var popup = $('.avatarPopup');
            var popupSize = popup.size();
            avatar.hover(function() {
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
                twitterLink.attr('href', 'http://twitter.com/' + username);
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
                    tagHtml += '#<a href="<%= Url.Action("Index", "Search", new { search = " " }) %>' + tag + '">' + tag + '</a> '

                });
                $('.tagsPara', popup).html(tagHtml);

                // Positioning
                var pos = $this.position();
                var height = popup.height();
                popup.css({
                    top: pos.top - height - 20,
                    left: (pos.left > 980 / 2) ? pos.left - 420 : pos.left + 48
                });

                $('#latestTweet', popup).text("Loading...");

                // Latest Tweet
                $.getJSON('http://twitter.com/statuses/user_timeline/' +
                    username +
                    '.json?count=1&callback=?',
                    function(tweets) {
                        $('#latestTweet', popup).text(tweets[0].text);
                    }
                );

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

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("SearchBar", ""); %>

    <div class="section randomAvatars">
    
        <% var r = new Random(); %>
        
        <% foreach (var user in Model) { %>
            <div class="roundAvatars" style="background-image: url(<%= user.ImageURL %>); top: <%= r.Next(500 - 73) %>px; left: <%= r.Next(980 - 73) %>px">
                <form>
                    <%= Html.Hidden("Username", user.Username) %>
                    <%= Html.Hidden("ImageURL", user.ImageURL) %>
                    <%= Html.Hidden("UserType", user.UserTypeID) %>
                    <% foreach (var tag in user.Tags.OrderByDescending(t => t.Created).Take(3)) { %>
                        <%= Html.Hidden("Tag", tag.Name) %>
                    <% } %>
                </form>
            </div>
        <% } %>
        
        <div class="profileBox avatarPopup">
            <div class="twitterProfile">
                <img height="73px" />
                <div class="bio">
                    <p>@<a class="twitterLink" href="http://twitter.com/ryancarson">Ryancarson</a></p>
                    <p id="latestTweet">Loading...</p>
                </div>
                <a href="#" class="close">Close</a>
            </div>
            <ul class="categories">
                <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
                    <li class="<%= ut.UserTypeID %>"><%= ut.Name %></li>
                <% } %>
            </ul>
            <p class="tagsPara">#<a href="#">Fake</a> #<a href="#">Fake</a> #<a href="#">Fake</a></p>
        </div>
        
    </div>

</asp:Content>

