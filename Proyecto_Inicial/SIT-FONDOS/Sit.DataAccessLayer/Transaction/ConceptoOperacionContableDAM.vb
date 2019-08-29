Imports System
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Imports System.Data.SqlClient
Imports Sit.BusinessEntities

''' <summary>
''' Clase para el acceso de los datos para ConceptoOperacionContable tabla.
''' </summary>
Public Class ConceptoOperacionContableDAM

    Private oConceptoOperacionContableRow As ConceptoOperacionContableBE.ConceptoOperacionContableRow

    Public Sub New()
    End Sub

#Region " /* Seleccionar Functions */ "

    ''' <summary>
    ''' Selecciona una lista Filtrada de los expedientes de ConceptoOperacionContable tabla , segun los paramepros .
    ''' <summary>
    ''' <param name="descripcion">String</param>
    ''' <param name="situacion">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoOperacionContableBE</returns>
    Public Function SeleccionarPorFiltros(ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ConceptoOperacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoOperacionContable_SeleccionarPorFiltro")
        Dim objeto As New ConceptoOperacionContableBE

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.LoadDataSet(dbCommand, objeto, "ConceptoOperacionContable")
        Return objeto
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="codigoConceptoOperacionContable">String</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoOperacionContableBE</returns>
    Public Function Seleccionar(ByVal codigoConceptoOperacionContable As String, ByVal dataRequest As DataSet) As ConceptoOperacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoOperacionContable_Seleccionar")

        Dim objeto As New ConceptoOperacionContableBE

        db.AddInParameter(dbCommand, "@p_CodigoConceptoContable", DbType.String, codigoConceptoOperacionContable)
        db.LoadDataSet(dbCommand, objeto, "ConceptoOperacionContable")

        Return objeto

    End Function

    ''' <summary>
    ''' Lista todos los expedientes de ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>ConceptoOperacionContableBE</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As ConceptoOperacionContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoOperacionContable_Listar")

        Dim objeto As New ConceptoOperacionContableBE

        db.LoadDataSet(dbCommand, objeto, "ConceptoOperacionContable")
        Return objeto

    End Function


#End Region

#Region " /* Insertar Functions */ "

    ''' <summary>
    ''' Inserta un expediente en ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoOperacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>String</returns>
    Public Function Insertar(ByVal ob As ConceptoOperacionContableBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoOperacionContable_Insertar")

        Dim Codigo As String = String.Empty

        oConceptoOperacionContableRow = CType(ob.ConceptoOperacionContable.Rows(0), ConceptoOperacionContableBE.ConceptoOperacionContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoConceptoContable", DbType.String, oConceptoOperacionContableRow.CodigoConceptoContable)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oConceptoOperacionContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oConceptoOperacionContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return "true"

    End Function
#End Region

#Region " /* Modificar Functions */"
    ''' <summary>
    ''' Midifica un expediente en ConceptoOperacionContable tabla.
    ''' <summary>
    ''' <param name="ob">ConceptoOperacionContableBE</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Modificar(ByVal ob As ConceptoOperacionContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoOperacionContable_Modificar")

        oConceptoOperacionContableRow = CType(ob.ConceptoOperacionContable.Rows(0), ConceptoOperacionContableBE.ConceptoOperacionContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoConceptoContable", DbType.String, oConceptoOperacionContableRow.CodigoConceptoContable)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oConceptoOperacionContableRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oConceptoOperacionContableRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

#End Region

#Region " /* Eliminar Functions */"
    ''' <summary>
    ''' Elimina un expediente de MatrizContable table por una llave primaria compuesta.
    ''' <summary>
    ''' <param name="codigoConceptoOperacionContable">Decimal</param>
    ''' <param name="dataRequest">DataSet</param>
    ''' <returns>Boolean</returns>
    Public Function Eliminar(ByVal codigoConceptoOperacionContable As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ConceptoOperacionContable_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoConceptoContable", DbType.String, codigoConceptoOperacionContable)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function
#End Region



End Class

