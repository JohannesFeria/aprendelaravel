Public Class PrevisionCuentasCorrientes

    Private iCodigo As Int32
    Public Property Codigo() As Int32
        Get
            Return iCodigo
        End Get
        Set(ByVal value As Int32)
            iCodigo = value
        End Set
    End Property

    Private sIdEntidad As String
    Public Property IdEntidad() As String
        Get
            Return sIdEntidad
        End Get
        Set(ByVal value As String)
            sIdEntidad = value
        End Set
    End Property

    Private sIdBanco As String
    Public Property IdBanco() As String
        Get
            Return sIdBanco
        End Get
        Set(ByVal value As String)
            sIdBanco = value
        End Set
    End Property

    Private sIdCuentaCorriente As String
    Public Property IdCuentaCorriente() As String
        Get
            Return sIdCuentaCorriente
        End Get
        Set(ByVal value As String)
            sIdCuentaCorriente = value
        End Set
    End Property

    Private sIdTipoCuenta As String
    Public Property IdTipoCuenta() As String
        Get
            Return sIdTipoCuenta
        End Get
        Set(ByVal value As String)
            sIdTipoCuenta = value
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

    Private sIdMoneda As String
    Public Property IdMoneda() As String
        Get
            Return sIdMoneda
        End Get
        Set(ByVal value As String)
            sIdMoneda = value
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

    Private sCodUsuario As String
    Public Property CodUsuario() As String
        Get
            Return sCodUsuario
        End Get
        Set(ByVal value As String)
            sCodUsuario = value
        End Set
    End Property

    Private iFechaRegistro As Integer
    Public Property FechaRegistro() As Integer
        Get
            Return iFechaRegistro
        End Get
        Set(ByVal value As Integer)
            iFechaRegistro = value
        End Set
    End Property

End Class
