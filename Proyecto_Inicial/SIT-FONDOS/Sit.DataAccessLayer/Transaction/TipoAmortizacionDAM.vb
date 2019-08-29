Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class TipoAmortizacionDAM

    Private sqlCommand As String = ""
    Private oRow As TipoAmortizacionBE.TipoAmortizacionRow
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function SeleccionarPorFiltros(ByVal situacion As String, ByVal descripcion As String, ByVal dataRequest As DataSet) As TipoAmortizacionBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_SeleccionarPorFiltro")
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)

        db.LoadDataSet(dbCommand, oTipoAmortizacionBE, "TipoAmortizacion")

        Return oTipoAmortizacionBE

    End Function

    Public Function Seleccionar(ByVal codigoTipoAmortizacion As String, ByVal dataRequest As DataSet) As TipoAmortizacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, codigoTipoAmortizacion)
        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        db.LoadDataSet(dbCommand, oTipoAmortizacionBE, "TipoAmortizacion")
        Return oTipoAmortizacionBE

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de TipoAmortizacionBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As TipoAmortizacionBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_Listar")

        Dim oTipoAmortizacionBE As New TipoAmortizacionBE
        db.LoadDataSet(dbCommand, oTipoAmortizacionBE, "TipoAmortizacion")
        Return oTipoAmortizacionBE

    End Function
#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoAmortizacionBE As TipoAmortizacionBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_Insertar")

        oRow = DirectCast(oTipoAmortizacionBE.TipoAmortizacion.Rows(0), TipoAmortizacionBE.TipoAmortizacionRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, oRow.CodigoTipoAmortizacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_NumeroDias", DbType.Int16, oRow.NumeroDias)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"


    Public Function Modificar(ByVal oTipoAmortizacionBE As TipoAmortizacionBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_Modificar")
        oRow = CType(oTipoAmortizacionBE.TipoAmortizacion.Rows(0), TipoAmortizacionBE.TipoAmortizacionRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, oRow.CodigoTipoAmortizacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_NumeroDias", DbType.Int16, oRow.NumeroDias)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function


#End Region

#Region " /* Funciones Eliminar */"

    Public Function Eliminar(ByVal codigoTipoAmortizacion As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoAmortizacion_Eliminar")
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_CodigoTipoAmortizacion", DbType.String, codigoTipoAmortizacion)

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function
#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

   
End Class

