Public Class PrevisionMemo

    Private sTipoReporte As String
    Public Property TipoReporte() As String
        Get
            Return sTipoReporte
        End Get
        Set(ByVal value As String)
            sTipoReporte = value
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

    Private sDescripcionA As String
    Public Property DescripcionA() As String
        Get
            Return sDescripcionA
        End Get
        Set(ByVal value As String)
            sDescripcionA = value
        End Set
    End Property

    Private sDescripcionDe As String
    Public Property DescripcionDe() As String
        Get
            Return sDescripcionDe
        End Get
        Set(ByVal value As String)
            sDescripcionDe = value
        End Set
    End Property

    Private sReferencia As String
    Public Property Referencia() As String
        Get
            Return sReferencia
        End Get
        Set(ByVal value As String)
            sReferencia = value
        End Set
    End Property

    Private sContenido As String
    Public Property Contenido() As String
        Get
            Return sContenido
        End Get
        Set(ByVal value As String)
            sContenido = value
        End Set
    End Property

    Private sDespedida As String
    Public Property Despedida() As String
        Get
            Return sDespedida
        End Get
        Set(ByVal value As String)
            sDespedida = value
        End Set
    End Property

    Private sUsuarioFirma As String
    Public Property UsuarioFirma() As String
        Get
            Return sUsuarioFirma
        End Get
        Set(ByVal value As String)
            sUsuarioFirma = value
        End Set
    End Property

    Private sAreaUsuarioFirma As String
    Public Property AreaUsuarioFirma() As String
        Get
            Return sAreaUsuarioFirma
        End Get
        Set(ByVal value As String)
            sAreaUsuarioFirma = value
        End Set
    End Property

    Private sInicialesDocumentador As String
    Public Property InicialesDocumentador() As String
        Get
            Return sInicialesDocumentador
        End Get
        Set(ByVal value As String)
            sInicialesDocumentador = value
        End Set
    End Property

End Class
