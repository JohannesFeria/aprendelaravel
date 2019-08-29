Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VectorTipoCambioDAM
    Private oVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow
    Public Sub New()
    End Sub
    'OT11004 - 21/12/2017 - Ian Pastor M.
    'Descripcion: Refactorizar código
    Public Function Insertar(ByVal objVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As Boolean
        Insertar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_Insertar")
            dbCommand.CommandTimeout = 1020
            oVectorTipoCambio = CType(objVectorTipoCambio, VectorTipoCambio.VectorTipoCambioRow)
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oVectorTipoCambio.Fecha)
            db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, oVectorTipoCambio.Valor)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oVectorTipoCambio.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, objVectorTipoCambio.EntidadExt)
            db.ExecuteNonQuery(dbCommand)
            Insertar = True
        End Using
    End Function
    Public Sub Actualiza_ValorCambioMoneda(ByVal Fecha As Decimal, ByVal entidadExt As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_upd_ValorCambioMoneda")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, entidadExt)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    Public Sub Borrar_TipoCambio(Fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_delVectorTipoCambio")
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Sub Copiar_TipoCambio(ByVal Fecha As Decimal, ByVal entidadExt As String)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_CopiaVectorTipoCambio")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, entidadExt)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub
    'OT11004 - Fin
    Public Function InsertarTipoCambioSPOT(ByVal objVectorTipoCambio As VectorTipoCambio, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioSPOT_Insertar")
        dbCommand.CommandTimeout = 1020
        oVectorTipoCambio = CType(objVectorTipoCambio.VectorTipoCambio.Rows(0), VectorTipoCambio.VectorTipoCambioRow)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oVectorTipoCambio.Fecha)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, oVectorTipoCambio.ValorPrimario)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oVectorTipoCambio.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function InsertarTipoCambioSBS(ByVal objVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioSBS_Insertar")
        oVectorTipoCambio = CType(objVectorTipoCambio, VectorTipoCambio.VectorTipoCambioRow)
        dbCommand.CommandTimeout = 1020
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oVectorTipoCambio.Fecha)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.Decimal, oVectorTipoCambio.Valor)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oVectorTipoCambio.CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function InsertarPorMantenimiento(ByVal ob As VectorTipoCambio, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambioMantenimiento_Insertar")
            oVectorTipoCambio = CType(ob.VectorTipoCambio.Rows(0), VectorTipoCambio.VectorTipoCambioRow)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, oVectorTipoCambio.Fecha)
            db.AddInParameter(dbCommand, "@Valor", DbType.Decimal, oVectorTipoCambio.Valor)
            db.AddInParameter(dbCommand, "@ValorPrimario", DbType.Decimal, oVectorTipoCambio.ValorPrimario)
            db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, oVectorTipoCambio.EntidadExt)
            db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oVectorTipoCambio.CodigoMoneda)
            db.AddInParameter(dbCommand, "@Manual", DbType.String, oVectorTipoCambio.Manual)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function Modificar(ByVal ob As VectorTipoCambio, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_Modificar")
            oVectorTipoCambio = CType(ob.VectorTipoCambio.Rows(0), VectorTipoCambio.VectorTipoCambioRow)
            db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oVectorTipoCambio.CodigoMoneda)
            db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, oVectorTipoCambio.Fecha)
            db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, oVectorTipoCambio.EntidadExt)
            db.AddInParameter(dbCommand, "@Valor", DbType.Decimal, oVectorTipoCambio.Valor)
            db.AddInParameter(dbCommand, "@ValorPrimario", DbType.Decimal, oVectorTipoCambio.ValorPrimario)
            db.AddInParameter(dbCommand, "@Manual", DbType.String, oVectorTipoCambio.Manual)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function ModificarFila(ByVal oVTC As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_Modificar")
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, oVTC.CodigoMoneda)
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, oVTC.Fecha)
        db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, oVTC.EntidadExt)
        db.AddInParameter(dbCommand, "@Valor", DbType.Decimal, oVTC.Valor)
        db.AddInParameter(dbCommand, "@ValorPrimario", DbType.Decimal, oVTC.ValorPrimario)
        db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function ModificarTipoCambioSBS(ByVal objVectorTipoCambio As VectorTipoCambio.VectorTipoCambioRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCambioSBS_Modificar")
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, objVectorTipoCambio.CodigoMoneda)
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, objVectorTipoCambio.Fecha)
        db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, objVectorTipoCambio.EntidadExt)
        db.AddInParameter(dbCommand, "@Valor", DbType.Decimal, objVectorTipoCambio.Valor)
        db.AddInParameter(dbCommand, "@ValorPrimario", DbType.Decimal, objVectorTipoCambio.ValorPrimario)
        db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function Listar(ByVal sFecha As String, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_Listar")
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, sFecha)
        db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, sEntidadExt)
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarTipoCambio(ByVal sFecha As String, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_Seleccionar")
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, sFecha)
        db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, sEntidadExt)
        Dim oVectorTipoCambio As New DataSet
        db.LoadDataSet(dbCommand, oVectorTipoCambio, "VectorTipoCambio")
        Return oVectorTipoCambio
    End Function
    Public Function SeleccionarPorFecha(ByVal fecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_SeleccionarPorFecha")
        dbCommand.CommandTimeout = 1020  'HDG 20120103
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, fecha)
        Dim oVectorTipoCambio As New DataSet
        db.LoadDataSet(dbCommand, oVectorTipoCambio, "VectorTipoCambio")
        Return oVectorTipoCambio
    End Function
    'OT11004 - 21/12/2017 - Ian Pastor M.
    'Descripcion: Refactorizar código
    Public Function EliminarTipoCambioReal_SBS(ByVal FechaCarga As Decimal, ByVal entidadExt As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_EliminarReal_SBS")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, FechaCarga)
            db.AddInParameter(dbCommand, "@p_EntidadExt", DbType.String, entidadExt)
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    'OT11004 - Fin
    Public Function EliminarTipoCambioReal(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_EliminarSBS")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function EliminarTipoCambioSBS(ByVal FechaCarga As Date) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("TipoCambioSBS_Eliminar")
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(FechaCarga))
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Validar_VectorTipoCambio(ByVal Fecha As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_Valida")
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, Fecha)
        Return Convert.ToInt16(db.ExecuteScalar(dbCommand))
    End Function
    Public Function SeleccionarTipoCambio(ByVal Fecha As Decimal, ByVal sEntidadExt As String, ByVal sCodigoMoneda As String) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("VectorTipoCambio_SeleccionarValorPrimario")
        db.AddInParameter(dbCommand, "@CodigoMoneda", DbType.String, sCodigoMoneda)
        db.AddInParameter(dbCommand, "@Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@EntidadExt", DbType.String, sEntidadExt)
        Dim TipoCambio As New Decimal
        TipoCambio = Convert.ToDecimal(db.ExecuteScalar(dbCommand))
        Return TipoCambio
    End Function
    Public Function TasaLibor_EliminarVectorCarga_PIP(ByVal FechaCarga As Decimal, ByVal fuente As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TasaLibor_EliminarVectorCarga_PIP")
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, FechaCarga)
            db.AddInParameter(dbCommand, "@p_Fuente", DbType.String, fuente)

            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function
    Public Function TasaLibor_InsertarVectorCarga_PIP(ByVal plazoCurva As Integer, ByVal valorCurva As Decimal, ByVal fechaVector As Decimal, ByVal codigoIndicador As String, ByVal dataRequest As DataSet) As Boolean
        TasaLibor_InsertarVectorCarga_PIP = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TasaLibor_InsertarVectorCarga_PIP")
            dbCommand.CommandTimeout = 1020
            db.AddInParameter(dbCommand, "@p_fechaVector", DbType.Decimal, fechaVector)
            db.AddInParameter(dbCommand, "@p_plazoCurva", DbType.Int32, plazoCurva)
            db.AddInParameter(dbCommand, "@p_valorCurva", DbType.Decimal, valorCurva)
            db.AddInParameter(dbCommand, "@p_codigoIndicador", DbType.String, codigoIndicador)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            TasaLibor_InsertarVectorCarga_PIP = True
        End Using
    End Function
End Class