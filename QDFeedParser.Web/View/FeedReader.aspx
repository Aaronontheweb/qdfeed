<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FeedReader.aspx.cs" Inherits="QDFeedParser.Examples.Web.View.FeedReader" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="jumbotron">
            <h1>
                <a id="TitleLink" runat="server"></a>
            </h1>
            <div class="row">
                <div class="col-md-12">
                    <img id="LogoImage" runat="server" class="img-rounded" src="" alt="" style="float: left; width: 200px; height: 200px; margin-right: 10px;"/>
                    <p><asp:Literal ID="DescriptionLiteral" runat="server"></asp:Literal></p>
                    <br />
                    <div style="font-size:15px">
                        <p><asp:Literal ID="ManagingEditorLiteral" runat="server"></asp:Literal></p>
                        <p><a id="SiteLink" runat="server"></a></p>
                        <p><asp:Literal ID="CategoryLiteral" runat="server"></asp:Literal></p>
                        <p style="font-size:15px;"><asp:Literal ID="LastDateLiteral" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
        </div>
        <h2>Episódios</h2>
         <div id="EpisodesList" runat="server"  class="panel-group">
        </div>      
    </div>
</asp:Content>
