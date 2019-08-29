<Serializable()>
Public Class TerceroBE
    Sub New()
    End Sub
    Private _CodigoTercero As String
    Public Property CodigoTercero() As String
        Get
            Return _CodigoTercero
        End Get
        Set(ByVal value As String)
            _CodigoTercero = value
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
    Private _Situacion As String
    Public Property Situacion() As String
        Get
            Return _Situacion
        End Get
        Set(ByVal value As String)
            _Situacion = value
        End Set
    End Property
    Private _Direccion As String
    Public Property Direccion() As String
        Get
            Return _Direccion
        End Get
        Set(ByVal value As String)
            _Direccion = value
        End Set
    End Property
    Private _CodigoPostal As String
    Public Property CodigoPostal() As String
        Get
            Return _CodigoPostal
        End Get
        Set(ByVal value As String)
            _CodigoPostal = value
        End Set
    End Property
    Private _CodigoPais As String
    Public Property CodigoPais() As String
        Get
            Return _CodigoPais
        End Get
        Set(ByVal value As String)
            _CodigoPais = value
        End Set
    End Property
    Private _EntidadFinanciera As String
    Public Property EntidadFinanciera() As String
        Get
            Return _EntidadFinanciera
        End Get
        Set(ByVal value As String)
            _EntidadFinanciera = value
        End Set
    End Property
    Private _CodigoTipoDocumento As String
    Public Property CodigoTipoDocumento() As String
        Get
            Return _CodigoTipoDocumento
        End Get
        Set(ByVal value As String)
            _CodigoTipoDocumento = value
        End Set
    End Property
    Private _CodigoDocumento As String
    Public Property CodigoDocumento() As String
        Get
            Return _CodigoDocumento
        End Get
        Set(ByVal value As String)
            _CodigoDocumento = value
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
    Private _CodigoSectorEmpresarial As String
    Public Property CodigoSectorEmpresarial() As String
        Get
            Return _CodigoSectorEmpresarial
        End Get
        Set(ByVal value As String)
            _CodigoSectorEmpresarial = value
        End Set
    End Property
    Private _Beneficiario As String
    Public Property Beneficiario() As String
        Get
            Return _Beneficiario
        End Get
        Set(ByVal value As String)
            _Beneficiario = value
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
    Private _UsuarioModificacion As String
    Public Property UsuarioModificacion() As String
        Get
            Return _UsuarioModificacion
        End Get
        Set(ByVal value As String)
            _UsuarioModificacion = value
        End Set
    End Property
    Private _FechaModificacion As Decimal
    Public Property FechaModificacion() As Decimal
        Get
            Return _FechaModificacion
        End Get
        Set(ByVal value As Decimal)
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
    Private _HoraModificacion As String
    Public Property HoraModificacion() As String
        Get
            Return _HoraModificacion
        End Get
        Set(ByVal value As String)
            _HoraModificacion = value
        End Set
    End Property
    Private _TipoTercero As String
    Public Property TipoTercero() As String
        Get
            Return _TipoTercero
        End Get
        Set(ByVal value As String)
            _TipoTercero = value
        End Set
    End Property
    Private _ClasificacionTercero As String
    Public Property ClasificacionTercero() As String
        Get
            Return _ClasificacionTercero
        End Get
        Set(ByVal value As String)
            _ClasificacionTercero = value
        End Set
    End Property
    Private _CodigoCustodio As String
    Public Property CodigoCustodio() As String
        Get
            Return _CodigoCustodio
        End Get
        Set(ByVal value As String)
            _CodigoCustodio = value
        End Set
    End Property
    Private _Rating As String
    Public Property Rating() As String
        Get
            Return _Rating
        End Get
        Set(ByVal value As String)
            _Rating = value
        End Set
    End Property
    Private _CodigoEmision As String
    Public Property CodigoEmision() As String
        Get
            Return _CodigoEmision
        End Get
        Set(ByVal value As String)
            _CodigoEmision = value
        End Set
    End Property
    Private _CodigoCalificionOficial As String
    Public Property CodigoCalificionOficial() As String
        Get
            Return _CodigoCalificionOficial
        End Get
        Set(ByVal value As String)
            _CodigoCalificionOficial = value
        End Set
    End Property
    Private _RatingInterno As String
    Public Property RatingInterno() As String
        Get
            Return _RatingInterno
        End Get
        Set(ByVal value As String)
            _RatingInterno = value
        End Set
    End Property
End Class
<Serializable()>
Public Class TerceroBEList
    Inherits Generic.List(Of TerceroBE)
End Class