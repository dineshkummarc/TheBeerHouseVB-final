Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.DAL
    Public NotInheritable Class SiteProvider

        Public Shared ReadOnly Property Articles() As ArticlesProvider
            Get
                Return ArticlesProvider.Instance
            End Get
        End Property

        Public Shared ReadOnly Property Polls() As PollsProvider
            Get
                Return PollsProvider.Instance
            End Get
        End Property

        Public Shared ReadOnly Property Newsletters() As NewslettersProvider
            Get
                Return NewslettersProvider.Instance
            End Get
        End Property

        Public Shared ReadOnly Property Forums() As ForumsProvider
            Get
                Return ForumsProvider.Instance
            End Get
        End Property

        Public Shared ReadOnly Property Store() As StoreProvider
            Get
                Return StoreProvider.Instance
            End Get
        End Property
    End Class
End Namespace
