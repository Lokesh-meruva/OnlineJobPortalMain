﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ContactList.aspx.cs" Inherits="OnlineJobPortalMain.Admin.ContactList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div <%--style="background-image: url('./images/bg.jpg');--%> width: 100%; height: 720px; background-repeat: no-repeat; background-size: cover; background-attachment: fixed;">
     <div class="container-fluid pt-4 pb-4">
         <div>
             <asp:Label ID="lblMsg" runat="server"></asp:Label>
         </div>
         <h3 class="text-center">Contact List</h3>
         <div>
             <div class="row mb-3 pt-sm-3">
                 <div class="col-md-12">
                 </div>

                 <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-borderless"
                     EmptyDataText="No Record To Display...!" AutoGenerateColumns="False" AllowPaging="True" PageSize="5"
                     OnPageIndexChanging="GridView1_PageIndexChanging" DataKeyNames="ContactId"  OnRowDeleting="GridView1_RowDeleting">
                     <Columns>
                         <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                             <ItemStyle HorizontalAlign="Center" />
                         </asp:BoundField>

                         <asp:BoundField DataField="Name" HeaderText="User Name">
                             <ItemStyle HorizontalAlign="Center" />
                         </asp:BoundField>

                         <asp:BoundField DataField="Email" HeaderText="Email">
                             <ItemStyle HorizontalAlign="Center" />
                         </asp:BoundField>

                         <asp:BoundField DataField="Subject" HeaderText="Subject">
                             <ItemStyle HorizontalAlign="Center" />
                         </asp:BoundField>

                         <asp:BoundField DataField="Message" HeaderText="Message">
                             <ItemStyle HorizontalAlign="Center" />
                         </asp:BoundField>

                         <asp:CommandField CausesValidation="false" HeaderText="Delete" ShowDeleteButton="true" DeleteImageUrl="~/assets/img/icon/trash icon.png" 
                            ButtonType="Image"><ControlStyle Height="25px"  Width="25px" /><ItemStyle HorizontalAlign="Center" />
                         </asp:CommandField>


                     </Columns>
                     <HeaderStyle BackColor="#7200cf" ForeColor="White" Font-Bold="true" /> 
                 </asp:GridView>

             </div>
         </div>
     </div>
 </div>

</asp:Content>
