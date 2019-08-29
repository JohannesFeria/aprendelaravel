Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities
''' <summary>
''' Clase para el acceso de los datos para MatrizContable tabla.
''' </summary>
Public Class MatrizContableDAM
    Private oMatrizContableRow As MatrizContableBE.MatrizContableRow
    Public Sub New()
    End Sub
#Region " /* Seleccionar Functions */ "
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal MatrizFondo As String) As MatrizContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MatrizContable_SeleccionarPorFiltro")
        Dim objeto As New MatrizContableBE
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_MatrizFondo", DbType.String, MatrizFondo)
        db.LoadDataSet(dbCommand, objeto, "MatrizContable")
        Return objeto
    End Function
    ''' <summary>
    ''' Selecciona un solo expediente de MatrizContable tabla.
    ''' <summary>
    ''' <param name="codigoMatriz">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function Seleccionar(ByVal codigoMatriz As Decimal, ByVal dataRequest As DataSet) As MatrizContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MatrizContable_Seleccionar")
        Dim objeto As New MatrizContableBE
        db.AddInParameter(dbCommand, "@p_CodigoMatrizContable", DbType.Decimal, codigoMatriz)
        db.LoadDataSet(dbCommand, objeto, "MatrizContable")
        Return objeto
    End Function
    ''' <summary>
    ''' Lista todos los expedientes de MatrizContable tabla.
    ''' <summary>
    ''' <param name="codigoMatriz">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As MatrizContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MatrizContable_Listar")
        Dim objeto As New MatrizContableBE
        db.LoadDataSet(dbCommand, objeto, "MatrizContable")
        Return objeto
    End Function
    ''' <summary>
    ''' Lista todos los expedientes de MatrizContable tabla.
    ''' <summary>
    ''' <param name="codigoMatriz">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function ListarTablaMatriz(ByVal dataRequest As DataSet) As MatrizContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MatrizContable_ListarTablaMatriz")
        Dim objeto As New MatrizContableBE
        db.LoadDataSet(dbCommand, objeto, "MatrizContable")
        Return objeto
    End Function
    ''' <summary>
    ''' Obtener el codigo segun la descripcion proporcionada
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>MatrizContableBE</returns>
    Public Function SeleccionarCodigo(ByVal descripcion As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("MatrizContable_SeleccionarCodigo")
        Dim objeto As New DataSet
        db.LoadDataSet(dbCommand, objeto, "MatrizContable")
        SeleccionarCodigo = objeto
    End Function
#End Region
#Region " /* Insertar Functions */ "
    ''' <summary>
    ''' Inserta un expediente en MatrizContable tabla.
    ''' <summary>
    ''' <param name="ob">MatrizContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As MatrizContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MatrizContable_Insertar")
        Dim Codigo As String = String.Empty
        oMatrizContableRow = CType(ob.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.Decimal, oMatrizContableRow.CodigoMatrizContable)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMatrizContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Tabla", DbType.String, oMatrizContableRow.TablaMatriz)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMatrizContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_ClaveInterfaz", DbType.String, oMatrizContableRow.ClaveInterfaz) 'RGF 20080716

        db.ExecuteNonQuery(dbCommand)
        Return "true"
    End Function
    ''' <summary>
    ''' Inserta un expediente en MatrizContable tabla sin ingresar el codigo.
    ''' <summary>
    ''' <param name="ob">MatrizContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar_1(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MatrizContable_Insertar_1")
        Dim Codigo As String = String.Empty
        'oMatrizContableRow = CType(ob.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return "true"
    End Function
#End Region
#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en MatrizContable tabla.
    ''' <summary>
    ''' <param name="ob">MatrizContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As MatrizContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MatrizContable_Modificar")
        oMatrizContableRow = CType(ob.MatrizContable.Rows(0), MatrizContableBE.MatrizContableRow)
        db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.Decimal, oMatrizContableRow.CodigoMatrizContable)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oMatrizContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oMatrizContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Tabla", DbType.String, oMatrizContableRow.TablaMatriz)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_ClaveInterfaz", DbType.String, oMatrizContableRow.ClaveInterfaz) 'RGF 20080716

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

#End Region
#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de MatrizContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoMatriz">Decimal</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoMatriz As Decimal, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("MatrizContable_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoMatriz", DbType.Decimal, codigoMatriz)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region
End Class