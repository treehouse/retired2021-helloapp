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
    
    <% using (Html.BeginForm("Search", "Event", FormMethod.Get, new { id = "searchForm" }))
       { %>
    <div id="filter">
    	<select onchange="this.form.submit()" id="viewBy" name="viewBy">
			<option value="twitter">View Twitter Icons</option>
			<option value="heatmap">View Heat Map</option>
		</select>
    </div>

	<ul class="tagCloud">
	
	    <% if (ViewData["Tags"] != null && ((IQueryable<Tag>)ViewData["Tags"]).Any())
        { %>
	    
	        <% foreach (var tag in (IQueryable<Tag>)ViewData["Tags"])
            { %>
            
                <li class="larger">
                    <%= Html.ActionLink(tag.Name, "Search", "Event", new { searchterm = tag.Name }, null)%>
                </li>
            
		    <% } %>
		
		<% }
        else
        { %>
		
	        <li class="large">
                <%= Html.ActionLink("Nobody has checked in yet, why don't you?", "Faq", "Home", null, null, "instructionscheckin", null, null)%>
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

    <div id="search">
    <p><label>Search:</label><input type="text" id="searchTerm" name="searchTerm" value="<%=Html.Encode(ViewData["SearchTerm"])%>" /></p>
    <p><input type="submit" id="submitBtn" name="submitBtn" value="Submit" /></p>
    </div>
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
                        if (sat == null)
                        {
                            %>
                            <img class="seat" width="24" height="24" src="<%= Url.Content("~/Content/images/presentation/smiley.jpg") %>" />
                            <%
                        }
                        else if (!String.IsNullOrEmpty((string)ViewData["ViewBy"]) && (string)ViewData["ViewBy"] == "heatmap")
                        {
                            %>
                            <div class="seat" style="width:24px; height: 24px; background-color: #<%= sat.User.UserType.DefaultColour %>; display: inline-block;"></div>   
                            <%
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty((string)ViewData["searchTerm"]) && !sat.User.HasTag((string)ViewData["searchTerm"]))
                            {
                            %><img class="seat" width="24" height="24" src="<%= Url.Content("~/Content/images/presentation/smiley.jpg") %>" /><%
                                                                                                                                               }
                            else
                            {
                            %><img class="seat" width="24" height="24" src="<%= sat.User.ImageURL %>" /><%
                                                                                                                                               }
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
