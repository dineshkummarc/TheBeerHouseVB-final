Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Threading
Imports System.Web.Profile
Imports MB.TheBeerHouse.DAL
Imports System.Text.RegularExpressions
Imports System.Net.Mail

Namespace MB.TheBeerHouse.BLL.Newsletters
    Public Structure SubscriberInfo
        Public UserName As String
        Public Email As String
        Public FirstName As String
        Public LastName As String
        Public SubscriptionType As SubscriptionType

        Public Sub New(ByVal userName As String, ByVal email As String, ByVal firstName As String, _
                ByVal lastName As String, ByVal subscriptionType As SubscriptionType)

            Me.UserName = userName
            Me.Email = email
            Me.FirstName = firstName
            Me.LastName = lastName
            Me.SubscriptionType = subscriptionType
        End Sub

    End Structure

    Public Class Newsletter
        Inherits BizObject

        ' ==========
        ' Private Variables
        ' ==========

        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""
        Private _subject As String = ""
        Private _plainTextBody As String ' For those that don't know:
        ' this is the equivalent of C# setting for null. VB variables are
        ' not assigned until initialized and are therefore null. Setting
        ' the VB variable to "nothing" initializes the variable type and sets
        ' it to a default value, ie. an empty string
        Private _htmlBody As String
        Public Shared Lock As New ReaderWriterLock
        Private Shared _isSending As Boolean = False
        Private Shared _percentageCompleted As Double = 0.0
        Private Shared _totalMails As Integer = -1
        Private Shared _sentMails As Integer = 0


        ' ==========
        ' Properties
        ' ==========
        Public Shared ReadOnly Property Settings() As NewslettersElement
            Get
                Return Globals.Settings.Newsletters
            End Get
        End Property

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property AddedDate() As DateTime
            Get
                Return _addedDate
            End Get
            Set(ByVal value As DateTime)
                _addedDate = value
            End Set
        End Property

        Public Property AddedBy() As String
            Get
                Return _addedBy
            End Get
            Set(ByVal value As String)
                _addedBy = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return _subject
            End Get
            Set(ByVal value As String)
                _subject = value
            End Set
        End Property

        Public Property PlainTextBody() As String
            Get
                If IsNothing(_plainTextBody) Then
                    FillBody()
                End If
                Return _plainTextBody
            End Get
            Set(ByVal value As String)
                _plainTextBody = value
            End Set
        End Property

        Public Property HtmlBody() As String
            Get
                If IsNothing(_htmlBody) Then
                    FillBody()
                End If
                Return _htmlBody
            End Get
            Set(ByVal value As String)
                _htmlBody = value
            End Set
        End Property

        Public Shared Property IsSending() As Boolean
            Get
                Return _isSending
            End Get
            Set(ByVal value As Boolean)
                _isSending = value
            End Set
        End Property

        Public Shared Property PercentageCompleted() As Double
            Get
                Return _percentageCompleted
            End Get
            Set(ByVal value As Double)
                _percentageCompleted = value
            End Set
        End Property

        Public Shared Property TotalMails() As Integer
            Get
                Return _totalMails
            End Get
            Set(ByVal value As Integer)
                _totalMails = value
            End Set
        End Property

        Public Shared Property SentMails() As Integer
            Get
                Return _sentMails
            End Get
            Set(ByVal value As Integer)
                _sentMails = value
            End Set
        End Property


        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, _
                ByVal subject As String, ByVal plainTextBody As String, ByVal htmlBody As String)

            Me.ID = id
            Me.AddedBy = addedBy
            Me.AddedDate = addedDate
            Me.Subject = subject
            Me.PlainTextBody = plainTextBody
            Me.HtmlBody = htmlBody
        End Sub


        ' ==========
        ' Methods
        ' ==========

        Public Function Delete() As Boolean
            Dim success As Boolean = Newsletter.DeleteNewsletter(Me.ID)
            If success Then
                Me.ID = 0
            End If
            Return success
        End Function

        Public Function Update() As Boolean
            Return Newsletter.UpdateNewsletter(Me.ID, Me.Subject, Me.PlainTextBody, Me.HtmlBody)
        End Function

        Private Sub FillBody()
            Dim record As NewsletterDetails = SiteProvider.Newsletters.GetNewsletterByID(Me.ID)
            Me.PlainTextBody = record.PlainTextBody
            Me.HtmlBody = record.HtmlBody
        End Sub


        ' ==========
        ' Shared Methods
        ' ==========

        ' Returns a collection with all newsletters sent before the specified date
        Public Shared Function GetNewsletters() As List(Of Newsletter)
            Return GetNewsletters(DateTime.Now)
        End Function

        Public Shared Function GetNewsletters(ByVal toDate As DateTime) As List(Of Newsletter)
            Dim newsletters As List(Of Newsletter)
            Dim key As String = "Newsletters_Newsletters_" + toDate.ToShortDateString()

            If Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                newsletters = CType(BizObject.Cache(key), List(Of Newsletter))
            Else
                Dim recordset As List(Of NewsletterDetails) = SiteProvider.Newsletters.GetNewsLetters(toDate)
                newsletters = GetNewsletterListFromNewsletterDetailsList(recordset)
                CacheData(key, newsletters)
            End If
            Return newsletters
        End Function

        ' Returns a Newsletter object with the specified ID
        Public Shared Function GetNewslettersByID(ByVal newsletterID As Integer) As Newsletter
            Dim newsletter As Newsletter
            Dim key As String = "Newsletters_Newsletter_" + newsletterID.ToString()

            If Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                newsletter = CType(BizObject.Cache(key), Newsletter)
            Else
                newsletter = GetNewsletterFromNewsletterDetails( _
                    SiteProvider.Newsletters.GetNewsletterByID(newsletterID))
                CacheData(key, newsletter)
            End If
            Return newsletter
        End Function

        ' Updates an existing newsletter
        Public Shared Function UpdateNewsletter(ByVal id As Integer, ByVal subject As String, _
                ByVal plainTextBody As String, ByVal htmlBody As String) As Boolean

            Dim record As New NewsletterDetails(id, DateTime.Now, "", subject, plainTextBody, htmlBody)
            Dim ret As Boolean = SiteProvider.Newsletters.UpdateNewsletter(record)
            BizObject.PurgeCacheItems("newsletters_newsletter")
            Return ret
        End Function

        ' Deletes an existing newsletter
        Public Shared Function DeleteNewsletter(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Newsletters.DeleteNewsletter(id)
            Dim ev As New RecordDeletedEvent("newsletter", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("newsletters_newsletter")
            Return ret
        End Function

        ' Sends a newsletter
        Public Shared Function SendNewsletter(ByVal subject As String, ByVal plainTextBody As String, ByVal htmlBody As String) As Integer
            Lock.AcquireWriterLock(Timeout.Infinite)
            Newsletter.TotalMails = -1
            Newsletter.SentMails = 0
            Newsletter.PercentageCompleted = 0.0
            Newsletter.IsSending = True
            Lock.ReleaseWriterLock()

            ' if the HTML body is an empty string, use the plain-text body
            ' converted to HTML
            If htmlBody.Trim.Length = 0 Then
                htmlBody = HttpUtility.HtmlEncode(plainTextBody).Replace( _
                    " ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;").Replace("\n", "<br />")
            End If

            ' create the record into the DB
            Dim record As New NewsletterDetails(0, DateTime.Now, BizObject.CurrentUserName, _
                subject, plainTextBody, htmlBody)
            Dim ret As Integer = SiteProvider.Newsletters.InsertNewsletter(record)
            BizObject.PurgeCacheItems("newsletters_newsletters_" & DateTime.Now.ToShortDateString)

            ' send the newsletters asyncronously
            Dim parameters() As Object = {subject, plainTextBody, htmlBody, HttpContext.Current}
            Dim pts As New ParameterizedThreadStart(AddressOf SendEmails)
            Dim thread As New Thread(pts)
            thread.Name = "SendEmails"
            thread.Priority = ThreadPriority.BelowNormal
            thread.Start(parameters)

            Return ret
        End Function

        ' Sends the newsletter e-mails to all subscribers
        Public Shared Sub SendEmails(ByVal data As Object)
            Dim parameters() As Object = CType(data, Object())
            Dim subject As String = CStr(parameters(0))
            Dim plainTextBody As String = CStr(parameters(1))
            Dim htmlBody As String = CStr(parameters(2))
            Dim context As HttpContext = CType(parameters(3), HttpContext)

            Lock.AcquireWriterLock(Timeout.Infinite)
            Newsletter.TotalMails = 0
            Lock.ReleaseWriterLock()

            ' check if the plain-text and HTML bodies have personalization placeholders
            ' the will need to be replaced on a per-mail basis. If not, the parsing will
            ' be completely avoided later.
            Dim plainTextIsPersonalized As Boolean = HasPersonalizationPlaceholders(plainTextBody, False)
            Dim htmlIsPersonalized As Boolean = HasPersonalizationPlaceholders(htmlBody, True)

            ' retreive all subscribers to the plain-text and HTML newsletter
            Dim subscribers As New List(Of SubscriberInfo)
            Dim profile As ProfileCommon = CType(context.Profile, ProfileCommon)

            For Each user As MembershipUser In Membership.GetAllUsers
                Dim userProfile As ProfileCommon = profile.GetProfile(user.UserName)
                If Not userProfile.Preferences.Newsletter = SubscriptionType.None Then
                    Dim subscriber As New SubscriberInfo(user.UserName, user.Email, _
                        userProfile.FirstName, userProfile.LastName, userProfile.Preferences.Newsletter)
                    subscribers.Add(subscriber)
                    Lock.AcquireWriterLock(Timeout.Infinite)
                    Newsletter.TotalMails += 1
                    Lock.ReleaseWriterLock()
                End If
            Next

            ' send the newsletter
            Dim smtpClient As New SmtpClient
            For Each subscriber As SubscriberInfo In subscribers
                Dim mail As New MailMessage
                mail.From = New MailAddress(Settings.FromEmail, Settings.FromDisplayName)
                mail.To.Add(subscriber.Email)
                mail.Subject = subject
                If subscriber.SubscriptionType = SubscriptionType.PlainText Then
                    Dim body As String = plainTextBody
                    If plainTextIsPersonalized Then
                        body = ReplacePersonalizationPlaceholders(body, subscriber, False)
                    End If
                    mail.Body = body
                    mail.IsBodyHtml = False
                Else
                    Dim body As String = htmlBody
                    If htmlIsPersonalized Then
                        body = ReplacePersonalizationPlaceholders(body, subscriber, True)
                    End If
                    mail.Body = body
                    mail.IsBodyHtml = True
                End If
                Try
                    smtpClient.Send(mail)
                Catch

                End Try

                Lock.AcquireWriterLock(Timeout.Infinite)
                Newsletter.SentMails += 1
                Newsletter.PercentageCompleted = CDbl(Newsletter.SentMails) * 100 / CDbl(Newsletter.TotalMails)
                Lock.ReleaseWriterLock()
            Next

            Lock.AcquireWriterLock(Timeout.Infinite)
            Newsletter.IsSending = False
            Lock.ReleaseWriterLock()
        End Sub

        ' Returns whether the input text contains personalization placeholders
        Private Shared Function HasPersonalizationPlaceholders(ByVal text As String, ByVal isHtml As Boolean) As Boolean
            If isHtml Then
                If Regex.IsMatch(text, "&lt;%\s*username\s*%&gt;", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
                If Regex.IsMatch(text, "&lt;%\s*email\s*%&gt;", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
                If Regex.IsMatch(text, "&lt;%\s*firstname\s*%&gt;", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
                If Regex.IsMatch(text, "&lt;%\s*lastname\s*%&gt;", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
            Else
                If Regex.IsMatch(text, "<%\s*username\s*%>", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
                If Regex.IsMatch(text, "<%\s*email\s*%>", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
                If Regex.IsMatch(text, "<%\s*firstname\s*%>", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
                If Regex.IsMatch(text, "<%\s*lastname\s*%>", RegexOptions.IgnoreCase Or _
                    RegexOptions.Compiled) Then
                    Return True
                End If
            End If
            Return False
        End Function

        ' Replaces the input text's personalization placeholders
        Private Shared Function ReplacePersonalizationPlaceholders(ByVal text As String, _
                ByVal subscriber As SubscriberInfo, ByVal isHtml As Boolean) As String

            If isHtml Then
                text = Regex.Replace(text, "&lt;%\s*username\s*%&gt;", _
                    subscriber.UserName, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                text = Regex.Replace(text, "&lt;%\s*email\s*%&gt;", _
                    subscriber.Email, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                Dim firstName As String = "reader"
                If subscriber.FirstName.Length > 0 Then
                    firstName = subscriber.FirstName
                End If
                text = Regex.Replace(text, "&lt;%\s*firstname\s*%&gt;", firstName, _
                   RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                text = Regex.Replace(text, "&lt;%\s*lastname\s*%&gt;", _
                   subscriber.LastName, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            Else
                text = Regex.Replace(text, "<%\s*username\s*%>", _
                   subscriber.UserName, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                text = Regex.Replace(text, "<%\s*email\s*%>", _
                   subscriber.Email, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                Dim firstName As String = "reader"
                If subscriber.FirstName.Length > 0 Then
                    firstName = subscriber.FirstName
                End If
                text = Regex.Replace(text, "<%\s*firstname\s*%>", firstName, _
                   RegexOptions.IgnoreCase Or RegexOptions.Compiled)
                text = Regex.Replace(text, "<%\s*lastname\s*%>", _
                   subscriber.LastName, RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            End If
            Return text
        End Function

        ' Returns a Newsletter object filled with the data taken from the input NewsletterDetails
        Private Shared Function GetNewsletterFromNewsletterDetails(ByVal record As NewsletterDetails) _
                As Newsletter

            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Newsletter(record.ID, record.AddedDate, record.AddedBy, record.Subject, _
                    record.PlainTextBody, record.HtmlBody)
            End If
        End Function

        ' Returns a list of Newsletter objects filled with the data taken from the input list of NewsletterDetails
        Private Shared Function GetNewsletterListFromNewsletterDetailsList( _
                ByVal recordset As List(Of NewsletterDetails)) _
                As List(Of Newsletter)

            Dim newsletters As New List(Of Newsletter)
            For Each record As NewsletterDetails In recordset
                newsletters.Add(GetNewsletterFromNewsletterDetails(record))
            Next
            Return newsletters
        End Function

        ' Cache the input data, if caching is enabled
        Private Shared Sub CacheData(ByVal key As String, ByVal data As Object)
            If Settings.EnableCaching AndAlso Not IsNothing(data) Then
                BizObject.Cache.Insert(key, data, Nothing, DateTime.Now.AddSeconds(Settings.CacheDuration), _
                    TimeSpan.Zero)
            End If
        End Sub
    End Class
End Namespace
