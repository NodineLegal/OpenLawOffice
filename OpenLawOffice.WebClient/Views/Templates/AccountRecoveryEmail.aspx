<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OpenLawOffice.WebClient.ViewModels.Account.RecoveryViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Account Recovery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Account Recovery</h2>
    
    <p style="margin-bottom: 20px; line-height: 1.6em;">
        You have received this email because we have received a request to recover your account credentials.
        If you did not make this request, please ignore this email.  However, if you did make this request,
        you may continue with the reset process by clicking the link below:
        <br />
        Username: <%: Model.UserName %><br />
        Requested At: <%: DateTime.Now.ToString() %><br />
        Request From: <%: Model.IpAddress %><br />
        <a href="<%: Model.ResetPwAddress %>">Reset Password</a>
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
