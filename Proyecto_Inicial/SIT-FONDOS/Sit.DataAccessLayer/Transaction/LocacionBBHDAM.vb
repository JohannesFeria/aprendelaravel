'Creado por: HDG OT 64765 20120312
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class LocacionBBHDAM
    Const IND_ELI_TRANSCUSTODIO_BBH As String = "E"

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oLocacionBBHBE As LocacionBBHBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_LocacionBBH_Insertar")
        Dim oRow As LocacionBBHBE.LocacionBBHRow

        oRow = DirectCast(oLocacionBBHBE.LocacionBBH.Rows(0), LocacionBBHBE.LocacionBBHRow)

        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, oRow.Mercado)
        db.AddInParameter(dbCommand, "@p_SOD_Name", DbType.String, oRow.SOD_Name)
        db.AddInParameter(dbCommand, "@p_NemoLocacion", DbType.String, oRow.NemoLocacion)
        db.AddInParameter(dbCommand, "@p_Settlement_location", DbType.String, oRow.Settlement_location)
        db.AddInParameter(dbCommand, "@p_Precio_Trans", DbType.Decimal, oRow.Precio_Trans)
        db.AddInParameter(dbCommand, "@p_Tasa_Custodio", DbType.Decimal, oRow.Tasa_Custodio)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function SeleccionarPorFiltro(ByVal location As String, ByVal situacion As String) As LocacionBBHBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_LocacionBBH_SeleccionarPorFiltro")
        Dim oLocacionBBHBE As New LocacionBBHBE

        db.AddInParameter(dbCommand, "@p_SOD_Name", DbType.String, location)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oLocacionBBHBE, "LocacionBBH")

        Return oLocacionBBHBE

    End Function

    Public Function Seleccionar(ByVal CodigoLocation As String) As LocacionBBHBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_LocacionBBH_Seleccionar")
        Dim oLocacionBBHBE As New LocacionBBHBE

        db.AddInParameter(dbCommand, "@p_CodigoLocacion", DbType.Decimal, CodigoLocation)

        db.LoadDataSet(dbCommand, oLocacionBBHBE, "LocacionBBH")

        Return oLocacionBBHBE

    End Function

    Public Function Modificar(ByVal oLocacionBBHBE As LocacionBBHBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_LocacionBBH_Modificar")
        Dim oRow As LocacionBBHBE.LocacionBBHRow

        oRow = DirectCast(oLocacionBBHBE.LocacionBBH.Rows(0), LocacionBBHBE.LocacionBBHRow)

        db.AddInParameter(dbCommand, "@p_CodigoLocacion", DbType.Decimal, oRow.CodigoLocacion)
        db.AddInParameter(dbCommand, "@p_Mercado", DbType.String, oRow.Mercado)
        db.AddInParameter(dbCommand, "@p_SOD_Name", DbType.String, oRow.SOD_Name)
        db.AddInParameter(dbCommand, "@p_NemoLocacion", DbType.String, oRow.NemoLocacion)
        db.AddInParameter(dbCommand, "@p_Settlement_location", DbType.String, oRow.Settlement_location)
        db.AddInParameter(dbCommand, "@p_Precio_Trans", DbType.Decimal, oRow.Precio_Trans)
        db.AddInParameter(dbCommand, "@p_Tasa_Custodio", DbType.Decimal, oRow.Tasa_Custodio)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal CodigoLocation As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_LocacionBBH_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoLocacion", DbType.String, CodigoLocation)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function ListarLocacionBBH() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_LocacionBBH_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function ActualizarTransactionPorExcel(ByVal dtData As DataTable, ByVal proceso As String, ByVal dataRequest As DataSet, ByRef strmensaje As String, ByVal fechaTrans As Decimal) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sTransCat As String
        Dim sMAFondo As String
        Dim sSetLoc As String
        Dim sMoneda As String
        Dim nTAmount As Decimal
        Dim sTransDesc As String
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_pro_ActualizaTransaccionesBBH")

        If proceso = IND_ELI_TRANSCUSTODIO_BBH Then
            db.AddInParameter(dbCommand, "@p_Proceso", DbType.String, proceso)
            db.ExecuteNonQuery(dbCommand)
        Else
            If dtData.Rows.Count > 0 Then
                sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
                nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
                sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

                db.AddInParameter(dbCommand, "@p_Proceso", DbType.String)
                db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_TransCategoria", DbType.String)
                db.AddInParameter(dbCommand, "@p_MAFondo", DbType.String)
                db.AddInParameter(dbCommand, "@p_SetLocacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_Moneda", DbType.String)
                db.AddInParameter(dbCommand, "@p_Trans_Amount", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_TransDescrip", DbType.String)
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_Host", DbType.String)

                For Each filaLinea As DataRow In dtData.Rows
                    sTransCat = filaLinea(3).ToString().Trim()
                    sMAFondo = filaLinea(1).ToString().Trim()
                    sSetLoc = filaLinea(2).ToString().Trim()
                    sMoneda = filaLinea(0).ToString().Trim()
                    nTAmount = Val(filaLinea(4).ToString().Trim())
                    sTransDesc = filaLinea(5).ToString().Trim()

                    db.SetParameterValue(dbCommand, "@p_Proceso", proceso)
                    db.SetParameterValue(dbCommand, "@p_FechaCarga", fechaTrans)
                    db.SetParameterValue(dbCommand, "@p_TransCategoria", sTransCat)
                    db.SetParameterValue(dbCommand, "@p_MAFondo", sMAFondo)
                    db.SetParameterValue(dbCommand, "@p_SetLocacion", sSetLoc)
                    db.SetParameterValue(dbCommand, "@p_Moneda", sMoneda)
                    db.SetParameterValue(dbCommand, "@p_Trans_Amount", nTAmount)
                    db.SetParameterValue(dbCommand, "@p_TransDescrip", sTransDesc)
                    db.SetParameterValue(dbCommand, "@p_Situacion", "A")
                    db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                    db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecMod)
                    db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.SetParameterValue(dbCommand, "@p_Host", sHost)
                    db.ExecuteNonQuery(dbCommand)
                Next
                strmensaje &= "Los datos se cargaron correctamente\n"
            End If
        End If

        Return True
    End Function

    Public Function ActualizarTransCustodioPorTxt(ByVal objTransCustodioBBHBE As TransCustodioBBHBE, ByVal proceso As String, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oRow As TransCustodioBBHBE.TransCustodioBBHRow
        Dim nFecCarga As Decimal
        Dim sFondo As String
        Dim sCodigoISIN As String
        Dim nUnidades As Decimal
        Dim nMonto As Decimal
        Dim sRegion As String
        Dim sNemoSetLoc As String
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal
        Dim intNroFilas, intIndice As Integer
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_pro_ActualizaTransCustodioBBH")

        If proceso = IND_ELI_TRANSCUSTODIO_BBH Then
            db.AddInParameter(dbCommand, "@p_Proceso", DbType.String, proceso)
            db.ExecuteNonQuery(dbCommand)
        Else
            intNroFilas = objTransCustodioBBHBE.TransCustodioBBH.Rows.Count
            If intNroFilas > 0 Then
                sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
                nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
                sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

                db.AddInParameter(dbCommand, "@p_Proceso", DbType.String)
                db.AddInParameter(dbCommand, "@p_FechaCarga", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_Fondo", DbType.String)
                db.AddInParameter(dbCommand, "@p_CodigoISIN", DbType.String)
                db.AddInParameter(dbCommand, "@p_Unidades", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_Region", DbType.String)
                db.AddInParameter(dbCommand, "@p_NemoSetLocacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_Host", DbType.String)

                For intIndice = 0 To intNroFilas - 1
                    oRow = DirectCast(objTransCustodioBBHBE.TransCustodioBBH.Rows(intIndice), TransCustodioBBHBE.TransCustodioBBHRow)
                    nFecCarga = oRow.FechaCarga.ToString
                    sFondo = oRow.Fondo.ToString
                    sCodigoISIN = oRow.CodigoISIN.ToString
                    nUnidades = oRow.Unidades.ToString
                    nMonto = oRow.Monto.ToString
                    sRegion = oRow.RegionLoc.ToString
                    sNemoSetLoc = oRow.NemoSetLocacion.ToString

                    db.SetParameterValue(dbCommand, "@p_Proceso", proceso)
                    db.SetParameterValue(dbCommand, "@p_FechaCarga", nFecCarga)
                    db.SetParameterValue(dbCommand, "@p_Fondo", sFondo)
                    db.SetParameterValue(dbCommand, "@p_CodigoISIN", sCodigoISIN)
                    db.SetParameterValue(dbCommand, "@p_Unidades", nUnidades)
                    db.SetParameterValue(dbCommand, "@p_Monto", nMonto)
                    db.SetParameterValue(dbCommand, "@p_Region", sRegion)
                    db.SetParameterValue(dbCommand, "@p_NemoSetLocacion", sNemoSetLoc)
                    db.SetParameterValue(dbCommand, "@p_Situacion", "A")
                    db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", sUsuario)
                    db.SetParameterValue(dbCommand, "@p_FechaCreacion", nFecMod)
                    db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.SetParameterValue(dbCommand, "@p_Host", sHost)
                    db.ExecuteNonQuery(dbCommand)
                Next
                strmensaje &= "Los datos se cargaron correctamente\n"
            End If
        End If

        Return True
    End Function

    Public Function ComisionCustodioTrasacciones(ByVal CodigoPortafolio As String, ByVal FechaInicio As Decimal, ByVal FechaFin As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteTransaccionesBBH")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, FechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, FechaFin)

        Dim oReporte As New DataSet
        db.LoadDataSet(dbCommand, oReporte, "Comisiones")
        Return oReporte
    End Function
End Class

