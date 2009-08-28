<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Hello.Repo.Message>" %>

<div class="tweetHeader">
    <p>
        <i>"<%= Html.Encode(Model.Text) %>"</i>
    </p>
    <p>
        <span>
            By @<a href="http://twitter.com/<%= Html.Encode(Model.Username) %>"><%= Html.Encode(Model.Username) %></a>
        </span>
    </p>
    <ul>
        <li>
            <%= Html.ActionLink("View all messages", "Index", "Event", null, null, "allmessages", null, null)%>
        </li>
        <li>
            <%= Html.ActionLink("How can I post", "Faq", "Home", null, null, "instructionsmessage", null, null) %>
        </li>
    </ul>
</div>