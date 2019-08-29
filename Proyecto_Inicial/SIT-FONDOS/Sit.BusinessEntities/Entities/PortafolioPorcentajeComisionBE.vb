Imports System.Data
Imports System.IO

Public Class PortafolioPorcentajeComisionBE

#Region "Constructor"

    Public Sub New()

    End Sub

#End Region

#Region "Propiedades Publicas"

    Public Property CodigoPortafolio As String
    Public Property Secuencia As Int32
    Public Property ValorMargenMinimo As Decimal
    Public Property ValorMargenMaximo As Decimal
    Public Property ValorPorcentajeComision As Decimal
    Public Property Situacion As String
    Public Property UsuarioCreacion As String
    Public Property FechaCreacion As Decimal
    Public Property HoraCreacion As String
    Public Property UsuarioModificacion As String
    Public Property FechaModificacion As Decimal
    Public Property HoraModificacion As String
    Public Property Host As String

#End Region

End Class
