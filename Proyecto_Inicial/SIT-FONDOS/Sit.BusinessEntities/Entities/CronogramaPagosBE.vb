Public Class CronogramaPagosBE

    Private iIDCronogramaPagos As Integer
    Public Property IDCronogramaPagos() As Integer
        Get
            Return iIDCronogramaPagos
        End Get
        Set(ByVal value As Integer)
            iIDCronogramaPagos = value
        End Set
    End Property

    Private sCodigoPortafolioSBS As String
    Public Property CodigoPortafolioSBS() As String
        Get
            Return sCodigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            sCodigoPortafolioSBS = value
        End Set
    End Property

    Private dFechaCronogramaPagos As Decimal
    Public Property FechaCronogramaPagos() As Decimal
        Get
            Return dFechaCronogramaPagos
        End Get
        Set(ByVal value As Decimal)
            dFechaCronogramaPagos = value
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

    Private sUsuarioCreacion As String
    Public Property UsuarioCreacion() As String
        Get
            Return sUsuarioCreacion
        End Get
        Set(ByVal value As String)
            sUsuarioCreacion = value
        End Set
    End Property

    Private dFechaCreacion As Decimal
    Public Property FechaCreacion() As Decimal
        Get
            Return dFechaCreacion
        End Get
        Set(ByVal value As Decimal)
            dFechaCreacion = value
        End Set
    End Property

    Private sHoraCreacion As String
    Public Property HoraCreacion() As String
        Get
            Return sHoraCreacion
        End Get
        Set(ByVal value As String)
            sHoraCreacion = value
        End Set
    End Property

    Private sUsuarioModificacion As String
    Public Property UsuarioModificacion() As String
        Get
            Return sUsuarioModificacion
        End Get
        Set(ByVal value As String)
            sUsuarioModificacion = value
        End Set
    End Property

    Private dFechaModificacion As Decimal
    Public Property FechaModificacion() As Decimal
        Get
            Return dFechaModificacion
        End Get
        Set(ByVal value As Decimal)
            dFechaModificacion = value
        End Set
    End Property

    Private sHoraModificacion As String
    Public Property HoraModificacion() As String
        Get
            Return sHoraModificacion
        End Get
        Set(ByVal value As String)
            sHoraModificacion = value
        End Set
    End Property

    Private sHost As String
    Public Property Host() As String
        Get
            Return sHost
        End Get
        Set(ByVal value As String)
            sHost = value
        End Set
    End Property

    Private sCodigoMnemonico As String
    Public Property codigoMnemonico() As String
        Get
            Return scodigoMnemonico
        End Get
        Set(ByVal value As String)
            scodigoMnemonico = value
        End Set
    End Property

    Private sfechaLiquidacion As String
    Public Property fechaLiquidacion() As String
        Get
            Return sfechaLiquidacion
        End Get
        Set(ByVal value As String)
            sfechaLiquidacion = value
        End Set
    End Property
    Private sTipoPago As String
    Public Property TipoPago() As String
        Get
            Return sTipoPago
        End Get
        Set(ByVal value As String)
            sTipoPago = value
        End Set
    End Property

    Public Sub New(ByVal codigoMnemonico As String, ByVal tipoPago As String, ByVal fechaLiquidacion As String)
        Me.codigoMnemonico = codigoMnemonico
        Me.TipoPago = tipoPago
        Me.fechaLiquidacion = fechaLiquidacion
    End Sub

End Class
