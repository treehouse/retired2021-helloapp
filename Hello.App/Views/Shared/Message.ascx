<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Hello.Repo.Message>" %>

<div class="message">
    <div>
        <i>"<%= Html.Encode(Model.Text) %>"</i>
    </div>
    <div>
        By @<a href="http://twitter.com/<%= Html.Encode(Model.Username) %>"><%= Html.Encode(Model.Username) %></a>
        <span class="button">View all messages</span>
        <span class="button">How can I post</span>
    </div>
</div>
    