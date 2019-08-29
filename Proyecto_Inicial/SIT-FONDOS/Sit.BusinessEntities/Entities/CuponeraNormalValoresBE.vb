Public Class CuponeraNormalValoresBE
    Private _fechaEmision As Decimal
    Private _fechaVencimiento As Decimal
    Private _fechaFinProximoCupon As Decimal
    Private _dtCuponeraNormal As DataTable
    Private _realizoBusqueda As Boolean
    Private _cambioValores As Boolean
    Private _codigoPeriodo As String
    Private _tipo As String
    Private _codigoTipoAmortizacion As String
    Private _tasaCupon As Decimal
    Private _baseDias As String
    Private _baseAnios As String
    Private _tipoTasaVariable As String
    Private _tasaVariable As Decimal
    Private _periodicidadTasaVariable As Integer
    Public Property periodicidadTasaVariable() As Integer
        Get
            Return _periodicidadTasaVariable
        End Get
        Set(ByVal value As Integer)
            _periodicidadTasaVariable = value
        End Set
    End Property
    Public Property tasaVariable() As Decimal
        Get
            Return _tasaVariable
        End Get
        Set(ByVal value As Decimal)
            _tasaVariable = value
        End Set
    End Property
    Public Property tipoTasaVariable() As String
        Get
            Return _tipoTasaVariable
        End Get
        Set(ByVal value As String)
            _tipoTasaVariable = value
        End Set
    End Property
    Public Property baseAnios() As String
        Get
            Return _baseAnios
        End Get
        Set(ByVal value As String)
            _baseAnios = value
        End Set
    End Property
    Public Property baseDias() As String
        Get
            Return _baseDias
        End Get
        Set(ByVal value As String)
            _baseDias = value
        End Set
    End Property
    Public Property TasaCupon() As Decimal
        Get
            Return _tasaCupon
        End Get
        Set(ByVal value As Decimal)
            _tasaCupon = value
        End Set
    End Property
    Public Property RealizoBusqueda() As Boolean
        Get
            Return _realizoBusqueda
        End Get
        Set(ByVal value As Boolean)
            _realizoBusqueda = value
        End Set
    End Property

    Public Property Tipo() As String
        Get
            Return _tipo
        End Get
        Set(ByVal value As String)
            _tipo = value
        End Set
    End Property


    Public Property CodigoTipoAmortizacion() As String
        Get
            Return _codigoTipoAmortizacion
        End Get
        Set(ByVal value As String)
            _codigoTipoAmortizacion = value
        End Set
    End Property

    Public Property CodigoPeriodo() As String
        Get
            Return _codigoPeriodo
        End Get
        Set(ByVal value As String)
            _codigoPeriodo = value
        End Set
    End Property

    Public Property CambioValores() As Boolean
        Get
            Return _cambioValores
        End Get
        Set(ByVal value As Boolean)
            _cambioValores = value
        End Set
    End Property

    Public Property FechaFinProximoCupon() As Decimal
        Get
            Return _fechaFinProximoCupon
        End Get
        Set(ByVal value As Decimal)
            _fechaFinProximoCupon = value
        End Set
    End Property

    Public Property FechaVencimiento() As Decimal
        Get
            Return _fechaVencimiento
        End Get
        Set(ByVal value As Decimal)
            _fechaVencimiento = value
        End Set
    End Property

    Public Property FechaEmision() As Decimal
        Get
            Return _fechaEmision
        End Get
        Set(ByVal value As Decimal)
            _fechaEmision = value
        End Set
    End Property

    Public Property DtCuponeraNormal() As DataTable
        Get
            Return _dtCuponeraNormal
        End Get
        Set(ByVal value As DataTable)
            _dtCuponeraNormal = value
        End Set
    End Property


End Class
