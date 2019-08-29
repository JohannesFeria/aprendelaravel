Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



	''' <summary>
	''' Clase para el acceso de los datos para LimiteInstrumento tabla.
	''' </summary>
	Public class LimiteInstrumentoDAM
    Private sqlCommand As String = ""
    Private oRow As LimiteInstrumentoBE.LimiteInstrumentoRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "



   
    Public Function Seleccionar(ByVal StrCodigoLimite As String, ByVal StrCodigoPosicion As String, ByVal dataRequest As DataSet) As LimiteInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteInstrumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, StrCodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPosicion", DbType.String, StrCodigoPosicion)

        Dim oLimiteInstrumentoBE As New LimiteInstrumentoBE
        db.LoadDataSet(dbCommand, oLimiteInstrumentoBE, "LimiteInstrumento")
        Return oLimiteInstrumentoBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de ValoresBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As LimiteInstrumentoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteInstrumento_Listar")

        Dim oLimiteInstrumentoBE As New LimiteInstrumentoBE
        db.LoadDataSet(dbCommand, oLimiteInstrumentoBE, "LimiteInstrumento")
        Return oLimiteInstrumentoBE

    End Function



    Public Function SeleccionarPorFiltro(ByVal StrCodigoLimite As String, ByVal StrCodigoPosicion As String, ByVal StrSituacion As String, ByVal dataRequest As DataSet) As LimiteInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteInstrumento_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, StrCodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPosicion", DbType.String, StrCodigoPosicion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, StrSituacion)


        Dim oLimiteInstrumentoBE As New LimiteInstrumentoBE
        db.LoadDataSet(dbCommand, oLimiteInstrumentoBE, "LimiteInstrumento")
        Return oLimiteInstrumentoBE

    End Function
#End Region

#Region "/* Funciones Insertar */"
    Public Function Insertar(ByVal oLimiteInstrumentoBE As LimiteInstrumentoBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteInstrumento_Insertar")
        oRow = CType(oLimiteInstrumentoBE.LimiteInstrumento.Rows(0), LimiteInstrumentoBE.LimiteInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, oRow.CodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPosicion", DbType.String, oRow.CodigoPosicion)
        db.AddInParameter(dbCommand, "@p_DescripcionPosicion", DbType.String, oRow.DescripcionPosicion)
        db.AddInParameter(dbCommand, "@p_TablaInstrumento", DbType.String, oRow.TablaInstrumento)
        db.AddInParameter(dbCommand, "@p_CampoInstrumento", DbType.String, oRow.CampoInstrumento)
        db.AddInParameter(dbCommand, "@p_CriterioTabla", DbType.String, oRow.CriterioTabla)
        db.AddInParameter(dbCommand, "@p_CriterioCampo", DbType.String, oRow.CriterioCampo)
        db.AddInParameter(dbCommand, "@p_CriterioValor", DbType.String, oRow.CriterioValor)
        db.AddInParameter(dbCommand, "@p_Relacionador", DbType.String, oRow.Relacionador)
        db.AddInParameter(dbCommand, "@p_SecuencialPosicion", DbType.String, oRow.SecuencialPosicion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function
#End Region

#Region "/*Funciones Modificar*/"
    Public Function Modificar(ByVal oLimiteInstrumentoBE As LimiteInstrumentoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteInstrumento_Modificar")

        oRow = CType(oLimiteInstrumentoBE.LimiteInstrumento.Rows(0), LimiteInstrumentoBE.LimiteInstrumentoRow)

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, oRow.CodigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPosicion", DbType.String, oRow.CodigoPosicion)
        db.AddInParameter(dbCommand, "@p_DescripcionPosicion", DbType.String, oRow.DescripcionPosicion)
        db.AddInParameter(dbCommand, "@p_TablaInstrumento", DbType.String, oRow.TablaInstrumento)
        db.AddInParameter(dbCommand, "@p_CampoInstrumento", DbType.String, oRow.CampoInstrumento)
        db.AddInParameter(dbCommand, "@p_CriterioTabla", DbType.String, oRow.CriterioTabla)
        db.AddInParameter(dbCommand, "@p_CriterioCampo", DbType.String, oRow.CriterioCampo)
        db.AddInParameter(dbCommand, "@p_CriterioValor", DbType.String, oRow.CriterioValor)
        db.AddInParameter(dbCommand, "@p_Relacionador", DbType.String, oRow.Relacionador)
        db.AddInParameter(dbCommand, "@p_SecuencialPosicion", DbType.String, oRow.SecuencialPosicion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region "/* Funciones Eliminar */"
    Public Function Eliminar(ByVal codigoLimite As String, ByVal codigoPosicion As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("LimiteInstrumento_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoLimite", DbType.String, codigoLimite)
        db.AddInParameter(dbCommand, "@p_CodigoPosicion", DbType.String, codigoPosicion)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

End Class

