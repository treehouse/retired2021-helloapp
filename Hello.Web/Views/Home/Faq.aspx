<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="narrowContent">
    <h2>How to use Hello</h2>
    
    <ul class="subNav">
    <li><a href="#overview">Overview</a></li>
    <li><a href="#points-badges">Points and Badges</a></li>
    <li><a href="#instructions1">Instructions</a></li>
		<li><a href="#quickref">Quick Ref Guide</a></li>
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
    
    <p><span class="read">When you meet someone, and they really like you, they can award you a "High Five", which awards you a special badge and gives you <%= Settings.Points.HiFive %> points.</span></p>
    
    <p><span class="read">Check out the <a href="#badges">complete list of badges</a>.</span></p>
    
    <h3 id="instructions1">Instructions</h3>

	<h4 id="instructionscheckin">Checking in and grabbing your seat</h4>

	<p><span class="read">Once you've sat down, there will be a passcode attached to your seat. <strong>Important:</strong> At the beginning of each session in the conference schedule, the seating chart is wiped clean and you'll need to re-checkin. (Replace 'XXX' with the passcode attached to your seat.)</span><br><code>@helloapp sat XXX</code></p>

		
		<h4>Picking your skill and tagging yourself</h4>

		<p><span class="read">Choose what you're best at (Design, Development, Business)
		and choose three tags for yourself. You can change your category and tags by just re-checking in.</span><br><code>@helloapp hello !des #ux #html5 #movies</code><br><code>@helloapp hello !dev #php #django #mysql</code><br><code>@helloapp hello !biz #ideas #fun #music</code></p>
		
		<h4>Getting points and meeting people</h4>

		<p><span class="read">When you meet someone and they give you their Twitter username, just tweet:</span><br><code>@helloapp met @their-username</code></p>
		
		<p><span class="read">If they return the favour and tweet:</span><br><code>@helloapp met @your-username</code></p>
		
		<p><span class="read">then you'll both get 10 points for meeting each other. Both people have to do it, in order for the points to be awarded.</span></p>
		
		<h4>Posting a message to the entire audience</h4>

		<p><span class="read">Once you earn the Silver Badge you can send a message to the entire audience which will be displayed on the HelloApp page. Just tweet (replace 'your-message' with your actual message):</span><br><code>@helloapp message your-message</code></p>
		
		<h4>Badges and Points</h4>

		<p><span class="read">The more people you meet and special tasks you complete during the conference, the more points and "badges" you'll be awarded.</span></p>
		<p><span class="read">If you earn a high enough rank, you'll be able to post public messages to the entire audience and win awesome prizes.</span></p>
		
		<h4>Tokens</h4>

		<p><span class="read">There are secret tokens hidden around the venue which are worth varying points. There are a limited number of tokens so once a token has been claimed, it can’t be re-used. Claim a token by tweeting (replace 'XXX' with the token code):</span><br><code>@helloapp claim XXX</code></p>
		
		<h4>High Fives (baby!)</h4>

		<p><span class="read">When you meet someone and you really like them, you can award them a "High Five", which gives them a special badge and 20 points. You don’t have to meet someone to give them a High Five. To award a high five, just Tweet:</span><br><code>@helloapp hi5 @their-username</code></p>
		
		<h4>Points</h4>

		<p><span class="read">Met: 10<br>High Five: 20<br>Tokens: 10 - 100</span></p>
		
		<h3 id="quickref">Quick Reference Guide</h4>

		<p>
			<span class="read">Checkin</span>
			<br><code>@helloapp sat XXX</code>
			<br />
			<span class="read">Categorise and tag yourself</span>
			<br><code>@helloapp hello ![des/dev/biz] #tag #tag #tag</code>
			<br />
			<span class="read">Met someone</span>
			<br><code>@helloapp hello @their-username</code>
			<br />
			<span class="read">Post message</span>
			<br><code>@helloapp message your-message</code>
			<br />
			<span class="read">Give High Five</span>
			<br><code>@helloapp hi5 @their-username</code>
			<br />
			<span class="read">Claim a token</span>
			<br><code>@helloapp claim XXX</code>
		</p>
		
    <h3 id="badges">Badges</h3>
    
	<p><span class="read">The more people you meet and special tasks you complete during the conference, the more points and "badges" you'll be awarded. If you earn a high enough rank, you'll be able to post public messages to the entire audience and win awesome prizes.</span></p>
    
    <ul class="badges">
    <li>
    <img src='<%= Url.Content("~/Content/images/content/bronze.png") %>' alt="bronze badge" />
    <p><span>Bronze Badge:</span> <%= Settings.Thresholds.Bronze %> points</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/silver.png") %>' alt="silver badge" />
    <p><span>Silver Badge:</span> <%= Settings.Thresholds.Silver %> points<br /><strong>You can now post messages!</strong></p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/gold.png") %>' alt="gold badge" />
    <p><span>Gold Badge:</span> <%= Settings.Thresholds.Gold %> points</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/diamond.png") %>' alt="diamond badge" />
    <p><span>Diamond Badge:</span> <%= Settings.Thresholds.Diamond %> points</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/high_five.png") %>' alt="high five badge" />
    <p><span>High Five Badge:</span> When you meet someone, and they really like you, they can award you a "High Five", which awards you a special badge and gives you <%= Settings.Points.HiFive %> points.</p>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/smiley.png") %>' alt="smiley badge" />
    <p><span>Smiley Badge:</span> The person who meets the most folk during the conference gets the mighty 'Super Friendly' badge.</p>
    </li>
    </ul>
	
    </div><!-- .narrowContent -->

</asp:Content>
