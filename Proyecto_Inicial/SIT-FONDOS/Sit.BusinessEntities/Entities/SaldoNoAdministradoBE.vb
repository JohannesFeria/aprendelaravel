Public Class SaldoNoAdministradoBE

    Private sCodigoSaldoNoAdministrado As String
    Public Property CodigoSaldoNoAdministrado() As String
        Get
            Return sCodigoSaldoNoAdministrado
        End Get
        Set(ByVal value As String)
            sCodigoSaldoNoAdministrado = value
        End Set
    End Property

    Private sCodigoTercero As String
    Public Property CodigoTercero() As String
        Get
            Return sCodigoTercero
        End Get
        Set(ByVal value As String)
            sCodigoTercero = value
        End Set
    End Property

    Private sFecha As Decimal
    Public Property Fecha() As Decimal
        Get
            Return sFecha
        End Get
        Set(ByVal value As Decimal)
            sFecha = value
        End Set
    End Property

    Private sCodigoTerceroFinanciero As String
    Public Property CodigoTerceroFinanciero() As String
        Get
            Return sCodigoTerceroFinanciero
        End Get
        Set(ByVal value As String)
            sCodigoTerceroFinanciero = value
        End Set
    End Property

    Private sTipoCuenta As String
    Public Property TipoCuenta() As String
        Get
            Return sTipoCuenta
        End Get
        Set(ByVal value As String)
            sTipoCuenta = value
        End Set
    End Property

    Private sSaldo As Decimal
    Public Property Saldo() As Decimal
        Get
            Return sSaldo
        End Get
        Set(ByVal value As Decimal)
            sSaldo = value
        End Set
    End Property

    Private sCodigoMoneda As String
    Public Property CodigoMoneda() As String
        Get
            Return sCodigoMoneda
        End Get
        Set(ByVal value As String)
            sCodigoMoneda = value
        End Set
    End Property

    Private sSituacion As String
    Public Property Situacion() As String
        Get
            Return sSituacion
        End Get
        Set(ByVal value As String)
            sSituacion = value
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

    Private sFechaCreacion As Decimal
    Public Property FechaCreacion() As Decimal
        Get
            Return sFechaCreacion
        End Get
        Set(ByVal value As Decimal)
            sFechaCreacion = value
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

    Private sFechaModificacion As Decimal
    Public Property FechaModificacion() As Decimal
        Get
            Return sFechaModificacion
        End Get
        Set(ByVal value As Decimal)
            sFechaModificacion = value
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
End Class
