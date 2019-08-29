Public Class PrevisionPago

    Private sCodigoPago As String
    Public Property CodigoPago() As String
        Get
            Return sCodigoPago
        End Get
        Set(ByVal value As String)
            sCodigoPago = value
        End Set
    End Property

    Private iCodigoSecuencial As Integer
    Public Property CodigoSecuencial() As Integer
        Get
            Return iCodigoSecuencial
        End Get
        Set(ByVal value As Integer)
            iCodigoSecuencial = value
        End Set
    End Property

    Private iFechaPago As Integer
    Public Property FechaPago() As Integer
        Get
            Return iFechaPago
        End Get
        Set(ByVal value As Integer)
            iFechaPago = value
        End Set
    End Property

    Private sIdTipoOperacion As String
    Public Property IdTipoOperacion() As String
        Get
            Return sIdTipoOperacion
        End Get
        Set(ByVal value As String)
            sIdTipoOperacion = value
        End Set
    End Property

    Private sEstado As String
    Public Property Estado() As String
        Get
            Return sEstado
        End Get
        Set(ByVal value As String)
            sEstado = value
        End Set
    End Property

    Private sUsuarioProvision As String
    Public Property UsuarioProvision() As String
        Get
            Return sUsuarioProvision
        End Get
        Set(ByVal value As String)
            sUsuarioProvision = value
        End Set
    End Property

    Private iFechaProvision As Integer
    Public Property FechaProvision() As Integer
        Get
            Return iFechaProvision
        End Get
        Set(ByVal value As Integer)
            iFechaProvision = value
        End Set
    End Property

    Private sUsuarioAprobacion As String
    Public Property UsuarioAprobacion() As String
        Get
            Return sUsuarioAprobacion
        End Get
        Set(ByVal value As String)
            sUsuarioAprobacion = value
        End Set
    End Property

    Private iFechaAprobacion As Integer
    Public Property FechaAprobacion() As Integer
        Get
            Return iFechaAprobacion
        End Get
        Set(ByVal value As Integer)
            iFechaAprobacion = value
        End Set
    End Property

    Private sUsuarioAnulacion As String
    Public Property UsuarioAnulacion() As String
        Get
            Return sUsuarioAnulacion
        End Get
        Set(ByVal value As String)
            sUsuarioAnulacion = value
        End Set
    End Property

    Private iFechaAnulacion As Integer
    Public Property FechaAnulacion() As Integer
        Get
            Return iFechaAnulacion
        End Get
        Set(ByVal value As Integer)
            iFechaAnulacion = value
        End Set
    End Property

End Class
