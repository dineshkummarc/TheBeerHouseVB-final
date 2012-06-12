Namespace MB.TheBeerHouse.UI
    Partial Class Template
        Inherits System.Web.UI.MasterPage

        Private _enablePersonalization As Boolean = False

        Public Property EnablePersonalization() As Boolean
            Get
                Return _enablePersonalization
            End Get
            Set(ByVal value As Boolean)
                _enablePersonalization = value
                PersonalizationManager1.Visible = ( _
                    Me.Page.User.Identity.IsAuthenticated And value)
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.Page.User.Identity.IsAuthenticated Then _
                PersonalizationManager1.Visible = False
        End Sub
    End Class
End Namespace

