Public Class PrevisionDetalleUsuario

    Private sCodUsuario As String
    Public Property CodUsuario() As String
        Get
            Return sCodUsuario
        End Get
        Set(ByVal value As String)
            sCodUsuario = value
        End Set
    End Property

    Private sIdTipoOperacion As String
    Public Property IdTipoOperacion() As String
        Get
            Return sIdTipoOperacion
        End Get
        Set(ByVal value As String)
            sIdTipoOperacion = value
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

    Private iFechaCreacion As String
    Public Property FechaCreacion() As String
        Get
            Return iFechaCreacion
        End Get
        Set(ByVal value As String)
            iFechaCreacion = value
        End Set
    End Property

End Class
