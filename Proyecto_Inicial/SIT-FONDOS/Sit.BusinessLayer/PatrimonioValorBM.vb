Imports System.Data
'Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Transactions

Public Class PatrimonioValorBM
    'Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function SeleccionarPorFiltro(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal dataRequest As DataSet) As PatrimonioValorBE
        Try
            Return New PatrimonioValorDAM().SeleccionarPorFiltro(CodigoTipoInstrumento, CodigoMnemonico, Situacion, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarPorFiltro(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal Situacion As String, ByVal fechaInicio As Integer, ByVal fechaFin As Integer) As PatrimonioValorBE
        Try
            Return New PatrimonioValorDAM().SeleccionarPorFiltro(CodigoTipoInstrumento, CodigoMnemonico, Situacion, fechaInicio, fechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Seleccionar(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal FechaVigencia As String, ByVal dataRequest As DataSet) As PatrimonioValorBE
        Try
            Return New PatrimonioValorDAM().Seleccionar(CodigoTipoInstrumento, CodigoMnemonico, FechaVigencia, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Listar(ByVal dataRequest As DataSet) As PatrimonioValorBE
        Try
            Return New PatrimonioValorDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ActualizarPatrimonioValorPorExcel(ByVal dtPatrimonio As DataTable, ByVal dataRequest As DataSet, ByRef strmensaje As String) As Boolean
        Dim Codigo As Boolean = False
        Try
            Dim oPatrimonioValorDAM As New PatrimonioValorDAM
            Codigo = oPatrimonioValorDAM.ActualizarPatrimonioValorPorExcel(dtPatrimonio, dataRequest, strmensaje)
        Catch ex As Exception
            Throw ex
        End Try
        Return True
    End Function

    Public Function Insertar(ByVal oPatrimonioValor As PatrimonioValorBE, ByVal dataRequest As DataSet) As Boolean
        Try
            Dim oPatrimonioValorDAM As New PatrimonioValorDAM
            Return oPatrimonioValorDAM.Insertar(oPatrimonioValor, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Modificar(ByVal oPatrimonioValor As PatrimonioValorBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oPatrimonioValorDAM As New PatrimonioValorDAM
            actualizado = oPatrimonioValorDAM.Modificar(oPatrimonioValor, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    'Public Function Eliminar(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal dataRequest As DataSet) As Boolean
    Public Function Eliminar(ByVal CodigoTipoInstrumento As String, ByVal CodigoMnemonico As String, ByVal FechaVigencia As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oPatrimonioValorDAM As New PatrimonioValorDAM
            eliminado = oPatrimonioValorDAM.Eliminar(CodigoTipoInstrumento, CodigoMnemonico, FechaVigencia, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function

    Public Function ModificarIncrementoDecremento(ByVal clasificacion As String, ByVal nombre As String, ByVal valor As String, ByVal tipoIngreso As String) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oPatrimonioValorDAM As New PatrimonioValorDAM
            actualizado = oPatrimonioValorDAM.ModificarIncrementoDecremento(clasificacion, nombre, valor, tipoIngreso)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    Public Function EliminarIncrementoDecremento(ByVal clasificacion As String, ByVal CategoriaIncDec As String) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oPatrimonioValorDAM As New PatrimonioValorDAM
            actualizado = oPatrimonioValorDAM.EliminarIncrementoDecremento(clasificacion, CategoriaIncDec)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function

    Public Function SeleccionarPatrimonioEmisor(ByVal p_Id As Integer, ByVal p_CodigoTercero As String, ByVal p_CodigoEntidad As String, ByVal p_TipoValor As String, ByVal p_FechaInicio As Integer, ByVal p_FechaFin As Integer) As DataTable
        Try
            Dim objPatrimonioValor As New PatrimonioValorDAM
            Return objPatrimonioValor.SeleccionarPatrimonioEmisor(p_Id, p_CodigoTercero, p_CodigoEntidad, p_TipoValor, p_FechaInicio, p_FechaFin)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub EliminarPatrimonioEmisor(ByVal p_Id As Integer)
        Try
            Dim objPatrimonioValor As New PatrimonioValorDAM
            objPatrimonioValor.EliminarPatrimonioEmisor(p_Id)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GuardarRegistros(ByVal dtEmision As DataTable, ByVal dtEmisor As DataTable, ByVal datosRequest As DataSet)
        Try
            Using trans As New TransactionScope
                Dim objPV_DAM As New PatrimonioValorDAM
                Dim pvBE As PatrimonioValorBE
                If dtEmision IsNot Nothing Then
                    For Each drEmision As DataRow In dtEmision.Rows
                        pvBE = New PatrimonioValorBE
                        Dim oRow As PatrimonioValorBE.PatrimonioValorRow
                        oRow = CType(pvBE.PatrimonioValor.NewRow(), PatrimonioValorBE.PatrimonioValorRow)
                        oRow.CodigoMnemonico = drEmision("CodigoNemonico")
                        oRow.CodigoTipoInstrumento = ""
                        oRow.Patrimonio = drEmision("Valor")
                        oRow.FechaVigencia = Decimal.Parse(Date.Parse(drEmision("Fecha")).ToString("yyyyMMdd"))
                        oRow.Situacion = "A"
                        pvBE.PatrimonioValor.AddPatrimonioValorRow(oRow)
                        pvBE.PatrimonioValor.AcceptChanges()
                        objPV_DAM.Eliminar("", oRow.CodigoMnemonico, oRow.FechaVigencia, datosRequest)
                        objPV_DAM.Insertar(pvBE, datosRequest)
                    Next
                End If

                Dim objPE_DAM As New PatrimonioEmisorDAM
                Dim objPEBE As PatrimonioEmisorBE
                If dtEmisor IsNot Nothing Then
                    For Each drEmisor As DataRow In dtEmisor.Rows
                        objPEBE = New PatrimonioEmisorBE
                        objPEBE.id = 0
                        objPEBE.codigoEntidad = drEmisor("CodigoEntidad")
                        objPEBE.codigoTercero = ""
                        objPEBE.tipoValor = drEmisor("TipoValor")
                        objPEBE.valor = drEmisor("Valor")
                        objPEBE.fecha = Integer.Parse(Date.Parse(drEmisor("Fecha")).ToString("yyyyMMdd"))
                        objPE_DAM.Eliminar(objPEBE)
                        objPE_DAM.Insertar(objPEBE, datosRequest)
                    Next
                End If
                trans.Complete()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
