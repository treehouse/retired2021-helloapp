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
                // HACK: to stop image glitching
                var img = $('img', popup)
                img.attr('src', '');
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
                    tagHtml += '#<a href="#">' + tag + '</a> '

                });
                $('.tagsPara', popup).html(tagHtml);

                // Positioning
                var pos = $this.position();
                var height = popup.height();
                popup.css({
                    top: pos.top - height - 20,
                    left: (pos.left > 980 / 2) ? pos.left - 420 : pos.left + 48
                });
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
                    <%= Html.Hidden("ImageURL", user.ImageURL)%>
                    <%= Html.Hidden("UserType", user.UserTypeID) %>
                    <% foreach (var tag in user.Tags) { %>
                        <%= Html.Hidden("Tag", tag.Name) %>
                    <% } %>
                </form>
            </div>
        <% } %>
        
        <div class="profileBox avatarPopup">
            <div class="twitterProfile">
            <img src="images/content/ryan_large.jpg" alt="Ryan Carson" />
            <div class="bio">
            <p>@<a class="twitterLink" href="http://twitter.com/ryancarson">Ryancarson</a></p>
            <p>American living in England - I'm a Father, Internet entrepreneur and lover of movies. Founder of Carsonified.com</p>
            </div>
            <a href="#" class="close">Close</a>
            </div>
            <ul class="categories">
                <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
                    <li class="<%= ut.UserTypeID %>"><%= ut.Name %></li>
                <% } %>
            </ul>
            <p class="tagsPara">#<a href="#">Reallylonggoogleytagye</a> #<a href="#">Reallylonggoogleytagye</a> #<a href="#">Reallylonggoogleytagye</a></p>
        </div>
        
    </div>

</asp:Content>

