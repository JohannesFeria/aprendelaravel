Public Class PagoFechaComisionBE

    Private _codigoPortafolioSBS As String
    Private _codigoBanco As String
    Private _codigoMoneda As String
    Private _numeroCuenta As String
    Private _cuentaContable As String
    Private _fechaInicio As Decimal
    Private _fechaFin As Decimal
    Private _fechaSolicitud As Decimal
    Private _usuarioSolicitud As String
    Private _estado As String
    Private _comisionAcumulada As Decimal
    Private _saldoDisponible As Decimal
    Private _fechaPago As Decimal
    Private _fechaCajaOperaciones As Decimal

    Private _saldoOnline As Decimal
    Private _id As Int32
    Private _tieneError As Boolean

    Private _numeroCuentaAdministradora As String
    Private _codigoBancoAdministradora As String
    Private _fechaConfirmacion As Decimal
    Public Property Id() As Int32
        Get
            Return _id
        End Get
        Set(ByVal value As Int32)
            _id = value
        End Set
    End Property
    Public Property TieneError() As Boolean
        Get
            Return _tieneError
        End Get
        Set(ByVal value As Boolean)
            _tieneError = value
        End Set
    End Property
    Public Property CodigoPortafolioSBS() As String
        Get
            Return _codigoPortafolioSBS
        End Get
        Set(ByVal value As String)
            _codigoPortafolioSBS = value
        End Set
    End Property
    Public Property CodigoBanco() As String
        Get
            Return _codigoBanco
        End Get
        Set(ByVal value As String)
            _codigoBanco = value
        End Set
    End Property
    Public Property CodigoBancoAdministradora() As String
        Get
            Return _codigoBancoAdministradora
        End Get
        Set(ByVal value As String)
            _codigoBancoAdministradora = value
        End Set
    End Property

    Public Property NumeroCuentaAdministradora() As String
        Get
            Return _numeroCuentaAdministradora
        End Get
        Set(ByVal value As String)
            _numeroCuentaAdministradora = value
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
    Public Property NumeroCuenta() As String
        Get
            Return _numeroCuenta
        End Get
        Set(ByVal value As String)
            _numeroCuenta = value
        End Set
    End Property
    Public Property CuentaContable() As String
        Get
            Return _cuentaContable
        End Get
        Set(ByVal value As String)
            _cuentaContable = value
        End Set
    End Property

    Public Property FechaPago() As Decimal
        Get
            Return _fechaPago
        End Get
        Set(ByVal value As Decimal)
            _fechaPago = value
        End Set
    End Property

    Public Property FechaInicio() As Decimal
        Get
            Return _fechaInicio
        End Get
        Set(ByVal value As Decimal)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin() As Decimal
        Get
            Return _fechaFin
        End Get
        Set(ByVal value As Decimal)
            _fechaFin = value
        End Set
    End Property
    Public Property FechaSolicitud() As Decimal
        Get
            Return _fechaSolicitud
        End Get
        Set(ByVal value As Decimal)
            _fechaSolicitud = value
        End Set
    End Property

    Public Property UsuarioSolicitud() As String
        Get
            Return _usuarioSolicitud
        End Get
        Set(ByVal value As String)
            _usuarioSolicitud = value
        End Set
    End Property
    Public Property FechaConfirmacion() As Decimal
        Get
            Return _fechaConfirmacion
        End Get
        Set(ByVal value As Decimal)
            _fechaConfirmacion = value
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

    Public Property ComisionAcumulada() As Decimal
        Get
            Return _comisionAcumulada
        End Get
        Set(ByVal value As Decimal)
            _comisionAcumulada = value
        End Set
    End Property
    Public Property SaldoDisponible() As Decimal
        Get
            Return _saldoDisponible
        End Get
        Set(ByVal value As Decimal)
            _saldoDisponible = value
        End Set
    End Property
    Public Property SaldoOnline() As Decimal
        Get
            Return _saldoOnline
        End Get
        Set(ByVal value As Decimal)
            _saldoOnline = value
        End Set
    End Property
    Public Property FechaCajaOperaciones() As Decimal
        Get
            Return _fechaCajaOperaciones
        End Get
        Set(ByVal value As Decimal)
            _fechaCajaOperaciones = value
        End Set
    End Property

    Public Sub New(ByVal dt As DataTable)

        If dt.Rows.Count > 0 Then
            Dim fila As DataRow = dt.Rows(0)
            Me.CodigoPortafolioSBS = fila("CodigoPortafolioSBS")
            Me.Id = fila("Id")
            Me.CodigoBanco = fila("CodigoBanco")
            Me.NumeroCuenta = fila("NumeroCuenta")
            Me.FechaInicio = fila("FechaInicio")
            Me.FechaFin = fila("FechaFin")
            Me.FechaSolicitud = fila("FechaSolicitud")
            Me.ComisionAcumulada = fila("ComisionAcumulada")
            Me.SaldoDisponible = fila("SaldoDisponible")
            Me.SaldoOnline = fila("SaldoOnline")
            Me.Estado = fila("Estado")
            Me.CodigoMoneda = fila("CodigoMoneda")
            Me.FechaCajaOperaciones = fila("FechaCaja")
        Else
            Me.Id = 0

        End If
       
    End Sub

    Public Sub New()
        Me.Id = 0
    End Sub
End Class
