<Serializable()>
Public Class PortafolioBE
    Sub New()
    End Sub
    Private _CodigoPortafolioSBS As String
    Public Property CodigoPortafolioSBS() As String
        Get
            Return _CodigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _CodigoPortafolioSBS = value
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
    Private _NombreCompleto As String
    Public Property NombreCompleto() As String
        Get
            Return _NombreCompleto
        End Get
        Set(ByVal value As String)
            _NombreCompleto = value
        End Set
    End Property
    Private _FechaNegocio As Decimal
    Public Property FechaNegocio() As Decimal
        Get
            Return _FechaNegocio
        End Get
        Set(ByVal value As Decimal)
            _FechaNegocio = value
        End Set
    End Property
    Private _FechaCajaOperaciones As Decimal
    Public Property FechaCajaOperaciones() As Decimal
        Get
            Return _FechaCajaOperaciones
        End Get
        Set(ByVal value As Decimal)
            _FechaCajaOperaciones = value
        End Set
    End Property
    Private _FechaConstitucion As Decimal
    Public Property FechaConstitucion() As Decimal
        Get
            Return _FechaConstitucion
        End Get
        Set(ByVal value As Decimal)
            _FechaConstitucion = value
        End Set
    End Property
    Private _FechaTermino As Decimal
    Public Property FechaTermino() As Decimal
        Get
            Return _FechaTermino
        End Get
        Set(ByVal value As Decimal)
            _FechaTermino = value
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
    Private _CodigoNegocio As String
    Public Property CodigoNegocio() As String
        Get
            Return _CodigoNegocio
        End Get
        Set(ByVal value As String)
            _CodigoNegocio = value
        End Set
    End Property
    Private _FechaAperturaContable As Decimal
    Public Property FechaAperturaContable() As Decimal
        Get
            Return _FechaAperturaContable
        End Get
        Set(ByVal value As Decimal)
            _FechaAperturaContable = value
        End Set
    End Property
End Class
Public Class PortafolioBEList
    Inherits Generic.List(Of PortafolioBE)
End Class