'Creado por: HDG OT 64769-4 20120404
Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class HechosImportanciaDAM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oHechosImportanciaBE As HechosImportanciaBE, ByVal dataRequest As DataSet) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_ins_HechosImportancia_Insertar")
        Dim oRow As HechosImportanciaBE.HechosImportanciaRow

        oRow = DirectCast(oHechosImportanciaBE.HechosImportancia.Rows(0), HechosImportanciaBE.HechosImportanciaRow)

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oRow.Fecha)
        db.AddInParameter(dbCommand, "@p_Hecho", DbType.String, oRow.Hecho)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal fecha As Decimal, ByVal situacion As String) As HechosImportanciaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_HechosImportancia_SeleccionarPorFiltro")
        Dim oHechosImportanciaBE As New HechosImportanciaBE

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oHechosImportanciaBE, "HechosImportancia")

        Return oHechosImportanciaBE

    End Function

    Public Function Seleccionar(ByVal CodigoHechos As String) As HechosImportanciaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_HechosImportancia_Seleccionar")
        Dim oHechosImportanciaBE As New HechosImportanciaBE

        db.AddInParameter(dbCommand, "@p_CodigoHechos", DbType.Decimal, CodigoHechos)

        db.LoadDataSet(dbCommand, oHechosImportanciaBE, "HechosImportancia")

        Return oHechosImportanciaBE

    End Function

    Public Function Modificar(ByVal oHechosImportanciaBE As HechosImportanciaBE, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_HechosImportancia_Modificar")
        Dim oRow As HechosImportanciaBE.HechosImportanciaRow

        oRow = DirectCast(oHechosImportanciaBE.HechosImportancia.Rows(0), HechosImportanciaBE.HechosImportanciaRow)

        db.AddInParameter(dbCommand, "@p_CodigoHechos", DbType.Decimal, oRow.CodigoHechos)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oRow.CodigoNemonico)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oRow.Fecha)
        db.AddInParameter(dbCommand, "@p_Hecho", DbType.String, oRow.Hecho)

        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oRow.Situacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function Eliminar(ByVal CodigoHechos As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_del_HechosImportancia_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoHechos", DbType.String, CodigoHechos)

        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True

    End Function

    Public Function ListarHechosImportancia() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_HechosImportancia_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
End Class

