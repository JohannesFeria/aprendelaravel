Public Class PrevisionParametria

    Private iIdTabla As Integer
    Public Property IdTabla() As Integer
        Get
            Return iIdTabla
        End Get
        Set(ByVal value As Integer)
            iIdTabla = value
        End Set
    End Property

    Private iIdRegistro As String
    Public Property IdRegistro() As String
        Get
            Return iIdRegistro
        End Get
        Set(ByVal value As String)
            iIdRegistro = value
        End Set
    End Property

    Private sDescripcion As String
    Public Property Descripcion() As String
        Get
            Return sDescripcion
        End Get
        Set(ByVal value As String)
            sDescripcion = value
        End Set
    End Property

    Private sValor As String
    Public Property Valor() As String
        Get
            Return sValor
        End Get
        Set(ByVal value As String)
            sValor = value
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
