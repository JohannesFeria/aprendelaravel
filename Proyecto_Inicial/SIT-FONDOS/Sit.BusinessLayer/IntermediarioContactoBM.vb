Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

    Public  Class IntermediarioContactoBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Function Insertar(ByVal oIntermediarioContactoBE As IntermediarioContactoBE, ByVal dataRequest As DataSet) As Boolean

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oIntermediarioContactoBE, dataRequest}

        Try
            Dim oIntermediarioContactoDAM As New IntermediarioContactoDAM

            oIntermediarioContactoDAM.Insertar(oIntermediarioContactoBE, dataRequest)

            RegistrarAuditora(parameters)

        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function

    Public Function SeleccionarPorFiltro(ByVal codigoTercero As String, ByVal codigoContacto As String, ByVal situacion As String, ByVal dataRequest As DataSet) As IntermediarioContactoBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTercero, codigoContacto, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New IntermediarioContactoDAM().SeleccionarPorFiltro(codigoTercero, codigoContacto, situacion)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

    End Function
    Public Function ExistenciaIntermediarioContacto(ByVal codigoTercero As String, ByVal codigoContacto As String, ByVal dataRequest As DataSet) As IntermediarioContactoBE

        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTercero, codigoContacto, dataRequest}

        Try
            RegistrarAuditora(parameters)

            Return New IntermediarioContactoDAM().ExistenciaIntermediarioContacto(codigoTercero, codigoContacto)

        Catch ex As Exception

            RegistrarAuditora(parameters, ex)

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

    Public Function Modificar(ByVal oIntermediarioContactoBE As IntermediarioContactoBE, ByVal dataRequest As DataSet) As Boolean

        Dim actualizado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oIntermediarioContactoBE, dataRequest}

        Dim oIntermediarioContactoDAM As New IntermediarioContactoDAM

        Try

            actualizado = oIntermediarioContactoDAM.Modificar(oIntermediarioContactoBE, dataRequest)
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

    Public Function Eliminar(ByVal codigoTercero As String, ByVal codigoContacto As String, ByVal dataRequest As DataSet) As Boolean

        Dim eliminado As Boolean = False
        Dim codigoEjecucion As Integer = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {codigoTercero, codigoContacto, dataRequest}
        Dim oIntermediarioContactoDAM As New IntermediarioContactoDAM

        Try

            eliminado = oIntermediarioContactoDAM.Eliminar(codigoTercero, codigoContacto, dataRequest)
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
            Dim dsTercerosAct As DataSet = New IntermediarioContactoDAM().ListarContactosXIntermediario(CodigoTercero)
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

