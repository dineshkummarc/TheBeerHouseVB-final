Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Polls
    Public MustInherit Class BasePoll
        Inherits BizObject

        ' ==========
        ' Private Variables
        ' ==========
        Private _id As Integer = 0
        Private _addedDate As DateTime = DateTime.Now
        Private _addedBy As String = ""

        ' ==========
        ' Properties
        ' ==========
        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Protected Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property AddedDate() As DateTime
            Get
                Return _addedDate
            End Get
            Protected Set(ByVal value As DateTime)
                _addedDate = value
            End Set
        End Property

        Public Property AddedBy() As String
            Get
                Return _addedBy
            End Get
            Protected Set(ByVal value As String)
                _addedBy = value
            End Set
        End Property

        Public Shared ReadOnly Property Settings() As PollsElement
            Get
                Return Globals.Settings.Polls
            End Get
        End Property

        ' ==========
        ' Methods
        ' ==========

        ' Cache the input data, if caching is enabled
        Public Shared Sub CacheData(ByVal key As String, ByVal data As Object)
            BizObject.Cache.Insert( _
                key, data, Nothing, DateTime.Now.AddSeconds(Settings.CacheDuration), TimeSpan.Zero)
        End Sub
    End Class
End Namespace
