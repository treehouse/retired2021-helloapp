<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">
        $(function() {
            $('.whatIdo a').click(function() {
                var tags = $('.tags');
                var newClass = $(this).attr('class');
                tags.slideUp(function() {
                    tags.attr('class', 'tags ' + newClass);
                });
                $('#userType').val(newClass);
                tags.slideDown();
                return false;
            });
        });
    </script>
    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="generalContent">
        <div class="joinInBox">
    	    <div class="whatIdo">
                <h2>I'm pretty good at...</h2>
                <ul>
                    <% foreach (var ut in ViewData["UserTypes"] as List<UserType>) { %>
                        <li><a href="#" class="<%= ut.UserTypeID %>"><%= ut.Name %></a></li>
                    <% } %>
                </ul>
            </div>
            <div class="tags" style="display: none;">
            <h3>Now name 3 tags that describe you best</h3>
                <% using (Html.BeginForm("Join", "Home", FormMethod.Post, new { id = "tags" })) { %>
                    <%= Html.Hidden("userType") %>
                    <p><input type="text" id="tag1" name="tag1" value="" /></p>
                    <p><input type="text" id="tag2" name="tag2" value="" /></p>
                    <p><input type="text" id="tag3" name="tag3" value="" /></p>
                    <p><input type="submit" id="submitBtn" name="submitBtn" value="Submit" /></p>
                <% } %>
            </div>
        </div>
    </div>

</asp:Content>
