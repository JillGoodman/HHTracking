<%@ Page Title="Home & Hospital Time Tracking" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
        <h2><%: Title %> </h2>
        <h3>
            <asp:Label ID="lblHeader" runat="server"></asp:Label>
        </h3>
        <div role="alert" class="alert alert-warning" id="ErrorDiv" runat="server" STYLE="width: 1805px; height: 20px">
            <asp:Label runat="server" ID="ErrorMsg"></asp:Label>
        </div>
        <div runat="server" id="divStudentInfo">
            <div class="panel panel-default">
                <div CLASS="panel-heading" STYLE="left: 1px; top: 0px">
                    <strong>Choose a student:</strong>
                    <asp:DropDownList ID="DDLStudents" runat="server" AutoPostBack="true" 
                        DataValueField="value_code" DataTextField="value_description" 
                        OnSelectedIndexChanged="DDLStudents_SelectedIndexChanged"
                        visible="false" Height="42px" Width="212px">
                    </asp:DropDownList>
                </div>
                <div class="panel-body">
                    <asp:Table ID="tblStudentInfo" runat="server" CellPadding="1" CellSpacing="10" Width="97%" BorderWidth="1" HorizontalAlign="Left">
                        <asp:TableRow>
                            <asp:TableCell Width="40%">
                            </asp:TableCell>
                        </asp:TableRow>
                   </asp:Table>
                    
                </div>
            </div>
        </div>
    </form>
</asp:Content>

