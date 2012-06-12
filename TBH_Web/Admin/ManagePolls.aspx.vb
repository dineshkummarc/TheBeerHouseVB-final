Imports MB.TheBeerHouse

Namespace MB.TheBeerHouse.UI.Admin
    Partial Class ManagePolls
        Inherits BasePage

        Private Sub DeselectPoll()
            gvwPolls.SelectedIndex = -1
            gvwPolls.DataBind()
            dvwPoll.ChangeMode(DetailsViewMode.Insert)
            panOptions.Visible = False
        End Sub

        Private Sub DeselectOption()
            gvwOptions.SelectedIndex = -1
            gvwOptions.DataBind()
            dvwOption.ChangeMode(DetailsViewMode.Insert)
        End Sub

        Protected Sub gvwPolls_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwPolls.SelectedIndexChanged
            dvwPoll.ChangeMode(DetailsViewMode.Edit)
            panOptions.Visible = True
        End Sub

        Protected Sub gvwPolls_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwPolls.RowDeleted
            DeselectPoll()
        End Sub

        Protected Sub gvwPolls_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwPolls.RowCommand
            If e.CommandName = "Archive" Then
                Dim pollID As Integer = Convert.ToInt32( _
                   gvwPolls.DataKeys(Convert.ToInt32(e.CommandArgument))(0))
                MB.TheBeerHouse.BLL.Polls.Poll.ArchivePoll(pollID)
                DeselectPoll()
            End If
        End Sub

        Protected Sub gvwPolls_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwPolls.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btnArchive As ImageButton = CType(e.Row.Cells(5).Controls(0), ImageButton)
                btnArchive.OnClientClick = "if (confirm('Are you sure you want to archive this poll?') == false) return false;"
                Dim btnDelete As ImageButton = CType(e.Row.Cells(6).Controls(0), ImageButton)
                btnDelete.OnClientClick = "if (confirm('Are you sure you want to delete this poll?') == false) return false;"
            End If
        End Sub

        Protected Sub dvwPoll_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwPoll.ItemInserted
            DeselectPoll()
        End Sub

        Protected Sub dvwPoll_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwPoll.ItemUpdated
            DeselectPoll()
        End Sub

        Protected Sub dvwPoll_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwPoll.ItemCommand
            If e.CommandName = "Cancel" Then
                DeselectPoll()
            End If
        End Sub

        Protected Sub dvwOption_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvwOption.ItemInserted
            DeselectOption()
        End Sub

        Protected Sub dvwOption_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvwOption.ItemUpdated
            DeselectOption()
        End Sub

        Protected Sub dvwOption_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewCommandEventArgs) Handles dvwOption.ItemCommand
            If e.CommandName = "Cancel" Then
                DeselectOption()
            End If
        End Sub

        Protected Sub gvwOptions_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwOptions.RowCreated
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim btn As ImageButton = CType(e.Row.Cells(4).Controls(0), ImageButton)
                btn.OnClientClick = "if (confirm('Are you sure you want to delete this option?') == false) return false;"
            End If
        End Sub

        Protected Sub gvwOptions_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwOptions.RowDeleted
            DeselectOption()
        End Sub

        Protected Sub gvwOptions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwOptions.SelectedIndexChanged
            dvwOption.ChangeMode(DetailsViewMode.Edit)
        End Sub

        Protected Sub dvwPoll_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwPoll.ItemCreated
            For Each ctl As Control In dvwPoll.Rows(dvwPoll.Rows.Count - 1).Controls(0).Controls
                If TypeOf (ctl) Is LinkButton Then
                    Dim lnk As LinkButton = CType(ctl, LinkButton)
                    If lnk.CommandName = "Insert" Or lnk.CommandName = "Update" Then
                        lnk.ValidationGroup = "Poll"
                    End If
                End If
            Next
        End Sub

        Protected Sub dvwOption_ItemCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles dvwOption.ItemCreated
            For Each ctl As Control In dvwOption.Rows(dvwOption.Rows.Count - 1).Controls(0).Controls
                If TypeOf (ctl) Is LinkButton Then
                    Dim lnk As LinkButton = CType(ctl, LinkButton)
                    If lnk.CommandName = "Insert" Or lnk.CommandName = "Update" Then
                        lnk.ValidationGroup = "Option"
                    End If
                End If
            Next
        End Sub
    End Class
End Namespace