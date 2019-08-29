Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class ArchivoPlanoEstructuraDAM

    Private sqlCommand As String = ""
    Private oArchivoPlanoEstructuraRow As ArchivoPlanoEstructuraBE.ArchivoPlanoEstructuraRow

    Public Function Seleccionar(ByVal ArchivoCodigo As String, ByVal ColumnaOrden As Integer, ByVal dataRequest As DataSet) As CustodioArchivoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ArchivoPlanoEstructura_seleccionar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@ArchivoCodigo", DbType.String, ArchivoCodigo)
            db.AddInParameter(dbCommand, "@ColumnaOrden", DbType.Decimal, ArchivoCodigo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function Listar(ByVal ArchivoCodigo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ArchivoPlanoEstructura_Listar")
            db.AddInParameter(dbCommand, "@ArchivoCodigo", DbType.String, ArchivoCodigo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function Insertar(ByVal ob As ArchivoPlanoEstructuraBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "ArchivoPlanoEstructura_Insertar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            oArchivoPlanoEstructuraRow = CType(ob.ArchivoPlanoEstructura.Rows(0), ArchivoPlanoEstructuraBE.ArchivoPlanoEstructuraRow)
            db.AddInParameter(dbCommand, "@ArchivoCodigo", DbType.String, oArchivoPlanoEstructuraRow.ArchivoCodigo)
            db.AddInParameter(dbCommand, "@ColumnaOrden", DbType.String, oArchivoPlanoEstructuraRow.ColumnaOrden)
            db.AddInParameter(dbCommand, "@ColumnaNombre", DbType.String, oArchivoPlanoEstructuraRow.ColumnaNombre)
            db.AddInParameter(dbCommand, "@ColumnaDescripcion", DbType.String, oArchivoPlanoEstructuraRow.ColumnaDescripcion)
            db.AddInParameter(dbCommand, "@ColumnaPosicionInicial", DbType.String, oArchivoPlanoEstructuraRow.ColumnaPosicionInicial)
            db.AddInParameter(dbCommand, "@ColumnaLongitud", DbType.String, oArchivoPlanoEstructuraRow.ColumnaLongitud)
            db.AddInParameter(dbCommand, "@ColumnaTipoDato", DbType.String, oArchivoPlanoEstructuraRow.ColumnaTipoDato)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, oArchivoPlanoEstructuraRow.UsuarioCreacion)
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, oArchivoPlanoEstructuraRow.FechaCreacion)
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, oArchivoPlanoEstructuraRow.HoraCreacion)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Function

    Public Function Modificar(ByVal ob As ArchivoPlanoEstructuraBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ArchivoPlanoEstructura_Modificar")
            oArchivoPlanoEstructuraRow = CType(ob.ArchivoPlanoEstructura.Rows(0), ArchivoPlanoEstructuraBE.ArchivoPlanoEstructuraRow)
            db.AddInParameter(dbCommand, "@ArchivoCodigo", DbType.String, oArchivoPlanoEstructuraRow.ArchivoCodigo)
            db.AddInParameter(dbCommand, "@ColumnaOrden", DbType.String, oArchivoPlanoEstructuraRow.ColumnaOrden)
            db.AddInParameter(dbCommand, "@ColumnaNombre", DbType.String, oArchivoPlanoEstructuraRow.ColumnaNombre)
            db.AddInParameter(dbCommand, "@ColumnaDescripcion", DbType.String, oArchivoPlanoEstructuraRow.ColumnaDescripcion)
            db.AddInParameter(dbCommand, "@ColumnaPosicionInicial", DbType.String, oArchivoPlanoEstructuraRow.ColumnaPosicionInicial)
            db.AddInParameter(dbCommand, "@ColumnaLongitud", DbType.String, oArchivoPlanoEstructuraRow.ColumnaLongitud)
            db.AddInParameter(dbCommand, "@ColumnaTipoDato", DbType.String, oArchivoPlanoEstructuraRow.ColumnaTipoDato)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, oArchivoPlanoEstructuraRow.UsuarioModificacion)
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, oArchivoPlanoEstructuraRow.FechaModificacion)
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, oArchivoPlanoEstructuraRow.HoraModificacion)
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function

    Public Function Eliminar(ByVal ArchivoCodigo As String, ByVal ColumnaOrden As Integer, ByVal dataRequest As DataSet) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("ArchivoPlanoEstructura_Eliminar")
            db.AddInParameter(dbCommand, "@ArchivoCodigo", DbType.String, ArchivoCodigo)
            db.AddInParameter(dbCommand, "@ColumnaOrden", DbType.Decimal, ArchivoCodigo)
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function

End Class
