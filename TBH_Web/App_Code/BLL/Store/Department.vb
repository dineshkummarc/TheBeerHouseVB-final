Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports MB.TheBeerHouse.DAL

Namespace MB.TheBeerHouse.BLL.Store
    Public Class Department
        Inherits BaseStore

        ' ==========
        ' Private variables
        ' ==========

        Private _title As String = ""
        Private _importance As Integer = 0
        Private _description As String = ""
        Private _imageUrl As String = ""
        Private _allProducts As List(Of Product)

        ' ==========
        ' Properties
        ' ==========

        Public Property Title() As String
            Get
                Return _title
            End Get
            Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property Importance() As Integer
            Get
                Return _importance
            End Get
            Set(ByVal value As Integer)
                _importance = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property ImageUrl() As String
            Get
                Return _imageUrl
            End Get
            Set(ByVal value As String)
                _imageUrl = value
            End Set
        End Property

        Public ReadOnly Property AllProducts() As List(Of Product)
            Get
                If IsNothing(_allProducts) Then _
                   _allProducts = Product.GetProducts(Me.ID, "", 0, BizObject.MAXROWS)
                Return _allProducts
            End Get
        End Property

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal addedDate As DateTime, ByVal addedBy As String, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String)
            Me.ID = id
            Me.AddedDate = addedDate
            Me.AddedBy = addedBy
            Me.Title = title
            Me.Importance = importance
            Me.Description = description
            Me.ImageUrl = imageUrl
        End Sub

        ' ==========
        ' Methods
        ' ==========

        Public Function Delete() As Boolean
            Dim success As Boolean = Department.DeleteDepartment(Me.ID)
            If success Then Me.ID = 0
            Return success
        End Function

        Public Function Update() As Boolean
            Return Department.UpdateDepartment(Me.ID, Me.Title, Me.Importance, Me.Description, Me.ImageUrl)
        End Function

        ' ==========
        ' Shared Methods
        ' ==========

        ' Returns a collection with all the departments
        Public Shared Function GetDepartments() As List(Of Department)
            Dim departments As List(Of Department)
            Dim key As String = "Store_Departments"

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                departments = CType(BizObject.Cache(key), List(Of Department))
            Else
                Dim recordset As List(Of DepartmentDetails) = SiteProvider.Store.GetDepartments()
                departments = GetDepartmentListFromDepartmentDetailsList(recordset)
                BaseStore.CacheData(key, departments)
            End If
            Return departments
        End Function

        ' Returns a Department object with the specified ID
        Public Shared Function GetDepartmentByID(ByVal departmentID As Integer) As Department
            Dim department As Department
            Dim key As String = "Store_Department_" & departmentID.ToString()

            If BaseStore.Settings.EnableCaching AndAlso Not IsNothing(BizObject.Cache(key)) Then
                department = CType(BizObject.Cache(key), Department)
            Else
                department = GetDepartmentFromDepartmentDetails(SiteProvider.Store.GetDepartmentByID(departmentID))
                BaseStore.CacheData(key, department)
            End If
            Return department
        End Function

        ' Updates an existing department
        Public Shared Function UpdateDepartment(ByVal id As Integer, ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String) As Boolean
            Dim record As New DepartmentDetails(id, DateTime.Now, "", title, importance, description, imageUrl)
            Dim ret As Boolean = SiteProvider.Store.UpdateDepartment(record)
            BizObject.PurgeCacheItems("store_department")
            Return ret
        End Function

        ' Deletes an existing department
        Public Shared Function DeleteDepartment(ByVal id As Integer) As Boolean
            Dim ret As Boolean = SiteProvider.Store.DeleteDepartment(id)
            Dim ev As New RecordDeletedEvent("department", id, Nothing)
            ev.Raise()
            BizObject.PurgeCacheItems("store_department")
            Return ret
        End Function

        ' Creates a new department
        Public Shared Function InsertDepartment(ByVal title As String, ByVal importance As Integer, ByVal description As String, ByVal imageUrl As String) As Integer
            Dim record As New DepartmentDetails(0, DateTime.Now, _
               BizObject.CurrentUserName, title, importance, description, imageUrl)
            Dim ret As Integer = SiteProvider.Store.InsertDepartment(record)
            BizObject.PurgeCacheItems("store_department")
            Return ret
        End Function

        ' Returns a Department object filled with the data taken from the input DepartmentDetails
        Private Shared Function GetDepartmentFromDepartmentDetails(ByVal record As DepartmentDetails) As Department
            If IsNothing(record) Then
                Return Nothing
            Else
                Return New Department(record.ID, record.AddedDate, record.AddedBy, _
                   record.Title, record.Importance, record.Description, record.ImageUrl)
            End If
        End Function

        ' Returns a list of Department objects filled with the data taken from the input list of DepartmentDetails
        Private Shared Function GetDepartmentListFromDepartmentDetailsList(ByVal recordset As List(Of DepartmentDetails)) As List(Of Department)
            Dim departments As New List(Of Department)
            For Each record As DepartmentDetails In recordset
                departments.Add(GetDepartmentFromDepartmentDetails(record))
            Next
            Return departments
        End Function
    End Class
End Namespace
