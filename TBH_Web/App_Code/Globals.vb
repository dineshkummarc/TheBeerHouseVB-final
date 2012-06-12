Imports System.Web.Configuration
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse
    Public NotInheritable Class Globals
        Public Shared ReadOnly Settings As TheBeerHouseSection = _
            CType(WebConfigurationManager.GetSection("theBeerHouse"), TheBeerHouseSection)

        Public Shared ThemesSelectorID As String = ""

    End Class
End Namespace
