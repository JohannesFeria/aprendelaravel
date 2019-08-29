Public Class PrevisionPagoDetalle

    Private sCodigoPago As String
    Public Property CodigoPago() As String
        Get
            Return sCodigoPago
        End Get
        Set(ByVal value As String)
            sCodigoPago = value
        End Set
    End Property

    Private sTipoMovimiento As String
    Public Property TipoMovimiento() As String
        Get
            Return sTipoMovimiento
        End Get
        Set(ByVal value As String)
            sTipoMovimiento = value
        End Set
    End Property

    Private sIdTipoMoneda As String
    Public Property IdTipoMoneda() As String
        Get
            Return sIdTipoMoneda
        End Get
        Set(ByVal value As String)
            sIdTipoMoneda = value
        End Set
    End Property

    Private dImporte As Double
    Public Property Importe() As Double
        Get
            Return dImporte
        End Get
        Set(ByVal value As Double)
            dImporte = value
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

End Class
