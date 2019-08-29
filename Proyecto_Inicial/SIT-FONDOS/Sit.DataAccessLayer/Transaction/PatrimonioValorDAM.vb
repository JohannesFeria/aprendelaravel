Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PatrimonioValorDAM
    Private sqlCommand As String = ""
    Private oPatrimonioValorRow As PatrimonioValorBE.PatrimonioValorRow

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function Listar() As PatrimonioValorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_Listar")
            Return db.ExecuteDataSet(dbCommand)
        End Using
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As PatrimonioValorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_Listar")
            Dim objeto As New PatrimonioValorBE
            db.LoadDataSet(dbCommand, objeto, "PatrimonioValor")
            Return objeto
        End Using
    End Function

    Public Function SeleccionarPorFiltro(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As PatrimonioValorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
            Dim objeto As New PatrimonioValorBE
            db.LoadDataSet(dbCommand, objeto, "PatrimonioValor")
            Return objeto
        End Using
    End Function

    Public Function SeleccionarPorFiltro(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As PatrimonioValorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_SeleccionarPorFiltro")
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.String, p_FechaInicio)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.String, p_FechaFin)
            Dim objeto As New PatrimonioValorBE
            db.LoadDataSet(dbCommand, objeto, "PatrimonioValor")
            Return objeto
        End Using
    End Function

    Public Function Seleccionar(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal FechaVigencia As String, ByVal dataRequest As DataSet) As PatrimonioValorBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_PatrimonioValor_Seleccionar")
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, FechaVigencia)
            Dim objeto As New PatrimonioValorBE
            db.LoadDataSet(dbCommand, objeto, "PatrimonioValor")
            Return objeto
        End Using
    End Function

#End Region

#Region " /* Funciones ActualizarporExcel */"

    Public Function ActualizarPatrimonioValorPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sDesTipoIns As String
        Dim sCodMnemo As String
        Dim sSituacion As String
        Dim nFecha As Decimal, nNuevaFecha As Decimal, nPatrimonio As Decimal
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaPatrimonioValorxExcel")

        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        'db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String)    'HDG OT 61045 20101118
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String)    'HDG OT 61045 20101118
        db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_NuevaFechaVigencia", DbType.Decimal)    'HDG OT 61045 20101118
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In dtDetalle.Rows

            sDesTipoIns = filaLinea(0).ToString().Trim()
            sCodMnemo = filaLinea(1).ToString().Trim()
            If filaLinea(3).ToString().Trim() = "" Then
                nFecha = 0
            Else
                nFecha = DataUtility.ObtenerFecha(filaLinea(3).ToString().Trim())
            End If

            If (Not sDesTipoIns.Equals("")) And _
                (Not sCodMnemo.Equals("")) And _
                (Not nFecha.ToString.Equals("0")) Then

                nPatrimonio = Val(filaLinea(2).ToString().Trim())
                'ini HDG OT 61045 20101118
                If filaLinea(4).ToString().Trim() = "" Then
                    nNuevaFecha = 0
                Else
                    nNuevaFecha = DataUtility.ObtenerFecha(filaLinea(4).ToString().Trim())
                End If
                sSituacion = filaLinea(5).ToString().Trim()
                'fin HDG OT 61045 20101118

                'db.SetParameterValue(dbCommand, "@p_CodigoTipoInstrumento", sDesTipoIns) 'HDG OT 61045 20101118
                db.SetParameterValue(dbCommand, "@p_Descripcion", sDesTipoIns)   'HDG OT 61045 20101118
                db.SetParameterValue(dbCommand, "@p_CodigoMnemonico", sCodMnemo)
                db.SetParameterValue(dbCommand, "@p_Patrimonio", nPatrimonio)
                db.SetParameterValue(dbCommand, "@p_FechaVigencia", nFecha)
                db.SetParameterValue(dbCommand, "@p_NuevaFechaVigencia", nNuevaFecha) 'HDG OT 61045 20101118
                db.SetParameterValue(dbCommand, "@p_Situacion", sSituacion)
                db.SetParameterValue(dbCommand, "@p_UsuarioModificacion", sUsuario)
                db.SetParameterValue(dbCommand, "@p_FechaModificacion", nFecMod)
                db.SetParameterValue(dbCommand, "@p_HoraModificacion", DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.SetParameterValue(dbCommand, "@p_Host", sHost)
                db.ExecuteNonQuery(dbCommand)

            End If
        Next
        strmensaje &= "Los datos se cargaron correctamente\n"
        Return True
    End Function
#End Region

    'Public Function Insertar(ByVal ob As PatrimonioValorBE, ByVal dataRequest As DataSet)
    Public Function Insertar(ByVal ob As PatrimonioValorBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_Insertar")
            oPatrimonioValorRow = CType(ob.PatrimonioValor.Rows(0), PatrimonioValorBE.PatrimonioValorRow)

            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oPatrimonioValorRow.CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oPatrimonioValorRow.CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, oPatrimonioValorRow.Patrimonio)
            db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oPatrimonioValorRow.FechaVigencia)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioValorRow.Situacion)

            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

            'db.ExecuteNonQuery(dbCommand)
            Return db.ExecuteScalar(dbCommand)
        End Using
    End Function


    Public Function Modificar(ByVal ob As PatrimonioValorBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_Modificar")
            oPatrimonioValorRow = CType(ob.PatrimonioValor.Rows(0), PatrimonioValorBE.PatrimonioValorRow)
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, oPatrimonioValorRow.CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, oPatrimonioValorRow.CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, oPatrimonioValorRow.Patrimonio)
            db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oPatrimonioValorRow.FechaVigencia)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioValorRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    'Public Function Eliminar(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
    Public Function Eliminar(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal FechaVigencia As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("PatrimonioValor_Eliminar")
            db.AddInParameter(dbCommand, "@p_CodigoTipoInstrumento", DbType.String, CodigoTipoInstrumento)
            db.AddInParameter(dbCommand, "@p_CodigoMnemonico", DbType.String, CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, Convert.ToDecimal(FechaVigencia))
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
            Return True
        End Using
    End Function

    'Modificar el incremento o disminucion del patrimonio (TABLA PARAMETROSGENERALES)
    'LETV 20090318
    Public Function ModificarIncrementoDecremento(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal tipoIngreso As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Patrimonio_ModificarIncrementoDecremento")

        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, clasificacion)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_Valor", DbType.String, valor)
        db.AddInParameter(dbCommand, "@p_TipoIngreso", DbType.String, tipoIngreso)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    'Eliminar el incremento o disminucion del patrimonio (TABLA PARAMETROSGENERALES)
    'LETV 20090318
    Public Function EliminarIncrementoDecremento(ByVal clasificacion As String, ByVal CategoriaIncDec As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Patrimonio_EliminarIncrementoDecremento")

        db.AddInParameter(dbCommand, "@p_Clasificacion", DbType.String, clasificacion)
        db.AddInParameter(dbCommand, "@p_valor", DbType.String, CategoriaIncDec)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function SeleccionarPatrimonioEmisor(ByVal p_Id As Integer, ByVal p_CodigoTercero As String, ByVal p_CodigoEntidad As String, ByVal p_TipoValor As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbComand As DbCommand = db.GetStoredProcCommand("Seleccionar_PatrimonioEmisor")
            db.AddInParameter(dbComand, "@p_Id", DbType.Int32, p_Id)
            db.AddInParameter(dbComand, "@p_CodigoTercero", DbType.String, p_CodigoTercero)
            db.AddInParameter(dbComand, "@p_CodigoEntidad", DbType.String, p_CodigoEntidad)
            db.AddInParameter(dbComand, "@p_TipoValor", DbType.String, p_TipoValor)
            db.AddInParameter(dbComand, "@p_FechaInicio", DbType.Int32, p_FechaInicio)
            db.AddInParameter(dbComand, "@p_FechaFin", DbType.Int32, p_FechaFin)
            Using ds As DataSet = db.ExecuteDataSet(dbComand)
                Return ds.Tables(0)
            End Using
        End Using
    End Function

    Public Sub EliminarPatrimonioEmisor(ByVal p_Id As Integer)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Eliminar_PatrimonioEmisor")
            db.AddInParameter(dbCommand, "@p_Id", DbType.Int32, p_Id)
            db.ExecuteNonQuery(dbCommand)
        End Using
    End Sub

End Class
