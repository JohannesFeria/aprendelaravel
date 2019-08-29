Imports System.Data
Imports MotorTransaccionesProxy
'Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class GrupoPortafolioBM
    Inherits InvokerCOM

#Region " /* Funciones Seleccionar */ "

    Public Function Listar(ByVal eGrupoPortafolioBE As GrupoPortafolioBE) As DataTable
        Try
            Return New GrupoPortafolioDAM().Listar(eGrupoPortafolioBE)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function CodigoPortafolioListar() As DataTable
        Try
            Return New GrupoPortafolioDAM().CodigoPortafolioListar()
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#End Region

End Class

