Public Class EventosAutomaticosBE

    Private _Id As Integer
    Public Property Id() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    Private _CodigoPortafolioSBS As String
    Public Property CodigoPortafolioSBS() As String
        Get
            Return _CodigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _CodigoPortafolioSBS = value
        End Set
    End Property

    Private _NumeroOperacion As String
    Public Property NumeroOperacion() As String
        Get
            Return _NumeroOperacion
        End Get
        Set(ByVal value As String)
            _NumeroOperacion = value
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

    Private _Egreso As String
    Public Property Egreso() As String
        Get
            Return _Egreso
        End Get
        Set(ByVal value As String)
            _Egreso = value
        End Set
    End Property

    Private _UsuarioCreacion As String
    Public Property UsuarioCreacion() As String
        Get
            Return _UsuarioCreacion
        End Get
        Set(ByVal value As String)
            _UsuarioCreacion = value
        End Set
    End Property

    Private _FechaCreacion As Decimal
    Public Property FechaCreacion() As Decimal
        Get
            Return _FechaCreacion
        End Get
        Set(ByVal value As Decimal)
            _FechaCreacion = value
        End Set
    End Property

    Private _HoraCreacion As String
    Public Property HoraCreacion() As String
        Get
            Return _HoraCreacion
        End Get
        Set(ByVal value As String)
            _HoraCreacion = value
        End Set
    End Property

    Private _Host As String
    Public Property Host() As String
        Get
            Return _Host
        End Get
        Set(ByVal value As String)
            _Host = value
        End Set
    End Property

    Private _FlagCorte As String
    Public Property FlagCorte() As String
        Get
            Return _FlagCorte
        End Get
        Set(ByVal value As String)
            _FlagCorte = value
        End Set
    End Property
End Class
