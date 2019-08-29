Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class TipoTituloDAM
    Private sqlCommand As String = ""
    Private oRow As TipoTituloBE.TipoTituloRow
    Public Sub New()
    End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function SeleccionarPorFiltro(ByVal codigoTipoTitulo As String, ByVal codigoMoneda As String, ByVal codigoTipoInstrumento As String,
    ByVal descripcion As String, ByVal codigo As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, codigo)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, codigoTipoInstrumento)
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    Public Function Seleccionar(ByVal codigoTipoTitulo As String, ByVal dataRequest As DataSet) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, codigoTipoTitulo)
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    Public Function Listar(ByVal dataRequest As DataSet) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_Listar")
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    Public Function ListarPorDepositoPlazo(ByVal dataRequest As DataSet) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_ListarPorDepositoPlazo")
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista tipo de titulo por categoria
    Public Function ListarTipoTitulo_CCI(ByVal Categoria As String) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("sp_SIT_lis_ListarTipoTitulo_CCI")
            db.AddInParameter(dbCommand, "@p_Categoria", DbType.String, Categoria)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function
    Public Function ObtenerFechaPorTipoTitulo(ByVal fechaOperacion As Decimal, ByVal codigoTipoInstrumentoSBS As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_RetornarFechaVencimiento")
            db.AddInParameter(dbCommand, "@p_fechaOperacion", DbType.Decimal, fechaOperacion)
            db.AddInParameter(dbCommand, "@p_codigoTipoInstrumentoSBS", DbType.String, codigoTipoInstrumentoSBS)
            Return db.ExecuteScalar(dbCommand)
        End Using
    End Function
    Public Function ListarOI(ByVal dataRequest As DataSet) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_ListarOI")
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    Public Function ListarPorCI(ByVal claseIns As String) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_ListarPorCI")
            db.AddInParameter(dbCommand, "@claseInstrumento", DbType.String, claseIns)
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    Public Function ListarPorTipoInstrumento(ByVal strCodigoTipoInstrumento As String) As TipoTituloBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_ListarPorTipoInstrumento")
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, strCodigoTipoInstrumento)
            Using oTipoTituloBE As New TipoTituloBE
                db.LoadDataSet(dbCommand, oTipoTituloBE, "TipoTitulo")
                Return oTipoTituloBE
            End Using
        End Using
    End Function
    Public Function ObtenerTasaEncaje(ByVal strCodigoTipoTitulo As String, ByVal strCodigoMnemonico As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TasaEncaje_Obtener")
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, strCodigoTipoTitulo)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, strCodigoMnemonico)
            Return db.ExecuteScalar(dbCommand)
        End Using
    End Function
#End Region
#Region " /* Funciones Insertar */ "
    Public Function Insertar(ByVal oTipoTituloBE As TipoTituloBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_Insertar")
            oRow = DirectCast(oTipoTituloBE.TipoTitulo.Rows(0), TipoTituloBE.TipoTituloRow)
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, oRow.CodigoTipoTitulo)
            db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, oRow.CodigoTipoRenta)
            db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, oRow.CodigoPeriodicidad)
            db.AddInParameter(dbCommand, "@p_CodigoIndicadorVAC", DbType.String, IIf(oRow.CodigoIndicadorVAC = "", DBNull.Value, oRow.CodigoIndicadorVAC))
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oRow.CodigoTipoInstrumentoSBS)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_BaseTir", DbType.Decimal, oRow.BaseTir)
            db.AddInParameter(dbCommand, "@p_BaseCupon", DbType.Decimal, oRow.BaseCupon)
            db.AddInParameter(dbCommand, "@p_BaseCuponDias", DbType.Decimal, oRow.BaseCuponDias)
            db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, oRow.CodigoTipoAmortizacion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, oRow.CodigoTipoCupon)
            db.AddInParameter(dbCommand, "@p_Observacion", DbType.String, oRow.Observacion)
            db.AddInParameter(dbCommand, "@p_TasaSpread", DbType.Decimal, oRow.TasaSpread)
            db.AddInParameter(dbCommand, "@p_BaseTirDias", DbType.Decimal, oRow.BaseTirDias)
            db.ExecuteNonQuery(dbCommand)
            Return Codigo
        End Using
    End Function
#End Region
#Region " /* Funciones Modificar */"
    Public Function Modificar(ByVal oTipoTituloBE As TipoTituloBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_Modificar")
            oRow = DirectCast(oTipoTituloBE.TipoTitulo.Rows(0), TipoTituloBE.TipoTituloRow)
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, oRow.CodigoTipoTitulo)
            db.AddInParameter(dbCommand, "@p_CodigoTipoRenta", DbType.String, oRow.CodigoTipoRenta)
            db.AddInParameter(dbCommand, "@p_CodigoPeriodicidad", DbType.String, oRow.CodigoPeriodicidad)
            db.AddInParameter(dbCommand, "@p_CodigoIndicadorVAC", DbType.String, IIf(oRow.CodigoIndicadorVAC = "", DBNull.Value, oRow.CodigoIndicadorVAC))
            db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oRow.CodigoTipoInstrumentoSBS)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.AddInParameter(dbCommand, "@p_BaseTir", DbType.Decimal, oRow.BaseTir)
            db.AddInParameter(dbCommand, "@p_BaseCupon", DbType.Decimal, oRow.BaseCupon)
            db.AddInParameter(dbCommand, "@p_BaseCuponDias", DbType.Decimal, oRow.BaseCuponDias)
            db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, oRow.CodigoTipoAmortizacion)
            db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, oRow.CodigoTipoCupon)
            db.AddInParameter(dbCommand, "@p_Observacion", DbType.String, oRow.Observacion)
            db.AddInParameter(dbCommand, "@p_TasaSpread", DbType.Decimal, oRow.TasaSpread)
            db.AddInParameter(dbCommand, "@p_BaseTirDias", DbType.Decimal, oRow.BaseTirDias)
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function
#End Region
#Region " /* Funciones Eliminar */"
    Public Function Eliminar(ByVal codigoTipoTitulo As String, ByVal dataRequest As DataSet) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("TipoTitulo_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, codigoTipoTitulo)
            db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function
#End Region
End Class