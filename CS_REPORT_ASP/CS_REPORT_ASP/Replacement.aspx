<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Replacement.aspx.vb" Inherits="CS_REPORT_ASP.Replacement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Delivery Note</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f8fa;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 1000px;
            margin: 30px auto;
            padding: 25px 30px;
            border: 1px solid #ccc;
            border-radius: 10px;
            background-color: #fff;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }

        .section-title {
            font-size: 22px;
            font-weight: bold;
            color: green;
            margin-bottom: 20px;
        }

        .group-title {
            font-weight: bold;
            font-size: 15px;
            color: #0033cc;
            text-decoration: underline;
            margin-top: 20px;
            margin-bottom: 10px;
        }

        table.detail-table {
            width: 100%;
            border-collapse: collapse;
        }

        table.detail-table td {
            padding: 5px;
            vertical-align: middle;
        }

        .input-text {
            width: 180px;
            padding: 3px 5px;
        }

        .input-long {
            width: 400px;
        }

        .input-short {
            width: 100px;
        }

        .button-bar {
            text-align: right;
            margin-bottom: 15px;
        }

        .btn {
            padding: 5px 10px;
            margin-left: 5px;
        }

        .grid {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .grid th, .grid td {
            border: 1px solid #ccc;
            padding: 6px;
            text-align: left;
        }

        .grid th {
            background-color: #e9e9e9;
        }
                .grid-input {
    width: 100%;
    box-sizing: border-box;
    padding: 3px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="section-title">Replacement Note Data</div>

<div style="margin-top: 20px; text-align: right;">
    <strong>Search: </strong>
    <asp:TextBox ID="txtRNSearch" runat="server" CssClass="form-control green" 
                 style="width: 300px; display: inline-block;" placeholder="RNSearch"></asp:TextBox>
                 <asp:Label ID="lblMessage" runat="server" ForeColor="Red" /></br>
                 
                 
    <asp:Button ID="btnEdit" runat="server" CssClass="btn" Text="Edit" OnClick="btnEdit_Click" />
</div>

            <div class="group-title">CUSTOMER DETAIL</div>
            <table class="detail-table">
                <tr>
                    <td>CUSTOMER CODE:</td>
                    <td><asp:TextBox ID="txtCustomerCode" runat="server" CssClass="input-text" /></td>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" />
                    <td><asp:Button ID="btnSearchCustomer" runat="server" Text="🔍" CssClass="btn" /></td>
                    <td>DELIVERY NOTE NO.:</td>
                    <td><asp:TextBox ID="txtReplacementNo" runat="server" CssClass="input-text" /></td>
                    <td>DATE:</td>
                    <td><asp:TextBox ID="txtDate" TextMode="Date" runat="server" CssClass="input-short" /></td>
                </tr>
                <tr>
                    <td>SOLD TO:</td>
                    <td colspan="6"><asp:TextBox ID="txtSoldTo" runat="server" CssClass="input-long" /></td>
                </tr>
                <tr>
                    <td>ADDRESS:</td>
                    <td colspan="6">
                        <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" Width="70%" />
                    </td>
                </tr>
                <tr>
                    <td>Tax ID. No.:</td>
                    <td><asp:TextBox ID="txtTaxId" runat="server" CssClass="input-text" /></td>
                    <td>Branch No.:</td>
                    <td><asp:TextBox ID="txtBranchNo" runat="server" CssClass="input-text" /></td>
                    
                </tr>
                <tr><td>Tel.:</td>
                    <td colspan="2"><asp:TextBox ID="txtTel" runat="server" CssClass="input-text" /></td>
                    <td>ATTN:</td>
                    <td><asp:TextBox ID="txtAttn" runat="server" CssClass="input-text" /></td>
                   
                </tr> <tr><td>จุดส่ง:</td>
                    <td colspan="4"><asp:TextBox ID="txtShipPoint" runat="server" CssClass="input-long" /></td></tr>
            </table>

            <div class="group-title">ITEM DETAIL</div>
            <table class="detail-table">
                <tr>
                    <td>ITEM CD:</td>
                    <td><asp:TextBox ID="txtItemCd" runat="server" CssClass="input-text" /></td>
                    <td><asp:Button ID="btnSearchItem" runat="server" Text="🔍" CssClass="btn" /></td>
                    <td>ITEM NM:</td>
                    <td><asp:TextBox ID="txtItemNm" runat="server" CssClass="input-text" /></td>
                    
                </tr>
                <tr>
                <td>DETAIL:</td>
                    <td><asp:TextBox ID="txtDetail" runat="server" CssClass="input-text" /></td>
                    <td>PO.NO.:</td>
                    <td><asp:TextBox ID="txtPoNo" runat="server" CssClass="input-text" /></td>
                    
                </tr>
                <tr><td>Q'ty:</td>
                    <td><asp:TextBox ID="txtQty" runat="server" CssClass="input-text" /></td>
                    <td>UNIT PRICE:</td>
                    <td><asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input-text" /></td>
                    <td>REMARK:</td>
   REMARK:</td>
                    <td><asp:TextBox ID="txtRemark" runat="server" CssClass="input-text" /></td>
                    <td><asp:Button ID="btnAddItem" runat="server" Text="+" CssClass="btn" /></td>
                </tr>
            </table>

<asp:GridView ID="gvItemList" runat="server" AutoGenerateColumns="False" CssClass="grid"
    AutoGenerateEditButton="False" DataKeyNames="ITM_CD"
    OnRowEditing="gvItemList_RowEditing"
    OnRowUpdating="gvItemList_RowUpdating"
    OnRowCancelingEdit="gvItemList_RowCancelingEdit"
    OnRowDeleting="gvItemList_RowDeleting"
    GridLines="None" CellPadding="5" Width="100%">
    
    <RowStyle BackColor="#ffffff" />
    <AlternatingRowStyle BackColor="#f0f8ff" />
    <HeaderStyle BackColor="#007acc" ForeColor="White" Font-Bold="True" />
    <EditRowStyle BackColor="#ffffcc" />

    <Columns>
        <asp:TemplateField HeaderText="No.">
            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="ITM_CD">
            <ItemTemplate><%# Eval("ITM_CD") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditPartNo" runat="server" Text='<%# Bind("ITM_CD") %>' CssClass="grid-input" />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="PART_NM">
            <ItemTemplate><%# Eval("ITM_NM") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditPartNm" runat="server" Text='<%# Bind("ITM_NM") %>'  CssClass="grid-input" />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="EX_ITM_NM">
            <ItemTemplate><%# Eval("EX_ITM_NM") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditExItmNm" runat="server" Text='<%# Bind("EX_ITM_NM") %>'  CssClass="grid-input" />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="PO_NO">
            <ItemTemplate><%# Eval("PO_NO") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditPoNo" runat="server" Text='<%# Bind("PO_NO") %>' CssClass="grid-input"  />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="QTY">
            <ItemTemplate><%# Eval("QTY") %></ItemTemplate>
            <EditItemTemplate>
               <asp:TextBox ID="txtEditQty" runat="server" Text='<%# Bind("QTY") %>' CssClass="grid-input" />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="UNIT_PRICE">
            <ItemTemplate><%# Eval("UNIT_PRICE") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditUnitPrice" runat="server" Text='<%# Bind("UNIT_PRICE") %>' CssClass="grid-input"  />
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="REMARK">
            <ItemTemplate><%# Eval("REMARK") %></ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtEditRemark" runat="server" Text='<%# Bind("REMARK") %>'  CssClass="grid-input"  />
            </EditItemTemplate>
        </asp:TemplateField>


        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" HeaderText="Actions" />
    </Columns>
</asp:GridView>
                <div style="margin-top: 20px; text-align: right;">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-cancel" Text="logout" OnClick="btnCancel_Click" />
        </div></div>
    </form>
</body>
</html>
