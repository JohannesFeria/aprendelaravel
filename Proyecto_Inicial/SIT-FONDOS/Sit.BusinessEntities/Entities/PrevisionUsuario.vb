Public Class PrevisionUsuario

    Private sCodUsuario As String
    Public Property CodUsuario() As String
        Get
            Return sCodUsuario
        End Get
        Set(ByVal value As String)
            sCodUsuario = value
        End Set
    End Property

    Private sNombreUsuario As String
    Public Property NombreUsuario() As String
        Get
            Return sNombreUsuario
        End Get
        Set(ByVal value As String)
            sNombreUsuario = value
        End Set
    End Property

    Private sArea As String
    Public Property Area() As String
        Get
            Return sArea
        End Get
        Set(ByVal value As String)
            sArea = value
        End Set
    End Property

    Private sSituacion As String
    Public Property Situacion() As String
        Get
            Return sSituacion
        End Get
        Set(ByVal value As String)
            sSituacion = value
        End Set
    End Property

    Private sUsuarioCreacion As String
    Public Property UsuarioCreacion() As String
        Get
            Return sUsuarioCreacion
        End Get
        Set(ByVal value As String)
            sUsuarioCreacion = value
        End Set
    End Property

    Private iFechaCreacion As Integer
    Public Property FechaCreacion() As String
        Get
            Return iFechaCreacion
        End Get
        Set(ByVal value As String)
            iFechaCreacion = value
        End Set
    End Property

End Class
