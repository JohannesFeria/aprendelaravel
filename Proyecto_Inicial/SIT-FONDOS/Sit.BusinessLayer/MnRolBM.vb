Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities
Imports System.Collections.Generic

Public Class MnRolBM

    Dim dam As New MnRolDAM

    Public Function Listar(rolBE As MnRolBE, ByVal dataRequest As DataSet) As List(Of MnRolBE)
        Return dam.Listar(rolBE, dataRequest)
    End Function

    Public Function Insertar(rolBE As MnRolBE, ByVal dataRequest As DataSet) As Boolean
        Return dam.Insertar(rolBE, dataRequest)
    End Function

    Public Function Actualizar(rolBE As MnRolBE, ByVal dataRequest As DataSet) As Boolean
        Return dam.Actualizar(rolBE, dataRequest)
    End Function

End Class
