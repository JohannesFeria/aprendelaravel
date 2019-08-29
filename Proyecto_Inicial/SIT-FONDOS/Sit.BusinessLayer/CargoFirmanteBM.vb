Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Public Class CargoFirmanteBM
    Inherits InvokerCOM

    Public Sub New()

    End Sub

    Public Sub InicializarCargoFirmante(ByRef oRow As CargoFirmanteBE.CargoFirmanteRow)
        Try
            Dim oCargoFirmanteDAM As New CargoFirmanteDAM
            oCargoFirmanteDAM.InicializarCargoFirmante(oRow)
            'Luego de terminar la ejecución de métodos(sin errores)             
        Catch ex As Exception
            'Si ocurre un error se invoca el mismo método pero se agrega el obj exception
            'Las siguientes 4 líneas deben agregarse para el Exception app block
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

#Region "Metodos Transaccionales"

#End Region

#Region "Metodos No Transaccionales"
    Public Function SeleccionarPorFiltro(ByVal oCargoFirmanteBE As CargoFirmanteBE, ByVal flagFirmante As Boolean, ByVal dataRequest As DataSet) As DataSet
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {oCargoFirmanteBE, flagFirmante, dataRequest}
        Try
            Dim oCargoFirmanteDAM As New CargoFirmanteDAM
            Return oCargoFirmanteDAM.SeleccionarPorFiltro(oCargoFirmanteBE, flagFirmante)
            RegistrarAuditora(parameters)
        Catch ex As Exception
            RegistrarAuditora(parameters, ex)
            Dim rethrow As Boolean = True
            If (rethrow) Then
                Throw
            End If
        End Try
    End Function
#End Region

End Class
