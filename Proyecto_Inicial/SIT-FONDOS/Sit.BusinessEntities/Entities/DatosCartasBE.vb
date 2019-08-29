Public Class DatosCartasBE

    Private _CodigoTipo As String
    Public Property CodigoTipo() As String
        Get
            Return _CodigoTipo
        End Get
        Set(ByVal value As String)
            _CodigoTipo = value
        End Set
    End Property

    Private _ValorTipo As String
    Public Property ValorTipo() As String
        Get
            Return _ValorTipo
        End Get
        Set(ByVal value As String)
            _ValorTipo = value
        End Set
    End Property

    Private _CodigoTercero As String
    Public Property CodigoTercero() As String
        Get
            Return _CodigoTercero
        End Get
        Set(ByVal value As String)
            _CodigoTercero = value
        End Set
    End Property

    Private _NumeroCuenta As String
    Public Property NumeroCuenta() As String
        Get
            Return _NumeroCuenta
        End Get
        Set(ByVal value As String)
            _NumeroCuenta = value
        End Set
    End Property

End Class
