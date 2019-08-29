Imports System
Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports sit.DataAccessLayer
Imports Sit.BusinessEntities

	''' <summary>
	''' Clase para el acceso de los datos para Cartera tabla.
	''' </summary>
Public Class CarteraBM
    Inherits InvokerCOM
    Public Sub New()

    End Sub

    Public Function GeneraSaldosCarteraTitulo(ByVal fechaInicial As Decimal, ByVal fechaFinal As Decimal, ByVal codigoPortafolioSBS As String, ByVal dataRequest As DataSet) As Boolean
        Dim blnResultado As Boolean = False
        Try
            blnResultado = New CarteraDAM().GeneraSaldosCarteraTitulo(fechaInicial, fechaFinal, codigoPortafolioSBS, dataRequest)
        Catch ex As Exception
            Throw ex
        End Try
        Return blnResultado
    End Function

    Public Function Insertar(ByVal codigoCartera As Decimal, ByVal fecha As Decimal, ByVal codigoPortafolio As String)

    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Cartera tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoCartera As Decimal) As DataSet

    End Function

    ''' <summary>
    ''' Selecciona expedientes de Cartera tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

    End Function

    ''' <summary>
    ''' Lista todos los expedientes de Cartera tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet

    End Function

    ''' <summary>
    ''' Midifica un expediente en Cartera tabla.
    ''' <summary>
    ''' <param name="codigoCartera"></param>
    ''' <param name="fecha"></param>
    ''' <param name="codigoPortafolio"></param>
    Public Function Modificar(ByVal codigoCartera As Decimal, ByVal fecha As Decimal, ByVal codigoPortafolio As String)

    End Function

    ''' <summary>
    ''' Elimina un expediente de Cartera table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoCartera As Decimal)

    End Function

    ''' <summary>
    ''' Elimina un expediente de Cartera table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String)

    End Function
End Class

