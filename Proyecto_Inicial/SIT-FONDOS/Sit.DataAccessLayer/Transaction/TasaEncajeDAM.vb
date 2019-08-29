Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class TasaEncajeDAM
    Private oTasaEncaje As TasaEncajeBE.TasaEncajeRow
    Public Sub New()

    End Sub
    Public Function Insertar(ByVal objTasaEncajeRow As TasaEncajeBE.TasaEncajeRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TasaEncaje_Insertar")
        oTasaEncaje = CType(objTasaEncajeRow, TasaEncajeBE.TasaEncajeRow)
        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, oTasaEncaje.CodigoCalificacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oTasaEncaje.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oTasaEncaje.CodigoEntidad)
        'db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, oTasaEncaje.CodigoMoneda)
        'db.AddInParameter(dbCommand, "@p_CodigoTipoTitulo", DbType.String, oTasaEncaje.CodigoTipoTitulo)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oTasaEncaje.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_ValorTasaEncaje", DbType.Decimal, oTasaEncaje.ValorTasaEncaje)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTasaEncaje.Situacion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oTasaEncaje.Observaciones)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Seleccionar(ByVal SecuenciaTasa As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TasaEncaje_Seleccionar")

        db.AddInParameter(dbCommand, "@p_SecuenciaTasa", DbType.String, SecuenciaTasa)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoEntidad As String, ByVal codigoCalificacion As String, _
    ByVal fechaVigencia As Decimal, ByVal codigoNemonico As String, ByVal fechaTasaVigente As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TasaEncaje_SeleccionarporFiltro")

        db.AddInParameter(dbCommand, "@p_codigoEntidad", DbType.String, codigoEntidad)
        db.AddInParameter(dbCommand, "@p_codigoCalificacion", DbType.String, codigoCalificacion)
        If (fechaVigencia = 0) Then
            db.AddInParameter(dbCommand, "@p_fechaVigencia", DbType.Decimal, Convert.DBNull)
        Else
            db.AddInParameter(dbCommand, "@p_fechaVigencia", DbType.Decimal, fechaVigencia)
        End If
        db.AddInParameter(dbCommand, "@p_codigoNemonico", DbType.String, codigoNemonico)
        If (fechaVigencia = 0) Then
            db.AddInParameter(dbCommand, "@p_TasaVigente", DbType.Decimal, Convert.DBNull)
        Else
            db.AddInParameter(dbCommand, "@p_TasaVigente", DbType.Decimal, fechaTasaVigente)
        End If


        Return db.ExecuteDataSet(dbCommand)
    End Function




    Public Function Modificar(ByVal objTasaEncajeRow As TasaEncajeBE.TasaEncajeRow, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TasaEncaje_Modificar")
        oTasaEncaje = CType(objTasaEncajeRow, TasaEncajeBE.TasaEncajeRow)
        db.AddInParameter(dbCommand, "@p_SecuenciaTasa", DbType.String, oTasaEncaje.SecuenciaTasa)
        db.AddInParameter(dbCommand, "@p_CodigoCalificacion", DbType.String, oTasaEncaje.CodigoCalificacion)
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oTasaEncaje.CodigoMnemonico)
        db.AddInParameter(dbCommand, "@p_CodigoEntidad", DbType.String, oTasaEncaje.CodigoEntidad)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oTasaEncaje.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_ValorTasaEncaje", DbType.Decimal, oTasaEncaje.ValorTasaEncaje)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oTasaEncaje.Situacion)
        db.AddInParameter(dbCommand, "@p_Observaciones", DbType.String, oTasaEncaje.Observaciones)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Elimina un expediente de Tasas table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal secueciaTasa As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TasaEncaje_Eliminar")

        db.AddInParameter(dbCommand, "@p_SecuenciaTasa", DbType.String, secueciaTasa)

        db.ExecuteNonQuery(dbCommand)
    End Function


End Class
