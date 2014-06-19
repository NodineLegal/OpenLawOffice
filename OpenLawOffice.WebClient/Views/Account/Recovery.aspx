<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.RecoveryViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Account Recovery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Account Recovery</h2>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true) %>
        <div>
            <fieldset>
                <legend>Recovery Information</legend>

                <% if (ViewData["Error"] != null) 
                   {
                       if ((string)ViewData["Error"] == "username")
                       { %>

                            <div style="color: Red; height: 35px;">Unable to find that username in our system.</div>
                        <% }
                       else if ((string)ViewData["Error"] == "email")
                       { %>
                            <div style="color: Red; height: 35px;">Unable to find that email address in our system.</div>
                <%     }
                    } %>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.UserName)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.UserName)%>
                    <%: Html.ValidationMessageFor(m => m.UserName)%>
                </div>

                <div>- OR -</div>

                <div class="editor-label">
                    <%: Html.LabelFor(m => m.Email)%>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email)%>
                    <%: Html.ValidationMessageFor(m => m.Email)%>
                </div>
                
                <p>
                    <input type="submit" value="Recover Account" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
