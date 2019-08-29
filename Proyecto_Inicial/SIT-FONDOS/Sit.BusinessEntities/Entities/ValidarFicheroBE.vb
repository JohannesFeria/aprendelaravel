Imports System.Data
Imports System.Collections.Generic

Public Class ComisionValor

    Private _PCOMIS_SAB As Decimal
    Private _PCUOTA_BVL As Decimal
    Private _PFND_GARNTIA As Decimal
    Private _PCONT_CONASE As Decimal
    Private _PCUOT_CAVLI As Decimal
    Private _PIGV_TOT As Decimal


    Public Property PCOMIS_SAB() As Decimal
        Get
            Return _PCOMIS_SAB
        End Get
        Set(ByVal value As Decimal)
            _PCOMIS_SAB = value
        End Set
    End Property

    Public Property PCUOTA_BVL() As Decimal
        Get
            Return _PCUOTA_BVL
        End Get
        Set(ByVal value As Decimal)
            _PCUOTA_BVL = value
        End Set
    End Property


    Public Property PFND_GARNTIA() As Decimal
        Get
            Return _PFND_GARNTIA
        End Get
        Set(ByVal value As Decimal)
            _PFND_GARNTIA = value
        End Set
    End Property

    Public Property PCONT_CONASE() As Decimal
        Get
            Return _PCONT_CONASE
        End Get
        Set(ByVal value As Decimal)
            _PCONT_CONASE = value
        End Set
    End Property

    Public Property PCUOT_CAVLI() As Decimal
        Get
            Return _PCUOT_CAVLI
        End Get
        Set(ByVal value As Decimal)
            _PCUOT_CAVLI = value
        End Set
    End Property

    Public Property PIGV_TOT() As Decimal
        Get
            Return _PIGV_TOT
        End Get
        Set(ByVal value As Decimal)
            _PIGV_TOT = value
        End Set
    End Property

End Class

Public Class ValidarFicheroBE

    Private _camposValidar As List(Of String)
    Private _codigosValidaciones As List(Of String)
    Private _objetoValido As Boolean
    Private _nombreArchivo As String
    Private _preOrdenInversion As PrevOrdenInversionBE
    Private _tercero As TercerosBE
    Private _dtAsignacion As DataTable
    Private _codigoNemonico As String
    Private _indice As Integer
    Private _tipoError As String
    Private _codigoFacturacion As String
    Private _existeOrden As Boolean
    Private _fechaEmision As Decimal
    Private _fechaVencimiento As Decimal
    Private _cantidad As Decimal
    Private _montoNominal As Decimal
    Private _montoNeto As Decimal
    Private _tasa As Decimal
    Private _codigoMoneda As String
    Private _codigoPortafolio As String
    Private _estado As String
    Private _codigoEntidad As String
    Private _comisionValor As ComisionValor

    Public Property ComisionValor() As ComisionValor
        Get
            Return _comisionValor
        End Get
        Set(ByVal value As ComisionValor)
            _comisionValor = value
        End Set
    End Property


    Public Property CodigoEntidad() As String
        Get
            Return _codigoEntidad
        End Get
        Set(ByVal value As String)
            _codigoEntidad = value
        End Set
    End Property

    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property CodigoPortafolio() As String
        Get
            Return _codigoPortafolio
        End Get
        Set(ByVal value As String)
            _codigoPortafolio = value
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

    Public Property Tasa() As Decimal
        Get
            Return _tasa
        End Get
        Set(ByVal value As Decimal)
            _tasa = value
        End Set
    End Property

    Public Property MontoNominal() As Decimal
        Get
            Return _montoNominal
        End Get
        Set(ByVal value As Decimal)
            _montoNominal = value
        End Set
    End Property
    Public Property MontoNeto() As Decimal
        Get
            Return _montoNeto
        End Get
        Set(ByVal value As Decimal)
            _montoNeto = value
        End Set
    End Property

    Public Property Cantidad() As Decimal
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Decimal)
            _cantidad = value
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
    Public Property FechaVencimiento() As Decimal
        Get
            Return _fechaVencimiento
        End Get
        Set(ByVal value As Decimal)
            _fechaVencimiento = value
        End Set
    End Property

    Public Property CodigoFacturacion() As String
        Get
            Return _codigoFacturacion
        End Get
        Set(ByVal value As String)
            _codigoFacturacion = value
        End Set
    End Property

    Public Property ExisteOrden() As Boolean
        Get
            Return _existeOrden
        End Get
        Set(ByVal value As Boolean)
            _existeOrden = value
        End Set
    End Property

    Public Property Indice() As Integer
        Get
            Return _indice
        End Get
        Set(ByVal value As Integer)
            _indice = value
        End Set
    End Property

    Public Property CodigoNemonico() As String
        Get
            Return _codigoNemonico
        End Get
        Set(ByVal value As String)
            _codigoNemonico = value
        End Set
    End Property

    Public Property TipoError() As String
        Get
            Return _tipoError
        End Get
        Set(ByVal value As String)
            _tipoError = value
        End Set
    End Property

    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(ByVal value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property DtAsignacion() As DataTable
        Get
            Return _dtAsignacion
        End Get
        Set(ByVal value As DataTable)
            _dtAsignacion = value
        End Set
    End Property

    Public Property CamposValidar() As List(Of String)
        Get
            Return _camposValidar
        End Get
        Set(ByVal value As List(Of String))
            _camposValidar = value
        End Set
    End Property

    Public Property CodigosValidaciones() As List(Of String)
        Get
            Return _codigosValidaciones
        End Get
        Set(ByVal value As List(Of String))
            _codigosValidaciones = value
        End Set
    End Property

    Public Property ObjetoValido() As Boolean
        Get
            Return _objetoValido
        End Get
        Set(ByVal value As Boolean)
            _objetoValido = value
        End Set
    End Property

    Public Property Tercero() As TercerosBE
        Get
            Return _tercero
        End Get
        Set(ByVal value As TercerosBE)
            _tercero = value
        End Set
    End Property

    Public Property PreOrdenInversion() As PrevOrdenInversionBE
        Get
            Return _preOrdenInversion
        End Get
        Set(ByVal value As PrevOrdenInversionBE)
            _preOrdenInversion = value
        End Set
    End Property


    Public Sub New()
        Me._camposValidar = New List(Of String)
        Me._codigosValidaciones = New List(Of String)
        Me._objetoValido = False
        Me._preOrdenInversion = New PrevOrdenInversionBE
        Me._nombreArchivo = String.Empty
        Me._tipoError = String.Empty
        Me._codigoEntidad = String.Empty
    End Sub
End Class

