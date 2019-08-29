Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Data.Common

Public Class AumentoCapitalDAM
#Region "Variables"
    Private sqlCommand As String = String.Empty
    Private oRow As AumentoCapitalBE
    Dim DECIMAL_NULO As Decimal = 0.0
#End Region
    Public Sub New()

    End Sub
#Region " /* Funciones Seleccionar */ "

    Public Function AumentoCapital_CalcularDistribucion(ByVal CategoriaInstrumento As String, ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As DataTable
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("AumentoCapital_CalcularDistribucion")
            bd.AddInParameter(dbcomand, "@p_CategoriaInstrumento", DbType.String, CategoriaInstrumento)
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            bd.AddInParameter(dbcomand, "@p_FechaAumentoCapital", DbType.Decimal, FechaAumentoCapital)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Function AumentoCapital_ExistePendientebyFechaPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("AumentoCapital_ExistePendientebyFechaPortafolio")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaAumentoCapital", DbType.Decimal, FechaAumentoCapital)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function

    Public Function AumentoCapital_ListarbyFechaPortafolio(ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As DataSet
        Dim bd As Database = DatabaseFactory.CreateDatabase()
        Using dbcomand As DbCommand = bd.GetStoredProcCommand("AumentoCapital_ListarbyFechaPortafolio")
            bd.AddInParameter(dbcomand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
            bd.AddInParameter(dbcomand, "@p_FechaAumentoCapital", DbType.Decimal, FechaAumentoCapital)
            Using ds As DataSet = bd.ExecuteDataSet(dbcomand)
                Return ds
            End Using
        End Using
    End Function

    Public Function AumentoCapital_ExisteGeneradaOI(ByVal CodigoPortafolioSBS As String, ByVal FechaAumentoCapital As Decimal) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("AumentoCapital_ExisteGeneradaOI")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_FechaAumentoCapital", DbType.Decimal, FechaAumentoCapital)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
#End Region

#Region " /* Funciones Insertar */ "
    Public Function AumentoCapital_Insertar(ByVal oRowAC As AumentoCapitalBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AumentoCapital_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRowAC.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaAumentoCapital", DbType.Decimal, oRowAC.FechaAumentoCapital)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, oRowAC.Importe)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRowAC.Estado)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Result"), String)
        End Using
    End Function
#End Region

#Region " /* Funciones Modificar */"
    Public Function AumentoCapital_Modificar(ByVal oRowAC As AumentoCapitalBE, ByVal fechaAumentoCapitalOriginal As Decimal, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AumentoCapital_Modificar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRowAC.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaAumentoCapital", DbType.Decimal, fechaAumentoCapitalOriginal)
            db.AddInParameter(dbCommand, "@p_FechaAumentoCapitalNuevo", DbType.Decimal, oRowAC.FechaAumentoCapital)
            db.AddInParameter(dbCommand, "@p_Importe", DbType.Decimal, oRowAC.Importe)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRowAC.Estado)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Result"), String)
        End Using
    End Function

    Public Function AumentoCapital_ActualizarGastoComision(ByVal codigoPortafolioSBS As String, ByVal FechaProceso As Decimal, ByVal DiferenciaComision As Decimal, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AumentoCapital_ActualizarGastoComision")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.String, FechaProceso)
            db.AddInParameter(dbCommand, "@p_DiferenciaComision", DbType.String, DiferenciaComision)
            db.AddInParameter(dbCommand, "@p_Usuario", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Result"), String)
        End Using
    End Function
#End Region

#Region " /* Funciones Eliminar */"
    Public Function AumentoCapital_Eliminar(ByVal oRowAC As AumentoCapitalBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("AumentoCapital_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRowAC.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@p_FechaAumentoCapital", DbType.Decimal, oRowAC.FechaAumentoCapital)
            db.AddInParameter(dbCommand, "@p_Estado", DbType.String, oRowAC.Estado)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddOutParameter(dbCommand, "@p_Result", DbType.String, 12)
            db.ExecuteNonQuery(dbCommand)
            Return CType(db.GetParameterValue(dbCommand, "@p_Result"), String)
        End Using
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub InicializarAumentoCapital(ByRef oRowAC As AumentoCapitalBE)
        oRowAC.CodigoPortafolioSBS = String.Empty
        oRowAC.FechaAumentoCapital = DECIMAL_NULO
        oRowAC.Importe = DECIMAL_NULO
        oRowAC.Estado = String.Empty
        oRowAC.CodigoOperacionCaja = String.Empty
        oRowAC.UsuarioCreacion = String.Empty
        oRowAC.FechaCreacion = DECIMAL_NULO
        oRowAC.HoraCreacion = String.Empty
        oRowAC.UsuarioModificacion = String.Empty
        oRowAC.FechaModificacion = DECIMAL_NULO
        oRowAC.HoraModificacion = String.Empty
    End Sub

#End Region
End Class
