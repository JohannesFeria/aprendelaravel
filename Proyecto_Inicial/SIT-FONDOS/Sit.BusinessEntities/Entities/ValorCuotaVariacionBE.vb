Imports System.Data

Public Class ValorCuotaVariacionBE
    Private _codigoPortafolioSBS As String
    Private _codigoSerie As String
    Private _fechaProceso As Decimal
    Private _carteraPrecio As Decimal
    Private _carteraTipoCambio As Decimal
    Private _derivados As Decimal
    Private _cuentasPorCobrarTipoCambio As Decimal
    Private _cuentasPorPagarTipoCambio As Decimal
    Private _cuentasPorCobrarPrecio As Decimal
    Private _cuentasPorPagarPrecio As Decimal
    Private _cajaTipoCambio As Decimal
    Private _porcentageVariacionEstimado As Decimal
    Private _totalRentabilidadInversiones As Decimal
    Private _estadoSemaforo As String
    Private _porcentageVariacionSIT As Decimal
    Private _diferenciaEstimadoSIT As Decimal
    Private _comision As Decimal


    Public Property CodigoPortafolioSBS() As String
        Get
            Return _codigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _codigoPortafolioSBS = value
        End Set
    End Property
    Public Property CodigoSerie() As String
        Get
            Return _codigoSerie
        End Get
        Set(ByVal value As String)
            _codigoSerie = value
        End Set
    End Property
    Public Property FechaProceso() As Decimal
        Get
            Return _fechaProceso
        End Get
        Set(ByVal value As Decimal)
            _fechaProceso = value
        End Set
    End Property
    Public Property CarteraPrecio() As Decimal
        Get
            Return _carteraPrecio
        End Get
        Set(ByVal value As Decimal)
            _carteraPrecio = value
        End Set
    End Property
    Public Property CarteraTipoCambio() As Decimal
        Get
            Return _carteraTipoCambio
        End Get
        Set(ByVal value As Decimal)
            _carteraTipoCambio = value
        End Set
    End Property
    Public Property Derivados() As Decimal
        Get
            Return _derivados
        End Get
        Set(ByVal value As Decimal)
            _derivados = value
        End Set
    End Property
    Public Property CajaTipoCambio() As Decimal
        Get
            Return _cajaTipoCambio
        End Get
        Set(ByVal value As Decimal)
            _cajaTipoCambio = value
        End Set
    End Property
    Public Property CuentasPorCobrarTipoCambio() As Decimal
        Get
            Return _cuentasPorCobrarTipoCambio
        End Get
        Set(ByVal value As Decimal)
            _cuentasPorCobrarTipoCambio = value
        End Set
    End Property
    Public Property CuentasPorPagarTipoCambio() As Decimal
        Get
            Return _cuentasPorPagarTipoCambio
        End Get
        Set(ByVal value As Decimal)
            _cuentasPorPagarTipoCambio = value
        End Set
    End Property
    Public Property CuentasPorCobrarPrecio() As Decimal
        Get
            Return _cuentasPorCobrarPrecio
        End Get
        Set(ByVal value As Decimal)
            _cuentasPorCobrarPrecio = value
        End Set
    End Property
    Public Property CuentasPorPagarPrecio() As Decimal
        Get
            Return _cuentasPorPagarPrecio
        End Get
        Set(ByVal value As Decimal)
            _cuentasPorPagarPrecio = value
        End Set
    End Property
    Public Property PorcentageVariacionEstimado() As Decimal
        Get
            Return _porcentageVariacionEstimado
        End Get
        Set(ByVal value As Decimal)
            _porcentageVariacionEstimado = value
        End Set
    End Property
    Public Property TotalRentabilidadInversiones() As Decimal
        Get
            Return _totalRentabilidadInversiones
        End Get
        Set(ByVal value As Decimal)
            _totalRentabilidadInversiones = value
        End Set
    End Property
    Public Property EstadoSemaforo() As String
        Get
            Return _estadoSemaforo
        End Get
        Set(ByVal value As String)
            _estadoSemaforo = value
        End Set
    End Property
    Public Property PorcentageVariacionSIT() As Decimal
        Get
            Return _porcentageVariacionSIT
        End Get
        Set(ByVal value As Decimal)
            _porcentageVariacionSIT = value
        End Set
    End Property
    Public Property DiferenciaEstimadoSIT() As Decimal
        Get
            Return _diferenciaEstimadoSIT
        End Get
        Set(ByVal value As Decimal)
            _diferenciaEstimadoSIT = value
        End Set
    End Property
    Public Property Comision() As Decimal
        Get
            Return _comision
        End Get
        Set(ByVal value As Decimal)
            _comision = value
        End Set
    End Property

    Public Sub New(ByVal fila As DataRow)
        Me.CodigoPortafolioSBS = fila("CodigoPortafolioSBS")
        Me.CodigoSerie = fila("CodigoSerie")
        Me.FechaProceso = fila("FechaProceso")
        Me.CarteraPrecio = fila("CarteraPrecio")
        Me.CarteraTipoCambio = fila("CarteraTipoCambio")
        Me.Derivados = fila("Derivados")
        Me.CuentasPorCobrarTipoCambio = fila("CuentasPorCobrarTipoCambio")
        Me.CuentasPorPagarTipoCambio = fila("CuentasPorPagarTipoCambio")
        Me.CuentasPorCobrarPrecio = fila("CuentasPorCobrarPrecio")
        Me.CuentasPorPagarPrecio = fila("CuentasPorPagarPrecio")
        Me.PorcentageVariacionEstimado = fila("PorcentageVariacionEstimado")
        Me.TotalRentabilidadInversiones = fila("TotalRentabilidadInversiones")
        Me.EstadoSemaforo = fila("EstadoSemaforo")
        Me.PorcentageVariacionSIT = fila("PorcentageVariacionSIT")
        Me.DiferenciaEstimadoSIT = fila("DiferenciaEstimadoSIT")
    End Sub
    Public Sub New()
        Me.CarteraPrecio = 0
        Me.CarteraTipoCambio = 0
        Me.Derivados = 0
        Me.CuentasPorCobrarTipoCambio = 0
        Me.CuentasPorPagarTipoCambio = 0
        Me.CuentasPorCobrarPrecio = 0
        Me.CuentasPorPagarPrecio = 0
        Me.PorcentageVariacionEstimado = 0
        Me.TotalRentabilidadInversiones = 0
        Me.EstadoSemaforo = "1"
        Me.PorcentageVariacionSIT = 0
    End Sub
End Class
