<Serializable()>
Public Class MercadoBE
    Sub New()
    End Sub
    Private _CodigoMercado As String
    Public Property CodigoMercado() As String
        Get
            Return _CodigoMercado
        End Get
        Set(ByVal value As String)
            _CodigoMercado = value
        End Set
    End Property
    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
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
    Public Property FechaCreacion() As String
        Get
            Return _FechaCreacion
        End Get
        Set(ByVal value As String)
            _FechaCreacion = value
        End Set
    End Property
    Private _UsuarioModificacion As String
    Public Property UsuarioModificacion() As String
        Get
            Return _UsuarioModificacion
        End Get
        Set(ByVal value As String)
            _UsuarioModificacion = value
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
    Private _FechaModificacion As Decimal
    Public Property FechaModificacion() As String
        Get
            Return _FechaModificacion
        End Get
        Set(ByVal value As String)
            _FechaModificacion = value
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
    Private _Situacion As String
    Public Property Situacion() As String
        Get
            Return _Situacion
        End Get
        Set(ByVal value As String)
            _Situacion = value
        End Set
    End Property
    Private _HoraModificacion As String
    Public Property HoraModificacion() As String
        Get
            Return _HoraModificacion
        End Get
        Set(ByVal value As String)
            _HoraModificacion = value
        End Set
    End Property
End Class
<Serializable()>
Public Class MercadoBEList
    Inherits Generic.List(Of MercadoBE)
End Class