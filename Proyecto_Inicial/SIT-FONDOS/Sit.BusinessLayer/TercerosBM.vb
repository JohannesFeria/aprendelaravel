Imports System.Data
Imports MotorTransaccionesProxy
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Transactions

Public Class TercerosBM
    'Inherits InvokerCOM
    Public Sub New()
    End Sub
    Public Sub Insertar(ByVal oTerceros As TercerosBE, ByVal oCuentaTerceros As CuentaTercerosBE, ByVal oListFondoCliente As ListFondoCliente, ByVal dataRequest As DataSet)
        Try
            Using trans As New TransactionScope
                Dim oTercerosDAM As New TercerosDAM
                oTercerosDAM.Insertar(oTerceros, oCuentaTerceros, dataRequest)
                Dim oTercerosRow As TercerosBE.TercerosRow
                oTercerosRow = DirectCast(oTerceros.Terceros.Rows(0), TercerosBE.TercerosRow)
                oTercerosDAM.EliminarFondoCliente(oTercerosRow.CodigoTercero)
                oTercerosDAM.InsertarFondoCliente(oListFondoCliente, dataRequest)
                trans.Complete()
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Function Modificar(ByVal oTerceros As TercerosBE, ByVal oCuentaTerceros As CuentaTercerosBE, ByVal dataRequest As DataSet) As Boolean
        Dim actualizado As Boolean = False
        Try
            Dim oTercerosDAM As New TercerosDAM
            actualizado = oTercerosDAM.Modificar(oTerceros, oCuentaTerceros, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return actualizado
    End Function
    Public Function Modificar(ByVal oTerceros As TercerosBE, ByVal oCuentaTerceros As CuentaTercerosBE, ByVal oListFondoCliente As ListFondoCliente, ByVal dataRequest As DataSet) As Boolean
        Try
            Modificar = False
            Using trans As New TransactionScope
                Dim oTercerosDAM As New TercerosDAM
                oTercerosDAM.Modificar(oTerceros, oCuentaTerceros, dataRequest)
                Dim oTercerosRow As TercerosBE.TercerosRow
                oTercerosRow = DirectCast(oTerceros.Terceros.Rows(0), TercerosBE.TercerosRow)
                oTercerosDAM.EliminarFondoCliente(oTercerosRow.CodigoTercero)
                oTercerosDAM.InsertarFondoCliente(oListFondoCliente, dataRequest)
                trans.Complete()
                Modificar = True
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Eliminar(ByVal codigoTercero As String, ByVal dataRequest As DataSet) As Boolean
        Dim eliminado As Boolean = False
        Try
            Dim oTercerosDAM As New TercerosDAM
            eliminado = oTercerosDAM.Eliminar(codigoTercero, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return eliminado
    End Function
    Public Function SeleccionarBancoPorMercPortMone(ByVal sCodigoMercado As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarBancoPorMercPortMone(sCodigoMercado, sCodigoPortafolioSBS, sCodigoMoneda)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarContactoPorBancoPorMercPortMone(ByVal sCodigoMercado As String, ByVal sCodigoPortafolioSBS As String, ByVal sCodigoMoneda As String,
    ByVal sCodigoBanco As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarContactoPorBancoPorMercPortMone(sCodigoMercado, sCodigoPortafolioSBS, sCodigoMoneda, sCodigoBanco)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    ' Selecciona expedientes de Terceros tabla por una llave extranjera.
    Public Function SeleccionarBancoPorCodigoMercado(ByVal codigoMercado As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarBancoPorCodigoMercado(codigoMercado)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    ' Selecciona expedientes de Terceros tabla por una llave extranjera.
    Public Function SeleccionarBancoPorCodigoMercadoYPortafolio(ByVal codigoMercado As String, ByVal codigoPortafolioSBS As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarBancoPorCodigoMercadoYPortafolio(codigoMercado, codigoPortafolioSBS)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorCodigoMercado(ByVal codigoMercado As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarPorCodigoMercado(codigoMercado)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltroMercado(ByVal clasificacionTercero As String, ByVal tipoTercero As String, ByVal sCodigoPais As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarPorFiltroMercado(clasificacionTercero, tipoTercero, sCodigoPais)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal clasificacionTercero As String, ByVal tipoTercero As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarPorFiltro(clasificacionTercero, tipoTercero)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarBanco(ByVal codigoTercero As String, ByVal CodigoMoneda As String) As TercerosBE
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.SeleccionarBanco(codigoTercero, CodigoMoneda)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal clasificacionTercero As String, ByVal descripcion As String,
    ByVal codigoTipoDocumento As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TercerosBE
        Try
            Return New TercerosDAM().SeleccionarPorFiltro(codigoTercero, clasificacionTercero, descripcion, codigoTipoDocumento, codigoSectorEmpresarial, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function SeleccionarPorFiltroActivo(ByVal codigoTercero As String, ByVal clasificacionTercero As String, ByVal descripcion As String,
    ByVal codigoTipoDocumento As String, ByVal codigoSectorEmpresarial As String, ByVal situacion As String, ByVal dataRequest As DataSet) As TercerosBE
        Try
            Return New TercerosDAM().SeleccionarPorFiltroActivo(codigoTercero, clasificacionTercero, descripcion, codigoTipoDocumento, codigoSectorEmpresarial, situacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Seleccionar(ByVal codigoTercero As String, ByVal dataRequest As DataSet) As TercerosBE
        Try
            Return New TercerosDAM().Seleccionar(codigoTercero)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Listar(ByVal dataRequest) As TercerosBE
        Try
            Return New TercerosDAM().Listar(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarTerceroPorGrupoIntermediario(ByVal Clasificacion As String, ByVal strFiltro As String) As DataSet
        Dim dsAux As DataSet
        Try
            Dim oTerceroDAM As New TercerosDAM
            dsAux = oTerceroDAM.ListarTerceroPorGrupoIntermediario(Clasificacion, strFiltro)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsAux
    End Function

    'Public Sub Actualizar(ByVal dataRequest As DataSet)
    '    Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
    '    Dim parameters As Object() = {dataRequest}
    '    RegistrarAuditora(parameters)
    'End Sub

    Public Function ListarTerceroPorClasificacion(ByVal Clasificacion As String) As DataSet
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.ListarTerceroPorClasificacion(Clasificacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarTerceroPorClasificacionSoloBancos(ByVal Clasificacion As String) As DataSet
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.ListarTerceroPorClasificacionSoloBancos(Clasificacion)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function ListarTerceroPorClasificacionValor(ByVal Clasificacion As String, ByVal codigoNemonico As String, ByVal codigoPortafolioSBS As String) As DataSet
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.ListarTerceroPorClasificacionValores(Clasificacion, codigoNemonico, codigoPortafolioSBS)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarTerceroPorClasificacionCustodio(ByVal Clasificacion As String, ByVal custodio As String) As DataSet
        Try
            Dim oTerceroDAM As New TercerosDAM
            Return oTerceroDAM.ListarTerceroPorClasificacionCustodio(Clasificacion, custodio)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
    Public Function ListarTerceroPorClasificacionCustodioXGrupoInter(ByVal Clasificacion As String, ByVal custodio As String, ByVal strFiltro As String) As DataSet
        Dim dsAux As DataSet
        Try
            Dim oTerceroDAM As New TercerosDAM
            dsAux = oTerceroDAM.ListarTerceroPorClasificacionCustodioXGrupoInter(Clasificacion, custodio, strFiltro)
        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return dsAux
    End Function
    Public Function Tercero_ListarEntidadFinanciera(ByVal dataRequest) As TercerosBE
        Try
            Return New TercerosDAM().Tercero_ListarEntidadFinanciera(dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Tercero_SeleccionarEntidadFinanciera(ByVal strCodigo As String, ByVal dataRequest As DataSet) As DataSet
        Try
            Return New TercerosDAM().Tercero_SeleccionarEntidadFinanciera(strCodigo, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function Tercero_ObtenerSaldoCustodio(ByVal strCodigoTercero As String, ByVal strCodigoPortafolioSBS As String, ByVal strCodigoMnemonico As String,
    ByVal decFecha As Decimal, ByVal dataRequest As DataSet) As DataSet
        Dim dsAux As DataSet = Nothing
        Try
            dsAux = New TercerosDAM().Tercero_ObtenerSaldoCustodio(strCodigoTercero, strCodigoPortafolioSBS, strCodigoMnemonico, decFecha)
        Catch ex As Exception
            Throw ex
        End Try
        Return dsAux
    End Function
    Public Function ListarTerceroReporte() As DataTable
        Try
            Return New TercerosDAM().ListarTerceroReporte()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GrupoIntermediario_SeleccionarPorTercero(ByVal CodigoTercero As String) As String
        Try
            Return New TercerosDAM().GrupoIntermediario_SeleccionarPorTercero(CodigoTercero)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT 10090 - 26/07/2017 - Carlos Espejo
    'Descripcion: Lista intermediarios por descripcion
    Public Function ListarIntermediarios(ByVal Descripcion As String) As TercerosEList
        Try
            Return New TercerosDAM().ListarIntermediarios(Descripcion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    'OT-11033 - 03/01/2017 - Ian Pastor M.
    'Descripcion: Verifica si existen terceros negociados (Ejecutados o confirmados)
    Public Function VerificarExisteTerceroNegociado(ByVal codigoTercero As String) As Boolean
        Dim existe As Boolean = False
        Dim objOrdenInversion As New OrdenInversionWorkFlowDAM
        Dim dt As DataTable = objOrdenInversion.OrdenInversion_ExisteTercero(codigoTercero)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("Cantidad") > 0 Then
                existe = True
            End If
        End If
        Return existe
    End Function

    Public Function ListarTercerPorTipoEntidad(ByVal codigoTipoEntidad As String) As DataTable
        Try
            Dim objTerceroDAM As New TercerosDAM
            Return objTerceroDAM.ListarTercerPorTipoEntidad(codigoTipoEntidad)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarTercero_FondoCliente(ByVal objTerceroFC As Terceros_FondoClienteBE) As DataTable
        Try
            Return New TercerosDAM().SeleccionarTercero_FondoCliente(objTerceroFC)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SeleccionarRatingTercero_Historia(ByVal p_CodigoTercero As String, ByVal p_CodigoEntidad As String, ByVal p_Fecha As Integer) As DataTable
        Try
            Return New TercerosDAM().SeleccionarRatingTercero_Historia(p_CodigoTercero, p_CodigoEntidad, p_Fecha)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarClientesMandato(ByVal p_Descripcion As String, ByVal p_Situacion As String, ByVal p_CodigoTercero As String) As TercerosBE
        Try
            Return New TercerosDAM().ListarClientesMandato(p_Descripcion, p_Situacion, p_CodigoTercero)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function ListarMandatos() As TercerosBE
        Try
            Return New TercerosDAM().ListarMandatos()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ListarBancosMandatos() As TercerosBE
        Try
            Return New TercerosDAM().ListarBancosMandatos()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
