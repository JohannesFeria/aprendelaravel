Option Explicit On 
Option Strict Off

#Region "/* Imports */"

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql

#End Region

Public Class TipoTerceroDAM

#Region "/* Constructor */"

    Public Sub New()

    End Sub

#End Region

#Region "/* Funciones No Transaccionales */"

    Public Function SeleccionarPorFiltro(ByVal codigoTipoTercero As String, ByVal descripcion As String, ByVal situacion As String) As TipoTerceroBE

        Dim oTipoTerceroBE As New TipoTerceroBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoTercero_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoTipoTercero", DbType.String, codigoTipoTercero)
        db.AddInParameter(dbCommand, "@p_Situacion", DbType.String, situacion)
        db.AddInParameter(dbCommand, "@p_Descripcion", DbType.String, situacion)

        db.LoadDataSet(dbCommand, oTipoTerceroBE, "TipoTercero")

        Return oTipoTerceroBE

    End Function

    Public Function Seleccionar(ByVal codigoIndicador As String) As TipoTerceroBE

        Dim oTipoTerceroBE As New TipoTerceroBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoTercero_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoTipoTercero", DbType.String, codigoIndicador)

        db.LoadDataSet(dbCommand, oTipoTerceroBE, "TipoTercero")

        Return oTipoTerceroBE

    End Function

    Public Function Listar() As TipoTerceroBE

        Dim oTipoTerceroBE As New TipoTerceroBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("TipoTercero_Listar")

        db.LoadDataSet(dbCommand, oTipoTerceroBE, "TipoTercero")

        Return oTipoTerceroBE

    End Function

#End Region

End Class
