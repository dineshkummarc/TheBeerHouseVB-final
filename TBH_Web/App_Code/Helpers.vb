Imports System.IO
Imports Microsoft.VisualBasic

Namespace MB.TheBeerHouse.UI
    Public NotInheritable Class Helpers
        Private Shared _countries As String() = { _
            "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", _
            "Angola", "Anguilla", "Antarctica", "Antigua And Barbuda", "Argentina", _
            "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", _
            "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", _
            "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", _
            "Bolivia", "Bosnia Hercegovina", "Botswana", "Bouvet Island", "Brazil", _
            "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Byelorussian SSR", _
            "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", _
            "Central African Republic", "Chad", "Chile", "China", "Christmas Island", _
            "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Cook Islands", _
            "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Cyprus", _
            "Czech Republic", "Czechoslovakia", "Denmark", "Djibouti", "Dominica", _
            "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", _
            "England", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", _
            "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", _
            "Gabon", "Gambia", "Georgia", "Germany", "Ghana", _
            "Gibraltar", "Great Britain", "Greece", "Greenland", "Grenada", _
            "Guadeloupe", "Guam", "Guatemela", "Guernsey", "Guiana", _
            "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Islands", _
            "Honduras", "Hong Kong", "Hungary", "Iceland", "India", _
            "Indonesia", "Iran", "Iraq", "Ireland", "Isle Of Man", _
            "Israel", "Italy", "Jamaica", "Japan", "Jersey", _
            "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, South", _
            "Korea, North", "Kuwait", "Kyrgyzstan", "Lao People's Dem. Rep.", "Latvia", _
            "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", _
            "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", _
            "Malawi", "Malaysia", "Maldives", "Mali", "Malta", _
            "Mariana Islands", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", _
            "Mayotte", "Mexico", "Micronesia", "Moldova", "Monaco", _
            "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", _
            "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", _
            "Neutral Zone", "New Caledonia", "New Zealand", "Nicaragua", "Niger", _
            "Nigeria", "Niue", "Norfolk Island", "Northern Ireland", "Norway", _
            "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", _
            "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", _
            "Polynesia", "Portugal", "Puerto Rico", "Qatar", "Reunion", _
            "Romania", "Russian Federation", "Rwanda", "Saint Helena", "Saint Kitts", _
            "Saint Lucia", "Saint Pierre", "Saint Vincent", "Samoa", "San Marino", _
            "Sao Tome and Principe", "Saudi Arabia", "Scotland", "Senegal", "Seychelles", _
            "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", _
            "Somalia", "South Africa", "South Georgia", "Spain", "Sri Lanka", _
            "Sudan", "Suriname", "Svalbard", "Swaziland", "Sweden", _
            "Switzerland", "Syrian Arab Republic", "Taiwan", "Tajikista", "Tanzania", _
            "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", _
            "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", _
            "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", _
            "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City State", "Venezuela", _
            "Vietnam", "Virgin Islands", "Wales", "Western Sahara", "Yemen", _
            "Yugoslavia", "Zaire", "Zambia", "Zimbabwe"}

        '
        ' Returns an array with all countries
        '
        Public Shared Function GetCountries() As StringCollection
            Dim countries As New StringCollection
            countries.AddRange(_countries)
            Return countries
        End Function

        Public Shared Function GetCountries(ByVal insertEmpty As Boolean) As SortedList
            Dim countries As New SortedList
            If (insertEmpty) Then
                countries.Add("", "Please select one...")
            End If
            For Each country As String In _countries
                countries.Add(country, country)
            Next
            Return countries
        End Function

        ' 
        ' Returns an array with the names of all local Themes
        '
        Public Shared Function GetThemes() As String()
            If Not IsNothing(HttpContext.Current.Cache("SiteThemes")) Then
                Return DirectCast(HttpContext.Current.Cache("SiteThemes"), String())
            Else
                Dim themesDirPath As String = HttpContext.Current.Server.MapPath("~/App_Themes")
                ' get the array of themes folders under /App_Themes
                Dim themes As String() = Directory.GetDirectories(themesDirPath)
                For i As Integer = 0 To themes.Length - 1
                    themes(i) = Path.GetFileName(themes(i))
                Next
                Dim dep As New CacheDependency(themesDirPath)
                HttpContext.Current.Cache.Insert("SiteThemes", themes, dep)
                Return themes
            End If

        End Function

        ' Adds the onfocus and onblur attributes to all input controls found in the specified parent,
        ' to change their apperance with the control has the focus
        Public Shared Sub SetInputControlsHighlight(ByVal container As Control, ByVal className As String, ByVal onlyTextBoxes As Boolean)
            For Each ctl As Control In container.Controls
                If (onlyTextBoxes AndAlso TypeOf (ctl) Is TextBox) OrElse TypeOf (ctl) Is TextBox OrElse TypeOf (ctl) Is DropDownList OrElse _
                 TypeOf (ctl) Is ListBox OrElse TypeOf (ctl) Is CheckBox OrElse TypeOf (ctl) Is RadioButton OrElse _
                 TypeOf (ctl) Is RadioButtonList OrElse TypeOf (ctl) Is CheckBoxList Then
                    Dim wctl As WebControl
                    wctl = CType(ctl, WebControl)
                    wctl.Attributes.Add("onfocus", String.Format("this.className = '{0}';", className))
                    wctl.Attributes.Add("onblur", "this.className = '';")
                Else
                    If (ctl.Controls.Count > 0) Then _
                        SetInputControlsHighlight(ctl, className, onlyTextBoxes)
                End If
            Next
        End Sub

        ' Converts the input plain-text to HTML version, replacing carriage returns
        ' and spaces with <br /> and &nbsp;
        Public Shared Function ConvertToHtml(ByVal content As String) As String
            content = HttpUtility.HtmlEncode(content)
            content = content.Replace("  ", "&nbsp;&nbsp;").Replace( _
               "\t", "&nbsp;&nbsp;&nbsp;").Replace("\n", "<br>")
            Return content
        End Function
    End Class
End Namespace
