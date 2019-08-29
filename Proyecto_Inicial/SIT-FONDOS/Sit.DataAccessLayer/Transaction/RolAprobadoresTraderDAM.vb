'Creado por: HDG OT 64480 20120119
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class RolAprobadoresTraderDAM
    Private sqlCommand As String = ""
    Private oRow As RolTraderBE.RolTraderRow
    Private oRowD As AprobadorTraderBE.AprobadorTraderRow
    Public Sub New()

    End Sub

#Region " /* Funciones No Transaccionales */ "

    Public Function ListarGrupoLimite(ByVal CodigoRenta As String) As RolTraderBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_RolTrader_Listar")
        Dim objeto As New RolTraderBE

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, CodigoRenta)

        db.LoadDataSet(dbCommand, objeto, "RolTrader")
        Return objeto
    End Function

    Public Function SeleccionarPorFiltro(ByVal strDescripcion As String, ByVal strSituacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_RolTrader_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, strDescripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, strSituacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorFiltroDetalle(ByVal CodigoRolTrader As Decimal) As AprobadorTraderBE
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_AprobadorTrader_SeleccionarPorFiltro")
        Dim objeto As New AprobadorTraderBE

        db.AddInParameter(dbCommand, "@p_CodigoRolTrader", DbType.Decimal, CodigoRolTrader)

        db.LoadDataSet(dbCommand, objeto, "AprobadorTrader")
        Return objeto
    End Function

    Public Function Seleccionar(ByVal CodigoRolTrader As Decimal) As RolTraderBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_RolTrader_Seleccionar")
        Dim objeto As New RolTraderBE

        db.AddInParameter(dbCommand, "@p_CodigoRolTrader", DbType.Decimal, CodigoRolTrader)

        db.LoadDataSet(dbCommand, objeto, "RolTrader")
        Return objeto

    End Function

    Public Function SeleccionarDetalle(ByVal CodigoRolTrader As Decimal, ByVal strCodigoUsuario As String, ByVal strCodigoRenta As String) As AprobadorTraderBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_AprobadorTrader_Seleccionar")
            Dim objeto As New AprobadorTraderBE
            db.AddInParameter(dbCommand, "@p_CodigoRolTrader", DbType.Decimal, CodigoRolTrader)
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, strCodigoUsuario)
            db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, strCodigoRenta)
            db.LoadDataSet(dbCommand, objeto, "AprobadorTrader")
            Return objeto
        End Using
    End Function

#End Region

#Region " /* Funciones Transaccionales */ "
    Public Function Eliminar(ByVal decCodigoRolTrader As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_RolTrader_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoRolTrader", DbType.Decimal, decCodigoRolTrader)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Insertar(ByVal oRolTraderBE As RolTraderBE, ByVal oAprobadorTraderBE As AprobadorTraderBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_RolTrader")
        Dim intResult As Decimal = 0
        oRow = CType(oRolTraderBE.RolTrader.Rows(0), RolTraderBE.RolTraderRow)

        db.AddInParameter(dbCommand, "@p_TipoCargo", DbType.String, oRow.TipoCargo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_CantidadPrincipal", DbType.String, oRow.CantidadPrincipal)
        db.AddInParameter(dbCommand, "@p_CantidadAlterno", DbType.String, oRow.CantidadAlterno)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_ValidaIngreso", DbType.Decimal, intResult)

        db.ExecuteNonQuery(dbCommand)
        intResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaIngreso"), Decimal)
        InsertarDet(oAprobadorTraderBE, "I", intResult, dataRequest)
        Return intResult
    End Function

    Public Function Modificar(ByVal oRolTraderBE As RolTraderBE, ByVal oAprobadorTraderBE As AprobadorTraderBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim bolResult As Boolean = False
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_RolTrader")
            oRow = CType(oRolTraderBE.RolTrader.Rows(0), RolTraderBE.RolTraderRow)
            db.AddInParameter(dbCommand, "@p_CodigoRolTrader", DbType.Decimal, oRow.CodigoRolTrader)
            db.AddInParameter(dbCommand, "@p_TipoCargo", DbType.String, oRow.TipoCargo)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_CantidadPrincipal", DbType.String, oRow.CantidadPrincipal)
            db.AddInParameter(dbCommand, "@p_CantidadAlterno", DbType.String, oRow.CantidadAlterno)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_ValidaIngreso", DbType.Boolean, bolResult)
            db.ExecuteNonQuery(dbCommand)
            bolResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaIngreso"), Boolean)
            InsertarDet(oAprobadorTraderBE, "M", Decimal.Zero, dataRequest)
            Modificar = bolResult
        End Using
    End Function

    Public Sub InsertarDet(ByVal oAprobadorTraderBE As AprobadorTraderBE, ByVal indProc As String, ByVal CodigoRolTrader As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oAprobadorTraderRow As AprobadorTraderBE.AprobadorTraderRow
        Dim intNroFilas, intIndice As Integer
        Dim dbCommand As DbCommand
        If indProc = "I" Then
            dbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_AprobadorTrader")
        Else
            dbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_AprobadorTrader")
        End If
        Using dbCommand
            db.AddInParameter(dbCommand, "@p_CodigoRolTrader", DbType.Decimal)
            db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String)
            db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String)
            db.AddInParameter(dbCommand, "@p_Tipo", DbType.String)
            db.AddInParameter(dbCommand, "@p_Aprobador", DbType.String)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
            db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String)
            If indProc = "I" Then
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
            Else
                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String)
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal)
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String)
            End If
            db.AddInParameter(dbCommand, "@p_Host", DbType.String)
            intNroFilas = oAprobadorTraderBE.AprobadorTrader.Rows.Count
            For intIndice = 0 To intNroFilas - 1
                oAprobadorTraderRow = DirectCast(oAprobadorTraderBE.AprobadorTrader.Rows(intIndice), AprobadorTraderBE.AprobadorTraderRow)
                db.SetParameterValue(dbCommand, "@p_CodigoRolTrader", IIf(indProc = "I", CodigoRolTrader, oAprobadorTraderRow.CodigoRolTrader))
                db.SetParameterValue(dbCommand, "@p_CodigoUsuario", oAprobadorTraderRow.CodigoUsuario)
                db.SetParameterValue(dbCommand, "@p_CodigoInterno", oAprobadorTraderRow.CodigoInterno)
                db.SetParameterValue(dbCommand, "@p_Tipo", oAprobadorTraderRow.Tipo)
                db.SetParameterValue(dbCommand, "@p_Aprobador", oAprobadorTraderRow.Aprobador)
                db.SetParameterValue(dbCommand, "@p_Situacion", oAprobadorTraderRow.Situacion)
                If oAprobadorTraderRow.IsCodigoRentaNull Then
                    db.SetParameterValue(dbCommand, "@p_CodigoRenta", "")
                Else
                    db.SetParameterValue(dbCommand, "@p_CodigoRenta", oAprobadorTraderRow.CodigoRenta)
                End If
                If indProc = "I" Then
                    db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                    db.SetParameterValue(dbCommand, "@p_FechaCreacion", DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                Else
                    db.SetParameterValue(dbCommand, "@p_UsuarioModificacion", DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                    db.SetParameterValue(dbCommand, "@p_FechaModificacion", DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                    db.SetParameterValue(dbCommand, "@p_HoraModificacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                End If
                db.SetParameterValue(dbCommand, "@p_Host", DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.ExecuteNonQuery(dbCommand)
            Next
        End Using
    End Sub
#End Region

#Region " /* Otras Funciones */ "
    Public Function ObtieneFechaActualizacionMovPersonal() As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ObtieneFechaActualizacionMovPersonal")

        Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)

        Return dt.Rows(0)("Fecha")
    End Function

    'HDG OT 64926 20120320
    Public Function SeleccionarOperadores(ByVal strCodigoRenta As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_AprobadoresTrader_Listar")

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, strCodigoRenta)

        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region
End Class
