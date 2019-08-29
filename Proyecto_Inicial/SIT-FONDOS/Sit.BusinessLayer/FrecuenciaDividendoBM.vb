
Imports System
Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Data.Common
Imports MotorTransaccionesProxy

'INICIO | ZOLUXIONES | RCE | ProyFondosII - RF002 - Creación de Objeto BusinessLayer para tabla FrecuenciaDividendo| 18/05/18

Public Class FrecuenciaDividendoBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

#Region " /* Funciones Seleccionar */ "

    Public Function Listar(ByVal dataRequest As DataSet) As FrecuenciaDividendoBE
        Dim codigoEjecucion = ObtenerCodigoEjecucion(dataRequest)
        Dim parameters As Object() = {dataRequest}
        Try

            Return New FrecuenciaDividendoDAM().Listar(dataRequest)
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
    End Function


#End Region

#Region " /* Funciones Insertar */ "

#End Region

#Region " /* Funciones Modificar */"

#End Region

#Region " /* Funciones Eliminar */"

#End Region

#Region " /* Funciones Personalizadas*/"

#End Region

    'FIN | ZOLUXIONES | RCE | ProyFondosII - RF002 - Creación de Objeto BusinessLayer para tabla FrecuenciaDividendo| 18/05/18


End Class

