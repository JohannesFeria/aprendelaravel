Public Class DevolucionComisionBE

    Private _codigoPortafolioSBS As String
    Private _fechaOperacion As Decimal
    Private _montoComision As Decimal
    Private _identificador As Int32
    Private _codigoMoneda As String
    Private _tipoPersistencia As String


    Public Property Identificador() As Int32
        Get
            Return _identificador
        End Get
        Set(ByVal value As Int32)
            _identificador = value
        End Set
    End Property
    Public Property TipoPersistencia() As String
        Get
            Return _tipoPersistencia
        End Get
        Set(ByVal value As String)
            _tipoPersistencia = value
        End Set
    End Property
    Public Property CodigoMoneda() As String
        Get
            Return _codigoMoneda
        End Get
        Set(ByVal value As String)
            _codigoMoneda = value
        End Set
    End Property

    Public Property CodigoPortafolioSBS() As String
        Get
            Return _codigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _codigoPortafolioSBS = value
        End Set
    End Property
    Public Property FechaOperacion() As Decimal
        Get
            Return _fechaOperacion
        End Get
        Set(ByVal value As Decimal)
            _fechaOperacion = value
        End Set
    End Property

    Public Property MontoComision() As Decimal
        Get
            Return _montoComision
        End Get
        Set(ByVal value As Decimal)
            _montoComision = value
        End Set
    End Property
End Class
