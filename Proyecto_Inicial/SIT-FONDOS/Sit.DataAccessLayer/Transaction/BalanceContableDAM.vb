Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

Public Class BalanceContableDAM
    Private sqlCommand As String = ""
    Private oBalanceContableRow As BalanceContableBE.BalanceContableRow

    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function Listar() As BalanceContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As BalanceContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_Listar")

        Dim objeto As New BalanceContableBE
        db.LoadDataSet(dbCommand, objeto, "BalanceContable")
        Return objeto
    End Function


    Public Function SeleccionarPorFiltro(ByVal CodigoEmisor As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As BalanceContableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, CodigoEmisor)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, Situacion)

        Dim objeto As New BalanceContableBE
        db.LoadDataSet(dbCommand, objeto, "BalanceContable")
        Return objeto
    End Function

#End Region


    Public Function Insertar(ByVal ob As BalanceContableBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_Insertar")
        oBalanceContableRow = CType(ob.BalanceContable.Rows(0), BalanceContableBE.BalanceContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, oBalanceContableRow.CodigoEmisor)
        db.AddInParameter(dbCommand, "@p_TotalActivo", DbType.Decimal, oBalanceContableRow.TotalActivo)
        db.AddInParameter(dbCommand, "@p_TotalPasivo", DbType.Decimal, oBalanceContableRow.TotalPasivo)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, oBalanceContableRow.Patrimonio)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oBalanceContableRow.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oBalanceContableRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
    End Function

    'OAB 20091023
    'Se implmenta para poder insertar registros a traves de un archivo excel
    Public Function ActualizarPorExcel(ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String
        Dim strCodigo As String = String.Empty
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim CodigoSBS, Estado As String
        Dim TotalActivo, TotalPasivo, Patrimonio, FechaVigencia As String
        Dim i, filaInicioExcel As Integer
        filaInicioExcel = 0
        i = 0

        For Each filaLinea As DataRow In dtDetalle.Rows
            If i >= filaInicioExcel Then

                Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_ActualizarPorExcel") 'HDG 20120417

                CodigoSBS = filaLinea(1).ToString().Trim()
                If (Not CodigoSBS.Equals("")) Then    'HDG 20120417
                    TotalActivo = filaLinea(3).ToString().Trim()
                    TotalPasivo = filaLinea(4).ToString().Trim()
                    Patrimonio = filaLinea(5).ToString().Trim()
                    Dim aFechaVigencia() As String = CDate(filaLinea(6).ToString().Trim()).ToShortDateString.Split("/")
                    FechaVigencia = CStr(aFechaVigencia(2) & aFechaVigencia(1) & aFechaVigencia(0))
                    Estado = filaLinea(7).ToString().Trim()

                    If (Not TotalActivo.Equals("")) And _
                        (Not TotalPasivo.Equals("")) And _
                        (Not Patrimonio.Equals("")) And _
                        (Not Estado.Equals("")) Then

                        db.AddInParameter(dbCommand, "@p_CodigoSBS", DbType.String, CodigoSBS)
                        db.AddInParameter(dbCommand, "@p_TotalActivo", DbType.Decimal, CDec(TotalActivo))
                        db.AddInParameter(dbCommand, "@p_TotalPasivo", DbType.Decimal, CDec(TotalPasivo))
                        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, CDec(Patrimonio))
                        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, CDec(FechaVigencia))

                        If Estado.ToUpper.Equals("ACTIVO") Then
                            db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "A")
                        Else
                            If Estado.ToUpper.Equals("INACTIVO") Then
                                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "I")
                            Else
                                'HDG INC 58619	20100318
                                db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, "I")
                            End If
                        End If

                        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
                        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
                        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

                        db.ExecuteNonQuery(dbCommand)
                    End If
                End If
            End If
            i = i + 1
        Next

        Return strCodigo
    End Function

    Public Function Modificar(ByVal ob As BalanceContableBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_Modificar")
        oBalanceContableRow = CType(ob.BalanceContable.Rows(0), BalanceContableBE.BalanceContableRow)

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, oBalanceContableRow.CodigoEmisor)
        db.AddInParameter(dbCommand, "@p_TotalActivo", DbType.Decimal, oBalanceContableRow.TotalActivo)
        db.AddInParameter(dbCommand, "@p_TotalPasivo", DbType.Decimal, oBalanceContableRow.TotalPasivo)
        db.AddInParameter(dbCommand, "@p_Patrimonio", DbType.Decimal, oBalanceContableRow.Patrimonio)
        db.AddInParameter(dbCommand, "@p_FechaVigencia", DbType.Decimal, oBalanceContableRow.FechaVigencia)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, oBalanceContableRow.Situacion)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function


    Public Function Eliminar(ByVal CodigoEmisor As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("BalanceContable_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoEmisor", DbType.String, CodigoEmisor)

        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(dataRequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(dataRequest, "Host"))

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
End Class
