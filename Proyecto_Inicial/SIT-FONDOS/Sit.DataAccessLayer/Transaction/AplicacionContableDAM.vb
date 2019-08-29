Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities


''' <summary>
''' Clase para el acceso de los datos para AplicacionContable tabla.
''' </summary>
Public Class AplicacionContableDAM

    Private oAplicacionContableRow As AplicacionContableBE.AplicacionContableRow

    Public Sub New()
    End Sub

#Region " /* Seleccionar Functions */ "
    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de AplicacionContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="Situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>AplicacionContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As AplicacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AplicacionContable_SeleccionarPorFiltro")
        Dim objeto As New AplicacionContableBE

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, objeto, "AplicacionContable")
        Return objeto
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de AplicacionContable tabla.
    ''' <summary>
    ''' <param name="codigoAplicacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>AplicacionContableBE</returns>
    Public Function Seleccionar(ByVal codigoAplicacionContable As String, ByVal dataRequest As DataSet) As AplicacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AplicacionContable_Seleccionar")

        Dim objeto As New AplicacionContableBE
        db.AddInParameter(dbCommand, "@p_CodigoAplicacionContable", DbType.String, codigoAplicacionContable)
        db.LoadDataSet(dbCommand, objeto, "AplicacionContable")
        Return objeto
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de AplicacionContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>AplicacionContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As AplicacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AplicacionContable_Listar")

        Dim objeto As New AplicacionContableBE
        db.LoadDataSet(dbCommand, objeto, "AplicacionContable")
        Return objeto
    End Function
#End Region

#Region " /* Insertar Functions */ "

    ''' <summary>
    ''' Inserta un expediente en AplicacionContable tabla.
    ''' <summary>
    ''' <param name="ob">AplicacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As AplicacionContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AplicacionContable_Insertar")
        Dim Codigo As String = String.Empty
        oAplicacionContableRow = CType(ob.AplicacionContable.Rows(0), AplicacionContableBE.AplicacionContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoAplicacionContable", DbType.String, oAplicacionContableRow.CodigoAplicacionContable)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oAplicacionContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAplicacionContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return "True"
    End Function
#End Region

#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en AplicacionContable tabla.
    ''' <summary>
    ''' <param name="ob">AplicacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>    
    Public Function Modificar(ByVal ob As AplicacionContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AplicacionContable_Modificar")
        oAplicacionContableRow = CType(ob.AplicacionContable.Rows(0), AplicacionContableBE.AplicacionContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoAplicacionContable", DbType.String, oAplicacionContableRow.CodigoAplicacionContable)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oAplicacionContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAplicacionContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de AplicacionContable table por una llave primaria compuesta.
    ''' <summary>
    ''  <param name="codigoAplicacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoAplicacionContable As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AplicacionContable_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoAplicacionContable", DbType.String, codigoAplicacionContable)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function
#End Region


End Class

