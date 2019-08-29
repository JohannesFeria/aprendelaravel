'Creado por: HDG OT 62087 Nro10-R19 20110310
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class NivelesCoberturaDAM

    Public Sub New()

    End Sub

    Public Function Insertar(codigotercero As String, situacion As String, dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_ins_NivelesCobertura_Insertar_sura")


        Dim i As Integer = 0
        Dim detalleXml As String = ""
        detalleXml = "<Detalles>"
        For i = 0 To dtDetalle.Rows.Count - 1
            detalleXml &= "<Detalle>"
            detalleXml &= "<CodigoPortafolio>" & dtDetalle.Rows(i)("CodigoPortafolio").ToString.Trim & "</CodigoPortafolio>"
            detalleXml &= "<Threshold>" & dtDetalle.Rows(i)("Threshold").ToString.Trim & "</Threshold>"
            detalleXml &= "<MTA>" & dtDetalle.Rows(i)("MTA").ToString.Trim & "</MTA>"
            detalleXml &= "</Detalle>"
        Next
        detalleXml &= "</Detalles>"

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigotercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_detalle", DbType.Xml, detalleXml)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)

        Return True


        'Dim oRow As NivelesCoberturaBE.NivelesCoberturaRow

        'oRow = DirectCast(oNivelesCoberturaBE.NivelesCobertura.Rows(0), NivelesCoberturaBE.NivelesCoberturaRow)

        'db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        'db.AddInParameter(dbCommand, "@p_ThresholdF1", DbType.String, oRow.ThresholdF1)
        'db.AddInParameter(dbCommand, "@p_MTAF1", DbType.String, oRow.MTAF1)
        'db.AddInParameter(dbCommand, "@p_ThresholdF2", DbType.String, oRow.ThresholdF2)
        'db.AddInParameter(dbCommand, "@p_MTAF2", DbType.String, oRow.MTAF2)
        'db.AddInParameter(dbCommand, "@p_ThresholdF3", DbType.String, oRow.ThresholdF3)
        'db.AddInParameter(dbCommand, "@p_MTAF3", DbType.String, oRow.MTAF3)
        'db.AddInParameter(dbCommand, "@p_ThresholdCP", DbType.String, oRow.ThresholdCP)
        'db.AddInParameter(dbCommand, "@p_MTACP", DbType.String, oRow.MTACP)

        'db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        'db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        'db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        'db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal situacion As String) As NivelesCoberturaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_NivelesCobertura_SeleccionarporFiltro")
        Dim oNivelesCoberturaBE As New NivelesCoberturaBE

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oNivelesCoberturaBE, "NivelesCobertura")

        Return oNivelesCoberturaBE

    End Function

    Public Function SeleccionarPorFiltro_sura(ByVal codigoTercero As String, ByVal situacion As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_NivelesCobertura_SeleccionarporFiltro_sura")


        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Return db.ExecuteDataSet(dbCommand).Tables(0)

    End Function

    Public Function SeleccionarPorFiltroDetalle_sura(ByVal codigoTercero As String, ByVal situacion As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_NivelesCobertura_SeleccionarporFiltroDetalle_sura")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    Public Function Seleccionar(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelesCobertura_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)
        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function seleccionarPortafoliosCobertura_sura() As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_sel_NivelesCobertura_fondos")

        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    Public Function SeleccionarPorCodigoIntermediario(ByVal codigoIntermediario As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelesCobertura_SeleccionarPorCodigoIntermediario")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoContacto(ByVal codigoContacto As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelesCobertura_SeleccionarPorCodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelesCobertura_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Modificar(ByVal oNivelesCoberturaBE As NivelesCoberturaBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_NivelesCobertura_Modificar")
        Dim oRow As NivelesCoberturaBE.NivelesCoberturaRow

        oRow = DirectCast(oNivelesCoberturaBE.NivelesCobertura.Rows(0), NivelesCoberturaBE.NivelesCoberturaRow)

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, oRow.CodigoTercero)
        db.AddInParameter(dbCommand, "@p_ThresholdF1", DbType.String, oRow.ThresholdF1)
        db.AddInParameter(dbCommand, "@p_MTAF1", DbType.String, oRow.MTAF1)
        db.AddInParameter(dbCommand, "@p_ThresholdF2", DbType.String, oRow.ThresholdF2)
        db.AddInParameter(dbCommand, "@p_MTAF2", DbType.String, oRow.MTAF2)
        db.AddInParameter(dbCommand, "@p_ThresholdF3", DbType.String, oRow.ThresholdF3)
        db.AddInParameter(dbCommand, "@p_MTAF3", DbType.String, oRow.MTAF3)
        db.AddInParameter(dbCommand, "@p_ThresholdCP", DbType.String, oRow.ThresholdCP)
        db.AddInParameter(dbCommand, "@p_MTACP", DbType.String, oRow.MTACP)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Modificar_sura(codigotercero As String, situacion As String, dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_NivelesCobertura_Modificar_sura")

        Dim i As Integer = 0
        Dim detalleXml As String = ""
        detalleXml = "<Detalles>"
        For i = 0 To dtDetalle.Rows.Count - 1
            detalleXml &= "<Detalle>"
            detalleXml &= "<CodigoPortafolio>" & dtDetalle.Rows(i)("CodigoPortafolio").ToString.Trim & "</CodigoPortafolio>"
            detalleXml &= "<Threshold>" & dtDetalle.Rows(i)("Threshold").ToString.Trim & "</Threshold>"
            detalleXml &= "<MTA>" & dtDetalle.Rows(i)("MTA").ToString.Trim & "</MTA>"
            detalleXml &= "</Detalle>"
        Next
        detalleXml &= "</Detalles>"

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigotercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_detalle", DbType.Xml, detalleXml)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal codigoTercero As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_del_NivelesCobertura_Eliminar_sura")

        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, codigoTercero)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function EliminarPorCodigoIntermediario(ByVal codigoIntermediario As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelesCobertura_EliminarPorCodigoIntermediario")

        db.AddInParameter(dbCommand, "@p_CodigoIntermediario", DbType.String, codigoIntermediario)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function EliminarPorCodigoContacto(ByVal codigoContacto As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("NivelesCobertura_EliminarPorCodigoContacto")

        db.AddInParameter(dbCommand, "@p_CodigoContacto", DbType.String, codigoContacto)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#Region "Funciones Alberto"
    Public Function ListarContactosXIntermediario(ByVal CodigoTercero As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("ListarComboContactosxIntermediario")
        db.AddInParameter(dbCommand, "@p_CodigoTercero", DbType.String, CodigoTercero)
        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region

End Class
