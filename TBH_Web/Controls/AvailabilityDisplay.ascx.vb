Namespace MB.TheBeerHouse.UI.Controls
    Partial Class AvailabilityDisplay
        Inherits System.Web.UI.UserControl

        Private _value As Integer = 0

        Public Property Value() As Integer
            Get
                Return _value
            End Get
            Set(ByVal value As Integer)
                _value = value
                If _value <= 0 Then
                    imgAvailability.ImageUrl = "~/images/lightred.gif"
                    imgAvailability.AlternateText = "Currently not available"
                ElseIf _value <= Globals.Settings.Store.LowAvailability Then
                    imgAvailability.ImageUrl = "~/images/lightyellow.gif"
                    imgAvailability.AlternateText = "Few units available"
                Else
                    imgAvailability.ImageUrl = "~/images/lightgreen.gif"
                    imgAvailability.AlternateText = "Available"
                End If
            End Set
        End Property

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.Page.RegisterRequiresControlState(Me)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal savedState As Object)
            Dim ctlState() As Object = CType(savedState, Object())
            MyBase.LoadControlState(ctlState(0))
            Me.Value = CInt(ctlState(1))
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim ctlState() As Object
            ReDim ctlState(2)
            ctlState(0) = MyBase.SaveControlState()
            ctlState(1) = Me.Value
            Return ctlState
        End Function
    End Class
End Namespace
