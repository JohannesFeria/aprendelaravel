<Serializable()>
Public Class AprobadorCartaBE
    Sub New()
    End Sub
    Private _CodigoInterno As String
    Public Property CodigoInterno() As String
        Get
            Return _CodigoInterno
        End Get
        Set(ByVal value As String)
            _CodigoInterno = value
        End Set
    End Property
    Private _Nombre As String
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property
    Private _Cargo As String
    Public Property Cargo() As String
        Get
            Return _Cargo
        End Get
        Set(ByVal value As String)
            _Cargo = value
        End Set
    End Property
    Private _DescripcionRol As String
    Public Property DescripcionRol() As String
        Get
            Return _DescripcionRol
        End Get
        Set(ByVal value As String)
            _DescripcionRol = value
        End Set
    End Property
    Private _Rol As String
    Public Property Rol() As String
        Get
            Return _Rol
        End Get
        Set(ByVal value As String)
            _Rol = value
        End Set
    End Property
    Private _Firma As String
    Public Property Firma() As String
        Get
            Return _Firma
        End Get
        Set(ByVal value As String)
            _Firma = value
        End Set
    End Property
    Private _DescripcionSituacion As String
    Public Property DescripcionSituacion() As String
        Get
            Return _DescripcionSituacion
        End Get
        Set(ByVal value As String)
            _DescripcionSituacion = value
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
    Private _CodigoUsuario As String
    Public Property CodigoUsuario() As String
        Get
            Return _CodigoUsuario
        End Get
        Set(ByVal value As String)
            _CodigoUsuario = value
        End Set
    End Property
    Private _email_trabajo As String
    Public Property email_trabajo() As String
        Get
            Return _email_trabajo
        End Get
        Set(ByVal value As String)
            _email_trabajo = value
        End Set
    End Property
    Private _email_personal As String
    Public Property email_personal() As String
        Get
            Return _email_personal
        End Get
        Set(ByVal value As String)
            _email_personal = value
        End Set
    End Property
End Class
Public Class AprobadorCartaBEList
    Inherits Generic.List(Of AprobadorCartaBE)
End Class