Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class CarteraIndirectaDAM

    Public Sub New()

    End Sub
    Public Function SeleccionarPorFiltro(ByVal CodigoCarteraI As String, ByVal FechaCarteraI As Decimal, ByVal GrupoEconomico As String, ByVal Fondo As String, ByVal Emisor As String, ByVal Tipo As String, ByVal CodigoPortafolio As String, ByVal CodigoEntidad As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SP_CarteraIndirecta_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoCarteraIndirecta", DbType.String, CodigoCarteraI)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, FechaCarteraI)
        db.AddInParameter(dbCommand, "@p_GrupoEconomico", DbType.String, GrupoEconomico)
        db.AddInParameter(dbCommand, "@p_Fondo", DbType.String, Fondo)
        db.AddInParameter(dbCommand, "@p_Emisor", DbType.String, Emisor)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
        Return db.ExecuteDataSet(dbCommand)
        'Dim objeto As New ObligacionTecnicaBE
        'db.LoadDataSet(dbCommand, objeto, "ObligacionTecnica")
        'Return objeto
    End Function

    Public Function Eliminar(ByVal codigoCarteraIndirecta As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CarteraIndirecta_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoCarteraIndirecta", DbType.String, codigoCarteraIndirecta)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    Public Function Insertar(ByVal ob As CarteraIndirectaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CarteraIndirecta_Insertar")
        'oObligacionTecnicaRow = CType(ob.ObligacionTecnica.Rows(0), ObligacionTecnicaBE.ObligacionTecnicaRow)

        'db.AddInParameter(dbCommand, "@p_CodigoObligacionTecnica", DbType.String, oObligacionTecnicaRow.CodigoObligacionTecnica)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, ob.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, ob.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, ob.CodigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_CodigoActividadEconomica", DbType.String, ob.CodigoActividadEconomica)
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, ob.CodigoPais)
        db.AddInParameter(dbCommand, "@p_Rating", DbType.String, ob.Rating)
        db.AddInParameter(dbCommand, "@p_Posicion", DbType.Decimal, ob.Posicion)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, ob.Patrimonio)
        db.AddInParameter(dbCommand, "@p_Participacion", DbType.Decimal, ob.Participacion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ob.Fecha)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, ob.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As CarteraIndirectaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CarteraIndirecta_Modificar")

        'oObligacionTecnicaRow = CType(ob.ObligacionTecnica.Rows(0), ObligacionTecnicaBE.ObligacionTecnicaRow)
        db.AddInParameter(dbCommand, "@p_CodigoCarteraIndirecta", DbType.String, ob.CodigoCarteraIndirecta)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, ob.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, ob.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, ob.CodigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_CodigoActividadEconomica", DbType.String, ob.CodigoActividadEconomica)
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, ob.CodigoPais)
        db.AddInParameter(dbCommand, "@p_Rating", DbType.String, ob.Rating)
        db.AddInParameter(dbCommand, "@p_Posicion", DbType.Decimal, ob.Posicion)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, ob.Patrimonio)
        db.AddInParameter(dbCommand, "@p_Participacion", DbType.Decimal, ob.Participacion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ob.Fecha)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, ob.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Listar_DatosEntidad(ByVal CodigoEntidad As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CarteraIndirecta_DatosEntidad_Listar")
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, CodigoEntidad)
        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function DesactivarRegistrosExcel(ByVal Fecha As Decimal, ByVal dataRequest As DataSet, ByRef strMensaje As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommandInactivar As DbCommand = db.GetStoredProcCommand("sp_CarteraIndirecta_DesactivarRegistros")

            db.AddInParameter(dbCommandInactivar, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommandInactivar, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommandInactivar, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommandInactivar, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommandInactivar, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommandInactivar)

        Catch ex As Exception
            strMensaje = "error al desactivar los registros de la fecha: " + Convert.ToString(Fecha)
        End Try


    End Function

    Public Function InsertarRegistrosExcel(ByVal ob As CarteraIndirectaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_CarteraIndirecta_InsertarExcel")
        'oObligacionTecnicaRow = CType(ob.ObligacionTecnica.Rows(0), ObligacionTecnicaBE.ObligacionTecnicaRow)

        'db.AddInParameter(dbCommand, "@p_CodigoObligacionTecnica", DbType.String, oObligacionTecnicaRow.CodigoObligacionTecnica)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, ob.CodigoPortafolio)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, ob.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_CodigoGrupoEconomico", DbType.String, ob.CodigoGrupoEconomico)
        db.AddInParameter(dbCommand, "@p_CodigoActividadEconomica", DbType.String, ob.CodigoActividadEconomica)
        db.AddInParameter(dbCommand, "@p_CodigoPais", DbType.String, ob.CodigoPais)
        db.AddInParameter(dbCommand, "@p_Rating", DbType.String, ob.Rating)
        db.AddInParameter(dbCommand, "@p_Posicion", DbType.Decimal, ob.Posicion)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, ob.Patrimonio)
        db.AddInParameter(dbCommand, "@p_Participacion", DbType.Decimal, ob.Participacion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, ob.Fecha)
        'db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, ob.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function
End Class
