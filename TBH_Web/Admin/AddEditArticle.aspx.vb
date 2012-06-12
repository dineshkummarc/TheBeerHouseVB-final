Imports System.Security
Imports FredCK.FCKeditorV2
Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class AddEditArticle
        Inherits BasePage

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                If Not String.IsNullOrEmpty(Me.Request.QueryString("ID")) Then
                    If Me.User.Identity.IsAuthenticated AndAlso _
                        (Me.User.IsInRole("Administrators") Or _
                        Me.User.IsInRole("Editors")) Then

                        dvwArticle.ChangeMode(DetailsViewMode.Edit)
                    Else
                        Throw New SecurityException( _
                            "You are not allowed to edit existent articles!")
                    End If
                End If
            End If
        End Sub

        Protected Sub dvwArticle_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwArticle.ItemCreated
            Dim ctl As Control = dvwArticle.FindControl("txtBody")
            If Not IsNothing(ctl) Then
                Dim txtBody As FCKeditor = CType(ctl, FCKeditor)
                txtBody.BasePath = Me.BaseUrl & "FCKeditor/"
            End If
        End Sub

        Protected Sub dvwArticle_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwArticle.DataBound
            If dvwArticle.CurrentMode = DetailsViewMode.Insert Then
                Dim chkApproved As CheckBox = _
                    CType(dvwArticle.FindControl("chkApproved"), CheckBox)
                Dim chkListed As CheckBox = _
                    CType(dvwArticle.FindControl("chkListed"), CheckBox)
                Dim chkCommentsEnabled As CheckBox = _
                    CType(dvwArticle.FindControl("chkCommentsEnabled"), CheckBox)

                chkListed.Checked = True
                chkCommentsEnabled.Checked = True

                Dim canApprove As Boolean = _
                    (Me.User.IsInRole("Administrators") OrElse _
                    Me.User.IsInRole("Editors"))
                chkApproved.Enabled = canApprove
                chkApproved.Checked = canApprove
            End If
        End Sub

        Protected Sub dvwArticle_ModeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwArticle.ModeChanged
            UpdateTitle()
        End Sub

        Private Sub UpdateTitle()
            lblNewArticle.Visible = (dvwArticle.CurrentMode = DetailsViewMode.Insert)
            lblEditArticle.Visible = Not lblNewArticle.Visible
        End Sub
    End Class
End Namespace
