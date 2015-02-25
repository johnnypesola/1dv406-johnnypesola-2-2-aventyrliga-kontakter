<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_2_2_aventyrliga_kontakter.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>2-2-aventyrliga-kontakter</title>

    <link href='http://fonts.googleapis.com/css?family=Source+Sans+Pro' rel='stylesheet' type='text/css' />

    <%: Styles.Render("~/Content/styles") %>
    <%: Scripts.Render("~/Content/javascript") %>  

</head>
<body>
    <form id="ContactForm" runat="server">
    
        <%-- Informational messages --%>
        <asp:Panel ID="InfoPanel" runat="server" Visible="False">
            <asp:Literal ID="InfoPanelLiteral" runat="server"></asp:Literal>
        </asp:Panel>

        <%-- Error messages --%>
        <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Följande fel inträffade:" CssClass="error-message" ValidationGroup="createGroup" />
        <asp:ValidationSummary ID="ValidationSummaryEdit" runat="server" HeaderText="Följande fel inträffade:" CssClass="error-message" ValidationGroup="editGroup" ShowModelStateErrors="False" />


        <asp:ListView ID="ContactListView" runat="server"
            ItemType="_2_2_aventyrliga_kontakter.Model.Contact"
            SelectMethod="ContactListView_GetData"
            InsertMethod="ContactListView_InsertItem"
            UpdateMethod="ContactListView_UpdateItem"
            DeleteMethod="ContactListView_DeleteItem"
            DataKeyNames="ContactId"
            InsertItemPosition="FirstItem"
            >

        <%-- Placeholder for contacts --%>
            <LayoutTemplate>
                <div class="contactContainer">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>

                <%-- Pager for contacts --%>
                <asp:DataPager ID="DataPager" runat="server" PageSize="12" QueryStringField="page">
                    <Fields>
                        <%-- Page settings, button options --%>
                        <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText=" << "
                            ShowNextPageButton="False" ShowPreviousPageButton="False"  />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText=" >> "
                            ShowNextPageButton="False" ShowPreviousPageButton="False"  />
                    </Fields>
                </asp:DataPager>

            </LayoutTemplate>

        <%-- Template for contacts --%>
            <ItemTemplate>
                <ul>
                    <li class="fname"><%#: Item.FirstName %></li>
                    <li class="lname"><%#: Item.LastName %></li>
                    <li class="email-address"><%#: Item.EmailAddress %></li>
                    <li class="buttons">
                        <asp:LinkButton runat="server" CommandName="Edit" Text="Redigera" CausesValidation="false" />
                        <asp:LinkButton runat="server" CommandName="Delete" Text="Ta bort" CausesValidation="false" OnClientClick="return ConfirmDelete(this)" />
                    </li>
                </ul>
            </ItemTemplate>

        <%-- Template for editing contacts --%>
            <EditItemTemplate>
                
                <ul>
                    <li>
                        <asp:TextBox ID="EditFirstName" runat="server" Text='<%# BindItem.FirstName %>' MaxLength="50" />
                    </li>
                    <li>
                        <asp:TextBox ID="EditLastName" runat="server" Text='<%# BindItem.LastName %>' MaxLength="50" />
                    </li>
                    <li>
                        <asp:TextBox ID="EditEmailAddress" runat="server" Text='<%# BindItem.EmailAddress %>' class="email-address" MaxLength="50" />
                    </li>
                    <li class="buttons">
                        <%-- Update button --%>
                        <asp:LinkButton runat="server" CommandName="Update" Text="Spara" ValidationGroup="editGroup" />
                        <%-- Cancel button --%>
                        <asp:LinkButton runat="server" CommandName="Cancel" Text="Avbryt" CausesValidation="false" />
                    </li>
                </ul>

                <%-- Validators --%>
                <asp:RequiredFieldValidator ID="EditFirstNameRequired" runat="server" ErrorMessage="Var vänlig ange ett förnamn" ControlToValidate="EditFirstName" Display="None" ValidationGroup="editGroup" />
                <asp:RequiredFieldValidator ID="EditLastNameRequired" runat="server" ErrorMessage="Var vänlig ange ett efternamn" ControlToValidate="EditLastName" Display="None" ValidationGroup="editGroup" />
                <asp:RequiredFieldValidator ID="EditEmailAddressRequired" runat="server" ErrorMessage="Var vänlig ange en e-postadress" ControlToValidate="EditEmailAddress" Display="None" ValidationGroup="editGroup" />

                <asp:RegularExpressionValidator
                    ID="EditEmailAddressRegex"
                    runat="server"
                    ControlToValidate="EditEmailAddress"
                    ErrorMessage="Var vänlig ange en giltig e-postadress"
                    ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                    ValidationGroup="editGroup"
                    Display="None"
                />

            </EditItemTemplate>

        <%-- Template for adding new cotact --%>
            <InsertItemTemplate>

                <table class="add-contact">
                    <tr>
                        <th>Förnamn</th>
                        <th>Efternamn</th>
                        <th colspan="2">E-postadress</th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="CreateFirstName" runat="server" Text='<%# BindItem.FirstName %>' MaxLength="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="CreateLastName" runat="server" Text='<%# BindItem.LastName%>' MaxLength="50" />
                        </td>
                        <td>
                            <asp:TextBox ID="CreateEmailAddress" runat="server" Text='<%# BindItem.EmailAddress %>' class="email-address" MaxLength="50" />
                        </td>
                        <td>
                            <%-- Add button --%>
                            <asp:LinkButton runat="server" CommandName="Insert" Text="Lägg till" ValidationGroup="createGroup" />
                            <%-- Clear button --%>
                            <asp:LinkButton runat="server" CommandName="Cancel" Text="Rensa" CausesValidation="false" />
                        </td>
                    </tr>
                </table>

                <%-- Validators --%>

                <asp:RequiredFieldValidator ID="CreateFirstNameRequired" runat="server" ErrorMessage="Var vänlig ange ett förnamn" ControlToValidate="CreateFirstName"  ValidationGroup="createGroup" Display="None" />
                <asp:RequiredFieldValidator ID="CreateLastNameRequired" runat="server" ErrorMessage="Var vänlig ange ett efternamn" ControlToValidate="CreateLastName"  ValidationGroup="createGroup" Display="None" />
                <asp:RequiredFieldValidator ID="CreateEmailAddressRequired" runat="server" ErrorMessage="Var vänlig ange en e-postadress" ControlToValidate="CreateEmailAddress" ValidationGroup="createGroup" Display="None" />

                <asp:RegularExpressionValidator
                    ID="CreateEmailAddressRegex"
                    runat="server"
                    ControlToValidate="CreateEmailAddress"
                    ErrorMessage="Var vänlig ange en giltig e-postadress"
                    ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                    ValidationGroup="createGroup" 
                    Display="None"
                />

            </InsertItemTemplate>
        </asp:ListView>
    </form>
</body>
</html>
