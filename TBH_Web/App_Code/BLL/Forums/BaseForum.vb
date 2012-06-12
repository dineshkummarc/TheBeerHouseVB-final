Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Forums
    Public MustInherit Class BaseForum
        Inherits BizObject

        ' ==========
        ' Private variables
        ' ==========

        Dim _id As Integer = 0
        Dim _addedDate As DateTime = DateTime.Now
        Dim _addedBy As String = ""

        ' ==========
        ' Properties
        ' ==========

        Public Property ID() As Integer
            Get
                Return (_id)
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

        Protected Shared ReadOnly Property Settings() As ForumsElement
            Get
                Return Globals.Settings.Forums
            End Get
        End Property

        ' ==========
        ' Shared methods
        ' ==========

        'Cache the input data, if caching is enabled
        Protected Shared Sub CacheData(ByVal key As String, ByVal data As Object)
            If Settings.EnableCaching AndAlso IsNothing(data) Then
                BizObject.Cache.Insert(key, data, Nothing, _
                   DateTime.Now.AddSeconds(Settings.CacheDuration), TimeSpan.Zero)
            End If
        End Sub
    End Class
End Namespace
