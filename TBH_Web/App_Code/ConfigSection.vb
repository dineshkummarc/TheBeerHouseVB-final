Imports System.Configuration
Imports System.Web.Configuration
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse
    Public Class ConfigSection

    End Class

    Public Class TheBeerHouseSection
        Inherits ConfigurationSection

        <ConfigurationProperty("defaultConnectionStringName", DefaultValue:="LocalSqlServer")> _
        Public Property DefaultConnectionStringName() As String
            Get
                Return CStr(Me("defaultConnectionStringName"))
            End Get
            Set(ByVal value As String)
                Me("DefaultConnectionStringName") = value
            End Set
        End Property

        <ConfigurationProperty("defaultCacheDuration", DefaultValue:="600")> _
        Public Property DefaultCacheDuration() As Integer
            Get
                Return CInt(Me("defaultCacheDuration"))
            End Get
            Set(ByVal value As Integer)
                Me("defaultCacheDuration") = value
            End Set
        End Property

        <ConfigurationProperty("contactForm", IsRequired:=True)> _
        Public ReadOnly Property ContactForm() As ContactFormElement
            Get
                Return CType(Me("contactForm"), ContactFormElement)
            End Get
        End Property

        <ConfigurationProperty("articles", IsRequired:=True)> _
        Public ReadOnly Property Articles() As ArticlesElement
            Get
                Return CType(Me("articles"), ArticlesElement)
            End Get
        End Property

        <ConfigurationProperty("polls", IsRequired:=True)> _
        Public ReadOnly Property Polls() As PollsElement
            Get
                Return CType(Me("polls"), PollsElement)
            End Get
        End Property

        <ConfigurationProperty("newsletters", IsRequired:=True)> _
        Public ReadOnly Property Newsletters() As NewslettersElement
            Get
                Return CType(Me("newsletters"), NewslettersElement)
            End Get
        End Property

        <ConfigurationProperty("forums", IsRequired:=True)> _
        Public ReadOnly Property Forums() As ForumsElement
            Get
                Return CType(Me("forums"), ForumsElement)
            End Get
        End Property

        <ConfigurationProperty("store", IsRequired:=True)> _
        Public ReadOnly Property Store() As StoreElement
            Get
                Return CType(Me("store"), StoreElement)
            End Get
        End Property
    End Class

    Public Class ContactFormElement
        Inherits ConfigurationElement

        <ConfigurationProperty("mailSubject", DefaultValue:="Mail from TheBeerHouse: {0}")> _
        Public Property MailSubject() As String
            Get
                Return CStr(Me("mailSubject"))
            End Get
            Set(ByVal value As String)
                Me("mailSubject") = value
            End Set
        End Property

        <ConfigurationProperty("mailTo", IsRequired:=True)> _
        Public Property MailTo() As String
            Get
                Return CStr(Me("mailTo"))
            End Get
            Set(ByVal value As String)
                Me("mailTo") = value
            End Set
        End Property

        <ConfigurationProperty("mailCC")> _
        Public Property MailCC() As String
            Get
                Return CStr(Me("mailCC"))
            End Get
            Set(ByVal value As String)
                Me("mailCC") = value
            End Set
        End Property

    End Class

    Public Class ArticlesElement
        Inherits ConfigurationElement

        <ConfigurationProperty("connectionStringName")> _
        Public Property ConnectionStringName() As String
            Get
                Return CStr(Me("connectionStringName"))
            End Get
            Set(ByVal value As String)
                Me("ConnectionStringName") = value
            End Set
        End Property

        Public ReadOnly Property ConnectionString() As String
            Get
                Dim connStringName As String
                If String.IsNullOrEmpty(Me.ConnectionStringName) Then
                    connStringName = Globals.Settings.DefaultConnectionStringName
                Else
                    connStringName = Me.ConnectionStringName
                End If
                Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
            End Get
        End Property

        <ConfigurationProperty("providerType", DefaultValue:="MB.TheBeerHouse.DAL.SqlClient.SqlArticlesProvider")> _
        Public Property ProviderType() As String
            Get
                Return CStr(Me("providerType"))
            End Get
            Set(ByVal value As String)
                Me("providerType") = value
            End Set
        End Property

        <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
        Public Property RatingLockInterval() As Integer
            Get
                Return CInt(Me("ratingLockInterval"))
            End Get
            Set(ByVal value As Integer)
                Me("ratingLockInterval") = value
            End Set
        End Property

        <ConfigurationProperty("pageSize", DefaultValue:="10")> _
        Public Property PageSize() As Integer
            Get
                Return CInt(Me("pageSize"))
            End Get
            Set(ByVal value As Integer)
                Me("pageSize") = value
            End Set
        End Property

        <ConfigurationProperty("rssItems", DefaultValue:="5")> _
        Public Property RssItems() As Integer
            Get
                Return CInt(Me("rssItems"))
            End Get
            Set(ByVal value As Integer)
                Me("rssItems") = value
            End Set
        End Property

        <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
        Public Property EnableCaching() As Boolean
            Get
                Return CBool(Me("enableCaching"))
            End Get
            Set(ByVal value As Boolean)
                Me("enableCaching") = value
            End Set
        End Property

        <ConfigurationProperty("cacheDuration")> _
        Public Property CacheDuration() As Integer
            Get
                Dim duration As Integer = CInt(Me("cacheDuration"))
                If duration > 0 Then
                    Return duration
                Else
                    Return Globals.Settings.DefaultCacheDuration
                End If
            End Get
            Set(ByVal value As Integer)
                Me("cacheDuration") = value
            End Set
        End Property
    End Class

    Public Class PollsElement
        Inherits ConfigurationElement

        <ConfigurationProperty("connectionStringName")> _
        Public Property ConnectionStringName() As String
            Get
                Return CStr(Me("connectionStringName"))
            End Get
            Set(ByVal value As String)
                Me("connectionStringName") = value
            End Set
        End Property

        Public ReadOnly Property ConnectionString() As String
            Get
                Dim connStringName As String
                If String.IsNullOrEmpty(Me.ConnectionStringName) Then
                    connStringName = Globals.Settings.DefaultConnectionStringName
                Else
                    connStringName = Me.ConnectionStringName
                End If
                Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
            End Get
        End Property

        <ConfigurationProperty("providerType", DefaultValue:="MB.TheBeerHouse.DAL.SqlClient.SqlPollsProvider")> _
        Public Property ProviderType() As String
            Get
                Return CStr(Me("providerType"))
            End Get
            Set(ByVal value As String)
                Me("providerType") = value
            End Set
        End Property

        <ConfigurationProperty("votingLockInterval", DefaultValue:="15")> _
        Public Property VotingLockInterval() As Integer
            Get
                Return CInt(Me("votingLockInterval"))
            End Get
            Set(ByVal value As Integer)
                Me("votingLockInterval") = value
            End Set
        End Property

        <ConfigurationProperty("votingLockByCookie", DefaultValue:="true")> _
        Public Property VotingLockByCookie() As Boolean
            Get
                Return CBool(Me("votingLockByCookie"))
            End Get
            Set(ByVal value As Boolean)
                Me("votingLockByCookie") = value
            End Set
        End Property

        <ConfigurationProperty("votingLockByIP", DefaultValue:="true")> _
        Public Property VotingLockByIP() As Boolean
            Get
                Return CBool(Me("votingLockByIP"))
            End Get
            Set(ByVal value As Boolean)
                Me("votingLockByIP") = value
            End Set
        End Property

        <ConfigurationProperty("archiveIsPublic", DefaultValue:="false")> _
        Public Property ArchiveIsPublic() As Boolean
            Get
                Return CBool(Me("archiveIsPublic"))
            End Get
            Set(ByVal value As Boolean)
                Me("archiveIsPublic") = value
            End Set
        End Property

        <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
        Public Property EnableCaching() As Boolean
            Get
                Return CBool(Me("enableCaching"))
            End Get
            Set(ByVal value As Boolean)
                Me("enableCaching") = value
            End Set
        End Property

        <ConfigurationProperty("cacheDuration")> _
        Public Property CacheDuration() As Integer
            Get
                Dim duration As Integer = CInt(Me("cacheDuration"))
                If duration <= 0 Then
                    duration = Globals.Settings.DefaultCacheDuration
                End If
                Return duration
            End Get
            Set(ByVal value As Integer)
                Me("cacheDuration") = value
            End Set
        End Property
    End Class

    Public Class NewslettersElement
        Inherits ConfigurationElement

        <ConfigurationProperty("connectionStringName")> _
        Public Property ConnectionStringName() As String
            Get
                Return CStr(Me("connectionStringName"))
            End Get
            Set(ByVal value As String)
                Me("connectionStringName") = value
            End Set
        End Property

        Public ReadOnly Property ConnectionString() As String
            Get
                Dim connStringName As String
                If String.IsNullOrEmpty(Me.ConnectionStringName) Then
                    connStringName = Globals.Settings.DefaultConnectionStringName
                Else
                    connStringName = Me.ConnectionStringName
                End If
                Return WebConfigurationManager.ConnectionStrings( _
                    connStringName).ConnectionString
            End Get
        End Property

        <ConfigurationProperty("providerType", _
            DefaultValue:="MB.TheBeerHouse.DAL.SqlClient.SqlNewslettersProvider")> _
        Public Property ProviderType() As String
            Get
                Return CStr(Me("providerType"))
            End Get
            Set(ByVal value As String)
                Me("providerType") = value
            End Set
        End Property

        <ConfigurationProperty("fromEmail", IsRequired:=True)> _
        Public Property FromEmail() As String
            Get
                Return CStr(Me("fromEmail"))
            End Get
            Set(ByVal value As String)
                Me("fromEmail") = value
            End Set
        End Property

        <ConfigurationProperty("fromDisplayName", isrequired:=True)> _
        Public Property FromDisplayName() As String
            Get
                Return CStr(Me("fromDisplayName"))
            End Get
            Set(ByVal value As String)
                Me("fromDisplayName") = value
            End Set
        End Property

        <ConfigurationProperty("hideFromArchiveInterval", defaultvalue:="15")> _
        Public Property HideFromArchiveInterval() As Integer
            Get
                Return CInt(Me("hideFromArchiveInterval"))
            End Get
            Set(ByVal value As Integer)
                Me("hideFromArchiveInterval") = value
            End Set
        End Property

        <ConfigurationProperty("archiveIsPublic", defaultvalue:="false")> _
        Public Property ArchiveIsPublic() As Boolean
            Get
                Return CBool(Me("archiveIsPublic"))
            End Get
            Set(ByVal value As Boolean)
                Me("archiveIsPublic") = value
            End Set
        End Property

        <ConfigurationProperty("enableCaching", defaultvalue:="true")> _
        Public Property EnableCaching() As Boolean
            Get
                Return CBool(Me("enableCaching"))
            End Get
            Set(ByVal value As Boolean)
                Me("enableCaching") = value
            End Set
        End Property

        <ConfigurationProperty("cacheDuration")> _
        Public Property CacheDuration() As Integer
            Get
                Dim duration As Integer = CInt(Me("cacheDuration"))
                If duration > 0 Then
                    Return duration
                Else
                    Return Globals.Settings.DefaultCacheDuration
                End If
            End Get
            Set(ByVal value As Integer)
                Me("cacheDuration") = value
            End Set
        End Property
    End Class

    Public Class ForumsElement
        Inherits ConfigurationElement

        <ConfigurationProperty("connectionStringName")> _
          Public Property ConnectionStringName() As String
            Get
                Return CStr(Me("connectionStringName"))
            End Get
            Set(ByVal value As String)
                Me("connectionStringName") = value
            End Set
        End Property

        Public ReadOnly Property ConnectionString() As String
            Get
                Dim connStringName As String
                If String.IsNullOrEmpty(Me.ConnectionStringName) Then
                    connStringName = Globals.Settings.DefaultConnectionStringName
                Else
                    connStringName = Me.ConnectionStringName
                End If
                Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
            End Get
        End Property

        <ConfigurationProperty("providerType", DefaultValue:="MB.TheBeerHouse.DAL.SqlClient.SqlForumsProvider")> _
        Public Property Providertype() As String
            Get
                Return CStr(Me("providerType"))
            End Get
            Set(ByVal value As String)
                Me("providerType") = value
            End Set
        End Property

        <ConfigurationProperty("threadsPageSize", DefaultValue:="25")> _
        Public Property ThreadsPageSize() As Integer
            Get
                Return CInt(Me("threadsPageSize"))
            End Get
            Set(ByVal value As Integer)
                Me("threadsPageSize") = value
            End Set
        End Property

        <ConfigurationProperty("postsPageSize", DefaultValue:="10")> _
        Public Property PostsPageSize() As Integer
            Get
                Return CInt(Me("postsPageSize"))
            End Get
            Set(ByVal value As Integer)
                Me("postsPageSize") = value
            End Set
        End Property

        <ConfigurationProperty("rssItems", DefaultValue:="5")> _
        Public Property RssItems() As Integer
            Get
                Return CInt(Me("rssItems"))
            End Get
            Set(ByVal value As Integer)
                Me("rssItems") = value
            End Set
        End Property

        <ConfigurationProperty("hotThreadPosts", DefaultValue:="25")> _
        Public Property HotThreadPosts() As Integer
            Get
                Return CInt(Me("hotThreadPosts"))
            End Get
            Set(ByVal value As Integer)
                Me("hotThreadPosts") = value
            End Set
        End Property

        <ConfigurationProperty("bronzePosterPosts", DefaultValue:="100")> _
        Public Property BronzePosterPosts() As Integer
            Get
                Return CInt(Me("bronzePosterPosts"))
            End Get
            Set(ByVal value As Integer)
                Me("bronzePosterPosts") = value
            End Set
        End Property

        <ConfigurationProperty("bronzePosterDescription", DefaultValue:="Bronze Poster")> _
        Public Property BronzePosterDescription() As String
            Get
                Return CStr(Me("bronzePosterDescription"))
            End Get
            Set(ByVal value As String)
                Me("bronzePosterDescription") = value
            End Set
        End Property

        <ConfigurationProperty("silverPosterPosts", DefaultValue:="500")> _
        Public Property SilverPosterPosts() As Integer
            Get
                Return CInt(Me("silverPosterPosts"))
            End Get
            Set(ByVal value As Integer)
                Me("silverPosterPosts") = value
            End Set
        End Property

        <ConfigurationProperty("silverPosterDescription", DefaultValue:="Silver Poster")> _
        Public Property SilverPosterDescription() As String
            Get
                Return CStr(Me("silverPosterDescription"))
            End Get
            Set(ByVal value As String)
                Me("silverPosterDescription") = value
            End Set
        End Property

        <ConfigurationProperty("goldPosterPosts", DefaultValue:="1000")> _
        Public Property GoldPosterPosts() As Integer
            Get
                Return CInt(Me("goldPosterPosts"))
            End Get
            Set(ByVal value As Integer)
                Me("goldPosterPosts") = value
            End Set
        End Property

        <ConfigurationProperty("goldPosterDescription", DefaultValue:="Gold Poster")> _
        Public Property GoldPosterDescription() As String
            Get
                Return CStr(Me("goldPosterDescription"))
            End Get
            Set(ByVal value As String)
                Me("goldPosterDescription") = value
            End Set
        End Property

        <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
        Public Property EnableCaching() As Boolean
            Get
                Return CBool(Me("enableCaching"))
            End Get
            Set(ByVal value As Boolean)
                Me("enableCaching") = value
            End Set
        End Property

        <ConfigurationProperty("cacheDuration")> _
        Public Property CacheDuration() As Integer
            Get
                Dim duration As Integer = CInt(Me("cacheDuration"))
                If duration > 0 Then
                    Return duration
                Else
                    Return Globals.Settings.DefaultCacheDuration
                End If
            End Get
            Set(ByVal value As Integer)
                Me("cacheDuration") = value
            End Set
        End Property
    End Class

    Public Class StoreElement
        Inherits ConfigurationElement

        <ConfigurationProperty("connectionStringName")> _
        Public Property ConnectionStringName() As String
            Get
                Return Me("connectionStringName")
            End Get
            Set(ByVal value As String)
                Me("connectionStringName") = value
            End Set
        End Property

        Public ReadOnly Property ConnectionString() As String
            Get
                Dim connStringName As String = Me.ConnectionStringName
                If String.IsNullOrEmpty(Me.ConnectionStringName) Then
                    connStringName = Globals.Settings.DefaultConnectionStringName
                End If
                Return WebConfigurationManager.ConnectionStrings(connStringName).ConnectionString
            End Get
        End Property

        <ConfigurationProperty("providerType", DefaultValue:="MB.TheBeerHouse.DAL.SqlClient.SqlStoreProvider")> _
        Public Property ProviderType() As String
            Get
                Return CStr(Me("providerType"))
            End Get
            Set(ByVal value As String)
                Me("providerType") = value
            End Set
        End Property

        <ConfigurationProperty("ratingLockInterval", DefaultValue:="15")> _
        Public Property RatingLockInterval() As Integer
            Get
                Return CInt(Me("ratingLockInterval"))
            End Get
            Set(ByVal value As Integer)
                Me("ratingLockInterval") = value
            End Set
        End Property

        <ConfigurationProperty("pageSize", DefaultValue:="10")> _
        Public Property PageSize() As Integer
            Get
                Return CInt(Me("pageSize"))
            End Get
            Set(ByVal value As Integer)
                Me("pageSize") = value
            End Set
        End Property

        <ConfigurationProperty("rssItems", DefaultValue:="5")> _
        Public Property RssItems() As Integer
            Get
                Return CInt(Me("rssItems"))
            End Get
            Set(ByVal value As Integer)
                Me("rssItems") = value
            End Set
        End Property

        <ConfigurationProperty("defaultOrderListInterval", DefaultValue:="7")> _
        Public Property DefaultOrderListInterval() As Integer
            Get
                Return CInt(Me("defaultOrderListInterval"))
            End Get
            Set(ByVal value As Integer)
                Me("defaultOrderListInterval") = value
            End Set
        End Property

        <ConfigurationProperty("sandboxMode", DefaultValue:="false")> _
        Public Property SandboxMode() As Boolean
            Get
                Return CBool(Me("sandboxMode"))
            End Get
            Set(ByVal value As Boolean)
                Me("sandboxMode") = value
            End Set
        End Property

        <ConfigurationProperty("businessEmail", IsRequired:=True)> _
        Public Property BusinessEmail() As String
            Get
                Return CStr(Me("businessEmail"))
            End Get
            Set(ByVal value As String)
                Me("businessEmail") = value
            End Set
        End Property

        <ConfigurationProperty("currencyCode", DefaultValue:="USD")> _
        Public Property CurrencyCode() As String
            Get
                Return CStr(Me("currencyCode"))
            End Get
            Set(ByVal value As String)
                Me("currencyCode") = value
            End Set
        End Property

        <ConfigurationProperty("lowAvailability", DefaultValue:="10")> _
        Public Property LowAvailability() As Integer
            Get
                Return CInt(Me("lowAvailability"))
            End Get
            Set(ByVal value As Integer)
                Me("lowAvailability") = value
            End Set
        End Property

        <ConfigurationProperty("enableCaching", DefaultValue:="true")> _
        Public Property EnableCaching() As Boolean
            Get
                Return CBool(Me("enableCaching"))
            End Get
            Set(ByVal value As Boolean)
                Me("enableCaching") = value
            End Set
        End Property

        <ConfigurationProperty("cacheDuration")> _
        Public Property CacheDuration() As Integer
            Get
                Dim duration As Integer = CInt(Me("cacheDuration"))
                If duration > 0 Then
                    Return duration
                Else
                    Return Globals.Settings.DefaultCacheDuration
                End If
            End Get
            Set(ByVal value As Integer)
                Me("cacheDuration") = value
            End Set
        End Property
    End Class
End Namespace
