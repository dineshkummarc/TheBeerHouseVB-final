Imports MB.TheBeerHouse.BLL.Newsletters

Namespace MB.TheBeerHouse.UI.Controls
    Partial Class UserProfile
        Inherits System.Web.UI.UserControl

        Private _userName As String = ""
        Public Property Username() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property


        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.Page.RegisterRequiresControlState(Me)
        End Sub

        Protected Overrides Sub LoadControlState(ByVal savedState As Object)
            Dim ctlState As Object() = CType(savedState, Object())
            MyBase.LoadControlState(savedState)
            _userName = CStr(ctlState(1))
        End Sub

        Protected Overrides Function SaveControlState() As Object
            Dim ctlState() As Object
            ReDim ctlState(1) ' Initializes the array with 2 objects
            ctlState(0) = MyBase.SaveControlState()
            ctlState(1) = _userName
            Return ctlState
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Me.IsPostBack Then
                ddlCountries.DataSource = Helpers.GetCountries
                ddlCountries.DataBind()

                ' if the UserName property contains an empty string, retrieve the profile
                ' for the current user, otherwise for the specified user
                Dim profile As ProfileCommon = Me.Profile
                If Me.Username.Length > 0 Then
                    profile = Me.Profile.GetProfile(_userName)
                End If

                ddlSubscriptions.SelectedValue = profile.Preferences.Newsletter.ToString
                ddlLanguages.SelectedValue = profile.Preferences.Culture
                txtFirstName.Text = profile.FirstName
                txtLastName.Text = profile.LastName
                ddlGenders.SelectedValue = profile.Gender
                If Not profile.BirthDate = DateTime.MinValue Then
                    txtBirthDate.Text = profile.BirthDate.ToShortDateString
                End If
                ddlOccupations.SelectedValue = profile.Occupation
                txtWebsite.Text = profile.Website
                txtStreet.Text = profile.Address.Street
                txtCity.Text = profile.Address.City
                txtPostalCode.Text = profile.Address.PostalCode
                txtState.Text = profile.Address.State
                ddlCountries.SelectedValue = profile.Address.Country
                txtPhone.Text = profile.Contacts.Phone
                txtFax.Text = profile.Contacts.Fax
                txtAvatarUrl.Text = profile.Forum.AvatarUrl
                txtSignature.Text = profile.Forum.Signature
            End If
        End Sub

        Public Sub SaveProfile()
            ' if the UserName property contains an emtpy string, save the current user's profile,
            ' othwerwise save the profile for the specified user
            Dim profile As ProfileCommon = Me.Profile
            If Me.Username.Length > 0 Then
                profile = Me.Profile.GetProfile(_userName)
            End If

            profile.Preferences.Newsletter = CType([Enum].Parse( _
                GetType(SubscriptionType), ddlSubscriptions.SelectedValue), SubscriptionType)
            profile.Preferences.Culture = ddlLanguages.SelectedValue
            profile.FirstName = txtFirstName.Text
            profile.LastName = txtLastName.Text
            profile.Gender = ddlGenders.SelectedValue
            If txtBirthDate.Text.Trim().Length > 0 Then
                profile.BirthDate = DateTime.Parse(txtBirthDate.Text)
            End If
            profile.Occupation = ddlOccupations.SelectedValue
            profile.Website = txtWebsite.Text
            profile.Address.Street = txtStreet.Text
            profile.Address.City = txtCity.Text
            profile.Address.PostalCode = txtPostalCode.Text
            profile.Address.State = txtState.Text
            profile.Address.Country = ddlCountries.SelectedValue
            profile.Contacts.Phone = txtPhone.Text
            profile.Contacts.Fax = txtFax.Text
            profile.Forum.AvatarUrl = txtAvatarUrl.Text
            profile.Forum.Signature = txtSignature.Text
            profile.Save()
        End Sub
    End Class
End Namespace
