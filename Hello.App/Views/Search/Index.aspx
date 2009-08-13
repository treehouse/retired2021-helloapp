<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.User>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">
        $(function() {
            $(".bio").each(function(i, item) {
                var username = $('.username', item).text();
                $.getJSON('http://twitter.com/statuses/user_timeline/' +
                    username +
                    '.json?count=1&callback=?',
                    function(tweets) {
                        $('#latestTweet' +
                            tweets[0].user.screen_name.toLowerCase())
                                .text(tweets[0].text);
                    }
                );
            });
        });
    </script>
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("SearchBar", ViewData["SearchTerm"]); %>
    
    <% if (Model == null) { %>
    
        <p>Please enter a search term.</p>

    <% } else if (!Model.Any()) { %>
    
        <p>There were no results for the "<b><%= Html.Encode(ViewData["SearchTerm"]) %></b>".</p>
    
    <% } else { %>
    
        <div class="section listings">
            <% var stripe = true; %>
            <% foreach (var user in Model)
               { %>
                <div class="profileBox <%= (stripe = !stripe) ? "right" : "left" %>">
                    <div class="twitterProfile">
                        <img src="<%= user.ImageURL %>" alt="<%= user.Username %>" />
                        <div class="bio">
                            <p>@<a class="username" href="http://twitter.com/<%= user.Username %>"><%= user.Username %></a></p>
                            <p id="latestTweet<%= user.Username %>">Loading...</p>
                        </div>
                    </div>
                    <ul class="categories">
                        <% foreach (var ut in ViewData["UserTypes"] as List<UserType>)
                           { %>
                            <li class="<%= ut.UserTypeID == user.UserTypeID ? "selected" : "" %>"><%= ut.Name %></li>
                        <% } %>
                    </ul>
                    <p>
                        <% foreach (var tag in user.Tags.OrderByDescending(t => t.Created).Take(3)) { %>
                            #<%= Html.ActionLink(tag.Name, "Index", new { search = tag.Name }) %>
                        <% } %>
                    </p>
                </div>
            <% } %>
        </div>
        
    <% } %>
</asp:Content>
