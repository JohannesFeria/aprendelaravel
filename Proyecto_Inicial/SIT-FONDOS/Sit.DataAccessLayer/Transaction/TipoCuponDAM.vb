Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
    Public Class TipoCuponDAM
      
    Private sqlCommand As String = ""
    Private oTipoCuponRow As TipoCuponBE.TipoCuponRow


    Public Sub New()


    End Sub


#Region " /* Funciones Seleccionar */ "


    ''' <summary>
    ''' Selecciona un solo expediente de TipoCuponBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorFiltros(ByVal codigoTipoCupon As String, ByVal descripcion As String, ByVal situación As String, ByVal dataRequest As DataSet) As TipoCuponBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, codigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situación)

        Dim objeto As New TipoCuponBE
        db.LoadDataSet(dbCommand, objeto, "TipoCupon")
        Return objeto


    End Function
    Public Function Seleccionar(ByVal codigoTipoCupon As String, ByVal dataRequest As DataSet) As TipoCuponBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, codigoTipoCupon)
        Dim objeto As New TipoCuponBE
        db.LoadDataSet(dbCommand, objeto, "TipoCupon")
        Return objeto

    End Function
    ''' <summary>
    ''' Lista todos los expedientes de TipoCuponBE tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar(ByVal dataRequest As DataSet) As TipoCuponBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_Listar")

        Dim objeto As New TipoCuponBE
        db.LoadDataSet(dbCommand, objeto, "TipoCupon")
        Return objeto
    End Function
    Public Function ListarCombo(ByVal strSituacion As String) As TipoCuponBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_ListarCombo")
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, strSituacion)
        Dim objeto As New TipoCuponBE
        db.LoadDataSet(dbCommand, objeto, "TipoCupon")
        Return objeto
    End Function

#End Region

#Region " /* Funciones Insertar */ "

    Public Function Insertar(ByVal oTipoCuponBE As TipoCuponBE, ByVal dataRequest As DataSet) As String
        Dim Codigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_Insertar")

        oTipoCuponRow = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, oTipoCuponRow.CodigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoCuponRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oTipoCuponRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoCuponRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return Codigo

    End Function

#End Region

#Region " /* Funciones Modificar */"

    ''' <summary>
    ''' Midifica un expediente en TipoCuponBE tabla.
    ''' <summary>
    ''' <param name="codigoTipoCupon"></param>
    ''' <param name="descripcion"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="onservaciones"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="situacion"></param>
    ''' <param name="host"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    Public Function Modificar(ByVal oTipoCuponBE As TipoCuponBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_Modificar")

        oTipoCuponRow = CType(oTipoCuponBE.TipoCupon.Rows(0), TipoCuponBE.TipoCuponRow)

        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, oTipoCuponRow.CodigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oTipoCuponRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oTipoCuponRow.Observaciones)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        'db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, oTipoCuponRow.HoraCreacion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTipoCuponRow.Situacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True


    End Function


#End Region

#Region " /* Funciones Eliminar */"

    ''' <summary>
    ''' Elimina un expediente de TipoCuponBE table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoTipoCupon As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoCupon_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoTipoCupon", DbType.String, codigoTipoCupon)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

   




    


End Class

