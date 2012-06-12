Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Namespace MB.TheBeerHouse.BLL.Store

    <Serializable()> _
    Public Class ShoppingCartItem

        ' ==========
        ' Private Variables
        ' ==========

        Private _id As Integer = 0
        Private _title As String = ""
        Private _sku As String = ""
        Private _unitPrice As Decimal
        Private _quantity As Integer = 1

        ' ==========
        ' Properties
        ' ==========

        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Private Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return _title
            End Get
            Private Set(ByVal value As String)
                _title = value
            End Set
        End Property

        Public Property SKU() As String
            Get
                Return _sku
            End Get
            Private Set(ByVal value As String)
                _sku = value
            End Set
        End Property

        Public Property UnitPrice() As Decimal
            Get
                Return _unitPrice
            End Get
            Private Set(ByVal value As Decimal)
                _unitPrice = value
            End Set
        End Property

        Public Property Quantity() As Integer
            Get
                Return _quantity
            End Get
            Set(ByVal value As Integer)
                _quantity = value
            End Set
        End Property

        ' ==========
        ' Constructor
        ' ==========

        Public Sub New(ByVal id As Integer, ByVal title As String, ByVal sku As String, ByVal unitPrice As Decimal)
            Me.ID = id
            Me.Title = title
            Me.SKU = sku
            Me.UnitPrice = unitPrice
        End Sub
    End Class

    <Serializable()> _
    Public Class ShoppingCart
        Private _items As New Dictionary(Of Integer, ShoppingCartItem)()

        Public ReadOnly Property Items() As ICollection
            Get
                Return _items.Values
            End Get
        End Property

        ' Gets the sum total of the items' prices
        Public ReadOnly Property Total() As Decimal
            Get
                Dim sum As Decimal = 0.0
                For Each item As ShoppingCartItem In _items.Values
                    sum += item.UnitPrice * item.Quantity
                Next
                Return sum
            End Get
        End Property

        ' Adds a new item to the shopping cart
        Public Sub InsertItem(ByVal id As Integer, ByVal title As String, ByVal sku As String, ByVal unitPrice As Decimal)
            If _items.ContainsKey(id) Then
                _items(id).Quantity += 1
            Else
                _items.Add(id, New ShoppingCartItem(id, title, sku, unitPrice))
            End If
        End Sub

        ' Removes an item from the shopping cart
        Public Sub DeleteItem(ByVal id As Integer)
            If _items.ContainsKey(id) Then
                Dim item As ShoppingCartItem = _items(id)
                item.Quantity -= 1
                If item.Quantity = 0 Then _
                   _items.Remove(id)
            End If
        End Sub

        ' Removes all items of a specified product from the shopping cart
        Public Sub DeleteProduct(ByVal id As Integer)
            If _items.ContainsKey(id) Then
                _items.Remove(id)
            End If
        End Sub

        ' Updates the quantity for an item
        Public Sub UpdateItemQuantity(ByVal id As Integer, ByVal quantity As Integer)
            If _items.ContainsKey(id) Then
                Dim item As ShoppingCartItem = _items(id)
                item.Quantity = quantity
                If item.Quantity <= 0 Then _
               _items.Remove(id)
            End If
        End Sub

        ' Clears the cart
        Public Sub Clear()
            _items.Clear()
        End Sub
    End Class

    Public Class CurrentUserShoppingCart

        Public Shared Function GetItems() As ICollection
            Return CType(HttpContext.Current.Profile, ProfileCommon).ShoppingCart.Items
        End Function

        Public Shared Sub DeleteItem(ByVal id As Integer)
            CType(HttpContext.Current.Profile, ProfileCommon).ShoppingCart.DeleteItem(id)
        End Sub

        Public Shared Sub DeleteProduct(ByVal id As Integer)
            CType(HttpContext.Current.Profile, ProfileCommon).ShoppingCart.DeleteProduct(id)
        End Sub
    End Class

End Namespace
