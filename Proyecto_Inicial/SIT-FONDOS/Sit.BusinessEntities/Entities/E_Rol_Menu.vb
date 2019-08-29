Public Class E_Rol_Menu

    Private sCODIGO_ROL As Integer
    Public Property CODIGO_ROL() As Integer
        Get
            Return sCODIGO_ROL
        End Get
        Set(ByVal value As Integer)
            sCODIGO_ROL = value
        End Set
    End Property

    Private sCODIGO_MENU As Integer
    Public Property CODIGO_MENU() As Integer
        Get
            Return sCODIGO_MENU
        End Get
        Set(ByVal value As Integer)
            sCODIGO_MENU = value
        End Set
    End Property


End Class
