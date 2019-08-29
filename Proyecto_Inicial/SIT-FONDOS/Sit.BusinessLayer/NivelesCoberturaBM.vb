'Creado por: HDG OT 62087 Nro10-R19 20110310
Imports System.Data
Imports MotorTransaccionesProxy
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class NivelesCoberturaBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(codigotercero As String, situacion As String, dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean

        Try
            Dim oNivelesCoberturaDAM As New NivelesCoberturaDAM

            oNivelesCoberturaDAM.Insertar(codigotercero, situacion, dtDetalle, dataRequest)
            Return True

        Catch ex As Exception
            Dim rethrow As Boolean = False
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal situacion As String) As NivelesCoberturaBE

        Dim parameters As Object() = {codigoTercero}

        Try
            RegistrarAuditora(parameters)

            Return New NivelesCoberturaDAM().SeleccionarPorFiltro(codigoTercero, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro_sura(ByVal codigoTercero As String, ByVal situacion As String) As DataTable

        Dim parameters As Object() = {codigoTercero}

        Try
            RegistrarAuditora(parameters)

            Return New NivelesCoberturaDAM().SeleccionarPorFiltro_sura(codigoTercero, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltroDetalle_sura(ByVal codigoTercero As String, ByVal situacion As String) As DataTable

        Dim parameters As Object() = {codigoTercero}

        Try
            RegistrarAuditora(parameters)

            Return New NivelesCoberturaDAM().SeleccionarPorFiltroDetalle_sura(codigoTercero, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function seleccionarPortafoliosCobertura_sura() As DataTable
        Try

            Return New NivelesCoberturaDAM().seleccionarPortafoliosCobertura_sura()

        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function

    Public Function Seleccionar(ByVal codigoIntermediario As String, ByVal codigoContacto As String) As DataSet

    End Function

    Public Function SeleccionarPorCodigoIntermediario(ByVal codigoIntermediario As String) As DataSet


    End Function

    Public Function SeleccionarPorCodigoContacto(ByVal codigoContacto As String) As DataSet


    End Function

    Public Function Listar() As DataSet

    End Function

    Public Function Modificar(ByVal oNivelesCoberturaBE As NivelesCoberturaBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oNivelesCoberturaBE, dataRequest}

        Dim oNivelesCoberturaDAM As New NivelesCoberturaDAM

        Try

            actualizado = oNivelesCoberturaDAM.Modificar(oNivelesCoberturaBE, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado

    End Function

    Public Function Modificar_sura(codigoportafolio As String, situacion As String, dtDetalle As DataTable, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)

        Dim oNivelesCoberturaDAM As New NivelesCoberturaDAM

        Try

            actualizado = oNivelesCoberturaDAM.Modificar_sura(codigoportafolio, situacion, dtDetalle, dataRequest)

        Catch ex As Exception
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return actualizado

    End Function

    Public Function Eliminar(ByVal codigoTercero As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTercero, dataRequest}
        Dim oNivelesCoberturaDAM As New NivelesCoberturaDAM

        Try

            eliminado = oNivelesCoberturaDAM.Eliminar(codigoTercero, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado

    End Function

#Region "Funciones Alberto"
    Public Function ListarContactosXIntermediario(ByVal CodigoTercero As String) As DataSet
        Try
            Dim dsTercerosAct As DataSet = New NivelesCoberturaDAM().ListarContactosXIntermediario(CodigoTercero)
            Dim dstercerosAct1 As New DataSet
            Dim dttercerosAct1 As New DataTable
            dttercerosAct1.Columns.Add("CodigoContacto", System.Type.GetType("System.String"))
            dttercerosAct1.Columns.Add("Descripcion", System.Type.GetType("System.String"))
            Dim drrow As DataRow = dttercerosAct1.NewRow()
            drrow("CodigoContacto") = "0"
            drrow("Descripcion") = "--SELECCIONE--"
            dttercerosAct1.Rows.Add(drrow)
            For Each dr As DataRow In dsTercerosAct.Tables(0).Rows
                drrow = dttercerosAct1.NewRow()
                drrow("CodigoContacto") = dr("CodigoContacto")
                drrow("Descripcion") = dr("DescripcionContacto")
                dttercerosAct1.Rows.Add(drrow)
            Next
            dstercerosAct1.Tables.Add(dttercerosAct1)
            Return dstercerosAct1

        Catch ex As Exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

#Region " /* Funciones Personalizadas*/"
    Public Sub Ingresar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
    Public Sub Actualizar(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub

    Public Sub Salir(ByVal dataRequest As DataSet)
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        RegistrarAuditora(parameters)
    End Sub
#End Region

End Class
