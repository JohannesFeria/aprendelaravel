Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities
''' <summary>
''' Clase para el acceso de los datos para Portafolio tabla.
''' </summary>
Public Class PortafolioDAM
    Private oPortafolioRow As PortafolioBE.PortafolioRow

    Public Sub New()
    End Sub

    'OT11237 - 15/02/2018 - Ian Pastor M.
    'Descripción: Refactorizar código
    Public Function Eliminar_PorcentajeSeries(ByVal Portafolio As String, ByVal FechaProceso As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PorcentajeSeries_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    Public Function Insertar_PorcentajeSeries(ByVal Portafolio As String, ByVal CodigoSerie As String, _
                                              ByVal FechaProceso As Decimal, ByVal Porcentaje As Decimal, ByVal dataRequest As DataSet, ByVal ValoresCierre As Decimal, ByVal CuotasCierre As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PorcentajeSeries_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, CodigoSerie)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
            db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, Porcentaje)
            db.AddInParameter(dbCommand, "@p_ValoresCierre", DbType.Decimal, ValoresCierre)
            db.AddInParameter(dbCommand, "@p_CuotasCierre", DbType.Decimal, CuotasCierre)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    'OT11237 - Fin
    Public Function Modificar_PorcentajeSeries(ByVal Portafolio As String, ByVal CodigoSerie As String, _
                                              ByVal FechaProceso As Decimal, ByVal Porcentaje As Decimal, ByVal dataRequest As DataSet) As Boolean
        Modificar_PorcentajeSeries = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PorcentajeSeries_Modificar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, Portafolio)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, CodigoSerie)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, FechaProceso)
            db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, Porcentaje)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Modificar_PorcentajeSeries = True
        End Using
    End Function
    Public Function Insertar(ByVal ob As PortafolioBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Insertar")
            oPortafolioRow = CType(ob.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oPortafolioRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oPortafolioRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_FechaConstitucion", DbType.Decimal, oPortafolioRow.FechaConstitucion)
            If oPortafolioRow.CodigoTipoValorizacion = "" Then
                db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, oPortafolioRow.CodigoTipoValorizacion)
            End If
            db.AddInParameter(dbCommand, "@p_EsMultiPortafolio", DbType.String, oPortafolioRow.EsMultiPortafolio)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPortafolioRow.Situacion)
            db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oPortafolioRow.CodigoNegocio)
            db.AddInParameter(dbCommand, "@p_FechaTermino", DbType.Decimal, oPortafolioRow.FechaTermino)
            db.AddInParameter(dbCommand, "@p_FechaAperturaContable", DbType.Decimal, oPortafolioRow.FechaAperturaContable)
            If oPortafolioRow.CodigoMoneda = "" Then
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, DBNull.Value)
            Else
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oPortafolioRow.CodigoMoneda)
            End If
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_InterfazContable", DbType.String, oPortafolioRow.InterfazContable)
            db.AddInParameter(dbCommand, "@p_TipoCartera", DbType.String, oPortafolioRow.TipoCartera)
            db.AddInParameter(dbCommand, "@p_CodContabilidadFondo", DbType.String, oPortafolioRow.CodContabilidadFondo)
            db.AddInParameter(dbCommand, "@p_PorcentajeComision", DbType.Decimal, oPortafolioRow.PorcentajeComision)
            db.AddInParameter(dbCommand, "@p_CodigoFondosMutuos", DbType.String, oPortafolioRow.CodigoFondosMutuos)
            db.AddInParameter(dbCommand, "@p_ValorInicialFondo", DbType.Decimal, oPortafolioRow.ValorInicialFondo)
            db.AddInParameter(dbCommand, "@p_IndicadorFondo", DbType.String, oPortafolioRow.IndicadorFondo)
            db.AddInParameter(dbCommand, "@p_NumeroCuotaPreCierre", DbType.Decimal, oPortafolioRow.NumeroCuotaPreCierre)
            db.AddInParameter(dbCommand, "@p_PorSerie", DbType.String, oPortafolioRow.PorSerie)
            db.AddInParameter(dbCommand, "@p_NombreCompleto", DbType.String, oPortafolioRow.NombreCompleto)
            db.AddInParameter(dbCommand, "@FlagComisionVariable", DbType.Boolean, oPortafolioRow.FlagComisionVariable)
            db.AddInParameter(dbCommand, "@MontoSuscripcionInicial", DbType.Decimal, oPortafolioRow.MontoSuscripcionInicial)
            db.AddInParameter(dbCommand, "@TopeValorCuota", DbType.Decimal, oPortafolioRow.TopeValorCuota)
            db.AddInParameter(dbCommand, "@FlagComisionSuscripInicial", DbType.Boolean, oPortafolioRow.FlagComisionSuscripInicial)
            db.AddInParameter(dbCommand, "@BDConta", DbType.String, oPortafolioRow.BDConta)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSisOpe", DbType.String, oPortafolioRow.CodigoPortafolioSisOpe)
            db.AddInParameter(dbCommand, "@p_RUC", DbType.String, oPortafolioRow.RUCPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oPortafolioRow.CodigoRenta)     '' MPENAL - 09/09/2016
            db.AddInParameter(dbCommand, "@p_TipoCalculoValorCuota", DbType.String, oPortafolioRow.TipoCalculoValorCuota)
            db.AddInParameter(dbCommand, "@p_FechaCajaOperaciones", DbType.Decimal, oPortafolioRow.FechaCajaOperaciones)
            db.AddInParameter(dbCommand, "@p_ValoracionMensual", DbType.String, oPortafolioRow.ValoracionMensual)
            db.AddInParameter(dbCommand, "@p_CuotasLiberadas", DbType.String, oPortafolioRow.CuotasLiberadas) 'OT10916
            db.AddInParameter(dbCommand, "@p_CPPadreSisOpe", DbType.String, oPortafolioRow.CPPadreSisOpe)
            db.AddInParameter(dbCommand, "@p_VectorPrecioVal", DbType.String, oPortafolioRow.VectorPrecioVal)
            '  INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
            db.AddInParameter(dbCommand, "@p_FlagAumentoCapital", DbType.Boolean, oPortafolioRow.FlagAumentoCapital)
            '  FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
            db.AddInParameter(dbCommand, "@p_TipoNegocio", DbType.String, oPortafolioRow.TipoNegocio)
            db.AddInParameter(dbCommand, "@p_FondoCliente", DbType.String, oPortafolioRow.FondoCliente)
            db.AddInParameter(dbCommand, "@p_CodigoTerceroCliente", DbType.String, oPortafolioRow.CodigoTerceroCliente)
            db.AddInParameter(dbCommand, "@p_TipoComision", DbType.String, oPortafolioRow.TipoComision)
            db.AddInParameter(dbCommand, "@p_Consolidado", DbType.Int16, oPortafolioRow.Consolidado)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function
    'OT11237 - 22/02/2018 - Ian Pastor M.
    'Descripción: Refactorizar código
    Public Sub InsertarSeries(ByVal CodigoPortafolio As String, ByVal dtDetallePortafolio As DataRow)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioSerie_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoSerie", DbType.String, dtDetallePortafolio("CodigoSerie"))
            db.AddInParameter(dbCommand, "@p_NombreSerie", DbType.String, dtDetallePortafolio("NombreSerie"))
            db.AddInParameter(dbCommand, "@p_Porcentaje", DbType.Decimal, dtDetallePortafolio("Porcentaje"))
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSO", DbType.String, dtDetallePortafolio("CodigoPortafolioSO"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT11237 - Fin
    Public Sub EliminarSerie(ByVal CodigoPortafolio As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioSerie_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ValidarValorizacion(ByVal CodigoPortafolio As String, ByVal FechaValorizacion As Decimal) As Boolean
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Validar_Valorizacion")
            db.AddInParameter(dbCommand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@P_FechaVal", DbType.Decimal, FechaValorizacion)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Dim Valor As Integer = ds.Tables(0).Rows(0)(0)
                Return CType(Valor, Boolean)
            End Using
        End Using
    End Function
    Public Function Portafolio_Series_Cuotas(ByVal CodigoPortafolio As String, ByVal FechaValorizacion As Decimal) As DataTable
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_Series_Cuotas")
            bd.AddInParameter(dbcomand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            bd.AddInParameter(dbcomand, "@P_FechaVal", DbType.Decimal, FechaValorizacion)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11143 - 05/03/2018 - Ian Pastor M.
    'Descripción: Obtener las series cuotas del fondo FIRBI
    Public Function Portafolio_Series_CuotasFirbi(ByVal CodigoPortafolio As String, ByVal FechaValorizacion As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Series_CuotasFirbi")
            db.AddInParameter(DbCommand, "@P_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(DbCommand, "@P_FechaVal", DbType.Decimal, FechaValorizacion)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11143 - Fin
    Public Function ListarSeries(ByVal CodigoPortafolio As String) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_lis_Series")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function FechaMaximaPortafolio() As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_FechaMaximaPortafolio")
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function InsertarDetalle(ByVal CodigoPortafolio As String, ByVal dtDetallePortafolio As DataTable, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetallePortafolio_Insertar")
            Dim codigoMoneda As String = ""
            Dim codigoCC As String = ""
            For Each filaLinea As DataRow In dtDetallePortafolio.Rows
                Dim oMonedaDAM As New MonedaDAM
                Dim oTablaMoneda As New DataTable
                Dim oClaseCuentaDAM As New ClaseCuentaDAM
                Dim oTablaCC As New DataTable
                Dim i As Integer
                oTablaMoneda = oMonedaDAM.Listar().Tables(0)
                For i = 0 To oTablaMoneda.Rows.Count - 1
                    If CStr(oTablaMoneda.Rows(i)(2)).ToUpper = filaLinea("CodigoMoneda").ToString().Trim().ToUpper Then
                        codigoMoneda = CStr(oTablaMoneda.Rows(i)(0))
                        Exit For
                    End If
                Next
                oTablaCC = oClaseCuentaDAM.Listar().Tables(0)
                For i = 0 To oTablaCC.Rows.Count - 1
                    If CStr(oTablaCC.Rows(i)(2)).ToUpper = filaLinea("CodigoClaseCuenta").ToString().Trim().ToUpper Then
                        codigoCC = CStr(oTablaCC.Rows(i)(0))
                        Exit For
                    End If
                Next
                db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
                db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoCC)
                db.AddInParameter(dbCommand, "@p_NumeroCuentaBancaria", DbType.String, filaLinea("NumeroCuentaBancaria").ToString().Trim())
                db.AddInParameter(dbCommand, "@p_NumeroCuentaInterbancario", DbType.String, filaLinea("NumeroCuentaInterbancario").ToString().Trim())
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, filaLinea("Situacion").ToString().Trim().Substring(0, 1))
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            Next
        End Using
        Return Codigo
    End Function
    Public Function Seleccionar(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As PortafolioBE
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Using objeto As New PortafolioBE
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Seleccionar")
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Seleccionar = objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarDetalle(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As DetallePortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetallePortafolio_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Using objeto As New DetallePortafolioBE
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarDetallePorFiltro(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal CodigoTercero As String, ByVal CodigoMoneda As String, ByVal CodigoMercado As String) As DetallePortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetallePortafolio_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
            Using objeto As New DetallePortafolioBE
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarBancoPorNumeroCuenta(ByVal NumeroCuenta As String, ByVal codigoPortafolio As String) As TercerosBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Terceros_SeleccionarPorNumeroCuenta")
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Using objeto As New TercerosBE
                db.LoadDataSet(dbCommand, objeto, "Terceros")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String) As PortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Using oPortafolioBE As New PortafolioBE
                db.LoadDataSet(dbCommand, oPortafolioBE, "Portafolio")
                Return oPortafolioBE
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoTipoValorizacion(ByVal codigoTipoValorizacion As String) As PortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SeleccionarPorCodigoTipoValorizacion")
            db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, codigoTipoValorizacion)
            Using objeto As New PortafolioBE
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoNegocio(ByVal codigoNegocio As String) As PortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SeleccionarPorCodigoNegocio")
            db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
            Using objeto As New PortafolioBE
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoMoneda(ByVal codigoMoneda As String) As PortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SeleccionarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            Using objeto As New PortafolioBE
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function ObtenerDatosPortafolio(Optional ByVal situacion As String = "") As PortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ObtenerDatosPortafolio_Listar")
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Using objeto As New PortafolioBE
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function Listar(Optional ByVal situacion As String = "", Optional ByVal CodigoNegocio As String = "") As PortafolioBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Listar")
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, CodigoNegocio)
            Dim objeto As New PortafolioBE
            db.LoadDataSet(dbCommand, objeto, "Portafolio")
            Return objeto
        End Using
    End Function
    Public Function PortafolioCodigoListar(ByVal portafolio As String, Optional ByVal s_Parametro As String = "S", Optional ByVal porSerie As String = "", Optional ByVal estado As String = "") As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_listarCodigoPortafolio")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_Administra", DbType.String, s_Parametro)
            bd.AddInParameter(dbcomand, "@p_PorSerie", DbType.String, porSerie)
            bd.AddInParameter(dbcomand, "@p_Estado", DbType.String, estado)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11008 - 22/01/2018 - Ian Pastor M.
    'Descripción: Optimizar código
    Public Function PortafolioCodigoListar_ValoresSerie(ByVal portafolio As String) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("ParametrosGenerales_Listar_ValoresSerie")
            bd.AddInParameter(dbcomand, "@P_CodigoPortafolioSBS", DbType.String, portafolio)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function Modificar(ByVal ob As PortafolioBE, ByVal dataRequest As DataSet) As Boolean
        Using ob
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Modificar")
                oPortafolioRow = CType(ob.Portafolio.Rows(0), PortafolioBE.PortafolioRow)
                db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oPortafolioRow.CodigoPortafolioSBS)
                db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oPortafolioRow.Descripcion)
                db.AddInParameter(dbCommand, "@p_FechaConstitucion", DbType.Decimal, oPortafolioRow.FechaConstitucion)
                db.AddInParameter(dbCommand, "@p_FechaAperturaContable", DbType.Decimal, oPortafolioRow.FechaAperturaContable) '20090226 LETV
                If oPortafolioRow.CodigoTipoValorizacion = "" Then
                    db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, oPortafolioRow.CodigoTipoValorizacion)
                End If
                db.AddInParameter(dbCommand, "@p_FechaTermino", DbType.Decimal, oPortafolioRow.FechaTermino)
                If oPortafolioRow.CodigoMoneda = "" Then
                    db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, DBNull.Value)
                Else
                    db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oPortafolioRow.CodigoMoneda)
                End If
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPortafolioRow.Situacion)
                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, oPortafolioRow.CodigoNegocio)
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.AddInParameter(dbCommand, "@p_InterfazContable", DbType.String, oPortafolioRow.InterfazContable)
                db.AddInParameter(dbCommand, "@p_TipoCartera", DbType.String, oPortafolioRow.TipoCartera)
                db.AddInParameter(dbCommand, "@p_CodContabilidadFondo", DbType.String, oPortafolioRow.CodContabilidadFondo)
                db.AddInParameter(dbCommand, "@p_PorcentajeComision", DbType.Decimal, oPortafolioRow.PorcentajeComision)
                db.AddInParameter(dbCommand, "@p_CodigoFondosMutuos", DbType.String, oPortafolioRow.CodigoFondosMutuos)
                db.AddInParameter(dbCommand, "@p_ValorInicialFondo", DbType.Decimal, oPortafolioRow.ValorInicialFondo)
                db.AddInParameter(dbCommand, "@p_IndicadorFondo", DbType.String, oPortafolioRow.IndicadorFondo)
                db.AddInParameter(dbCommand, "@p_NumeroCuotaPreCierre", DbType.Decimal, oPortafolioRow.NumeroCuotaPreCierre)
                db.AddInParameter(dbCommand, "@p_PorSerie", DbType.String, oPortafolioRow.PorSerie)
                db.AddInParameter(dbCommand, "@p_NombreCompleto", DbType.String, oPortafolioRow.NombreCompleto)
                db.AddInParameter(dbCommand, "@FlagComisionVariable", DbType.Boolean, oPortafolioRow.FlagComisionVariable)
                db.AddInParameter(dbCommand, "@MontoSuscripcionInicial", DbType.Decimal, oPortafolioRow.MontoSuscripcionInicial)
                db.AddInParameter(dbCommand, "@TopeValorCuota", DbType.Decimal, oPortafolioRow.TopeValorCuota)
                db.AddInParameter(dbCommand, "@FlagComisionSuscripInicial", DbType.Boolean, oPortafolioRow.FlagComisionSuscripInicial)
                db.AddInParameter(dbCommand, "@BDConta", DbType.String, oPortafolioRow.BDConta)
                db.AddInParameter(dbCommand, "@CodigoPortafolioSisOpe", DbType.String, oPortafolioRow.CodigoPortafolioSisOpe)
                db.AddInParameter(dbCommand, "@p_RUC", DbType.String, oPortafolioRow.RUCPortafolio)
                db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oPortafolioRow.CodigoRenta)     '' MPENAL - 09/09/2016
                db.AddInParameter(dbCommand, "@p_TipoCalculoValorCuota", DbType.String, oPortafolioRow.TipoCalculoValorCuota)
                db.AddInParameter(dbCommand, "@p_ValoracionMensual", DbType.String, oPortafolioRow.ValoracionMensual)
                db.AddInParameter(dbCommand, "@p_CuotasLiberadas", DbType.String, oPortafolioRow.CuotasLiberadas) 'OT10916
                db.AddInParameter(dbCommand, "@p_CPPadreSisOpe", DbType.String, oPortafolioRow.CPPadreSisOpe)
                db.AddInParameter(dbCommand, "@p_VectorPrecioVal", DbType.String, oPortafolioRow.VectorPrecioVal)
                '  INICIO | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
                db.AddInParameter(dbCommand, "@p_FlagAumentoCapital", DbType.Boolean, oPortafolioRow.FlagAumentoCapital)
                '  FIN | ZOLUXIONES | rcolonia | Proy - Aumento de Capital - | Creación de nuevo Campo Aumento de Portafolio | 17092018
                db.AddInParameter(dbCommand, "@p_TipoNegocio", DbType.String, oPortafolioRow.TipoNegocio)
                db.AddInParameter(dbCommand, "@p_FondoCliente", DbType.String, oPortafolioRow.FondoCliente)
                db.AddInParameter(dbCommand, "@p_CodigoTerceroCliente", DbType.String, oPortafolioRow.CodigoTerceroCliente)
                db.AddInParameter(dbCommand, "@p_TipoComision", DbType.String, oPortafolioRow.TipoComision)
                db.AddInParameter(dbCommand, "@p_Consolidado", DbType.Int16, oPortafolioRow.Consolidado) 'Fin - Creacion nuevo campo OT12012                db.ExecuteNonQuery(dbCommand)
                db.ExecuteNonQuery(dbCommand)
                Return True
            End Using
        End Using
    End Function
    Public Function ModificarDetalle(ByVal CodigoPortafolio As String, ByVal dtDetallePortafolio As DataTable, ByVal dataRequest As DataSet) As Boolean
        Dim codigoDetalle As String
        Dim codigoMoneda As String
        Dim codigoClaseCuenta As String
        Dim numeroCuentaBanc As String
        Dim numeroCuentaInter As String
        Dim situacion As String
        Dim dtt As DataTable
        dtt = dtDetallePortafolio
        If (dtt.Rows.Count > 0) Then
            For Each filaLinea As DataRow In dtt.Rows
                codigoDetalle = filaLinea("CodigoDetallePortafolio").ToString().Trim()
                codigoMoneda = filaLinea("CodigoMoneda").ToString().Trim()
                codigoClaseCuenta = filaLinea("CodigoClaseCuenta").ToString().Trim()
                numeroCuentaBanc = filaLinea("NumeroCuentaBancaria").ToString().Trim()
                numeroCuentaInter = filaLinea("NumeroCuentaInterbancario").ToString().Trim()
                situacion = filaLinea("Situacion").ToString().Trim()
                If codigoDetalle <> "-1" Then
                    ModificarDetalleAux(CodigoPortafolio, codigoDetalle, codigoMoneda, codigoClaseCuenta, numeroCuentaBanc, numeroCuentaInter, situacion, dataRequest)
                Else

                    InsertarDetalleAux(CodigoPortafolio, codigoMoneda, codigoClaseCuenta, numeroCuentaBanc, numeroCuentaInter, situacion, dataRequest)
                End If
            Next
        End If
    End Function
    Public Function Eliminar(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function
    ''' <summary>
    ''' Elimina el detalle de DetallePortafolio table por una llave primaria compuesta.
    ''' <summary>
    Public Function EliminarDetalle(ByVal codigoPortafolio As String, ByVal dataRequest As DataSet) As Boolean
        EliminarDetalle = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetallePortafolio_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
            EliminarDetalle = True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de Portafolio table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoTipoValorizacion(ByVal codigoTipoValorizacion As String) As Boolean
        EliminarPorCodigoTipoValorizacion = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_EliminarPorCodigoTipoValorizacion")
            db.AddInParameter(dbCommand, "@p_CodigoTipoValorizacion", DbType.String, codigoTipoValorizacion)
            db.ExecuteNonQuery(dbCommand)
            EliminarPorCodigoTipoValorizacion = True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de Portafolio table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoNegocio(ByVal codigoNegocio As String) As Boolean
        EliminarPorCodigoNegocio = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_EliminarPorCodigoNegocio")
            db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
            db.ExecuteNonQuery(dbCommand)
            EliminarPorCodigoNegocio = True
        End Using
    End Function
    ''' <summary>
    ''' Elimina un expediente de Portafolio table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean
        EliminarPorCodigoMoneda = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_EliminarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.ExecuteNonQuery(dbCommand)
            EliminarPorCodigoMoneda = True
        End Using
    End Function
    Public Sub InsertarDetalleAux(ByVal CodigoPortafolio As String, ByVal codigoMoneda As String, ByVal codigoClaseCuenta As String, ByVal numeroCuentaBanc As String, ByVal numeroCuentaInter As String, ByVal situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetallePortafolio_Insertar")
            Dim oMonedaDAM As New MonedaDAM
            Dim oTablaMoneda As New DataTable
            Dim oClaseCuentaDAM As New ClaseCuentaDAM
            Dim oTablaCC As New DataTable
            Dim i As Integer
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            oTablaMoneda = oMonedaDAM.Listar().Tables(0)
            For i = 0 To oTablaMoneda.Rows.Count - 1
                If CStr(oTablaMoneda.Rows(i)(2)).ToUpper = codigoMoneda.ToUpper Then
                    codigoMoneda = CStr(oTablaMoneda.Rows(i)(0))
                    Exit For
                End If
            Next
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            oTablaCC = oClaseCuentaDAM.Listar().Tables(0)
            For i = 0 To oTablaCC.Rows.Count - 1
                If CStr(oTablaCC.Rows(i)(2)).ToUpper = codigoClaseCuenta.ToUpper Then
                    codigoClaseCuenta = CStr(oTablaCC.Rows(i)(0))
                    Exit For
                End If
            Next
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaBancaria", DbType.String, numeroCuentaBanc)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaInterbancario", DbType.String, numeroCuentaInter)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion.Substring(0, 1))
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub ModificarDetalleAux(ByVal CodigoPortafolio As String, ByVal codigoDetalle As String, ByVal codigoMoneda As String, ByVal codigoClaseCuenta As String, ByVal numeroCuentaBanc As String, ByVal numeroCuentaInter As String, ByVal situacion As String, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("DetallePortafolio_Modificar")
            Dim oMonedaDAM As New MonedaDAM
            Dim oTablaMoneda As New DataTable
            Dim oClaseCuentaDAM As New ClaseCuentaDAM
            Dim oTablaCC As New DataTable
            Dim i As Integer
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CodigoDetallePortafolio", DbType.String, codigoDetalle)
            oTablaMoneda = oMonedaDAM.Listar().Tables(0)
            For i = 0 To oTablaMoneda.Rows.Count - 1
                If CStr(oTablaMoneda.Rows(i)(2)).ToUpper = codigoMoneda.ToUpper Then
                    codigoMoneda = CStr(oTablaMoneda.Rows(i)(0))
                    Exit For
                End If
            Next
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            oTablaCC = oClaseCuentaDAM.Listar().Tables(0)
            For i = 0 To oTablaCC.Rows.Count - 1
                If CStr(oTablaCC.Rows(i)(2)).ToUpper = codigoClaseCuenta.ToUpper Then
                    codigoClaseCuenta = CStr(oTablaCC.Rows(i)(0))
                    Exit For
                End If
            Next
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaBancaria", DbType.String, numeroCuentaBanc)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaInterbancario", DbType.String, numeroCuentaInter)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion.Substring(0, 1))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub Aperturar(ByVal codigoPortafolio As String, ByVal fechaApertura As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Aperturar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaApertura", DbType.Decimal, fechaApertura)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub Cerrar(ByVal codigoPortafolio As String, ByVal fechaApertura As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Cerrar")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaApertura", DbType.Decimal, fechaApertura)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub CerrarReproceso(ByVal codigoPortafolio As String, ByVal fechaCierre As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_Cerrar_Reproceso")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaCierre", DbType.Decimal, fechaCierre)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub ModificarCierreContable(ByVal codigoPortafolio As String, ByVal fechaCierre As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioCierreContable_Modificar")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaOperacion", DbType.Decimal, fechaCierre)
            db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function SeleccionarPortafolioPorCustodioValores(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "PortafolioPorCustodioValores_Listar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCustodioPorPortafolio(ByVal CodigoPortafolioSBS As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CustodioPorPortafolio_Listar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCodigoNemonicoPorCustodioPortafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CodigoNemonicoPorCustodioPortafolio_Listar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCodigoISINPorCustodioPortafolio(ByVal CodigoPortafolioSBS As String, ByVal CodigoCustodio As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "CodigoISINPorCustodioPortafolio_Listar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@CodigoCustodio", DbType.String, CodigoCustodio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ValidarCierre(ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Portafolio_ValidarCierre"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Return (CInt(db.ExecuteScalar(dbCommand)) = 0)
        End Using
    End Function
    Public Function ValidarApertura(ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Portafolio_ValidarApertura"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            dbCommand.CommandTimeout = 1020  'HDG 20110912
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Return (CInt(db.ExecuteScalar(dbCommand)) = 0)
        End Using
    End Function
    Public Function ListarOrdenesFaltantes(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Portafolio_ListarOrdenenesFaltantesParaCierre"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ListarOrdenesFaltantesValoracion(ByVal codigoPortafolio As String, ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Portafolio_ListarOrdenenesFaltantesParaValoracion"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaValoracion", DbType.String, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ListarOrdenesFaltantesApertura(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Portafolio_ListarOrdenenesFaltantesApertura"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPortafolioPorFiltro(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Portafolio_SeleccionarFechaAperturaContable"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ListarPortafolioPorUsuario(ByVal loginUsuario As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_ListarPorUsuario")
            db.AddInParameter(dbCommand, "@p_LoginUsuario", DbType.String, loginUsuario)
            Using objeto As DataSet = db.ExecuteDataSet(dbCommand)
                Return objeto.Tables(0)
            End Using
        End Using
    End Function
    Public Function ValidarCuponesLibor(ByVal codigoPortafolio As String, ByVal fechaApertura As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_ValidarCupon")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaApertura", DbType.String, fechaApertura)
            Using objeto As DataSet = db.ExecuteDataSet(dbCommand)
                Return objeto
            End Using
        End Using
    End Function
    Public Function ValidarFechasLibor(ByVal fechaApertura As Decimal) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_ValidacionApertura_TasaVariable")
            db.AddInParameter(dbCommand, "@decFechaApertura", DbType.String, fechaApertura)
            Dim objeto As String
            objeto = db.ExecuteDataSet(dbCommand).Tables(0).Rows(0)(0)
            Return objeto
        End Using
    End Function
    Public Function ListarFechaNegociacion() As Hashtable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Portafolio_ListarFechaNegociacion")
            Dim ht As New Hashtable
            Using ir As IDataReader = db.ExecuteReader(dbCommand)
                While ir.Read()
                    ht.Add(ir("codigoportafoliosbs"), ir("fechanegocio"))
                End While
                ir.Close()
            End Using
            Return ht
        End Using
    End Function
    Public Function ListarOrdenesFaltantesValoracionEstimada(ByVal codigoPortafolio As String, ByVal fecha As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "pr_SIT_gl_ListarOrdenesFaltantesValoracionEstimada"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.String, fecha)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarDetalleGrupoPortafolio(ByVal codigoPortafolio As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_DetalleGrupoPortafolio_Listar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Sub InsertarDetalleGrupoPortafolio(ByVal codigoPortafolioPadre As String, ByVal codigoPortafolioHijo As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_DetalleGrupoPortafolio_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioPadre", DbType.String, codigoPortafolioPadre)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioHijo", DbType.String, codigoPortafolioHijo)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

    '-- INICIO | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio
    Public Sub InsertarSecuenciaVector(ByVal codigoPortafolio As String, ByVal tipoVector As String, ByVal fuenteVector As String, ByVal secuencia As Integer)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SecuenciaVector_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_TipoVector", DbType.String, tipoVector)
            db.AddInParameter(dbCommand, "@p_FuenteVector", DbType.String, fuenteVector)
            db.AddInParameter(dbCommand, "@p_Secuencia", DbType.Int32, secuencia)

            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub EliminarSecuenciaVector(ByVal codigoPortafolio As String, ByVal tipoVector As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SecuenciaVector_Elimina")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_TipoVector", DbType.String, tipoVector)

            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ListarSecuenciaVector(ByVal codigoPortafolio As String, ByVal tipoVector As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_SecuenciaVector_Listar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_TipoVector", DbType.String, tipoVector)

            Return db.ExecuteDataSet(dbCommand).Tables(0)
        End Using
    End Function
    '-- FIN | Proyecto SIT Fondos - Mandato | Sprint I | CRumiche | 2018-08-23 | Mantenimiento portafolio

    Public Sub EliminarDetalleGrupoPortafolio(ByVal codigoPortafolioPadre As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_DetalleGrupoPortafolio_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioPadre", DbType.String, codigoPortafolioPadre)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub Actualiza_FechaNegocio(ByVal CodigoPortafolioSBS As String, ByVal FechaNegocio As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("SP_SIT_UPD_PortafolioFechaNegocio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaNegocio", DbType.Decimal, FechaNegocio)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Function ValidaIndicadores(ByVal FechaNegocio As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_ValidaIndicadores")
            bd.AddInParameter(dbcomand, "@FechaProceso", DbType.Decimal, FechaNegocio)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT10749 - Refactorizar código
    Public Sub AperturaCajaRecaudo(ByVal CodigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal FechaCajaOperaciones As Decimal, ByVal dataRequest As DataSet)
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_AperturaCajaRecaudo")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            bd.AddInParameter(dbcomand, "@p_FechaCajaOperaciones", DbType.Decimal, FechaCajaOperaciones)
            If CodigoClaseCuenta <> "" Then
                bd.AddInParameter(dbcomand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            End If
            bd.AddInParameter(dbcomand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            bd.ExecuteNonQuery(dbcomand)
        End Using
    End Sub
    Public Sub ReversaCajaRecaudo(ByVal CodigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal FechaCajaOperaciones As Decimal)
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_ReversaCajaRecaudo")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            If CodigoClaseCuenta <> "" Then
                bd.AddInParameter(dbcomand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            End If
            bd.AddInParameter(dbcomand, "@p_FechaCajaOperaciones", DbType.Decimal, FechaCajaOperaciones)
            bd.ExecuteNonQuery(dbcomand)
        End Using
    End Sub
    Public Sub GeneraSaldoBanco(ByVal FechaSaldo As Decimal, ByVal decFechaAnterior As Decimal, ByVal CodigoPortafolio As String, ByVal ClaseCuenta As String, ByVal dataRequest As DataSet)
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_GeneraSaldoBanco")
            bd.AddInParameter(dbcomand, "@p_FechaSaldo", DbType.Decimal, FechaSaldo)
            bd.AddInParameter(dbcomand, "@p_decFechaAnterior", DbType.Decimal, decFechaAnterior)
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            bd.AddInParameter(dbcomand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            bd.AddInParameter(dbcomand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            bd.AddInParameter(dbcomand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            bd.AddInParameter(dbcomand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            bd.AddInParameter(dbcomand, "@p_ClaseCuenta", DbType.Decimal, ClaseCuenta)
            bd.ExecuteNonQuery(dbcomand)
        End Using
    End Sub
    'OT10749 - Fin
    Public Function PortafolioCodAuto() As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioGenerarCod")
            Using ds As New DataSet
                db.LoadDataSet(dbCommand, ds, "Portafolio")
                Return ds.Tables(0).Rows(0).Item(0)
            End Using
        End Using
    End Function
    'OT 10238 - 08/05/2017 - Carlos Espejo
    'Descripcion: Lista Inconsistencia de valorización
    Public Function InconsisteciasValorizacion(ByVal CodigoPortafolio As String, ByVal FechaOperacion As Decimal) As DataTable
        'OT10916 - 06/11/2017 - Ian Pastor M. Refactorizar y ordenar código.
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_SIT_lis_InconsisteciasValorizacion")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.Decimal, CodigoPortafolio)
            bd.AddInParameter(dbcomand, "@p_FechaOperacion", DbType.Decimal, FechaOperacion)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                InconsisteciasValorizacion = ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function Portafolio_ListarPortafolioMensual(ByVal p_CodigoPortafolio As String, ByVal p_ValoracionMensual As String, ByVal p_Estado As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using DbCommand As DbCommand = db.GetStoredProcCommand("Portafolio_listarPorValoracionMensual")
            db.AddInParameter(DbCommand, "@p_CodigoPortafolio", DbType.String, p_CodigoPortafolio)
            db.AddInParameter(DbCommand, "@p_ValoracionMensual", DbType.String, p_ValoracionMensual)
            db.AddInParameter(DbCommand, "@p_Estado", DbType.String, p_Estado)
            Using ds As DataSet = db.ExecuteDataSet(DbCommand)
                Portafolio_ListarPortafolioMensual = ds.Tables(0)
            End Using
        End Using
    End Function

    'INICIO | PROYECTO SIT - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 21/05/2018
    Public Function PortafolioSelectById(ByVal Idportafolio As String) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_SelectByIdPortafolio")
            bd.AddInParameter(dbcomand, "@CodigoPortafolio", DbType.String, Idportafolio)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function PortafolioCodigoListarByNemonico(ByVal portafolio As String, ByVal Nemonico As String, Optional ByVal s_Parametro As String = "S", Optional ByVal porSerie As String = "", Optional ByVal estado As String = "") As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_listarCodigoPortafolioByNemonico")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_Administra", DbType.String, s_Parametro)
            bd.AddInParameter(dbcomand, "@p_PorSerie", DbType.String, porSerie)
            bd.AddInParameter(dbcomand, "@p_Estado", DbType.String, estado)
            bd.AddInParameter(dbcomand, "@P_CodigoNemonico", DbType.String, Nemonico)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'FIN | PROYECTO SIT - ZOLUXIONES | DACV | RF009 - DESCRIPCION | 21/05/2018

    'INICIO | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF014 - DESCRIPCION | 12/06/2018
    '
    Public Function ListarNemonicoXValorizar(ByVal portafolio As String, ByVal fecha As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("ListarNemonicoXValorizar")
            bd.AddInParameter(dbcomand, "@p_Portafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_fecha", DbType.Decimal, fecha)
            dbcomand.CommandTimeout = 1020
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function VerificarVectorPrecio(ByVal fechaOperacion As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Verificar_VectorPrecio")
            bd.AddInParameter(dbcomand, "@p_fecha", DbType.Decimal, fechaOperacion)
            dbcomand.CommandTimeout = 1020
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function CargarValorizador(ByVal fechaOperacion As Decimal, ByVal tipoNegocio As String, ByVal estado As String) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Valorizacion_listar")
            bd.AddInParameter(dbcomand, "@p_FechaOperacion", DbType.Decimal, fechaOperacion)
            bd.AddInParameter(dbcomand, "@p_TipoNegocio", DbType.String, tipoNegocio)
            bd.AddInParameter(dbcomand, "@p_estado", DbType.String, estado)
            dbcomand.CommandTimeout = 1020
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function CargarDatosComplementarios(ByVal portafolio As String, ByVal fecha As String) As DataSet
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("GenerarReporteVL_Inconsistencias")
            bd.AddInParameter(dbcomand, "@p_Portafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_fecha", DbType.Decimal, fecha)
            dbcomand.CommandTimeout = 1020
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds
            End Using
        End Using
    End Function

    Public Function CargarDatos_InversionAnterior(ByVal portafolio As String, ByVal fecha As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("GenerarReporteVL_Inconsistencias")
            bd.AddInParameter(dbcomand, "@p_Portafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_fecha", DbType.Decimal, fecha)
            dbcomand.CommandTimeout = 1020
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(3)
            End Using
        End Using
    End Function

    'FIN | PROYECTO FONDOS II - ZOLUXIONES | DACV | RF014 - DESCRIPCION | 12/06/2018

    'INICIO | PROYECTO AUMENTO CAPITAL | rcolonia | Obtener Portafolio por Aumento de Capital | 18092018
    Public Function PortafolioListarbyAumentoCapital() As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_listarbyAumentoCapital")
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'FIN | PROYECTO AUMENTO CAPITAL | rcolonia | Obtener Portafolio por Aumento de Capital | 18092018
    Public Function PortafolioListar(ByVal portafolio As String, ByVal TipoNegocio As String, ByVal estado As String) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_listarCodigoPortafolio")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_Administra", DbType.String, "S")
            bd.AddInParameter(dbcomand, "@p_PorSerie", DbType.String, "")
            bd.AddInParameter(dbcomand, "@p_Estado", DbType.String, "")
            bd.AddInParameter(dbcomand, "@p_TipoNegocio", DbType.String, TipoNegocio)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function PortafolioListar(ByVal portafolio As String, ByVal TipoNegocio As String, ByVal estado As String, ByVal fondoCliente As String) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("Portafolio_listarCodigoPortafolio")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolio", DbType.String, portafolio)
            bd.AddInParameter(dbcomand, "@p_Administra", DbType.String, "S")
            bd.AddInParameter(dbcomand, "@p_PorSerie", DbType.String, "")
            bd.AddInParameter(dbcomand, "@p_Estado", DbType.String, "")
            bd.AddInParameter(dbcomand, "@p_TipoNegocio", DbType.String, TipoNegocio)
            bd.AddInParameter(dbcomand, "@p_FondoVinculado", DbType.String, fondoCliente)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT12028 | OC11111 - Rcolonia | Zoluxiones | Función para validar cierre de caja con fecha de registro de pago de comisión
    Public Function CierreCajas_ValidarFechaIngresoPagoComision(ByVal CodigoPortafolioSBS As String, ByVal CodigoClaseCuenta As String, ByVal fechaActualCaja As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CierreCajas_ValidarFechaIngresoPagoComision")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_fechaActualCaja", DbType.Decimal, fechaActualCaja)

            Return db.ExecuteDataSet(dbCommand).Tables(0).Rows(0)(0)

        End Using
    End Function
    'OT12028 | OC11111 - FIN
    Public Function PortafolioPorDescripcion(ByVal portafolio As String) As String

        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("sp_ListarPortafolio_Descripcion")
            bd.AddInParameter(dbcomand, "@p_DesPortafolio", DbType.String, portafolio)
            bd.AddOutParameter(dbcomand, "@p_Out", DbType.String, 10)
            bd.ExecuteNonQuery(dbcomand)
            Return CType(bd.GetParameterValue(dbcomand, "@p_Out"), String)
        End Using
    End Function
End Class