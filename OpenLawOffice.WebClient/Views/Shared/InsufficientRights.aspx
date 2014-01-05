<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Insufficient Access Rights
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Insufficient Access Rights</h2>

    <p>
        You do not have the necessary access rights to take the requested action.  If you believe
        this is an error, please contact your system administrator.
    </p>

</asp:Content>
