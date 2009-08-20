<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Hello.Repo.Event>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="TopContent" runat="server">

    <% if (ViewData["Message"] != null) { %>
        <% Html.RenderPartial("Message", ViewData["Message"]); %>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="checkInBar">
    <h2>Grab your seat!</h2>
    <h2 class="right">Grab your seat!</h2>
    <%= Html.ActionLink("Check In", "Faq", "Home", null, null, "instructionscheckin", null, new { @class = "checkIn" }) %>
    </div>
    
    <div class="generalContent">
    <form id="filter" method="post" action="" enctype="multipart/form-data">
    	<select id="viewBy">
			<option>View by number of Twitter followers</option>
			<option></option>
			<option></option>
		</select>
    </form>

	<ul class="tagCloud">
	
	    <% if (ViewData["Tags"] != null && ((IQueryable<Tag>)ViewData["Tags"]).Any()) { %>
	    
	        <% foreach (var tag in (IQueryable<Tag>)ViewData["Tags"]) { %>
            
                <li class="larger">
                    <%= Html.ActionLink(tag.Name, "Search", "Event", new { searchterm = tag.Name }, null)%>
                </li>
            
		    <% } %>
		
		<% } else { %>
		
	        <li class="large">
                <%= Html.ActionLink("Nobody has checked in yet, why don't you?", "Faq", "Home", null, null, "instructionscheckin", null, null) %>
            </li>
        
		<% } %>
                
	    <%--
		    <li class="larger"><a href="#">PHP</a></li>
		    <li class="small"><a href="#">Rails</a></li>
		    <li class="smaller"><a href="#">Design</a></li>
		    <li class="larger"><a href="#">Business</a></li>
		    <li class="largest"><a href="#">CSS</a></li>
		    <li class="smallest"><a href="#">Web</a></li>
		    <li class="medium"><a href="#">Apps</a></li>
		    <li class="large"><a href="#">Dev</a></li>
		--%>
		
	</ul>

    <% using (Html.BeginForm("Search", "Event", FormMethod.Get, new { id = "search" })) { %>
    <p><label>Search:</label><input type="text" id="searchTerm" name="searchTerm" value="<%=Html.Encode(ViewData["SearchTerm"])%>" /></p>
    <p><input type="submit" id="submit" name="submit" value="Submit" /></p>
    <% } %>
    </div>
    <br clear="all" />
    
	<div class="seating">
    
        <% var sats = (IEnumerable<Sat>)ViewData["Sats"]; %>
        
        <% foreach (var row in Model.Seats.GroupBy(s => s.Row)) { %>
    	
	        <div class="row">
    	    
	            <% foreach (var seat in row) {

                    if (seat.Code == null)
                    {
                        %><img class="space" width="24" height="24" src="<%= Url.Content("~/Content/images/presentation/spacer.gif") %>" /><%
                    }
                    else
                    {
                        var sat = sats.SingleOrDefault(s => s.SeatID == seat.SeatID);

                        if (sat == null || (!String.IsNullOrEmpty((string)ViewData["searchTerm"]) && !sat.User.HasTag((string)ViewData["searchTerm"])))
                        {
                            %><img class="seat" width="24" height="24" src="<%= Url.Content("~/Content/images/presentation/smiley.jpg") %>" /><%
                        }
                        else
                        {
                            %><img class="seat" width="24" height="24" src="<%= sat.User.ImageURL %>" /><%
                        }
                    }
                } %>
            
            </div>
    	        
	    <% } %>
	
    </div>
    
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
