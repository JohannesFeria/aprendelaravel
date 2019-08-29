Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Sit
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class PatrimonioFideicomisoDAM
    Private sqlCommand As String = ""
    Private oPatrimonioFideicomisoRow As PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow
    Private oPatrimonioFideicomisoDetalleRow As PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow

    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal codigo As String, ByVal descripcion As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As PatrimonioFideicomisoBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomiso_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, codigo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)

        Dim objPatrimonioFideicomisoBE As New PatrimonioFideicomisoBE
        db.LoadDataSet(dbCommand, objPatrimonioFideicomisoBE, "PatrimonioFideicomisoBE")
        Return objPatrimonioFideicomisoBE
    End Function


    'HDG OT 61045 20101020
    Public Function SeleccionarPorFiltroExportar(ByVal codigo As String, ByVal descripcion As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_PatrimonioFideicomiso_PorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, codigo)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, descripcion)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)

        'Dim objPatrimonioFideicomisoBE As New PatrimonioFideicomisoBE
        'db.LoadDataSet(dbCommand, objPatrimonioFideicomisoBE, "PatrimonioFideicomisoBE")
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function

    Public Function Insertar(ByVal ob As PatrimonioFideicomisoBE, ByVal dataRequest As DataSet) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomiso_Insertar")
        oPatrimonioFideicomisoRow = CType(ob.PatrimonioFideicomisoBE.Rows(0), PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow)

        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oPatrimonioFideicomisoRow.Descripcion.ToUpper)
        db.AddInParameter(dbCommand, "@p_TotalActivo", DbType.Decimal, oPatrimonioFideicomisoRow.TotalActivo)
        db.AddInParameter(dbCommand, "@p_TotalPasivo", DbType.Decimal, oPatrimonioFideicomisoRow.TotalPasivo)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, oPatrimonioFideicomisoRow.Patrimonio)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oPatrimonioFideicomisoRow.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioFideicomisoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.AddInParameter(dbCommand, "@p_FactorRiesgo", DbType.Decimal, oPatrimonioFideicomisoRow.FactorRiesgo)
        db.AddInParameter(dbCommand, "@p_FactorLiquidez", DbType.Decimal, oPatrimonioFideicomisoRow.FactorLiquidez)

        Dim dsAux As DataSet = db.ExecuteDataSet(dbCommand)
        Return dsAux.Tables(0).Rows(0)(0)
    End Function

    Public Function Modificar(ByVal ob As PatrimonioFideicomisoBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomiso_Modificar")
        oPatrimonioFideicomisoRow = CType(ob.PatrimonioFideicomisoBE.Rows(0), PatrimonioFideicomisoBE.PatrimonioFideicomisoBERow)

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, oPatrimonioFideicomisoRow.CodigoPatrimonioFideicomiso)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, oPatrimonioFideicomisoRow.Descripcion)
        db.AddInParameter(dbCommand, "@p_TotalActivo", DbType.Decimal, oPatrimonioFideicomisoRow.TotalActivo)
        db.AddInParameter(dbCommand, "@p_TotalPasivo", DbType.Decimal, oPatrimonioFideicomisoRow.TotalPasivo)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, oPatrimonioFideicomisoRow.Patrimonio)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oPatrimonioFideicomisoRow.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioFideicomisoRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))


        db.AddInParameter(dbCommand, "@p_FactorRiesgo", DbType.Decimal, oPatrimonioFideicomisoRow.FactorRiesgo)
        db.AddInParameter(dbCommand, "@p_FactorLiquidez", DbType.Decimal, oPatrimonioFideicomisoRow.FactorLiquidez)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function Eliminar(ByVal CodigoPatrimonioFideicomiso As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomiso_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, CodigoPatrimonioFideicomiso)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function SeleccionarPorCodigoDetalle(ByVal CodigoPatrimonioFideicomiso As String) As PatrimonioFideicomisoDetalleBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_SeleccionarPorCodigo")

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, CodigoPatrimonioFideicomiso)
        Dim dsAux As DataSet = db.ExecuteDataSet(dbCommand)
        Dim objPatrimonioFideicomisoDetalleBE As New PatrimonioFideicomisoDetalleBE
        Dim objRow As PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow
        Dim i As Integer
        With dsAux.Tables(0)
            For i = 0 To .Rows.Count - 1
                objRow = objPatrimonioFideicomisoDetalleBE.Tables(0).NewRow
                objRow.CodigoPatrimonioFideicomisoDetalle = .Rows(i)("CodigoPatrimonioFideicomisoDetalle")
                objRow.PatrimonioFideicomiso = .Rows(i)("PatrimonioFideicomiso")
                objRow.Emisor = .Rows(i)("Emisor")
                objRow.CodigoIsin = .Rows(i)("CodigoIsin")
                objRow.Descripcion = .Rows(i)("Descripcion")
                objRow.CodigoMnemonico = .Rows(i)("CodigoMnemonico")
                objRow.Situacion = .Rows(i)("Situacion")
                objPatrimonioFideicomisoDetalleBE.Tables(0).Rows.Add(objRow)
            Next
        End With

        Return objPatrimonioFideicomisoDetalleBE
    End Function

    Public Function SeleccionarNemonicoCaracteristicas(ByVal CodigoNemonico As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_SeleccionarCaracteristicas")

        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function InsertarModificarDetalle(ByVal ob As PatrimonioFideicomisoDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim strCodigoLocal As String = ""
        Dim dbCommand As dbCommand '= db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_SeleccionarCaracteristicas")
        For Each oPatrimonioFideicomisoDetalleRow In ob.Tables(0).Rows
            strCodigoLocal = oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomisoDetalle
            If strCodigoLocal.Length >= 4 Then strCodigoLocal = strCodigoLocal.Substring(0, 4)
            If strCodigoLocal = "TEMP" Then
                'ingresar
                dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_Insertar")
                db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomiso)
                db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioFideicomisoDetalleRow.Situacion)
                db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            Else
                'modificar
                dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_Modificar")
                db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomisoDetalle", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomisoDetalle)
                db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomiso)
                db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoMnemonico)
                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioFideicomisoDetalleRow.Situacion)
                db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
                db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            End If
            db.ExecuteNonQuery(dbCommand)
        Next
        'oPatrimonioFideicomisoDetalleRow = CType(ob.PatrimonioFideicomisoDetalle.Rows(0), PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow)
        Return True
    End Function

    Public Function InsertarDetalle(ByVal ob As PatrimonioFideicomisoDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()

        For Each oPatrimonioFideicomisoDetalleRow In ob.Tables(0).Rows
            Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_Insertar")
            db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomiso)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioFideicomisoDetalleRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        Next
        'oPatrimonioFideicomisoDetalleRow = CType(ob.PatrimonioFideicomisoDetalle.Rows(0), PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow)
        Return True
    End Function

    Public Function ModificarDetalle(ByVal ob As PatrimonioFideicomisoDetalleBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_Modificar")
        For Each oPatrimonioFideicomisoDetalleRow In ob.Tables(0).Rows
            db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomisoDetalle", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomisoDetalle)
            db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoPatrimonioFideicomiso)
            db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, oPatrimonioFideicomisoDetalleRow.CodigoMnemonico)
            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oPatrimonioFideicomisoDetalleRow.Situacion)
            db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
            db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
            db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))
            db.ExecuteNonQuery(dbCommand)
        Next
        'oPatrimonioFideicomisoDetalleRow = CType(ob.PatrimonioFideicomisoDetalle.Rows(0), PatrimonioFideicomisoDetalleBE.PatrimonioFideicomisoDetalleRow)
        Return True
    End Function

    Public Function EliminarDetalle(ByVal CodigoPatrimonioFideicomisoDetalle As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomisoDetalle", DbType.String, CodigoPatrimonioFideicomisoDetalle)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ActualizarPatrimonioFideicomisoPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sCodPadFid As String
        Dim sSituacion As String
        Dim nFecha As Decimal, nPatrimonio As Decimal
        Dim nTotAct As Decimal, nTotPas As Decimal
        Dim nFacRies As Decimal, nFacLiq As Decimal
        Dim sUsuario As String, sHost As String
        Dim nFecMod As Decimal
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_upd_ActualizaPatrimonioFideicomisoxExcel")

        sUsuario = DataUtility.ObtenerValorRequest(dataRequest, "Usuario")
        nFecMod = DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha"))
        sHost = DataUtility.ObtenerValorRequest(dataRequest, "Host")

        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioFideicomiso", DbType.String)
        db.AddInParameter(dbCommand, "@p_TotalActivo", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_TotalPasivo", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_FactorRiesgo", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_FactorLiquidez", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String)

        For Each filaLinea As DataRow In dtDetalle.Rows

            sCodPadFid = filaLinea(0).ToString().Trim()

            If (Not sCodPadFid.Equals("")) Then

                nTotAct = Val(filaLinea(1).ToString().Trim())
                nTotPas = Val(filaLinea(2).ToString().Trim())
                nPatrimonio = Val(filaLinea(3).ToString().Trim())
                If filaLinea(4).ToString().Trim() = "" Then
                    nFecha = 0
                Else
                    nFecha = DataUtility.ObtenerFecha(filaLinea(4).ToString().Trim())
                End If
                nFacRies = Val(filaLinea(5).ToString().Trim())
                nFacLiq = Val(filaLinea(6).ToString().Trim())
                sSituacion = filaLinea(7).ToString().Trim()

                db.SetParameterValue(dbCommand, "@p_CodigoPatrimonioFideicomiso", sCodPadFid)
                db.SetParameterValue(dbCommand, "@p_TotalActivo", nTotAct)
                db.SetParameterValue(dbCommand, "@p_TotalPasivo", nTotPas)
                db.SetParameterValue(dbCommand, "@p_Patrimonio", nPatrimonio)
                db.SetParameterValue(dbCommand, "@p_FechaVigencia", nFecha)
                db.SetParameterValue(dbCommand, "@p_FactorRiesgo", nFacRies)
                db.SetParameterValue(dbCommand, "@p_FactorLiquidez", nFacLiq)
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

    Public Function ValidarExistencia(ByVal CodPatriFideiDetalle As String, ByVal CodigoNemonico As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomisoDetalle_ValidarExistencia")
        db.AddInParameter(dbCommand, "@p_CodigoPatrimonioDetalle", DbType.String, CodPatriFideiDetalle)
        db.AddInParameter(dbCommand, "@p_CodigoNemonico", DbType.String, CodigoNemonico)
        Dim dsResul As DataSet = db.ExecuteDataSet(dbCommand)
        If dsResul.Tables(0).Rows(0)(0) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function ValidarPatrimonio(ByVal codigoPatrimonio As String, ByVal nombre As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("PatrimonioFideicomiso_ValidarPatrimonio")
        db.AddInParameter(dbCommand, "@p_CodigoPatrimonio", DbType.String, codigoPatrimonio)
        db.AddInParameter(dbCommand, "@p_Nombre", DbType.String, nombre)
        Dim dsResul As DataSet = db.ExecuteDataSet(dbCommand)
        If dsResul.Tables(0).Rows(0)(0) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
