<%@ Page Title="Error" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <div class="panel panel-danger" runat="server" id="ErrorDiv">
            <div class="panel-body">
                <div>
                    <h4 style="color:red"><strong>The application has encountered an unknown error. 
                        <br />Try again after some time. 
                        <br />
                        <br />Please contact your school if you get this error more often.</strong></h4>
                </div> 
            </div>
        </div>
</asp:Content>

