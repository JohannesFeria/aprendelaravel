Public Class PrevisionCierre

    Private sHoraCierre As String
    Public Property HoraCierre() As String
        Get
            Return sHoraCierre
        End Get
        Set(ByVal value As String)
            sHoraCierre = value
        End Set
    End Property

    Private sTipoCierre As String
    Public Property TipoCierre() As String
        Get
            Return sTipoCierre
        End Get
        Set(ByVal value As String)
            sTipoCierre = value
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

    Private dFechaCreacion As Decimal
    Public Property FechaCreacion() As Decimal
        Get
            Return dFechaCreacion
        End Get
        Set(ByVal value As Decimal)
            dFechaCreacion = value
        End Set
    End Property

    Private sUsuarioModificacion As String
    Public Property UsuarioModificacion() As String
        Get
            Return sUsuarioModificacion
        End Get
        Set(ByVal value As String)
            sUsuarioModificacion = value
        End Set
    End Property

    Private dFechaModificacion As Decimal
    Public Property FechaModificacion() As Decimal
        Get
            Return dFechaModificacion
        End Get
        Set(ByVal value As Decimal)
            dFechaModificacion = value
        End Set
    End Property

End Class
