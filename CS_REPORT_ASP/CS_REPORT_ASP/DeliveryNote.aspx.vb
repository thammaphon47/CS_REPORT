Imports System.Data.SqlClient
Imports System.Data
Imports Oracle.DataAccess.Client
Imports System.Web.UI.WebControls

Public Class DeliveryNote
    Inherits System.Web.UI.Page

    Private Sub InitItemTable()
        Dim dt As New DataTable()
        dt.Columns.Add("ITM_CD", GetType(String))
        dt.Columns.Add("ITM_NM", GetType(String))
        dt.Columns.Add("EX_ITM_NM", GetType(String))
        dt.Columns.Add("PO_NO", GetType(String))
        dt.Columns.Add("QTY", GetType(String))
        dt.Columns.Add("UNIT_PRICE", GetType(String))
        dt.Columns.Add("REMARK", GetType(String))
        Session("ItemTable") = dt
        gvItemList.DataSource = dt
        gvItemList.DataBind()
    End Sub

    Private Property ItemTable As DataTable
        Get
            Return TryCast(Session("ItemTable"), DataTable)
        End Get
        Set(value As DataTable)
            Session("ItemTable") = value
        End Set
    End Property

    ' Connection Strings
    Dim ConStr As String = "Data Source=192.168.1.7;Initial Catalog=CS_DATA;User Id=sa;Password=p@ssw0rd;"
    Dim oradb As String = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.6)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MCF)));User Id=MCF380;Password=MCF380;"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitItemTable()
            BindGrid()
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
        End If
    End Sub

    ' Bind GridView
    Private Sub BindGrid()
        Dim dt As DataTable = TryCast(Session("ItemTable"), DataTable)
        If dt IsNot Nothing Then
            gvItemList.DataSource = dt
            gvItemList.DataKeyNames = New String() {"ITM_CD"}
            gvItemList.DataBind()
        End If
    End Sub

    ' เพิ่มรายการสินค้า
    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        Dim dt As DataTable = TryCast(Session("ItemTable"), DataTable)

        If dt Is Nothing Then
            InitItemTable()
            dt = TryCast(Session("ItemTable"), DataTable)
        End If

        Dim row As DataRow = dt.NewRow()
        row("ITM_CD") = txtItemCd.Text.Trim()
        row("ITM_NM") = txtItemNm.Text.Trim()
        row("EX_ITM_NM") = txtDetail.Text.Trim()
        row("PO_NO") = txtPoNo.Text.Trim()
        row("QTY") = txtQty.Text.Trim()
        row("UNIT_PRICE") = txtUnitPrice.Text.Trim()
        row("REMARK") = txtRemark.Text.Trim()

        dt.Rows.Add(row)
        Session("ItemTable") = dt

        gvItemList.DataSource = dt
        gvItemList.DataBind()

        txtItemCd.Text = ""
        txtItemNm.Text = ""
        txtDetail.Text = ""
        txtPoNo.Text = ""
        txtQty.Text = ""
        txtUnitPrice.Text = ""
        txtRemark.Text = ""
    End Sub

    Protected Sub gvItemList_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvItemList.EditIndex = e.NewEditIndex
        BindGrid()
    End Sub

    ' GridView RowCancelingEdit
    Protected Sub gvItemList_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvItemList.EditIndex = -1
        BindGrid()
    End Sub

    ' GridView RowUpdating
    Protected Sub gvItemList_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Dim dt As DataTable = ItemTable
        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            lblStatus.Text = "ไม่มีข้อมูลสินค้าให้แก้ไข"
            lblStatus.ForeColor = Drawing.Color.Red
            Return
        End If

        Dim itmCd As String = gvItemList.DataKeys(e.RowIndex).Value.ToString()
        Dim row As GridViewRow = gvItemList.Rows(e.RowIndex)

        Dim newItmCd As String = CType(row.FindControl("txtEditPartNo"), TextBox).Text.Trim()
        Dim newItmNm As String = CType(row.FindControl("txtEditPartNm"), TextBox).Text.Trim()
        Dim newExItmNm As String = CType(row.FindControl("txtEditExItmNm"), TextBox).Text.Trim()
        Dim newPoNo As String = CType(row.FindControl("txtEditPoNo"), TextBox).Text.Trim()

        Dim newQty As Decimal = 0
        If Not Decimal.TryParse(CType(row.FindControl("txtEditQty"), TextBox).Text.Trim(), newQty) OrElse newQty <= 0 Then
            lblStatus.Text = "จำนวนต้องเป็นตัวเลขบวก"
            lblStatus.ForeColor = Drawing.Color.Red
            Return
        End If

        Dim newUnitPrice As Decimal = 0
        Decimal.TryParse(CType(row.FindControl("txtEditUnitPrice"), TextBox).Text.Trim(), newUnitPrice)
        Dim newRemark As String = CType(row.FindControl("txtEditRemark"), TextBox).Text.Trim()

        Dim foundRows() As DataRow = dt.Select("ITM_CD = '" & itmCd.Replace("'", "''") & "'")
        If foundRows.Length > 0 Then
            foundRows(0)("ITM_CD") = newItmCd
            foundRows(0)("ITM_NM") = newItmNm
            foundRows(0)("EX_ITM_NM") = newExItmNm
            foundRows(0)("PO_NO") = newPoNo
            foundRows(0)("QTY") = newQty
            foundRows(0)("UNIT_PRICE") = newUnitPrice
            foundRows(0)("REMARK") = newRemark
        Else
            lblStatus.Text = "ไม่พบข้อมูลสินค้าในตาราง"
            lblStatus.ForeColor = Drawing.Color.Red
            Return
        End If

        ItemTable = dt
        gvItemList.EditIndex = -1
        BindGrid()

        lblStatus.Text = "แก้ไขข้อมูลสินค้าเรียบร้อย"
        lblStatus.ForeColor = Drawing.Color.Green
    End Sub

    ' GridView RowDeleting
    Protected Sub gvItemList_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim dt As DataTable = ItemTable
        Dim itmCd As String = gvItemList.DataKeys(e.RowIndex).Value.ToString()

        Dim foundRows = dt.Select("ITM_CD = '" & itmCd.Replace("'", "''") & "'")
        For Each r As DataRow In foundRows
            dt.Rows.Remove(r)
        Next

        ItemTable = dt
        BindGrid()

        If Not String.IsNullOrEmpty(txtDeliveryNoteNo.Text.Trim()) Then
            Using conn As New SqlConnection(ConStr)
                conn.Open()
                Using cmd As New SqlCommand("DELETE FROM [CS_DATA].[dbo].[DELIVERY_NOTE_D] WHERE DL_NO = @DL_NO AND ITM_CD = @ITM_CD", conn)
                    cmd.Parameters.AddWithValue("@DL_NO", txtDeliveryNoteNo.Text.Trim())
                    cmd.Parameters.AddWithValue("@ITM_CD", itmCd)
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End If

        lblStatus.Text = "ลบรายการสินค้าสำเร็จ"
        lblStatus.ForeColor = Drawing.Color.Green
    End Sub

    ' ค้นหาข้อมูลลูกค้าใน Oracle
    Protected Sub btnSearchCustomer_Click(sender As Object, e As EventArgs) Handles btnSearchCustomer.Click
        Try
            Dim dt As New DataTable()
            Dim MySelect As String = "SELECT * FROM CM_BP_ALL WHERE COMPANY_CD = :COMPANY_CD"

            Using conn As New OracleConnection(oradb)
                Using cmd As New OracleCommand(MySelect, conn)
                    cmd.Parameters.Add(New OracleParameter("COMPANY_CD", txtCustomerCode.Text.Trim()))

                    Using da As New OracleDataAdapter(cmd)
                        conn.Open()
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            If dt.Rows.Count > 0 Then
                txtSoldTo.Text = dt.Rows(0)("OFCL_NM").ToString()
                txtAddress.Text = String.Join(" ", dt.Rows(0)("ADDR1").ToString(), dt.Rows(0)("ADDR2").ToString(), dt.Rows(0)("ADDR3").ToString(), dt.Rows(0)("ADDR4").ToString())
                txtTaxId.Text = dt.Rows(0)("CNTCT_PSN").ToString()
                txtBranchNo.Text = dt.Rows(0)("KANA_NM").ToString()
                txtAttn.Text = dt.Rows(0)("USR_NM").ToString()
                txtTel.Text = dt.Rows(0)("TEL").ToString()
                lblMessage.Text = ""
            Else
                lblMessage.Text = "ไม่พบข้อมูลลูกค้าตามรหัสที่ระบุ"
            End If

        Catch ex As Exception
            lblMessage.Text = "เกิดข้อผิดพลาด: " & ex.Message
        End Try
    End Sub

    ' ค้นหาข้อมูลสินค้าใน Oracle
    Protected Sub btnSearchItem_Click(sender As Object, e As EventArgs) Handles btnSearchItem.Click
        Try
            Dim dt As New DataTable()
            Dim MySelect As String = "SELECT * FROM CM_HINMO_ALL WHERE ITM_CD = :ITM_CD"

            Using conn As New OracleConnection(oradb)
                Using cmd As New OracleCommand(MySelect, conn)
                    cmd.Parameters.Add(New OracleParameter("ITM_CD", txtItemCd.Text.Trim()))

                    Using da As New OracleDataAdapter(cmd)
                        conn.Open()
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            If dt.Rows.Count > 0 Then
                txtItemNm.Text = dt.Rows(0)("ITM_NM").ToString()
                txtDetail.Text = dt.Rows(0)("EXT_ITM_NM").ToString()
            Else
                txtItemNm.Text = ""
                txtDetail.Text = ""
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Item not found.');", True)
            End If

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Error: " & ex.Message.Replace("'", "\'") & "');", True)
        End Try
    End Sub

    ' บันทึกข้อมูล Delivery Note Header + Detail
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim DeliverNoteNo As String = txtDeliveryNoteNo.Text.Trim()

            If String.IsNullOrWhiteSpace(DeliverNoteNo) Then
                lblStatus.Text = "Please input Delivery Note number"
                txtDeliveryNoteNo.Focus()
                Exit Sub
            End If

            Dim dt As DataTable = CType(Session("ItemTable"), DataTable)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                lblStatus.Text = "No items to save."
                Exit Sub
            End If

            ' ตรวจสอบว่าเป็นโหมดแก้ไขหรือไม่
            Dim isEditMode As Boolean = If(Session("EditMode") IsNot Nothing AndAlso CBool(Session("EditMode")), True, False)

            Using conn As New SqlConnection(ConStr)
                conn.Open()

                ' ตรวจสอบว่ามี DL_NO อยู่แล้วหรือไม่
                Dim dlExists As Boolean = False
                Dim checkSql As String = "SELECT COUNT(*) FROM [CS_DATA].[dbo].[DELIVERY_NOTE_H] WHERE DL_NO = @DL_NO"
                Using checkCmd As New SqlCommand(checkSql, conn)
                    checkCmd.Parameters.AddWithValue("@DL_NO", DeliverNoteNo)
                    dlExists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0
                End Using

                ' ถ้า DL_NO มีอยู่แล้วแต่ไม่ได้มาจากโหมดแก้ไข ให้เตือนและไม่ให้บันทึก
                If dlExists AndAlso Not isEditMode Then
                    lblStatus.ForeColor = Drawing.Color.Red
                    lblStatus.Text = "Delivery Note No already exists. Please use a different number or click Edit to modify."
                    Exit Sub
                End If

                If dlExists Then
                    ' --- UPDATE HEADER ---
                    Dim updateSql As String = "UPDATE [CS_DATA].[dbo].[DELIVERY_NOTE_H] SET " &
                        "DL_DT = @DL_DT, CS_CD = @CS_CD, SOLD_TO = @SOLD_TO, CS_ADDRESS = @CS_ADDRESS, " &
                        "TAX_ID = @TAX_ID, BRANCH_NO = @BRANCH_NO, ATTN_NM = @ATTN_NM, TEL_NO = @TEL_NO, " &
                        "DL_TKR_NM = @DL_TKR_NM, DL_UPDATE = @DL_UPDATE, LOC_SEND = @LOC_SEND " &
                        "WHERE DL_NO = @DL_NO"

                    Using cmd As New SqlCommand(updateSql, conn)
                        cmd.Parameters.AddWithValue("@DL_NO", DeliverNoteNo)
                        cmd.Parameters.AddWithValue("@DL_DT", Convert.ToDateTime(txtDate.Text))
                        cmd.Parameters.AddWithValue("@CS_CD", txtCustomerCode.Text.Trim())
                        cmd.Parameters.AddWithValue("@SOLD_TO", txtSoldTo.Text.Trim())
                        cmd.Parameters.AddWithValue("@CS_ADDRESS", txtAddress.Text.Trim())
                        cmd.Parameters.AddWithValue("@TAX_ID", txtTaxId.Text.Trim())
                        cmd.Parameters.AddWithValue("@BRANCH_NO", txtBranchNo.Text.Trim())
                        cmd.Parameters.AddWithValue("@ATTN_NM", txtAttn.Text.Trim())
                        cmd.Parameters.AddWithValue("@TEL_NO", txtTel.Text.Trim())
                        cmd.Parameters.AddWithValue("@DL_TKR_NM", "")
                        cmd.Parameters.AddWithValue("@DL_UPDATE", Now)
                        cmd.Parameters.AddWithValue("@LOC_SEND", txtShipPoint.Text.Trim())
                        cmd.ExecuteNonQuery()
                    End Using

                    ' ลบรายการเก่าก่อนเพิ่มใหม่
                    Dim deleteSql As String = "DELETE FROM [CS_DATA].[dbo].[DELIVERY_NOTE_D] WHERE DL_NO = @DL_NO"
                    Using delCmd As New SqlCommand(deleteSql, conn)
                        delCmd.Parameters.AddWithValue("@DL_NO", DeliverNoteNo)
                        delCmd.ExecuteNonQuery()
                    End Using

                Else
                    ' --- INSERT HEADER ---
                    Dim insertSql As String = "INSERT INTO [CS_DATA].[dbo].[DELIVERY_NOTE_H] " &
                        "(DL_NO, DL_DT, CS_CD, SOLD_TO, CS_ADDRESS, TAX_ID, BRANCH_NO, ATTN_NM, TEL_NO, LOC_SEND) " &
                        "VALUES (@DL_NO, @DL_DT, @CS_CD, @SOLD_TO, @CS_ADDRESS, @TAX_ID, @BRANCH_NO, @ATTN_NM, @TEL_NO, @LOC_SEND)"

                    Using cmd As New SqlCommand(insertSql, conn)
                        cmd.Parameters.AddWithValue("@DL_NO", DeliverNoteNo)
                        cmd.Parameters.AddWithValue("@DL_DT", Convert.ToDateTime(txtDate.Text))
                        cmd.Parameters.AddWithValue("@CS_CD", txtCustomerCode.Text.Trim())
                        cmd.Parameters.AddWithValue("@SOLD_TO", txtSoldTo.Text.Trim())
                        cmd.Parameters.AddWithValue("@CS_ADDRESS", txtAddress.Text.Trim())
                        cmd.Parameters.AddWithValue("@TAX_ID", txtTaxId.Text.Trim())
                        cmd.Parameters.AddWithValue("@BRANCH_NO", txtBranchNo.Text.Trim())
                        cmd.Parameters.AddWithValue("@ATTN_NM", txtAttn.Text.Trim())
                        cmd.Parameters.AddWithValue("@TEL_NO", txtTel.Text.Trim())
                        cmd.Parameters.AddWithValue("@LOC_SEND", txtShipPoint.Text.Trim())
                        cmd.ExecuteNonQuery()
                    End Using
                End If

                ' --- INSERT DETAIL ---
                For Each row As DataRow In dt.Rows
                    Dim detailSql As String = "INSERT INTO [CS_DATA].[dbo].[DELIVERY_NOTE_D] " &
                        "(DL_NO, ITM_CD, ITM_NM, EX_ITM_NM, PO_NO, QTY, UNIT_PRICE, REMARK) " &
                        "VALUES (@DL_NO, @ITM_CD, @ITM_NM, @EX_ITM_NM, @PO_NO, @QTY, @UNIT_PRICE, @REMARK)"

                    Using cmd As New SqlCommand(detailSql, conn)
                        cmd.Parameters.AddWithValue("@DL_NO", DeliverNoteNo)
                        cmd.Parameters.AddWithValue("@ITM_CD", row("ITM_CD").ToString())
                        cmd.Parameters.AddWithValue("@ITM_NM", row("ITM_NM").ToString())
                        cmd.Parameters.AddWithValue("@EX_ITM_NM", row("EX_ITM_NM").ToString())
                        cmd.Parameters.AddWithValue("@PO_NO", row("PO_NO").ToString())
                        cmd.Parameters.AddWithValue("@QTY", Convert.ToDecimal(row("QTY")))
                        cmd.Parameters.AddWithValue("@UNIT_PRICE", Convert.ToDecimal(row("UNIT_PRICE")))
                        cmd.Parameters.AddWithValue("@REMARK", If(row.Table.Columns.Contains("REMARK"), row("REMARK").ToString(), ""))
                        cmd.ExecuteNonQuery()
                    End Using
                Next

                lblStatus.ForeColor = Drawing.Color.Green
                lblStatus.Text = "Data saved successfully."

                ' ล้างข้อมูล
                InitItemTable()
                txtDeliveryNoteNo.Text = ""
                txtDate.Text = ""
                txtCustomerCode.Text = ""
                txtSoldTo.Text = ""
                txtAddress.Text = ""
                txtTaxId.Text = ""
                txtBranchNo.Text = ""
                txtAttn.Text = ""
                txtTel.Text = ""
                txtShipPoint.Text = ""
                Session("EditMode") = Nothing

            End Using
        Catch ex As Exception
            lblStatus.ForeColor = Drawing.Color.Red
            lblStatus.Text = "Error occurred: " & ex.Message
        End Try
    End Sub

    ' เคลียร์ฟอร์มหลัก
    Private Sub ClearForm()
        txtDeliveryNoteNo.Text = ""
        txtCustomerCode.Text = ""
        txtSoldTo.Text = ""
        txtAddress.Text = ""
        txtTaxId.Text = ""
        txtBranchNo.Text = ""
        txtAttn.Text = ""
        txtTel.Text = ""
        txtShipPoint.Text = ""
        txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim deliveryNo As String = txtDNSearch.Text.Trim()

        If String.IsNullOrEmpty(deliveryNo) Then
            lblMessage.Text = "Please enter Delivery Note Number."
            Exit Sub
        End If

        Try
            Using MyConn As New SqlConnection(ConStr)
                MyConn.Open()

                ' Header
                Dim sqlHeader As String = "SELECT * FROM [CS_DATA].[dbo].[DELIVERY_NOTE_H] WHERE DL_NO = @DL_NO"
                Dim cmdHeader As New SqlCommand(sqlHeader, MyConn)
                cmdHeader.Parameters.AddWithValue("@DL_NO", deliveryNo)
                Dim dtHeader As New DataTable()
                Using daHeader As New SqlDataAdapter(cmdHeader)
                    daHeader.Fill(dtHeader)
                End Using

                If dtHeader.Rows.Count = 0 Then
                    lblMessage.Text = "No data found for Delivery Note Number " & deliveryNo
                    ClearForm()
                    Return
                End If

                ' เก็บสถานะว่าเป็นโหมดแก้ไข
                Session("EditMode") = True

                Dim rowHeader = dtHeader.Rows(0)
                txtCustomerCode.Text = rowHeader("CS_CD").ToString()
                txtDeliveryNoteNo.Text = rowHeader("DL_NO").ToString()
                If IsDate(rowHeader("DL_DT")) Then
                    txtDate.Text = Convert.ToDateTime(rowHeader("DL_DT")).ToString("yyyy-MM-dd")
                End If
                txtSoldTo.Text = rowHeader("SOLD_TO").ToString()
                txtAddress.Text = rowHeader("CS_ADDRESS").ToString()
                txtTaxId.Text = rowHeader("TAX_ID").ToString()
                txtBranchNo.Text = rowHeader("BRANCH_NO").ToString()
                txtAttn.Text = rowHeader("ATTN_NM").ToString()
                txtTel.Text = rowHeader("TEL_NO").ToString()
                txtShipPoint.Text = rowHeader("LOC_SEND").ToString()

                ' Detail
                Dim sqlDetail As String = "SELECT * FROM [CS_DATA].[dbo].[DELIVERY_NOTE_D] WHERE DL_NO = @DL_NO"
                Dim cmdDetail As New SqlCommand(sqlDetail, MyConn)
                cmdDetail.Parameters.AddWithValue("@DL_NO", deliveryNo)

                Dim dtDetail As New DataTable()
                Using daDetail As New SqlDataAdapter(cmdDetail)
                    daDetail.Fill(dtDetail)
                End Using

                Session("ItemTable") = dtDetail
                BindGrid()

                lblMessage.Text = ""
            End Using
        Catch ex As Exception
            lblMessage.Text = "Error: " & ex.Message
            ClearForm()
        End Try
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("home.aspx")
    End Sub

End Class
