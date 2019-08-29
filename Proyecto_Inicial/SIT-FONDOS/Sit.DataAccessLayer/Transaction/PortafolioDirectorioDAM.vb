Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PortafolioDirectorioDAM


    Private sqlCommand As String = ""
    Private oPortafolioDirectorioRow As PortafolioDirectorioBE.PortafolioDirectorioRow

    Public Function Seleccionar(ByVal PortafolioCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "PortafolioDirectorio_seleccionar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            db.AddInParameter(dbCommand, "@CodigoPortafolioSBS", DbType.String, PortafolioCodigo)
            Using ds As DataSet = db.ExecuteDataSet(dbCommand)
                Return ds
            End Using
        End Using
    End Function

    Public Function Insertar(ByVal ob As PortafolioDirectorioBE, ByVal dataRequest As DataSet) As Boolean
        Insertar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "PortafolioDirectorio_Insertar"
        Using dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
            oPortafolioDirectorioRow = CType(ob.PortafolioDirectorio.Rows(0), PortafolioDirectorioBE.PortafolioDirectorioRow)
            db.AddInParameter(dbCommand, "@PortafolioCodigo", DbType.String, oPortafolioDirectorioRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@PortafolioNombre", DbType.String, oPortafolioDirectorioRow.PortafolioDirectorio)
            db.AddInParameter(dbCommand, "@UsuarioCreacion", DbType.String, oPortafolioDirectorioRow.UsuarioCreacion)
            db.AddInParameter(dbCommand, "@FechaCreacion", DbType.Decimal, oPortafolioDirectorioRow.FechaCreacion)
            db.AddInParameter(dbCommand, "@HoraCreacion", DbType.String, oPortafolioDirectorioRow.HoraCreacion)
            db.ExecuteNonQuery(dbCommand)
            Insertar = True
        End Using
    End Function

    Public Function Modificar(ByVal ob As PortafolioDirectorioBE, ByVal dataRequest As DataSet) As Boolean
        Modificar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioDirectorio_Modificar")
            oPortafolioDirectorioRow = CType(ob.PortafolioDirectorio.Rows(0), PortafolioDirectorioBE.PortafolioDirectorioRow)
            db.AddInParameter(dbCommand, "@PortafolioCodigo", DbType.String, oPortafolioDirectorioRow.CodigoPortafolioSBS)
            db.AddInParameter(dbCommand, "@PortafolioNombre", DbType.String, oPortafolioDirectorioRow.PortafolioDirectorio)
            db.AddInParameter(dbCommand, "@UsuarioModificacion", DbType.String, oPortafolioDirectorioRow.UsuarioModificacion)
            db.AddInParameter(dbCommand, "@FechaModificacion", DbType.Decimal, oPortafolioDirectorioRow.FechaModificacion)
            db.AddInParameter(dbCommand, "@HoraModificacion", DbType.String, oPortafolioDirectorioRow.HoraModificacion)
            db.ExecuteNonQuery(dbCommand)
            Modificar = True
        End Using
    End Function

    Public Function Eliminar(ByVal PortafolioCodigo As String, ByVal dataRequest As DataSet) As Boolean
        Eliminar = False
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PortafolioDirectorio_Eliminar")
            db.AddInParameter(dbCommand, "@PortafolioCodigo", DbType.String, PortafolioCodigo)
            db.ExecuteNonQuery(dbCommand)
            Eliminar = True
        End Using
    End Function

End Class
