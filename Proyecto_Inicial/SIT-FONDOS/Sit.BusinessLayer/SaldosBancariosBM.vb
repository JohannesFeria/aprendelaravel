Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit.DataAccessLayer
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
	''' Clase para el acceso de los datos para SaldosBancarios tabla.
    Public  Class SaldosBancariosBM
    Inherits InvokerCOM
        Public Sub New()

        End Sub
    Public Function SeleccionarPorFiltro(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarPorFiltro(dsCuentaEconomica)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ValidarSaldosBancariosNegativos(CodigoPortafolioSBS As String, FechaOperacion As Decimal) As DataSet
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().ValidarSaldosBancariosNegativos(CodigoPortafolioSBS, FechaOperacion)
            Return dsSaldos
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarPorFiltro2(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarPorFiltro2(dsCuentaEconomica)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro3(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarPorFiltro3(dsCuentaEconomica)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro4(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As DataSet 'Agregado por LC 22082008 Ejecutar Proc Formato Grilla
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarPorFiltro4(dsCuentaEconomica)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarSaldoPorNumeroCuenta(ByVal codigoPortafolio As String, ByVal numeroCuenta As String) As DataSet
        Try
            Return New SaldosBancariosDAM().SeleccionarPorNumeroCuenta(codigoPortafolio, numeroCuenta)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Seleccionar(ByVal codigoPortafolio As String, ByVal numeroCuenta As String, ByVal fecha As Decimal) As DataSet
        Try
            Return New SaldosBancariosDAM().Seleccionar(codigoPortafolio, numeroCuenta, fecha)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    'RGF 20090119 se agregó parámetro PorDivisas
    Public Function SeleccionarFlujoEstimadoPorFiltro(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal Periodo As String, ByVal PorDivisas As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, FechaInicio, FechaFin, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarFlujoEstimadoPorFiltro(dsCuentaEconomica, FechaInicio, FechaFin, Periodo, PorDivisas)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarFlujoEstimadoPorFiltro2(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal, ByVal Periodo As String, ByVal PorDivisas As String, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, FechaInicio, FechaFin, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarFlujoEstimadoPorFiltro2(dsCuentaEconomica, FechaInicio, FechaFin, Periodo, PorDivisas)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarFlujoEstimado(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal codOperacion As String, ByVal codTipoOperacion As String, ByVal tipoMovimiento As String, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet   'HDG INC 64511	20120110
        Try
            Return New SaldosBancariosDAM().ListarFlujoEstimado(dsCuentaEconomica, codOperacion, codTipoOperacion, tipoMovimiento, fechaInicio, fechaFin) 'HDG INC 64511	20120110
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function EliminarFlujoEstimado(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal fechaFlujo As Decimal, ByVal nroSecuencial As String, ByVal tipoMovimiento As String)
        Try
            Return New SaldosBancariosDAM().EliminarFlujoEstimado(dsCuentaEconomica, fechaFlujo, nroSecuencial, tipoMovimiento)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ActualizaSaldosBancarios(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal)
        Try
            Return New SaldosBancariosDAM().ActualizarSaldo(codigoPortafolio, fechaOperacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ActualizaSaldosBancariosbyFecha(ByVal fechaOperacion As Decimal, ByVal codigoPortafolio As String)
        Try
            Return New SaldosBancariosDAM().ActualizarSaldobyFecha(fechaOperacion, codigoPortafolio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarFlujoEstimado(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal fechaFlujo As Decimal, ByVal codOperacion As String, ByVal fechaDescargo As Decimal, ByVal importe As Decimal, ByVal descripcion As String, ByVal fechavencimiento As Decimal, ByVal codigoTipoCuenta As String)
        Try
            Return New SaldosBancariosDAM().InsertarFlujoEstimado(dsCuentaEconomica, fechaFlujo, codOperacion, fechaDescargo, importe, descripcion, fechavencimiento, codigoTipoCuenta)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarMovimientosNegociacionPorFiltro(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal FechaNegociacion As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarMovimientosNegociacionPorFiltro(dsCuentaEconomica, FechaNegociacion)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Insertar(ByVal dsSaldos As SaldosBancariosBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsSaldos, dataRequest}
        Try
            Dim daSaldos As New SaldosBancariosDAM
            daSaldos.Insertar(dsSaldos, dataRequest)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function CargarSaldosBancarios(ByVal dsArchivos As DataSet, ByVal dsDatos As DataSet, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsArchivos, dataRequest}
        Dim dsArchivosCargados As DataSet
        Try
            Dim daSaldos As New SaldosBancariosDAM
            Dim tableIndex As Integer = 0
            For Each row As DataRow In dsArchivos.Tables(0).Rows
                dsArchivosCargados = CargarArchivo(row("ArchivoCodigo"), row("ArchivoSeparador"), row("Param1"), dsDatos.Tables(tableIndex), fecha, dataRequest)
                tableIndex = tableIndex + 1
            Next
            RegistrarAuditora(parameters)
            Return dsArchivosCargados
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Private Function CargarArchivo(ByVal CodigoArchivo As String, ByVal separador As String, ByVal codigoBanco As String, ByVal dtDatos As DataTable, ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim oEstructura As DataSet = New ArchivoPlanoEstructuraDAM().Listar(CodigoArchivo)
        Dim dsResultado As New DataSet
        Dim longitudSC As Integer = ObtenerValor(oEstructura, "SaldoContable", "ColumnaLongitud")
        Dim longitudSD As Integer = ObtenerValor(oEstructura, "SaldoDisponible", "ColumnaLongitud")
        Dim longitudNC As Integer = ObtenerValor(oEstructura, "NumeroCuenta", "ColumnaLongitud")
        Dim LongitudP As Integer = ObtenerValor(oEstructura, "Portafolio", "ColumnaLongitud")

        Dim posInicialSC As Integer = ObtenerValor(oEstructura, "SaldoContable", "ColumnaPosicionInicial")
        Dim posInicialSD As Integer = ObtenerValor(oEstructura, "SaldoDisponible", "ColumnaPosicionInicial")
        Dim posInicialNC As Integer = ObtenerValor(oEstructura, "NumeroCuenta", "ColumnaPosicionInicial")
        Dim posInicialP As Integer = ObtenerValor(oEstructura, "Portafolio", "ColumnaPosicionInicial")

        Dim ordenSC As Integer = ObtenerValor(oEstructura, "SaldoContable", "ColumnaOrden")
        Dim ordenSD As Integer = ObtenerValor(oEstructura, "SaldoDisponible", "ColumnaOrden")
        Dim ordenNC As Integer = ObtenerValor(oEstructura, "NumeroCuenta", "ColumnaOrden")
        Dim ordenP As Integer = ObtenerValor(oEstructura, "Portafolio", "ColumnaOrden")

        Dim saldoContable As Decimal
        Dim saldoDisponible As Decimal
        Dim numeroCuenta As String
        Dim portafolio As String

        Dim decimalseparator As String = System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator
        Dim oSaldosBancarios As New SaldosBancariosDAM

        Dim archivoCargado As New DataTable
        archivoCargado.Columns.Add("Banco")
        archivoCargado.Columns.Add("NumeroCuenta")
        archivoCargado.Columns.Add("SaldoDisponible", System.Type.GetType("System.Decimal"))
        archivoCargado.Columns.Add("SaldoContable", System.Type.GetType("System.Decimal"))
        archivoCargado.Columns.Add("Portafolio")
        Dim archivoNoCargado As DataTable = archivoCargado.Copy
        Dim dsBancos As TercerosBE = New TercerosDAM().Listar(Nothing)
        For Each drDato As DataRow In dtDatos.Rows
            Dim sDato As String = drDato(0)
            If longitudSC = 0 Or longitudSD = 0 Then
                saldoContable = Decimal.Parse(sDato.Split(Char.Parse(separador))(ordenSC - 1).Replace(",", "").Replace(".", decimalseparator).Replace("""", ""))
                saldoDisponible = Decimal.Parse(sDato.Split(Char.Parse(separador))(ordenSD - 1).Replace(",", "").Replace(".", decimalseparator).Replace("""", ""))
                numeroCuenta = sDato.Split(Char.Parse(separador))(ordenNC - 1).Replace("""", "")
                portafolio = sDato.Split(Char.Parse(separador))(ordenP - 1).Replace("""", "")
            Else
                saldoContable = Decimal.Parse(sDato.Substring(posInicialSC - 1, longitudSC).Replace(".", decimalseparator))
                saldoDisponible = Decimal.Parse(sDato.Substring(posInicialSD - 1, longitudSD).Replace(".", decimalseparator))
                numeroCuenta = sDato.Substring(posInicialNC - 1, longitudNC)
                portafolio = sDato.Substring(posInicialP - 1, LongitudP)
            End If
            Try
                Dim dsSaldos As New SaldosBancariosBE
                Dim saldo As SaldosBancariosBE.SaldosBancariosRow = dsSaldos.SaldosBancarios.NewSaldosBancariosRow
                saldo.FechaOperacion = fecha
                saldo.CodigoTercero = codigoBanco
                saldo.CodigoPortafolioSBS = portafolio
                saldo.NumeroCuenta = numeroCuenta
                saldo.SaldoDisponibleInicial = saldoDisponible
                saldo.SaldoContableInicial = saldoContable
                saldo.TipoIngreso = "A"
                dsSaldos.SaldosBancarios.AddSaldosBancariosRow(saldo)
                oSaldosBancarios.Insertar(dsSaldos, dataRequest)
                Dim banco As String = dsBancos.Terceros.Select("EntidadFinanciera='" & codigoBanco & "'")(0)("Descripcion")
                Dim row As Object() = {banco, numeroCuenta, saldoDisponible, saldoContable, portafolio}
                archivoCargado.Rows.Add(row)
            Catch ex As Exception
                Dim banco As String = dsBancos.Terceros.Select("EntidadFinanciera='" & codigoBanco & "'")(0)("Descripcion")
                Dim row As Object() = {banco, numeroCuenta, saldoDisponible, saldoContable, portafolio}
                archivoNoCargado.Rows.Add(row)
            End Try
        Next
        dsResultado.Tables.Add(archivoCargado)
        dsResultado.Tables.Add(archivoNoCargado)
        Return dsResultado
    End Function

    Private Function ObtenerValor(ByVal oEstructura As DataSet, ByVal columna As String, ByVal atributo As String) As String
        For Each columnaArchivo As DataRow In oEstructura.Tables(0).Rows
            If columnaArchivo("columnanombre") = columna Then
                Return columnaArchivo(atributo)
            End If
        Next
    End Function

    Public Function EliminarMovimientoNegociacion(ByVal dsMontoNegociado As MontoNegociadoBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsMontoNegociado, dataRequest}
        Dim dsArchivosCargados As New DataSet
        Try
            Dim daSaldos As New SaldosBancariosDAM
            RegistrarAuditora(parameters)
            daSaldos.EliminarMovimientoNegociacion(dsMontoNegociado, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function InsertarMovimientoNegociacion(ByVal dsMontoNegociado As MontoNegociadoBE, ByVal dataRequest As DataSet)
        Dim codigoEjecucion As String = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsMontoNegociado, dataRequest}
        Dim dsArchivosCargados As New DataSet
        Try
            Dim daSaldos As New SaldosBancariosDAM
            RegistrarAuditora(parameters)
            daSaldos.InsertarMovimientoNegociacion(dsMontoNegociado, dataRequest)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function DetalleSaldosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().DetalleSaldosBancarios(dsCuentaEconomica)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function SeleccionarMovimientosNegociacionOnLine(ByVal codigoPortafolioSBS As String, ByVal codigoMercado As String, ByVal FechaNegociacion As Decimal, ByVal Imprimir As String, ByVal dataRequest As DataSet) As DataSet

        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoPortafolioSBS, FechaNegociacion, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarMovimientosNegociacionOnLine(codigoPortafolioSBS, codigoMercado, FechaNegociacion, Imprimir)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    'HDG 20130719
    Public Function SeleccionarMovimientosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dsCuentaEconomica, dataRequest}
        Try
            Dim dsSaldos As DataSet = New SaldosBancariosDAM().SeleccionarMovimientosBancarios(dsCuentaEconomica)
            RegistrarAuditora(parameters)
            Return dsSaldos
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
End Class
