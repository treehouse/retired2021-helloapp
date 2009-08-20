<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Hello.Repo.User>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">
        $(fetchTweets);

        function fetchTweets() {
            $(".bio").each(function(i, item) {
                var username = $('.username', item).text();
                if ($('#latestTweet' + username + ':visible', item).size() > 0) {
                    $.getJSON('http://twitter.com/statuses/user_timeline/' +
                    username +
                    '.json?count=1&callback=?',
                    function(tweets) {
                        $('#latestTweet' +
                            tweets[0].user.screen_name.toLowerCase())
                                .text(tweets[0].text);
                        }
                    );
                }
            });
        }
    </script>
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("SearchBar", ViewData["SearchTerm"]); %>
    
    <% if (Model == null) { %>
    
        <p>Please enter a search term.</p>
    
    <% } else if (!Model.Any()) { %>
    
        <p>There were no results for the "<b><%= Html.Encode(ViewData["SearchTerm"]) %></b>".</p>
    
    <% } else { %>
    
        <% if (Model.Count() == Settings.MaxSearchResults) { %>
        
            <p class="searchMessage">Not all results for "<b><%= Html.Encode(ViewData["SearchTerm"]) %></b>" are displayed, try refining your search.</p>
        
        <% } %>
        
        <div class="section listings">
        
            <% bool? stripe = true; %>
            <% foreach (var user in Model) { %>
            
                <% Html.RenderPartial(
                       "User",
                       user,
                       new ViewDataDictionary
                       {
                           { "Stripe", (stripe = !stripe) },
                           { "UserTypes", ViewData["UserTypes"] },
                           { "DelayTweetLoad", true }
                       }); %>
            
            <% } %>
        
        </div>
        
    <% } %>
</asp:Content>
