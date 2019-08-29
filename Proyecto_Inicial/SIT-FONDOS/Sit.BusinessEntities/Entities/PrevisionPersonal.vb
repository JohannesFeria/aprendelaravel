Public Class PrevisionPersonal

    Private sCodigoInterno As String
    Public Property CodigoInterno() As String
        Get
            Return sCodigoInterno
        End Get
        Set(ByVal value As String)
            sCodigoInterno = value
        End Set
    End Property

    Private sCodigoUsuario As String
    Public Property CodigoUsuario() As String
        Get
            Return sCodigoUsuario
        End Get
        Set(ByVal value As String)
            sCodigoUsuario = value
        End Set
    End Property

    Private sNumeroDocumento As String
    Public Property NumeroDocumento() As String
        Get
            Return sNumeroDocumento
        End Get
        Set(ByVal value As String)
            sNumeroDocumento = value
        End Set
    End Property

    Private sNombreCompleto As String
    Public Property NombreCompleto() As String
        Get
            Return sNombreCompleto
        End Get
        Set(ByVal value As String)
            sNombreCompleto = value
        End Set
    End Property

End Class
