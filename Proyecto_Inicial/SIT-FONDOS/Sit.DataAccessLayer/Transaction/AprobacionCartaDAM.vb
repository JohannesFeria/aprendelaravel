Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql


	Public class AprobacionCartaDAM

    Private sqlCommand As String = ""
    Public oAprobacionCartaRow As AprobacionCartaBE.AprobacionCartaRow
		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoModelo As String) As AprobacionCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AprobacionCarta_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarPorCodigoModelo(ByVal codigoModelo As String) As AprobacionCartaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AprobacionCarta_SeleccionarPorCodigoModelo")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar() As AprobacionCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AprobacionCarta_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
#End Region

    Public Function Insertar(ByVal ob As AprobacionCartaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AprobacionCarta_Insertar")
        oAprobacionCartaRow = CType(ob.AprobacionCarta.Rows(0), AprobacionCartaBE.AprobacionCartaRow)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oAprobacionCartaRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_NombreUsuario1", DbType.String, oAprobacionCartaRow.NombreUsuario1)
        db.AddInParameter(dbCommand, "@p_CargoUsuario1", DbType.String, oAprobacionCartaRow.CargoUsuario1)
        db.AddInParameter(dbCommand, "@p_NombreSuplente1", DbType.String, oAprobacionCartaRow.NombreSuplente1)
        db.AddInParameter(dbCommand, "@p_CargoSuplente1", DbType.String, oAprobacionCartaRow.CargoSuplente1)
        db.AddInParameter(dbCommand, "@p_NombreAccesitario", DbType.String, oAprobacionCartaRow.NombreAccesitario)
        db.AddInParameter(dbCommand, "@p_CargoAccesitario", DbType.String, oAprobacionCartaRow.CargoAccesitario)
        db.AddInParameter(dbCommand, "@p_NombreTitularUsuario", DbType.String, oAprobacionCartaRow.NombreTitularUsuario)
        db.AddInParameter(dbCommand, "@p_CargoTitularUsuario", DbType.String, oAprobacionCartaRow.CargoTitularUsuario)
        db.AddInParameter(dbCommand, "@p_NombreSuplente2", DbType.String, oAprobacionCartaRow.NombreSuplente2)
        db.AddInParameter(dbCommand, "@p_CargoSuplente2", DbType.String, oAprobacionCartaRow.CargoSuplente2)
        db.AddInParameter(dbCommand, "@p_NombreAccesitario2", DbType.String, oAprobacionCartaRow.NombreAccesitario2)
        db.AddInParameter(dbCommand, "@p_CargoAccesitario2", DbType.String, oAprobacionCartaRow.CargoAccesitario2)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As AprobacionCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AprobacionCarta_Modificar")
        oAprobacionCartaRow = CType(ob.AprobacionCarta.Rows(0), AprobacionCartaBE.AprobacionCartaRow)

        db.AddInParameter(dbCommand, "@p_NombreUsuario1", DbType.String, oAprobacionCartaRow.NombreUsuario1)
        db.AddInParameter(dbCommand, "@p_CargoUsuario1", DbType.String, oAprobacionCartaRow.CargoUsuario1)
        db.AddInParameter(dbCommand, "@p_NombreSuplente1", DbType.String, oAprobacionCartaRow.NombreSuplente1)
        db.AddInParameter(dbCommand, "@p_CargoSuplente1", DbType.String, oAprobacionCartaRow.CargoSuplente1)
        db.AddInParameter(dbCommand, "@p_NombreAccesitario", DbType.String, oAprobacionCartaRow.NombreAccesitario)
        db.AddInParameter(dbCommand, "@p_CargoAccesitario", DbType.String, oAprobacionCartaRow.CargoAccesitario)
        db.AddInParameter(dbCommand, "@p_NombreTitularUsuario", DbType.String, oAprobacionCartaRow.NombreTitularUsuario)
        db.AddInParameter(dbCommand, "@p_CargoTitularUsuario", DbType.String, oAprobacionCartaRow.CargoTitularUsuario)
        db.AddInParameter(dbCommand, "@p_NombreSuplente2", DbType.String, oAprobacionCartaRow.NombreSuplente2)
        db.AddInParameter(dbCommand, "@p_CargoSuplente2", DbType.String, oAprobacionCartaRow.CargoSuplente2)
        db.AddInParameter(dbCommand, "@p_NombreAccesitario2", DbType.String, oAprobacionCartaRow.NombreAccesitario2)
        db.AddInParameter(dbCommand, "@p_CargoAccesitario2", DbType.String, oAprobacionCartaRow.CargoAccesitario2)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oAprobacionCartaRow.Situacion)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oAprobacionCartaRow.CodigoModelo)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoModelo As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("AprobacionCarta_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

End Class

