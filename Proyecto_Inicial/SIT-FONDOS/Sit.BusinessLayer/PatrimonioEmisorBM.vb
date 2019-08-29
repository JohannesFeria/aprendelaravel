Imports System.Data
Imports Sit.DataAccessLayer
Imports Sit.BusinessEntities

Public Class PatrimonioEmisorBM

    Dim objPatrimionioEmisorDAM As PatrimonioEmisorDAM
    Dim objPatrimonioEmisorBE As PatrimonioEmisorBE

    Public Sub Insertar(ByVal objPE_BE As PatrimonioEmisorBE, ByVal datosRequest As DataSet)
        Try
            objPatrimionioEmisorDAM = New PatrimonioEmisorDAM
            objPatrimionioEmisorDAM.Insertar(objPE_BE, datosRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Actualizar(ByVal objPE_BE As PatrimonioEmisorBE, ByVal datosRequest As DataSet)
        Try
            objPatrimionioEmisorDAM = New PatrimonioEmisorDAM
            objPatrimionioEmisorDAM.Actualizar(objPE_BE, datosRequest)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
