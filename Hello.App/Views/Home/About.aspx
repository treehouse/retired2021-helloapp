<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="narrowContent">

    <h2>Howdy folks!</h2>

	<h3>Hello is a simple app that helps people make friends and have more fun at conferences.</h3>

	<h4>What does it do?</h4>

	<p><span class="read">The idea is fairly simple: When you arrive at the conference, simply say which seat you're sitting in, via Twitter. You can then browse the rest of the audience and see else is sitting around you, what their interest are, and whether you have anything in common. You can also search the audience for specific names or skillset (php, designer, marketer, etc).</span></p>

	<img src='<%= Url.Content("~/Content/images/content/screen_01.jpg") %>' class="genImg2" alt="personal profile" />

	<img src='<%= Url.Content("~/Content/images/content/heat_map.jpg") %>' class="genImg" alt="heat map" />

	<h4>Why did we build it?</h4>

	<p><span class="read">We built the app in four days with <a href="http://www.asp.net/mvc/">ASP.NET MVC</a> and the amazing help of Matt Lee from <a href="http://www.red-gate.com/">Red Gate</a>.</span></p>

	<p><span class="read">Microsoft has been a huge supporter of our conferences (Future of Web Design and Future of Web Apps) so we asked if we could allocate some of their sponsorship budget to build an app on the Microsoft stack, testing what it was like and how well the technology works.</span></p>

	<p><span class="read">We normally code in Rails and we were absolutely blown away by the power of the Visual Studio environment. The debugging, Intellisense and database design functionality was extremely mature and powerful.</span></p>
    </div><!-- .narrowContent -->
    
    <div class="castWrapper">
    <h3>Who was involved?</h3>
    <ul class="cast">
    <li>
    <img src='<%= Url.Content("~/Content/images/content/carsonified.jpg") %>' alt="Carsonified" />
    <div class="role">
    <p class="five">@<a href="http://twitter.com/carsonified">Carsonified</a></p>
    <p>The crazy company behind this project :)</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/matt.jpg") %>' alt="Matt Lee" />
    <div class="role">
    <p><a href="http://twitter.com/thatismatt">Matt Lee</a></p>
    <p>ASP.NET MVC Developer</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/mike.jpg") %>' alt="Mike Kus" />
    <div class="role">
    <p><a href="http://twitter.com/mikekus">Mike Kus</a></p>
    <p>Designer</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/ryan_large.jpg") %>' alt="Ryan Carson" />
    <div class="role">
    <p><a href="http://twitter.com/ryancarson">Ryan Carson</a></p>
    <p>Wireframes and project management</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/keir.jpg") %>' alt="Keir Whitaker" />
    <div class="role">
    <p><a href="http://twitter.com/keirwhitaker">Keir Whitaker</a></p>
    <p>Development advisor and hosting logistics</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/microsoft.jpg") %>' alt="Microsoft Logo" />
    <div class="role">
    <p><a href="http://www.asp.net/mvc">ASP.NET MVC</a></p>
    <p>Framework used to build Hello</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/mark_quirk.jpg") %>' alt="Mark Quirk" />
    <div class="role">
    <p><a href="http://twitter.com/markqu">Mark Quirk</a></p>
    <p>Ideas and support</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/mark_j.jpg") %>' alt="Mark Johnston" />
    <div class="role">
    <p><a href="http://twitter.com/markjo">Mark Johnston</a></p>
    <p>Ideas and support</p>
    </div>
    </li>
    
    <li>
    <img src='<%= Url.Content("~/Content/images/content/red_gate.jpg") %>' alt="Red Gate" />
    <div class="role">
    <p><a href="http://twitter.com/redgate">Red Gate</a></p>
    <p>SQL and .NET developer tools</p>
    </div>
    </li>
    
    </ul>
    </div><!-- .castWrapper -->

</asp:Content>
