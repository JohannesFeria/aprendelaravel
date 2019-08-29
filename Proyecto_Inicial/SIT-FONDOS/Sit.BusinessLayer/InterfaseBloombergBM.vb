Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Public Class InterfaseBloombergBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub
#Region " /* Funciones Insertar */ "

    Public Function RecuperaDivLibBloombergNoReg_Listar(ByVal FechaInformacion As Decimal, ByVal sEntidadExterna As String, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaInformacion, sEntidadExterna, dataRequest}
        Dim Tabla As New DataTable
        Try
            RegistrarAuditora(parameters)
            Tabla = New InterfaseBloombergDAM().RecuperaDivLibBloombergNoReg_Listar(FechaInformacion, sEntidadExterna).Tables(0)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tabla
    End Function


    Public Function SeleccionarPreciosBloomberg(ByVal fecha As Date, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim Tabla As New DataTable
        Try
            RegistrarAuditora(parameters)
            Tabla = New InterfaseBloombergDAM().SeleccionarPrecioBloomberg(fecha).Tables(0)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tabla
    End Function

    Public Function SeleccionarTipoCambioBloomberg(ByVal fecha As Date, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim Tabla As New DataTable
        Try
            RegistrarAuditora(parameters)
            Tabla = New InterfaseBloombergDAM().SeleccionarTipoCambioBloomberg(fecha).Tables(0)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tabla
    End Function
    Public Function SeleccionarIndDivLib(ByVal fecha As Date, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim Tabla As New DataTable
        Try
            RegistrarAuditora(parameters)
            Tabla = New InterfaseBloombergDAM().SeleccionarIndDivLib(fecha).Tables(0)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tabla
    End Function
    Public Function SeleccionarIndInversiones(ByVal fecha As Date, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim Tabla As New DataTable
        Try
            RegistrarAuditora(parameters)
            Tabla = New InterfaseBloombergDAM().SeleccionarIndInversiones(fecha).Tables(0)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tabla
    End Function
    Public Function SeleccionarIndBloomberg(ByVal fecha As Date, ByVal dataRequest As DataSet) As DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {fecha, dataRequest}
        Dim Tabla As New DataTable
        Try
            RegistrarAuditora(parameters)
            Tabla = New InterfaseBloombergDAM().SeleccionarIndBloomberg(fecha).Tables(0)


        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return Tabla
    End Function
    Public Function InsertarPrecioBloomberg(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = oInterfaseBloombergDAM.InsertarPreciosBloomberg(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function

    Public Function InsertarPrecioReal(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As DataTable

        Dim tabla As New DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            tabla = oInterfaseBloombergDAM.InsertarPreciosReales(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return tabla

    End Function
    Public Function InsertarTipoCambioReal(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As DataTable

        Dim Tabla As New DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            Tabla = oInterfaseBloombergDAM.InsertarTipoCambioReales(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return Tabla

    End Function
    Public Function InsertarTipoCambioBloomberg(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = oInterfaseBloombergDAM.InsertarTipoCambioBloomberg(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function
    Public Function InsertarIndicadorBloomberg(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = oInterfaseBloombergDAM.InsertarIndBloomberg(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function
    Public Function InsertarIndicadorInversiones(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = oInterfaseBloombergDAM.InsertarIndInversiones(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function

    Public Function InsertarIndicadorDivLib(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = oInterfaseBloombergDAM.InsertarIndDivLib(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function

    Public Function EliminarPreciosBloomberg(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarPrecioBloomberg(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarPreciosReales(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarPrecioReal(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarTipoCambioBloomberg(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarTipoCambioBloomberg(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarTipoCambio(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarTipoCambioReal(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarIndDivLib(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarIndDivLib(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarIndInversiones(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarIndInv(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function
    Public Function EliminarIndBloomberg(ByVal FechaCarga As Date, ByVal dataRequest As DataSet)

        Dim eliminado As Boolean = False
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {FechaCarga, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            eliminado = oInterfaseBloombergDAM.EliminarIndBloomberg(FechaCarga)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = true
            If (rethrow) Then
                Throw
            End If
        End Try
        Return eliminado
    End Function

    'HDG INC 59941	20100526
    Public Function ActualizarPrecioBloomberg(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As String

        Dim codigo As String = String.Empty
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            codigo = oInterfaseBloombergDAM.ActualizarPrecioBloomberg(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return codigo

    End Function

    'HDG INC 59941	20100526
    Public Function ActualizarPrecioReal(ByVal fecha As Date, ByVal dtDetalle As DataTable, ByVal dataRequest As DataSet) As DataTable

        Dim tabla As New DataTable
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dtDetalle, dataRequest}
        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM

            tabla = oInterfaseBloombergDAM.ActualizarPrecioReal(fecha, dtDetalle, dataRequest)
            RegistrarAuditora(parameters)

        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try

        Return tabla

    End Function

    'HDG OT 65289 20120626
    Public Function InsertarPrecioNav(ByVal objVectorPrecioBloombergNavBE As VectorPrecioBloombergNavBE, ByVal dataRequest As DataSet) As Boolean
        Dim strCodigo As String
        'Se solicita un código de ejecución al motor de transacciones
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        'Se agrupan los parámetros enviados en el mismo orden
        Dim parameters As Object() = {dataRequest}

        Try
            Dim oInterfaseBloombergDAM As New InterfaseBloombergDAM
            strCodigo = oInterfaseBloombergDAM.InsertarPrecioNav(objVectorPrecioBloombergNavBE, dataRequest)
            'Luego de terminar la ejecución de métodos(sin errores)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            RegistrarAuditora(parameters, ex)
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
        Return strCodigo
    End Function
#End Region
End Class
