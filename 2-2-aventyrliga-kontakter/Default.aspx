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
    
        <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Följande fel inträffade:" />

        <asp:ListView ID="ContactListView" runat="server"
            ItemType="_2_2_aventyrliga_kontakter.Model.Contact"
            SelectMethod="ContactListView_GetData"
            InsertMethod="ContactListView_InsertItem"
            UpdateMethod="ContactListView_UpdateItem"
            DeleteMethod="ContactListView_DeleteItem"
            DataKeyNames="ContactId"
            InsertItemPosition="FirstItem"
            >

            <LayoutTemplate>
                <div class="contactContainer">
                    <%-- Platshållare för nya rader --%>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>

                <asp:DataPager ID="DataPager" runat="server" PageSize="12">
                    <Fields>
                        <asp:NextPreviousPagerField ShowFirstPageButton="True" FirstPageText=" << "
                            ShowNextPageButton="False" ShowPreviousPageButton="False"  />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ShowLastPageButton="True" LastPageText=" >> "
                            ShowNextPageButton="False" ShowPreviousPageButton="False"  />
                    </Fields>
                </asp:DataPager>

            </LayoutTemplate>

            <ItemTemplate>
                <ul>
                    <li><%#: Item.FirstName %></li>
                    <li><%#: Item.LastName %></li>
                    <li class="email-address"><%#: Item.EmailAddress %></li>
                    <li class="buttons">
                        <asp:LinkButton runat="server" CommandName="Edit" Text="Redigera" CausesValidation="false" />
                        <asp:LinkButton runat="server" CommandName="Delete" Text="Ta bort" CausesValidation="false" />
                    </li>
                </ul>
            </ItemTemplate>
            <EditItemTemplate>
                <%-- Mall för rad i tabellen för att redigera kunduppgifter. --%>
                <ul>
                    <li>
                        <asp:TextBox ID="Name" runat="server" Text='<%# BindItem.FirstName %>' />
                    </li>
                    <li>
                        <asp:TextBox ID="Address" runat="server" Text='<%# BindItem.LastName%>' />
                    </li>
                    <li>
                        <asp:TextBox ID="PostalCode" runat="server" Text='<%# BindItem.EmailAddress %>' class="email-address" />
                    </li>
                    <li class="buttons">
                        <%-- "Kommandknappar" för att uppdatera en kunduppgift och avbryta. Kommandonamnen är VIKTIGA! --%>
                        <asp:LinkButton runat="server" CommandName="Update" Text="Spara" />
                        <asp:LinkButton runat="server" CommandName="Cancel" Text="Avbryt" CausesValidation="false" />
                    </li>
                </ul>
            </EditItemTemplate>
            <InsertItemTemplate>
                <%-- Mall för rad i tabellen för att lägga till nya kunduppgifter. Visas bara om InsertItemPosition 
                    har värdet FirstItemPosition eller LasItemPosition.--%>
                <table class="add-contact">
                    <tr>
                        <th>Förnamn</th>
                        <th>Efternamn</th>
                        <th>E-postadress</th>
                        <th></th>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="Name" runat="server" Text='<%# BindItem.FirstName %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="Address" runat="server" Text='<%# BindItem.LastName %>' />
                        </td>
                        <td>
                            <asp:TextBox ID="PostalCode" runat="server" Text='<%# BindItem.EmailAddress %>' class="email-address" />
                        </td>
                        <td>
                            <%-- "Kommandknappar" för att lägga till en ny kunduppgift och rensa texfälten. Kommandonamnen är VIKTIGA! --%>
                            <asp:LinkButton runat="server" CommandName="Insert" Text="Lägg till" />
                            <asp:LinkButton runat="server" CommandName="Cancel" Text="Rensa" CausesValidation="false" />
                        </td>
                    </tr>

                </table>
            </InsertItemTemplate>

        </asp:ListView>
        &nbsp;
    </form>
</body>
</html>
