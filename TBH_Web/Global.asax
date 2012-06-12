<%@ Application Language="VB" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
    
    Sub Profile_MigrateAnonymous(ByVal sender As Object, ByVal e As ProfileMigrateEventArgs)
        ' get a reference to the previously anonymous user's profile
        Dim anonProfile As ProfileCommon = Profile.GetProfile(e.AnonymousID)
        ' if set, copy its Theme to the current user's profile
        If Not String.IsNullOrEmpty(anonProfile.Preferences.Theme) Then
            Profile.Preferences.Theme = anonProfile.Preferences.Theme
        End If
        ' delete the anonymous profile
        ProfileManager.DeleteProfile(e.AnonymousID)
        AnonymousIdentificationModule.ClearAnonymousIdentifier()
    End Sub
       
</script>