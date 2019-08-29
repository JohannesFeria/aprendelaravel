Imports Microsoft.VisualBasic

<Serializable()> _
Public Class Portafolio

    Private _fila As String
    Public Property Fila() As String
        Get
            Return _fila
        End Get
        Set(ByVal value As String)
            _fila = value
        End Set
    End Property

    Private _CodPortafolio As String
    Public Property CodPortafolio() As String
        Get
            Return _CodPortafolio
        End Get
        Set(ByVal value As String)
            _CodPortafolio = value
        End Set
    End Property


    Private _NomPortafolio As String
    Public Property NomPortafolio() As String
        Get
            Return _NomPortafolio
        End Get
        Set(ByVal value As String)
            _NomPortafolio = value
        End Set
    End Property


    Private _FechaApertura As Decimal
    Public Property FechaApertura() As Decimal
        Get
            Return _FechaApertura
        End Get
        Set(ByVal value As Decimal)
            _FechaApertura = value
        End Set
    End Property

    Private _Estado As Boolean
    Public Property Estado() As Boolean
        Get
            Return _Estado
        End Get
        Set(ByVal value As Boolean)
            _Estado = value
        End Set
    End Property

    Private _FechaOperacion As Decimal
    Public Property FechaOperacion() As Decimal
        Get
            Return _FechaOperacion
        End Get
        Set(ByVal value As Decimal)
            _FechaOperacion = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class PortafolioVisor

    Private _CodPortafolio As String
    Public Property CodPortafolio() As String
        Get
            Return _CodPortafolio
        End Get
        Set(ByVal value As String)
            _CodPortafolio = value
        End Set
    End Property


    Private _FechaOperacion As Decimal
    Public Property FechaOperacion() As Decimal
        Get
            Return _FechaOperacion
        End Get
        Set(ByVal value As Decimal)
            _FechaOperacion = value
        End Set
    End Property


    Private _VL As Integer
    Public Property VL() As Integer
        Get
            Return _VL
        End Get
        Set(ByVal value As Integer)
            _VL = value
        End Set
    End Property

    Private _Interes As Integer
    Public Property Interes() As Integer
        Get
            Return _Interes
        End Get
        Set(ByVal value As Integer)
            _Interes = value
        End Set
    End Property

    Private _Ganancia As Integer
    Public Property Ganancia() As Integer
        Get
            Return _Ganancia
        End Get
        Set(ByVal value As Integer)
            _Ganancia = value
        End Set
    End Property

    Private _Inversion As Integer
    Public Property Inversion() As Integer
        Get
            Return _Inversion
        End Get
        Set(ByVal value As Integer)
            _Inversion = value
        End Set
    End Property

    Private _Valorizacion As Integer
    Public Property Valorizacion() As Integer
        Get
            Return _Valorizacion
        End Get
        Set(ByVal value As Integer)
            _Valorizacion = value
        End Set
    End Property

End Class

