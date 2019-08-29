Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

	Public class ModeloCartaDAM
    Private sqlCommand As String = ""
    Public oModeloCartaRow As ModeloCartaBE.ModeloCartaRow
		Public Sub New()

		End Sub
#Region " /* Funciones Seleccionar */ "
    Public Function Seleccionar(ByVal codigoModelo As String) As ModeloCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorCodigoRuta(ByVal codigoRuta As String) As ModeloCartaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_SeleccionarPorCodigoRuta")

        db.AddInParameter(dbCommand, "@p_CodigoRuta", DbType.String, codigoRuta)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function Listar() As ModeloCartaBE

        Dim oModeloCartaBE As New ModeloCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_Listar")

        db.LoadDataSet(dbCommand, oModeloCartaBE, "ModeloCarta")

        Return oModeloCartaBE

    End Function
    Public Function ListarDescripcionConcatenada() As ModeloCartaBE

        Dim oModeloCartaBE As New ModeloCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_ListarDescripcionConcatenada")

        db.LoadDataSet(dbCommand, oModeloCartaBE, "ModeloCarta")

        Return oModeloCartaBE

    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoModelo As String, ByVal descripcion As String, ByVal situacion As String, ByVal dataRequest As DataSet) As ModeloCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        Dim objeto As New ModeloCartaBE
        db.LoadDataSet(dbCommand, objeto, "ModeloCarta")
        Return objeto
    End Function
    Public Function SeleccionarPorOperacion(ByVal codigoOperacion As String, ByVal dataRequest As DataSet) As ModeloCartaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_SeleccionarPorOperacion")
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
        Dim objeto As New ModeloCartaBE
        db.LoadDataSet(dbCommand, objeto, "ModeloCarta")
        Return objeto
    End Function
    'Public Function SeleccionarPorOperacion2(ByVal codigoOperacion As String, ByVal dataRequest As DataSet) As ModeloCartaBE
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_SeleccionarPorOperacion2")
    '    db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, codigoOperacion)
    '    Dim objeto As New ModeloCartaBE
    '    db.LoadDataSet(dbCommand, objeto, "ModeloCarta")
    '    Return objeto
    'End Function
#End Region

    Public Function Insertar(ByVal ob As ModeloCartaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_Insertar")
        oModeloCartaRow = CType(ob.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oModeloCartaRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oModeloCartaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioValidador1", DbType.String, oModeloCartaRow.UsuarioValidador1)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oModeloCartaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioValidador2", DbType.String, oModeloCartaRow.UsuarioValidador2)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oModeloCartaRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_ArchivoPlantilla", DbType.String, oModeloCartaRow.ArchivoPlantilla)

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As ModeloCartaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_Modificar")
        oModeloCartaRow = CType(ob.ModeloCarta.Rows(0), ModeloCartaBE.ModeloCartaRow)

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, oModeloCartaRow.CodigoModelo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oModeloCartaRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioValidador1", DbType.String, oModeloCartaRow.UsuarioValidador1)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oModeloCartaRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioValidador2", DbType.String, oModeloCartaRow.UsuarioValidador2)
        db.AddInParameter(dbCommand, "@p_CodigoOperacion", DbType.String, oModeloCartaRow.CodigoOperacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_ArchivoPlantilla", DbType.String, oModeloCartaRow.ArchivoPlantilla)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal codigoModelo As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function SeleccionarCartaEstructuraPorModelo(ByVal codigoModelo As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCartaEstructura_SeleccionarPorModelo")
        db.AddInParameter(dbCommand, "@p_CodigoModelo", DbType.String, codigoModelo)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    ''Public Function EliminarPorCodigoRuta(ByVal codigoRuta As String) As Boolean

    ''    Dim db As Database = DatabaseFactory.CreateDatabase()
    ''    Dim dbCommand As dbCommand = db.GetStoredProcCommand("ModeloCarta_EliminarPorCodigoRuta")

    ''    db.AddInParameter(dbCommand, "@p_CodigoRuta", DbType.String, codigoRuta)

    ''    db.ExecuteNonQuery(dbCommand)
    ''    Return True
    ''End Function
End Class

