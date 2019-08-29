Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class SaldosBancariosDAM
    Public Sub New()
    End Sub
    Public Sub Insertar(ByVal dsSaldosBancarios As SaldosBancariosBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim saldos As SaldosBancariosBE.SaldosBancariosRow = dsSaldosBancarios.SaldosBancarios.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Insertar")
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, saldos.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, saldos.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_SaldoContable", DbType.Decimal, saldos.SaldoContableInicial)
            db.AddInParameter(dbCommand, "@p_SaldoDisponible", DbType.Decimal, saldos.SaldoDisponibleInicial)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, saldos.FechaOperacion)
            db.AddInParameter(dbCommand, "@p_TipoIngreso", DbType.String, saldos.TipoIngreso)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    ''' <summary>
    ''' Selecciona un solo expediente de SaldosBancarios tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoSaldo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoSaldo", DbType.String, codigoSaldo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Seleccionar = ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de SaldosBancarios tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoMoneda"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Selecciona expedientes de SaldosBancarios tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorCodigoPortafolio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorCodigoTerceroSBS")
            db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorNumeroCuenta(ByVal codigoPortafolio As String, ByVal numeroCuenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorNumeroCuenta")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_numeroCuenta", DbType.String, numeroCuenta)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, drCuentaEconomica.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ValidarSaldosBancariosNegativos(CodigoPortafolioSBS As String, FechaOperacion As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_ValidaSaldosNegativos")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro2(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorFiltro2")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, drCuentaEconomica.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarPorFiltro2 = ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro3(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_SeleccionarPorFiltro3")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, drCuentaEconomica.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarPorFiltro3 = ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro4(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet 'Agregado por LC 22082008 Ejecutar Proc Formato Grilla
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancariosGrilla_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, drCuentaEconomica.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                SeleccionarPorFiltro4 = ds
            End Using
        End Using
    End Function
    Public Function Seleccionar(ByVal codigoPortafolio As String, ByVal numeroCuenta As String, ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Seleccionar")
            db.AddInParameter(dbCommand, "@p_numeroCuenta", DbType.String, numeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Seleccionar = ds
            End Using
        End Using
    End Function
    Public Function SeleccionarFlujoEstimadoPorFiltro(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal FechaInicial As Decimal, ByVal FechaFin As Decimal, ByVal Periodo As String, ByVal PorDivisas As String) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FlujoEstimado_Generar2")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaVencIni", DbType.Decimal, FechaInicial)
            db.AddInParameter(dbCommand, "@p_FechaVencFin", DbType.Decimal, FechaFin)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_Busqueda", DbType.String, Periodo)
            db.AddInParameter(dbCommand, "@p_PorDivisas", DbType.String, PorDivisas)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarFlujoEstimadoPorFiltro2(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal FechaInicial As Decimal, ByVal FechaFin As Decimal, ByVal Periodo As String, ByVal PorDivisas As String) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FlujoEstimado_Generar3")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaVencIni", DbType.Decimal, FechaInicial)
            db.AddInParameter(dbCommand, "@p_FechaVencFin", DbType.Decimal, FechaFin)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_Busqueda", DbType.String, Periodo)
            db.AddInParameter(dbCommand, "@p_PorDivisas", DbType.String, PorDivisas)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function ListarFlujoEstimado(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal codOperacion As String, ByVal codTipoOperacion As String, ByVal tipoMovimiento As String, ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal) As DataSet   'HDG INC 64511	20120110
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FlujoEstimado_Listar")
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoOperacion", DbType.String, codTipoOperacion)
            db.AddInParameter(dbCommand, "@p_TipoMovimiento", DbType.String, tipoMovimiento)
            db.AddInParameter(dbCommand, "@p_FechaVencIni", DbType.Decimal, fechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaVencFin", DbType.Decimal, fechaFin)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                ListarFlujoEstimado = ds
            End Using
        End Using
    End Function
    Public Function EliminarFlujoEstimado(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal fechaFlujo As Decimal, ByVal nroSecuencial As String, ByVal tipoMovimiento As String)
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FlujoEstimado_Eliminar")
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            db.AddInParameter(dbCommand, "@P_NumeroSecuencial", DbType.String, nroSecuencial)
            db.AddInParameter(dbCommand, "@p_FechaFlujo", DbType.Decimal, fechaFlujo)
            db.ExecuteScalar(dbCommand)
        End Using
    End Function
    Public Function InsertarFlujoEstimado(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal fechaFlujo As Decimal, ByVal codOperacion As String, ByVal fechaDescargo As Decimal, ByVal importe As Decimal, ByVal descripcion As String, ByVal fechaVencimiento As Decimal, ByVal codigoTipoCuenta As String)
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("FlujoEstimado_Insertar")
            db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codOperacion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoCuenta", DbType.String, codigoTipoCuenta)
            db.AddInParameter(dbCommand, "@p_FechaDescargo", DbType.Decimal, fechaDescargo)
            db.AddInParameter(dbCommand, "@p_FechaFlujo", DbType.Decimal, fechaFlujo)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, importe)
            db.AddInParameter(dbCommand, "@p_DescripcionOperacion", DbType.String, descripcion)
            db.AddInParameter(dbCommand, "@p_FechaVencimiento", DbType.Decimal, fechaVencimiento)
            db.ExecuteScalar(dbCommand)
        End Using
    End Function
    Public Function SeleccionarMovimientosNegociacionPorFiltro(ByVal dsCuentaEconomica As CuentaEconomicaBE, ByVal FechaNegociacion As Decimal) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("MovimientosNegociacion_Generar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_FechaNegociacion", DbType.Decimal, FechaNegociacion)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Listar")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Listar = ds
            End Using
        End Using
    End Function

    Public Function Modificar(ByVal codigoSaldo As String, ByVal codigoMoneda As String, ByVal codigoPortafolio As String, ByVal numeroCuenta As String, ByVal saldoDisponibleInicial As Decimal, ByVal saldoContableInicial As Decimal, ByVal ingresosEstimados As Decimal, ByVal egresosEstimados As Decimal, ByVal saldoDisponibleFinalConfirmado As Decimal, ByVal fechaOperacion As Decimal, ByVal saldoContableFinalEstimado As Decimal, ByVal usuarioCreacion As String, ByVal fechaModificacion As Decimal, ByVal fechaCreacion As Decimal, ByVal hOST As String, ByVal horaCreacion As String, ByVal codigoTerceroSBS As Decimal, ByVal usuarioModificacion As String, ByVal horaModificacion As String, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Modificar")
            db.AddInParameter(dbCommand, "@p_CodigoSaldo", DbType.String, codigoSaldo)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
            db.AddInParameter(dbCommand, "@p_SaldoDisponibleInicial", DbType.Decimal, saldoDisponibleInicial)
            db.AddInParameter(dbCommand, "@p_SaldoContableInicial", DbType.Decimal, saldoContableInicial)
            db.AddInParameter(dbCommand, "@p_IngresosEstimados", DbType.Decimal, ingresosEstimados)
            db.AddInParameter(dbCommand, "@p_EgresosEstimados", DbType.Decimal, egresosEstimados)
            db.AddInParameter(dbCommand, "@p_SaldoDisponibleFinalConfirmado", DbType.Decimal, saldoDisponibleFinalConfirmado)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_SaldoContableFinalEstimado", DbType.Decimal, saldoContableFinalEstimado)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
            db.AddInParameter(dbCommand, "@p_HOST", DbType.String, hOST)
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, horaCreacion)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, horaModificacion)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function

    ''' <summary>
    ''' Elimina un expediente de SaldosBancarios table por una llave primaria compuesta.
    ''' <summary>
    Public Function EliminarFlujoEstimado(ByVal codigoPortafolio As String, ByVal numeroCuenta As String, ByVal DataRequest As DataSet) As Boolean
        EliminarFlujoEstimado = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(DataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(DataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(DataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(DataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            EliminarFlujoEstimado = True
        End Using
    End Function
    ''' <summary>
    ''' Actualiza SaldosBancarios segun la fecha negociacion.
    ''' <summary>
    Public Function ActualizarSaldo(ByVal codigoPortafolio As String, ByVal fechaOperacion As Decimal) As Boolean
        ActualizarSaldo = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Actualizar2")
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@FechaOperacion", DbType.Decimal, fechaOperacion)
            db.ExecuteNonQuery(dbCommand)
            ActualizarSaldo = True
        End Using
    End Function
    ''' <summary>
    ''' Actualiza SaldosBancarios segun la fecha negociacion.
    ''' <summary>
    Public Function ActualizarSaldobyFecha(ByVal fechaOperacion As Decimal, ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Actualizar")
            db.AddInParameter(dbCommand, "@FechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    Public Function IngresarFlujoEstimado(ByVal codigoPortafolio As String, ByVal numeroCuenta As String, ByVal importe As Decimal, ByVal DataRequest As DataSet) As Boolean
        IngresarFlujoEstimado = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, numeroCuenta)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.String, importe)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(DataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(DataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(DataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(DataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            IngresarFlujoEstimado = True
        End Using
    End Function

    ''' <summary>
    ''' Elimina un expediente de SaldosBancarios table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_EliminarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    ''' <summary>
    ''' Elimina un expediente de SaldosBancarios table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean
        EliminarPorCodigoPortafolio = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_EliminarPorCodigoPortafolio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
            EliminarPorCodigoPortafolio = True
        End Using
    End Function

    ''' <summary>
    ''' Elimina un expediente de SaldosBancarios table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal) As Boolean
        EliminarPorCodigoTerceroSBS = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SaldosBancarios_EliminarPorCodigoTerceroSBS")
            db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
            db.ExecuteNonQuery(dbCommand)
            EliminarPorCodigoTerceroSBS = True
        End Using
    End Function
    Public Sub InsertarMovimientoNegociacion(ByVal dsMontoNegociado As MontoNegociadoBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim movNeg As MontoNegociadoBE.MontoNegociadoRow = dsMontoNegociado.MontoNegociado.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("MovimientosNegociacion_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, movNeg.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, movNeg.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, movNeg.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_FechaNegociacion", DbType.Decimal, movNeg.FechaNegociacion)
            db.AddInParameter(dbCommand, "@P_codigoTipoOperacion", DbType.String, movNeg.CodigoTipoOperacion)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, movNeg.Descripcion)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, movNeg.Importe)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Sub EliminarMovimientoNegociacion(ByVal dsMontoNegociado As MontoNegociadoBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim movNeg As MontoNegociadoBE.MontoNegociadoRow = dsMontoNegociado.MontoNegociado.Rows(0)
        Using dbCommand As DbCommand = db.GetStoredProcCommand("MovimientosNegociacion_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, movNeg.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroSecuencial", DbType.String, movNeg.NumeroSecuencial)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, movNeg.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, movNeg.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_FechaNegociacion", DbType.Decimal, movNeg.FechaNegociacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    Public Function DetalleSaldosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetalleCuenta_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, drCuentaEconomica.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                DetalleSaldosBancarios = ds
            End Using
        End Using
    End Function

    Public Function SeleccionarMovimientosNegociacionOnLine(ByVal codigoPortafolioSBS As String, ByVal codigoMercado As String, ByVal FechaNegociacion As Decimal, ByVal Imprimir As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Reporte_MovimientoNegociacionOnLine")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.String, FechaNegociacion)
            db.AddInParameter(dbCommand, "@p_Imprimir", DbType.String, Imprimir)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarMovimientosBancarios(ByVal dsCuentaEconomica As CuentaEconomicaBE) As DataSet
        Dim drCuentaEconomica As CuentaEconomicaBE.CuentaEconomicaRow = dsCuentaEconomica.CuentaEconomica.Rows(0)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_pro_MovimientosCajaBancos")
            dbCommand.CommandTimeout = 1260
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, drCuentaEconomica.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, drCuentaEconomica.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, drCuentaEconomica.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, drCuentaEconomica.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, drCuentaEconomica.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, drCuentaEconomica.FechaCreacion)
            db.AddInParameter(dbCommand, "@nvcCodigoMercado", DbType.String, drCuentaEconomica.CodigoMercado)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
End Class

