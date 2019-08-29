Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ObligacionTecnicaDAM
    Private sqlCommand As String = ""
    Private oObligacionTecnicaRow As ObligacionTecnicaBE.ObligacionTecnicaRow

    Public Sub New()

    End Sub

#Region " /* Funciones */ "
    Public Function ValidarRegistro(ByVal CodOT As String, ByVal FechaOT As Decimal, ByVal PortafolioOT As String, ByVal Tipo As String, ByVal dataRequest As DataSet) As ObligacionTecnicaBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_ValidarRegistro")

        'db.AddInParameter(dbCommand, "@p_ObligacionTecnica", DbType.String, CodOT)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, PortafolioOT)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, FechaOT)
        'db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)

        Dim objeto As New ObligacionTecnicaBE
        db.LoadDataSet(dbCommand, objeto, "ObligacionTecnica")
        Return objeto

    End Function

    Public Function SeleccionarPorFiltro(ByVal CodObligacionTecnica As String, ByVal CodPortafolio As String, ByVal Descripcion As String, ByVal Fecha As Decimal, ByVal Tipo As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("SP_ObligacionTecnica_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodObligacionTecnica", DbType.String, CodObligacionTecnica)
        db.AddInParameter(dbCommand, "@p_CodPortafolio", DbType.String, CodPortafolio)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, Descripcion)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, Fecha)
        db.AddInParameter(dbCommand, "@p_Tipo", DbType.String, Tipo)
        Return db.ExecuteDataSet(dbCommand)
        'Dim objeto As New ObligacionTecnicaBE
        'db.LoadDataSet(dbCommand, objeto, "ObligacionTecnica")
        'Return objeto
    End Function

    Public Function Insertar(ByVal ob As ObligacionTecnicaBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_Insertar")
        oObligacionTecnicaRow = CType(ob.ObligacionTecnica.Rows(0), ObligacionTecnicaBE.ObligacionTecnicaRow)

        'db.AddInParameter(dbCommand, "@p_CodigoObligacionTecnica", DbType.String, oObligacionTecnicaRow.CodigoObligacionTecnica)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, oObligacionTecnicaRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oObligacionTecnicaRow.Fecha)
        db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal, oObligacionTecnicaRow.Monto)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function Modificar(ByVal ob As ObligacionTecnicaBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_Modificar")

        oObligacionTecnicaRow = CType(ob.ObligacionTecnica.Rows(0), ObligacionTecnicaBE.ObligacionTecnicaRow)

        db.AddInParameter(dbCommand, "@p_CodigoObligacionTecnica", DbType.String, oObligacionTecnicaRow.CodigoObligacionTecnica)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, oObligacionTecnicaRow.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal, oObligacionTecnicaRow.Fecha)
        db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal, oObligacionTecnicaRow.Monto)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True

    End Function

    Public Function Eliminar(ByVal codigoObligacionTecnica As String, ByVal dataRequest As DataSet) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoObligacionTecnica", DbType.String, codigoObligacionTecnica)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)

        Return True
    End Function

    'Public Function InsertarObligacionesTecnicasPorExcel(ByVal FechaObligacionTecnica As Decimal, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean

    '    Dim strMensajeEliminar As String = ""
    '    Dim strMensajeInsertar As String = ""
    '    'Invoca Funcion desactivar registros
    '    Try

    '        'EliminarRegistrosExcel(FechaObligacionTecnica, dtDetalle, dataRequest, strmensaje)

    '        InsertarRegistrosExcel(FechaObligacionTecnica, dtDetalle, dataRequest, strmensaje)

    '    Catch ex As Exception
    '        strmensaje = ex.Message
    '    End Try

    'End Function

    Public Function DesactivarRegistrosExcel(ByVal Fecha As Decimal, ByVal dataRequest As DataSet, ByRef strMensaje As String)
        Try
            Dim db As Database = DatabaseFactory.CreateDatabase()
            Dim dbCommandInactivar As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_DesactivarRegistros")

            db.AddInParameter(dbCommandInactivar, "@p_Fecha", DbType.Decimal, Fecha)
            db.AddInParameter(dbCommandInactivar, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommandInactivar, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommandInactivar, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommandInactivar, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommandInactivar)

        Catch ex As Exception
            strMensaje = "error al desactivar los registros de la fecha: " + Convert.ToString(Fecha)
        End Try


    End Function

    Public Function InsertarObligacionesTecnicasPorExcel(ByVal ob As ObligacionTecnicaBE.ObligacionTecnicaRow, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_InsertarExcel")

        db.AddInParameter(dbCommand, "@p_Fecha", DbType.String, ob.Fecha)
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, ob.CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal, ob.Monto)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    'Public Function EliminarRegistrosExcel(ByVal FechaObligacionTecnica As Decimal, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strMensaje As String) As Boolean
    '    Try
    '        Dim db As Database = DatabaseFactory.CreateDatabase()
    '        Dim dbCommandInactivar As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_DesactivarRegistros")

    '        db.AddInParameter(dbCommandInactivar, "@p_Fecha", DbType.Decimal, FechaObligacionTecnica)
    '        db.AddInParameter(dbCommandInactivar, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
    '        db.AddInParameter(dbCommandInactivar, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '        db.AddInParameter(dbCommandInactivar, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '        db.AddInParameter(dbCommandInactivar, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
    '        db.ExecuteNonQuery(dbCommandInactivar)

    '    Catch ex As Exception
    '        strMensaje = "error al desactivar los registros de la fecha: " + Convert.ToString(FechaObligacionTecnica)
    '    End Try


    'End Function
    'Public Function InsertarRegistrosExcel(ByVal FechaObligacionTecnica As Decimal, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strMensaje As String) As Boolean

    '    'Insertar los nuevos registros
    '    Dim db As Database = DatabaseFactory.CreateDatabase()
    '    Dim dbCommand As DbCommand = db.GetStoredProcCommand("sp_ObligacionTecnica_InsertarExcel")

    '    Dim sPortafolio As String
    '    Dim sMonto As String
    '    Dim sRegError As String = ""
    '    Dim num As Integer = 0

    '    db.AddInParameter(dbCommand, "@p_Fecha", DbType.Decimal)
    '    db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String)
    '    db.AddInParameter(dbCommand, "@p_Monto", DbType.Decimal)
    '    db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String)
    '    db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal)
    '    db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String)
    '    db.AddInParameter(dbCommand, "@p_Host", DbType.String)


    '    For Each filaLinea As DataRow In dtDetalle.Rows

    '        num = num + 1
    '        If (String.IsNullOrEmpty(filaLinea(0).ToString().Trim())) Then
    '            sPortafolio = ""
    '        Else
    '            sPortafolio = filaLinea(0).ToString().Trim()
    '        End If
    '        If (String.IsNullOrEmpty(filaLinea(1).ToString().Trim())) Then
    '            sMonto = ""
    '        Else
    '            sMonto = filaLinea(1).ToString().Trim()
    '        End If

    '        If (sPortafolio.Equals("") Or sMonto.Equals("")) Then
    '            'sRegError = sRegError + "," + Convert.ToString(num)
    '        Else
    '            Try
    '                db.SetParameterValue(dbCommand, "@p_Fecha", FechaObligacionTecnica)
    '                db.SetParameterValue(dbCommand, "@p_Portafolio", sPortafolio)
    '                db.SetParameterValue(dbCommand, "@p_Monto", sMonto)
    '                db.SetParameterValue(dbCommand, "@p_UsuarioCreacion", DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
    '                db.SetParameterValue(dbCommand, "@p_FechaCreacion", DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '                db.SetParameterValue(dbCommand, "@p_HoraCreacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
    '                db.SetParameterValue(dbCommand, "@p_Host", DataUtility.ObtenerValorRequest(dataRequest, "Host"))
    '                db.ExecuteNonQuery(dbCommand)
    '            Catch ex As Exception
    '                strMensaje = strMensaje + "," + num
    '            End Try
    '        End If
    '    Next

    '    strMensaje &= "Los datos se cargaron correctamente\n"
    '    Return True
    'End Function
#End Region
End Class
