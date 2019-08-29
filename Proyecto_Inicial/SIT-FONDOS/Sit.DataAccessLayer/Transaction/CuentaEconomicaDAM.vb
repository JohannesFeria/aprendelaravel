Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports Sit.BusinessEntities
Public Class CuentaEconomicaDAM
    Private sqlCommand As String = ""
    Public oCuentaEconomicaRow As CuentaEconomicaBE.CuentaEconomicaRow

    Public Sub New()
    End Sub

#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoCuentaEconomica As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoCuentaEconomica", DbType.String, codigoCuentaEconomica)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltroMant(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal entidadFinanciera As String, _
    ByVal CodigoMoneda As String, ByVal CodigoMercado As String, ByVal NumeroCuenta As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using objeto As New CuentaEconomicaBE
            Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_SeleccionarPorFiltro2")
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
                db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, entidadFinanciera)
                db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
                db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
                db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
                db.LoadDataSet(dbCommand, objeto, "CuentaEconomica")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal CodigoTercero As String, _
    ByVal CodigoMoneda As String, ByVal CodigoMercado As String) As CuentaEconomicaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_SeleccionarPorFiltro")
            Using objeto As New CuentaEconomicaBE
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
                db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
                db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro_CAD(ByVal codigoPortafolio As String, ByVal CodigoClaseCuenta As String, ByVal CodigoTercero As String, _
    ByVal CodigoMercado As String) As CuentaEconomicaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_SeleccionarPorFiltro_CAD")
            Using objeto As New CuentaEconomicaBE
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
                db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, CodigoClaseCuenta)
                db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
                db.LoadDataSet(dbCommand, objeto, "Portafolio")
                Return objeto
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using ds As New DataSet
            Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_SeleccionarPorFiltro")
                db.LoadDataSet(dbCommand, ds, "Portafolio")
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro2(ByVal codigoPortafolio As String, ByVal codigoTercero As String, ByVal codigoClaseCuenta As String, _
    ByVal codigoMoneda As String, ByVal CodigoMercado As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using ds As New DataSet
            Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_SeleccionarPorFiltro")
                db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
                db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
                db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
                db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
                db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, CodigoMercado)
                db.LoadDataSet(dbCommand, ds, "Portafolio")
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoCuentaEconomica As String, ByVal codigoTercero As String, ByVal codigoMoneda As String, _
    ByVal codigoClaseCuenta As String, ByVal codigoPortafolio As String, ByVal situacion As String, ByVal dataRequest As DataSet) As CuentaEconomicaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoCuentaEconomica", DbType.String, codigoCuentaEconomica)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            Using objeto As New CuentaEconomicaBE
                db.LoadDataSet(dbCommand, objeto, "CuentaEconomica")
                Return objeto
            End Using
        End Using
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_Listar")
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCodigoTercero(ByVal CodigoTercero As String, ByVal CodigoEntidadFinanciera As String, ByVal CodigoMercado As String, _
    ByVal CodigoMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaTerceros_SeleccionarPorTercero")
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, IIf(CodigoTercero = "--Seleccione--", "", CodigoTercero))
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, IIf(CodigoEntidadFinanciera = "--Seleccione--", "", CodigoEntidadFinanciera))
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function ObtenerCuentaContable(ByVal CodigoEntidad As String, ByVal CodigoMoneda As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_OBT_CuentaContableTerceros")
            db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            db.AddOutParameter(dbCommand, "@p_CuentaContable", DbType.String, 20)
            db.ExecuteNonQuery(dbCommand)
            Dim CuentaContable As String = String.Empty
            CuentaContable = CType(db.GetParameterValue(dbCommand, "@p_CuentaContable"), String)
            Return CuentaContable
        End Using
    End Function
    Public Function ObtenerSituacion(ByVal CodigoPortafolioSBS As String, ByVal CuentaContable As String, ByVal EntidadFinanciera As String, _
    ByVal CodigoMoneda As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_OBT_SituacionCuentaEconomica")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, CuentaContable)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            db.AddOutParameter(dbCommand, "@p_Situacion", DbType.String, 1)
            db.ExecuteNonQuery(dbCommand)
            Dim Situacion As String = String.Empty
            Situacion = CType(db.GetParameterValue(dbCommand, "@p_Situacion"), String)
            Return Situacion
        End Using
    End Function
    ' Inicio de Cambio OT-10795
    Public Function ObtenerSituacionNuevo(ByVal CodigoPortafolioSBS As String, ByVal CuentaContable As String, ByVal EntidadFinanciera As String, _
    ByVal CodigoMoneda As String, ByVal NuevoNroCuenta As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("dbo.SP_SIT_OBT_SituacionCuentaEconomica")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, CuentaContable)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NuevoNroCuenta)
            db.AddOutParameter(dbCommand, "@p_Situacion", DbType.String, 1)
            db.ExecuteNonQuery(dbCommand)
            Dim Situacion As String = String.Empty
            Situacion = CType(db.GetParameterValue(dbCommand, "@p_Situacion"), String)
            Return Situacion
        End Using
    End Function
    ' Fin de Cambio OT-10795
    Public Function EncontrarCuentasExistentes(ByVal strEntidadFinanciera As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_EncontrarExistentes")
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, strEntidadFinanciera)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function
    Public Function SeleccionarCuentaEconomica(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal,
    ByVal CodigoMoneda As String, ByVal CodBanco As String, ByVal CodigoMercado As String, ByVal CodigoClasecuenta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using ds As New DataSet
            Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_CuentaEconomica")
                db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbCommand, "@FechaOperacion", DbType.Decimal, FechaOperacion)
                db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, CodigoMoneda)
                db.AddInParameter(dbCommand, "@CodBanco", DbType.String, CodBanco)
                db.AddInParameter(dbCommand, "@CodigoMercado", DbType.String, CodigoMercado)
                db.AddInParameter(dbCommand, "@CodigoClasecuenta", DbType.String, CodigoClasecuenta)
                db.LoadDataSet(dbCommand, ds, "CE")
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function SeleccionarCuentaEconomica_Movimientos(ByVal CodigoPortafolioSBS As String, ByVal FechaOperacion As Decimal, ByVal NumeroCuenta As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using ds As New DataSet
            Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_sel_CuentaEconomica_Movimientos")
                db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
                db.AddInParameter(dbCommand, "@FechaOperacion", DbType.Decimal, FechaOperacion)
                db.AddInParameter(dbCommand, "@NumeroCuenta", DbType.String, NumeroCuenta)
                db.LoadDataSet(dbCommand, ds, "CEM")
                Return ds.Tables(0)
            End Using
        End Using
    End Function
#End Region

    Public Function Insertar(ByVal ob As CuentaEconomicaBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_Insertar")
            oCuentaEconomicaRow = CType(ob.CuentaEconomica.Rows(0), CuentaEconomicaBE.CuentaEconomicaRow)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaEconomicaRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oCuentaEconomicaRow.CuentaContable)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oCuentaEconomicaRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oCuentaEconomicaRow.EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaEconomicaRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oCuentaEconomicaRow.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oCuentaEconomicaRow.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_TipoCuenta", DbType.String, oCuentaEconomicaRow.TipoCuenta)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaEconomicaRow.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CuentaInterbancaria", DbType.String, oCuentaEconomicaRow.CuentaInterbancaria)
            db.AddInParameter(dbCommand, "@p_Tasa", DbType.Decimal, oCuentaEconomicaRow.Tasa)
            db.AddInParameter(dbCommand, "@p_Comision", DbType.Decimal, oCuentaEconomicaRow.Comision)
            db.AddInParameter(dbCommand, "@p_Portes", DbType.Decimal, oCuentaEconomicaRow.Portes)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCuentaEconomicaRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCuentaEconomicaRow.Situacion)
            db.AddInParameter(dbCommand, "@p_NroDocumento", DbType.String, oCuentaEconomicaRow.NroDocumento)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_rp", DbType.String, 1) 'OT-10795
            db.ExecuteNonQuery(dbCommand)
            Dim Situacion As String = String.Empty 'OT-10795
            Situacion = CType(db.GetParameterValue(dbCommand, "@p_rp"), String) 'OT-10795
            Return Situacion 'OT-10795
        End Using
    End Function
    Public Function CambiarCuenta(ByVal ob As CuentaEconomicaBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_CambiarCuenta")
            oCuentaEconomicaRow = CType(ob.CuentaEconomica.Rows(0), CuentaEconomicaBE.CuentaEconomicaRow)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaEconomicaRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oCuentaEconomicaRow.CuentaContable)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oCuentaEconomicaRow.EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaEconomicaRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaEconomicaRow.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_NuevoNumeroCuenta", DbType.String, oCuentaEconomicaRow.NuevoNumeroCuenta)
            db.AddInParameter(dbCommand, "@p_MotivoCambio", DbType.String, oCuentaEconomicaRow.MotivoCambio)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_rp", DbType.String, 1) 'OT-10795
            db.ExecuteNonQuery(dbCommand)
            Dim Situacion As String = String.Empty 'OT-10795
            Situacion = CType(db.GetParameterValue(dbCommand, "@p_rp"), String) 'OT-10795
            Return Situacion 'OT-10795
        End Using
    End Function
    Public Function Modificar(ByVal ob As CuentaEconomicaBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_Modificar")
            oCuentaEconomicaRow = CType(ob.CuentaEconomica.Rows(0), CuentaEconomicaBE.CuentaEconomicaRow)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaEconomicaRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oCuentaEconomicaRow.CuentaContable)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oCuentaEconomicaRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaEconomicaRow.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CuentaInterbancaria", DbType.String, oCuentaEconomicaRow.CuentaInterbancaria)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaEconomicaRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oCuentaEconomicaRow.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oCuentaEconomicaRow.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_Tasa", DbType.Decimal, oCuentaEconomicaRow.Tasa)
            db.AddInParameter(dbCommand, "@p_Comision", DbType.Decimal, oCuentaEconomicaRow.Comision)
            db.AddInParameter(dbCommand, "@p_Portes", DbType.Decimal, oCuentaEconomicaRow.Portes)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCuentaEconomicaRow.Situacion)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCuentaEconomicaRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oCuentaEconomicaRow.EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_NroDocumento", DbType.String, oCuentaEconomicaRow.NroDocumento)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
    Public Function Modificar(ByVal ob As CuentaEconomicaBE, ByVal p_CodigoPortafolioSBS As String, ByVal p_CuentaContable As String, ByVal p_NumeroCuenta As String, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_ModificarRegistro")
            oCuentaEconomicaRow = CType(ob.CuentaEconomica.Rows(0), CuentaEconomicaBE.CuentaEconomicaRow)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oCuentaEconomicaRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oCuentaEconomicaRow.CuentaContable)
            db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oCuentaEconomicaRow.CodigoTercero)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, oCuentaEconomicaRow.NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_CuentaInterbancaria", DbType.String, oCuentaEconomicaRow.CuentaInterbancaria)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oCuentaEconomicaRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, oCuentaEconomicaRow.CodigoClaseCuenta)
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, oCuentaEconomicaRow.CodigoMercado)
            db.AddInParameter(dbCommand, "@p_Tasa", DbType.Decimal, oCuentaEconomicaRow.Tasa)
            db.AddInParameter(dbCommand, "@p_Comision", DbType.Decimal, oCuentaEconomicaRow.Comision)
            db.AddInParameter(dbCommand, "@p_Portes", DbType.Decimal, oCuentaEconomicaRow.Portes)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oCuentaEconomicaRow.Situacion)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oCuentaEconomicaRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_EntidadFinanciera", DbType.String, oCuentaEconomicaRow.EntidadFinanciera)
            db.AddInParameter(dbCommand, "@p_NroDocumento", DbType.String, oCuentaEconomicaRow.NroDocumento)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBSOld", DbType.String, p_CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_CuentaContableOLD", DbType.String, p_CuentaContable)
            db.AddInParameter(dbCommand, "@p_NumeroCuentaOLD", DbType.String, p_NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
#Region " /* Funciones Eliminar */ "
    Public Function Eliminar(ByVal codigoPortafolio As String, ByVal cuentaContable As String, ByVal NumeroCuenta As String, ByVal dataRequest As DataSet) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, cuentaContable)
            db.AddInParameter(dbCommand, "@p_NumeroCuenta", DbType.String, NumeroCuenta)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function
    Public Function EliminarPorCodigoClaseCuenta(ByVal codigoClaseCuenta As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_EliminarPorCodigoClaseCuenta")
            db.AddInParameter(dbCommand, "@p_CodigoClaseCuenta", DbType.String, codigoClaseCuenta)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_EliminarPorCodigoPortafolio")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function EliminarPorCodigoMoneda(ByVal codigoMoneda As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_EliminarPorCodigoMoneda")
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function EliminarPorCodigoTerceroSBS(ByVal codigoTerceroSBS As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_EliminarPorCodigoTerceroSBS")
            db.AddInParameter(dbCommand, "@p_CodigoTerceroSBS", DbType.Decimal, codigoTerceroSBS)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function EliminarPorCodigoMercado(ByVal codigoMercado As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("CuentaEconomica_EliminarPorCodigoMercado")
            db.AddInParameter(dbCommand, "@p_CodigoMercado", DbType.String, codigoMercado)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    'OT 10328 - 28/04/2017 - Carlos Espejo
    'Descripcion: Lista el reporte de comisiones - Recaudo
    Public Function ReporteComisionesRecaudo(ByVal CodigoPortafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_ReporteComisiones")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_FechaIncio", DbType.Decimal, FechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - 22/02/2018 - Ian Pastor M.
    'Descripción: Obtener los códigos de entidad equivalentes al Sistema de Operaciones
    Public Function ObtenerViaPagoSisOpe(ByVal p_CodigoTabla As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_TABLA_GENERAL")
            db.AddInParameter(dbCommand, "@codigoTabla", DbType.String, p_CodigoTabla)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - Fin
    'OT11237 - 22/02/2018 - Ian Pastor M.
    'Descripción: Obtener los rescates preliminares de un fondo, especificando su fecha y su entidad financiera
    Public Function ObtenerRescatesPreliminaresSisOpe(ByVal p_CodigoPortafolio As Integer, ByVal p_FechaOperacion As Date, _
                                                      ByVal p_ViaPago As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase("ConexionOperaciones")
        'Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_SOLICITUDES_RESCATE_PRELIMINAR")
        Using dbCommand As DbCommand = db.GetStoredProcCommand("INGF_LIS_RESCATE_PRELIMINAR")
            db.AddInParameter(dbCommand, "@idFondo", DbType.Int32, p_CodigoPortafolio)
            db.AddInParameter(dbCommand, "@fecha", DbType.Date, p_FechaOperacion)
            db.AddInParameter(dbCommand, "@viaPago", DbType.String, p_ViaPago)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    'OT11237 - Fin
#End Region
End Class