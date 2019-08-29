<Serializable()>
Public Class SeleccionCartaBE
    Sub New()
    End Sub
    Private _DescripcionPortafolio As String
    Public Property DescripcionPortafolio() As String
        Get
            Return _DescripcionPortafolio
        End Get
        Set(ByVal value As String)
            _DescripcionPortafolio = value
        End Set
    End Property
    Private _DescripcionOperacion As String
    Public Property DescripcionOperacion() As String
        Get
            Return _DescripcionOperacion
        End Get
        Set(ByVal value As String)
            _DescripcionOperacion = value
        End Set
    End Property
    Private _ModeloCarta As String
    Public Property ModeloCarta() As String
        Get
            Return _ModeloCarta
        End Get
        Set(ByVal value As String)
            _ModeloCarta = value
        End Set
    End Property
    Private _CodigoAgrupado As String
    Public Property CodigoAgrupado() As String
        Get
            Return _CodigoAgrupado
        End Get
        Set(ByVal value As String)
            _CodigoAgrupado = value
        End Set
    End Property

    Private _DescripcionIntermediario As String
    Public Property DescripcionIntermediario() As String
        Get
            Return _DescripcionIntermediario
        End Get
        Set(ByVal value As String)
            _DescripcionIntermediario = value
        End Set
    End Property
    Private _CodigoMoneda As String
    Public Property CodigoMoneda() As String
        Get
            Return _CodigoMoneda
        End Get
        Set(ByVal value As String)
            _CodigoMoneda = value
        End Set
    End Property
    Private _Importe As Decimal
    Public Property Importe() As Decimal
        Get
            Return _Importe
        End Get
        Set(ByVal value As Decimal)
            _Importe = value
        End Set
    End Property
    Private _NumeroOrden As String
    Public Property NumeroOrden() As String
        Get
            Return _NumeroOrden
        End Get
        Set(ByVal value As String)
            _NumeroOrden = value
        End Set
    End Property
    Private _VBADMIN As String
    Public Property VBADMIN() As String
        Get
            Return _VBADMIN
        End Get
        Set(ByVal value As String)
            _VBADMIN = value
        End Set
    End Property
    Private _VBGERF1 As String
    Public Property VBGERF1() As String
        Get
            Return _VBGERF1
        End Get
        Set(ByVal value As String)
            _VBGERF1 = value
        End Set
    End Property
    Private _VBGERF2 As String
    Public Property VBGERF2() As String
        Get
            Return _VBGERF2
        End Get
        Set(ByVal value As String)
            _VBGERF2 = value
        End Set
    End Property

    Private _CodigoOperacion As String
    Public Property CodigoOperacion() As String
        Get
            Return _CodigoOperacion
        End Get
        Set(ByVal value As String)
            _CodigoOperacion = value
        End Set
    End Property
    Private _CodigoOperacionCaja As String
    Public Property CodigoOperacionCaja() As String
        Get
            Return _CodigoOperacionCaja
        End Get
        Set(ByVal value As String)
            _CodigoOperacionCaja = value
        End Set
    End Property
    Private _EstadoCarta As String
    Public Property EstadoCarta() As String
        Get
            Return _EstadoCarta
        End Get
        Set(ByVal value As String)
            _EstadoCarta = value
        End Set
    End Property
    Private _CodigoModelo As String
    Public Property CodigoModelo() As String
        Get
            Return _CodigoModelo
        End Get
        Set(ByVal value As String)
            _CodigoModelo = value
        End Set
    End Property
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se agrega el campo CodigoPortafolioSBS
    Private _CodigoPortafolioSBS As String
    Public Property CodigoPortafolioSBS() As String
        Get
            Return _CodigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _CodigoPortafolioSBS = value
        End Set
    End Property
    Private _check As Boolean
    Public Property check() As Boolean
        Get
            Return _check
        End Get
        Set(ByVal value As Boolean)
            _check = value
        End Set
    End Property
    'OT 10025 21/02/2017 - Carlos Espejo
    'Descripcion: Se agrega el campo Correlativo Cartas
    Private _CorrelativoCartas As Integer
    Public Property CorrelativoCartas() As Integer
        Get
            Return _CorrelativoCartas
        End Get
        Set(ByVal value As Integer)
            _CorrelativoCartas = value
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
    Private _Banco As String
    Public Property Banco() As String
        Get
            Return _Banco
        End Get
        Set(ByVal value As String)
            _Banco = value
        End Set
    End Property
    Private _CodigoCartaAgrupado As Integer
    Public Property CodigoCartaAgrupado() As Integer
        Get
            Return _CodigoCartaAgrupado
        End Get
        Set(ByVal value As Integer)
            _CodigoCartaAgrupado = value
        End Set
    End Property
    Private _CantidadOperacion As Integer
    Public Property CantidadOperacion() As Integer
        Get
            Return _CantidadOperacion
        End Get
        Set(ByVal value As Integer)
            _CantidadOperacion = value
        End Set
    End Property
    Private _Tipo As String
    Public Property Tipo() As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
        End Set
    End Property
End Class
<Serializable()>
Public Class SeleccionCartaBEList
    Inherits Generic.List(Of SeleccionCartaBE)
End Class