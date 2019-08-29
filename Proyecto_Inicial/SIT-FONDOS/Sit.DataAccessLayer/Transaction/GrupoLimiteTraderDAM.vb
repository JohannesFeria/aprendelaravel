'Creado por: HDG OT 64291 20111202
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class GrupoLimiteTraderDAM
    Private sqlCommand As String = ""
    Private oRow As GrupoLimiteTraderBE.GrupoLimiteTraderRow
    Private oRowD As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow
    Public Sub New()

    End Sub

    Public Sub InicializarGrupoLimiteTrader(ByRef oRow As GrupoLimiteTraderBE.GrupoLimiteTraderRow)
        oRow.CodigoGrupLimTrader = Decimal.Zero
        oRow.Nombre = String.Empty
        oRow.Tipo = String.Empty
        oRow.CodigoRenta = String.Empty
        oRow.Situacion = String.Empty
        oRow.UsuarioCreacion = String.Empty
        oRow.FechaCreacion = Decimal.Zero
        oRow.HoraCreacion = String.Empty
        oRow.UsuarioModificacion = String.Empty
        oRow.FechaModificacion = Decimal.Zero
        oRow.HoraModificacion = String.Empty
        oRow.Host = String.Empty
    End Sub

    Public Sub InicializarGrupoLimiteTraderDetalle(ByRef oRowD As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow)
        oRowD.CodigoGrupLimTrader = Decimal.Zero
        oRowD.Valor = String.Empty
        oRowD.Situacion = String.Empty
        oRowD.UsuarioCreacion = String.Empty
        oRowD.FechaCreacion = Decimal.Zero
        oRowD.HoraCreacion = String.Empty
        oRowD.UsuarioModificacion = String.Empty
        oRowD.FechaModificacion = Decimal.Zero
        oRowD.HoraModificacion = String.Empty
        oRowD.Host = String.Empty
    End Sub

#Region " /* Funciones No Transaccionales */ "

    Public Function ListarGrupoLimite(ByVal CodigoRenta As String) As GrupoLimiteTraderBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoLimiteTrader_Listar")
        Dim objeto As New GrupoLimiteTraderBE

        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, CodigoRenta)

        db.LoadDataSet(dbCommand, objeto, "GrupoLimiteTrader")
        Return objeto
    End Function

    Public Function SeleccionarPorFiltro(ByVal strDescripcion As String, ByVal strSituacion As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoLimiteTrader_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, strDescripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, strSituacion)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorFiltroDetalle(ByVal codigoGrupLimTrader As Decimal, ByVal strTipo As String) As GrupoLimiteTraderDetalleBE
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoLimiteTraderDetalle_SeleccionarPorFiltro")
        Dim objeto As New GrupoLimiteTraderDetalleBE

        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, codigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, strTipo)

        db.LoadDataSet(dbCommand, objeto, "GrupoLimiteTraderDetalle")
        Return objeto
        'Return db.ExecuteDataSet(dbCommand, objeto, "GrupoLimiteTraderDetalle")
    End Function

    Public Function Seleccionar(ByVal codigoGrupLimTrader As Decimal) As GrupoLimiteTraderBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoLimiteTrader_Seleccionar")
        Dim objeto As New GrupoLimiteTraderBE

        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, codigoGrupLimTrader)

        db.LoadDataSet(dbCommand, objeto, "GrupoLimiteTrader")
        Return objeto

    End Function

    Public Function SeleccionarDetalle(ByVal codigoGrupLimTrader As Decimal, ByVal strValor As String) As GrupoLimiteTraderDetalleBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_GrupoLimiteTraderDetalle_Seleccionar")

        Dim objeto As New GrupoLimiteTraderDetalleBE
        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, codigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, strValor)

        db.LoadDataSet(dbCommand, objeto, "GrupoLimiteTraderDetalle")
        Return objeto

    End Function

#End Region

#Region " /* Funciones Transaccionales */ "
    Public Function Eliminar(ByVal decCodigoGrupLimTrader As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_GrupoLimiteTrader_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, decCodigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Insertar(ByVal oGrupoLimiteTraderBE As GrupoLimiteTraderBE, ByVal oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_GrupoLimiteTrader")
        Dim intResult As Decimal = 0
        oRow = CType(oGrupoLimiteTraderBE.GrupoLimiteTrader.Rows(0), GrupoLimiteTraderBE.GrupoLimiteTraderRow)

        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, oRow.Nombre)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, oRow.Tipo)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_ValidaIngreso", DbType.Decimal, intResult)

        db.ExecuteNonQuery(dbCommand)
        intResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaIngreso"), Decimal)
        InsertarDet(oGrupoLimiteTraderDetalleBE, "I", intResult, dataRequest)
        Return intResult
    End Function

    Public Function Modificar(ByVal oGrupoLimiteTraderBE As GrupoLimiteTraderBE, ByVal oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_GrupoLimiteTrader")
        Dim bolResult As Boolean = False
        oRow = CType(oGrupoLimiteTraderBE.GrupoLimiteTrader.Rows(0), GrupoLimiteTraderBE.GrupoLimiteTraderRow)

        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal, oRow.CodigoGrupLimTrader)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, oRow.Nombre)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, oRow.Tipo)
        db.AddInParameter(dbCommand, "@p_CodigoRenta", DbType.String, oRow.CodigoRenta)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddOutParameter(dbCommand, "@p_ValidaIngreso", DbType.Boolean, bolResult)

        db.ExecuteNonQuery(dbCommand)
        bolResult = CType(db.GetParameterValue(dbCommand, "@p_ValidaIngreso"), Boolean)
        InsertarDet(oGrupoLimiteTraderDetalleBE, "M", Decimal.Zero, dataRequest)
        Return bolResult
    End Function

    Public Function InsertarDet(ByVal oGrupoLimiteTraderDetalleBE As GrupoLimiteTraderDetalleBE, ByVal indProc As String, ByVal CodigoGrupLimTrader As Decimal, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim oGrupoLimiteTraderDetalleRow As GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow
        Dim intNroFilas, intIndice As Integer
        Dim dbCommand As dbCommand
        If indProc = "I" Then
            dbCommand = db.GetStoredProcCommand("pr_SIT_ins_Insertar_GrupoLimiteTraderDetalle")
        Else
            dbCommand = db.GetStoredProcCommand("pr_SIT_upd_Actualizar_GrupoLimiteTraderDetalle")
        End If
        db.AddInParameter(dbCommand, "@p_CodigoGrupLimTrader", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
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

        intNroFilas = oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows.Count
        For intIndice = 0 To intNroFilas - 1
            oGrupoLimiteTraderDetalleRow = DirectCast(oGrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalle.Rows(intIndice), GrupoLimiteTraderDetalleBE.GrupoLimiteTraderDetalleRow)

            db.SetParameterValue(dbCommand, "@p_CodigoGrupLimTrader", IIf(indProc = "I", CodigoGrupLimTrader, oGrupoLimiteTraderDetalleRow.CodigoGrupLimTrader))
            db.SetParameterValue(dbCommand, "@p_Valor", oGrupoLimiteTraderDetalleRow.Valor)
            db.SetParameterValue(dbCommand, "@p_Situacion", oGrupoLimiteTraderDetalleRow.Situacion)
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

    End Function
#End Region
End Class
