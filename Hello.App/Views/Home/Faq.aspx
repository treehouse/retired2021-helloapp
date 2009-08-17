<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="narrowContent">
    <h2>How to use Hello</h2>
    
    <ul class="subNav">
    <li><a href="#overview">Overview</a></li>
    <li><a href="#points-badges">Points and Badges</a></li>
    <li><a href="#instructions1">Instructions</a></li>
    <li><a href="#badges">List of Badges</a></li>
	</ul>
    
    <h3 id="overview">Overview</h3>

	<p><span class="read">The idea is fairly simple: When you arrive at the conference, you simply say which seat you're sitting in via Twitter.</span></p>
    
	<p><span class="read">You can then browse the seating chart and see who is sitting around you, what their interests are, and whether you have anything in common. You can also search the audience for specific names or skillsets (php, designer, CSS3, etc).</span></p>
    
    <img src='<%= Url.Content("~/Content/images/content/skill_set.jpg") %>' class="genImg2" alt="The audience color-coded based on their skill set" />

	<img src='<%= Url.Content("~/Content/images/content/heat_map.jpg") %>' class="genImg" alt="A heat map of the audience based on Twitter followers" />
    
    <h3 id="points-badges">Earn points and badges</h3>

	<p><span class="read">The more people you meet and special tasks you complete during the conference, the more points and "badges" you'll be awarded. If you earn a high enough rank, you'll be able to post public messages to the entire audience and win awesome prizes.</span></p>
    
    <img src='<%= Url.Content("~/Content/images/content/badges_01.jpg") %>' class="genImg2" alt="Bronze, Silver, Gold and Diamond badges" />
    
    <p><span class="read">The more people you meet, the higher the badge rank you'll achieve.</span></p>
    
    <img src='<%= Url.Content("~/Content/images/content/high_five.jpg") %>' class="genImg" alt="High Five Badge" />
    
    <p><span class="read">When you meet someone, and they really like you, they can award you a "High Five", which awards you a special badge and gives you more points.</span></p>
    
    <p><span class="read">Check out the <a href="#badges">complete list of badges</a>.</span></p>
    
    <h3 id="instructions1">Instructions</h3>

	<h4>Checking in and grabbing your seat</h4>

	<p><span class="read">Once you've sat down, there will be a password attached to your seat. Checkin to Hello by tweeting:</span><br><code>@helloapp sat your-pass-code</code></p>

	<h4>Picking your skill and tagging yourself</h4>

	<p><span class="read">Choose what you're best at (Design, Development, Business) and choose three tags for yourself:</span><br><code>@helloapp hello !des #ux #html5 #movies</code><br><code>@helloapp hello !dev #php #django #mysql</code><br><code>@helloapp hello !biz #piano #entrepreneur #marketing</code></p>

	<p><span class="read">You'll be assigned a colour, based on whether you choose Design, Development or Business. You can view the seating chart based on skillset:</span></p>
    
    <img src='<%= Url.Content("~/Content/images/content/skill_set.jpg") %>' class="genImg2" alt="The audience color-coded based on their skill set" />
    
    <p><span class="read">You can also view it by tags. In this example, I've searched for everyone tagged with #ux:</span></p>
    
    <img src='<%= Url.Content("~/Content/images/content/ux_map.jpg") %>' class="genImg" alt="The seat chart with some people highlighted that chose ux as their tag" />
    
    <h4>Get points by meeting people</h4>

	<p><span class="read">When you meet someone and they give you their Twitter username, just tweet:</span><br><code>@helloapp @their-username</code>. <br><span class="read">If they return the favor and tweet:</span><br> <code>@helloapp @your-username</code> <br><span class="read">then you'll both get points for meeting eachother. Both people have to do it, in order for the points to be awarded.</span></p>

	<h4>Post a message to the entire audience</h4>

	<p><span class="read">Once you earn the Silver Badge you can send a message to the entire audience by tweeting:</span><br><code>@helloapp message "My message goes here"</code>. <br><span class="read">This message will be displayed like this:</span></p>
    
    <img src='<%= Url.Content("~/Content/images/content/message.jpg") %>' class="genImg2" alt="Screenshot showing the message area highlighted" />
    
    <h3>Frequently Asked Questions</h3>

	<p><span class="read">Q: Can I change my category (Design, Development, Business)?</span><br><span class="read">A: Yes, just re-check-in by tweeting:</span><br><code>@helloapp hello !des #tag-one #tag-two #tag-three</code></p>
    
    <p><span class="read">If you have any more questions just email: <a href="mailto:&#104;&#101;&#108;&#108;&#111;&#64;&#99;&#97;&#114;&#115;&#111;&#110;&#105;&#102;&#105;&#101;d&#46;&#99;&#111;&#109;">h&#101;&#108;l&#111;&#64;ca&#114;&#115;&#111;n&#105;&#102;&#105;&#101;&#100;&#46;&#99;&#111;&#109;</a></span></p>
    
    <h3 id="badges">Badges</h3>
    
	<p><span class="read">The more people you meet and special tasks you complete during the conference, the more points and "badges" you'll be awarded. If you earn a high enough rank, you'll be able to post public messages to the entire audience and win awesome prizes.</span></p>
    
    <ul class="badges">
    <li>
    <img src='<%= Url.Content("~/Content/images/content/bronze.png") %>' alt="bronze badge" />
    <p><span>Bronze Badge:</span> 15 points</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/silver.png") %>' alt="silver badge" />
    <p><span>Silver Badge:</span> 30 points<br><strong>You can now post messages!</strong></p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/gold.png") %>' alt="gold badge" />
    <p><span>Gold Badge:</span> 60 points</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/diamond.png") %>' alt="diamond badge" />
    <p><span>Diamond Badge:</span> 100 points</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/high_five.png") %>' alt="high five badge" />
    <p><span>High Five Badge:</span> When you meet someone, and they really like you, they can award you a "High Five", which awards you a special badge and gives you more points.</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/smiley.png") %>' alt="smiley badge" />
    <p><span>Smiley Badge:</span> The person who meets the most folk during the conference gets the mighty 'Super Friendly' badge.</p>
    </li>
    </ul>

	
    </div><!-- .narrowContent -->

</asp:Content>
