Public Class home
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnDeliveryNote_Click(sender As Object, e As EventArgs)
        Response.Redirect("DeliveryNote.aspx")
    End Sub

    Protected Sub btnReplacement_Click(sender As Object, e As EventArgs)
        Response.Redirect("Replacement.aspx")
    End Sub
End Class