Namespace MB.TheBeerHouse.UI.Controls
    Partial Class RatingDisplay
        Inherits System.Web.UI.UserControl

        Private _value As Double = 0.0
        Public Property Value() As Double
            Get
                Return _value
            End Get
            Set(ByVal value As Double)
                _value = value
                If _value >= 1 Then
                    lblNotRated.Visible = False
                    imgRating.Visible = True
                    imgRating.AlternateText = "Average rating: " + _value.ToString("N1")
                    Dim url As String = "~/images/stars{0}.gif"
                    If _value <= 1.3 Then
                        url = String.Format(url, "10")
                    ElseIf _value <= 1.8 Then
                        url = String.Format(url, "15")
                    ElseIf _value <= 2.3 Then
                        url = String.Format(url, "20")
                    ElseIf _value <= 2.8 Then
                        url = String.Format(url, "25")
                    ElseIf _value <= 3.3 Then
                        url = String.Format(url, "30")
                    ElseIf _value <= 3.8 Then
                        url = String.Format(url, "35")
                    ElseIf _value <= 4.3 Then
                        url = String.Format(url, "40")
                    ElseIf _value <= 4.8 Then
                        url = String.Format(url, "45")
                    Else
                        url = String.Format(url, "50")
                    End If
                    imgRating.ImageUrl = url
                Else
                    lblNotRated.Visible = True
                    imgRating.Visible = False
                End If
            End Set
        End Property

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.Page.RegisterRequiresControlState(Me)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal savedState As Object)
            Dim ctlState As Object() = CType(savedState, Object())
            MyBase.LoadControlState(ctlState(0))
            Me.Value = CDbl(ctlState(1))
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim ctlState As Object()
            ReDim ctlState(2)
            ctlState(0) = MyBase.SaveControlState()
            ctlState(1) = Me.Value
            Return ctlState
        End Function
    End Class
End Namespace
