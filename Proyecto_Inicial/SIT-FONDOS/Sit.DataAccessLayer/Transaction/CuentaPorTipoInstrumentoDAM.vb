Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class CuentaPorTipoInstrumentoDAM
    Private sqlCommand As String = ""
    Private oRow As CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltro(ByVal codigoInstrumentoSBS As String, ByVal codigoMoneda As String, ByVal grupoContable As String, ByVal situacion As String, ByVal portafolio As String, ByVal dataRequest As DataSet) As CuentasPorTipoInstrumentoBE
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentaContableTipoInstrumento_SeleccionarPorFiltro")

            db.AddInParameter(dbCommand, "@p_CodigoInstrumentoSBS", DbType.String, codigoInstrumentoSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, codigoMoneda)
            db.AddInParameter(dbCommand, "@p_GrupoContable", DbType.String, grupoContable)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, portafolio)

            Dim oCuentasPorTipoInstrumentoBE As New CuentasPorTipoInstrumentoBE
            db.LoadDataSet(dbCommand, oCuentasPorTipoInstrumentoBE, "CuentasPorTipoInstrumento")
            Return oCuentasPorTipoInstrumentoBE
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Seleccionar(ByVal Secuencial As Int32, ByVal dataRequest As DataSet) As CuentasPorTipoInstrumentoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentaContableTipoInstrumento_Seleccionar")

        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Int32, Secuencial)

        Dim oCuentasPorTipoInstrumentoBE As New CuentasPorTipoInstrumentoBE
        db.LoadDataSet(dbCommand, oCuentasPorTipoInstrumentoBE, "CuentasPorTipoInstrumento")
        Return oCuentasPorTipoInstrumentoBE

    End Function

    'RGF 20090113
    Public Function Listar(ByVal portafolio As String) As DataTable

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentaContableTipoInstrumento_Listar")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)

        Dim ds As New DataSet
        db.LoadDataSet(dbCommand, ds, "CuentasPorTipoInstrumento")
        Return ds.Tables(0)

    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oCuentasPorTipoInstrumentoBE As CuentasPorTipoInstrumentoBE, ByVal dataRequest As DataSet) As String
        Try
            'Dim Codigo As String = String.Empty
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentaContableTipoInstrumento_Insertar")

            oRow = CType(oCuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumento.Rows(0), CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow)

            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumentoSBS", DbType.String, oRow.CodigoTipoInstrumentoSBS)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_GrupoContable", DbType.String, oRow.GrupoContable)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oRow.CuentaContable)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oRow.CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.ExecuteNonQuery(dbCommand)
        Catch ex As Exception
            Throw ex
        End Try
        'Return Codigo
    End Function

#End Region

#Region " /* Funciones Modificar */"

    Public Function Modificar(ByVal oCuentasPorTipoInstrumentoBE As CuentasPorTipoInstrumentoBE, ByVal dataRequest As DataSet) As Boolean

        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommand As DbCommand = db.GetStoredProcCommand("CuentaContableTipoInstrumento_Modificar")
            oRow = CType(oCuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumento.Rows(0), CuentasPorTipoInstrumentoBE.CuentasPorTipoInstrumentoRow)
            db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Int32, oRow.Secuencial)
            db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oRow.CodigoMoneda)
            db.AddInParameter(dbCommand, "@p_CuentaContable", DbType.String, oRow.CuentaContable)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oRow.CodigoPortafolio)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
            db.ExecuteNonQuery(dbCommand)
            Return True
        Catch ex As Exception
            Throw ex
        End Try
        
    End Function


#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal secuencial As Int32, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("CuentaContableTipoInstrumento_Eliminar")
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.Int32, secuencial)
        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

#End Region

#Region " /* Funciones Personalizadas*/"

#End Region


End Class
