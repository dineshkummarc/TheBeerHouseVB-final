Imports System.Data
Imports System.Data.Common
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public MustInherit Class DataAccess

        '===================
        ' Private variables
        '===================
        '   In the C# code, Marco interspersed the private variables among the
        '   properties. I think that separating private variables is neater.
        '
        Private _connectionString As String = ""
        Private _enableCaching As Boolean = True
        Private _cacheDuration As Integer = 0

        '===================
        ' Class Properties
        '===================
        Protected Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
            Set(ByVal value As String)
                _connectionString = value
            End Set
        End Property

        Protected Property EnableCaching() As Boolean
            Get
                Return _enableCaching
            End Get
            Set(ByVal value As Boolean)
                _enableCaching = value
            End Set
        End Property

        Protected Property CacheDuration() As Integer
            Get
                Return _cacheDuration
            End Get
            Set(ByVal value As Integer)
                _cacheDuration = value
            End Set
        End Property

        Protected ReadOnly Property Cache() As Cache
            Get
                Return HttpContext.Current.Cache
            End Get
        End Property

        '===============
        ' Methods
        '===============

        Protected Function ExecuteNonQuery(ByVal cmd As DbCommand) As Integer
            If HttpContext.Current.User.Identity.Name.ToLower() = "sampleeditor" Then
                For Each param As DbParameter In cmd.Parameters
                    If param.Direction = Data.ParameterDirection.Output Or _
                        param.Direction = Data.ParameterDirection.ReturnValue Then
                        Select Case param.DbType
                            Case Data.DbType.AnsiString
                            Case Data.DbType.AnsiStringFixedLength
                            Case Data.DbType.String
                            Case Data.DbType.StringFixedLength
                            Case Data.DbType.Xml
                                param.Value = ""

                            Case Data.DbType.Boolean
                                param.Value = False

                            Case Data.DbType.Byte
                                param.Value = Byte.MinValue

                            Case Data.DbType.Date
                            Case Data.DbType.DateTime
                                param.Value = DateTime.MinValue

                            Case Data.DbType.Currency
                            Case Data.DbType.Decimal
                                param.Value = Decimal.MinValue

                            Case Data.DbType.Guid
                                param.Value = Guid.Empty

                            Case Data.DbType.Double
                            Case Data.DbType.Int16
                            Case Data.DbType.Int32
                            Case Data.DbType.Int64
                                param.Value = 0

                            Case Else
                                param.Value = Nothing
                        End Select
                    End If
                Next
                Return 1
            Else
                Return cmd.ExecuteNonQuery()
            End If
        End Function

        Protected Function ExecuteReader(ByVal cmd As DbCommand) As IDataReader
            Return ExecuteReader(cmd, CommandBehavior.Default)
        End Function

        Protected Function ExecuteReader(ByVal cmd As DbCommand, ByVal behavior As CommandBehavior) As IDataReader
            Return cmd.ExecuteReader(behavior)
        End Function

        Protected Function ExecuteScalar(ByVal cmd As DbCommand) As Object
            Return cmd.ExecuteScalar()
        End Function
    End Class
End Namespace
