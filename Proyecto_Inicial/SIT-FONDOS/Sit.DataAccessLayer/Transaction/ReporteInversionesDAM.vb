Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class ReporteInversionesDAM


    Public Sub New()

    End Sub

    'CMB OT 65473 20120919
    Public Function ReporteOperacionRentaFija(ByVal param1 As Decimal, ByVal portafolio As String, ByVal flag As Decimal, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_OperacionesRentaFija")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20080930
        db.AddInParameter(dbCommand, "@p_FlagMostrarFirma", DbType.Decimal, flag)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    Public Function SeleccionarOperacionRentaFija(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As RentaFijaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_OperacionesRentaFija")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20080930

        Dim oReporte As New RentaFijaBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteOperacionRentaFija")
        Return oReporte

    End Function

    'CMB OT 65473 20120919
    Public Function ReporteOperacionRentaVariable(ByVal param1 As Decimal, ByVal portafolio As String, ByVal flag As Decimal, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_OperacionesRentaVariable")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20080930
        db.AddInParameter(dbCommand, "@p_FlagMostrarFirma", DbType.Decimal, flag)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function SeleccionarOperacionRentaVariable(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As RentaVariableBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_OperacionesRentaVariable")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
            Using oReporte As New RentaVariableBE
                db.LoadDataSet(dbCommand, oReporte, "ReporteOperacionRentaVariable")
                Return oReporte
            End Using
        End Using
    End Function

    'CMB OT 65473 20120919
    Public Function ReporteOperacionDivisa(ByVal param1 As Decimal, ByVal portafolio As String, ByVal flag As Decimal, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_OperacionesDivisas")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20080930
        db.AddInParameter(dbCommand, "@p_FlagMostrarFirma", DbType.Decimal, flag)

        Return db.ExecuteDataSet(dbCommand)

    End Function

    Public Function SeleccionarOperacionDivisa(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DivisasBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("Inversiones_Reportes_OperacionesDivisas")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
            Using oReporte As New DivisasBE
                db.LoadDataSet(dbCommand, oReporte, "ReporteOperacionDivisa")
                Return oReporte
            End Using
        End Using
    End Function
    Public Function SeleccionarPorCorrelativo(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As PorCorrelativoBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_PorCorrelativo")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20081001

        Dim oReporte As New PorCorrelativoBE
        db.LoadDataSet(dbCommand, oReporte, "PorCorrelativo")
        Return oReporte

    End Function
    Public Function SeleccionarPorGestor_Divisa(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal dataRequest As DataSet) As DivisasBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_PorGestor_Divisas")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)


        Dim oReporte As New DivisasBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteOperacionDivisa")
        Return oReporte

    End Function
    Public Function SeleccionarPorGestor_RentaVariable(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal dataRequest As DataSet) As RentaVariableBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_PorGestor_RentaVariable")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)


        Dim oReporte As New RentaVariableBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteOperacionRentaVariable")
        Return oReporte

    End Function

    Public Function SeleccionarPorGestor_RentaFija(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal dataRequest As DataSet) As RentaFijaBE

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_PorGestor_RentaFija")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)

        Dim oReporte As New RentaFijaBE
        db.LoadDataSet(dbCommand, oReporte, "ReporteOperacionRentaFija")
        Return oReporte

    End Function

    Public Function SeleccionarPorGestor(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_PorGestor")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio) 'RGF 20080930

        Return db.ExecuteDataSet(dbCommand)

    End Function

    'CMB OT 64769 20120329
    Public Function SeleccionarOperacionesCashCall(ByVal fechaInicio As Decimal, ByVal fechaFin As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReporteInversiones_OperacionesCashCall")

        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, fechaInicio)
        db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, fechaFin)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function
    '============================================================================
    ' CREADO POR  : Zoluxiones Consulting S.A.C (JVC)
    ' DESCRIPCIÓN : Obtiene datos de contratos de forwards
    ' FECHA DE CREACIÓN : 06/04/2009
    ' PARÁMETROS ENTRADA: p_CodigoPortafolioSBS: Código de portafolio SBS
    '                     p_CodigoMoneda	   : Moneda negociada
    '                     p_CodigoMonedaDestino: Moneda destino
    '	                  p_fechainicio        : fecha de inicio
    '                     p_fechafin           : fecha de fin
    '============================================================================
    Public Function InventarioForward(ByVal p_CodigoPortafolioSBS As String, _
                                      ByVal p_CodigoMoneda As String, _
                                      ByVal p_CodigoMonDes As String, _
                                      ByVal p_fechainicio As Decimal, _
                                      ByVal p_fechafin As Decimal) As DataTable
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Inversiones_Reportes_InventarioForward")
        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, p_CodigoPortafolioSBS)
        db.AddInParameter(dbCommand, "@p_CodigoMoneda", DbType.String, p_CodigoMoneda)
        db.AddInParameter(dbCommand, "@p_CodigoMonedaDestino", DbType.String, p_CodigoMonDes)
        db.AddInParameter(dbCommand, "@p_fechainicio", DbType.Decimal, p_fechainicio)
        db.AddInParameter(dbCommand, "@p_fechafin", DbType.Decimal, p_fechafin)
        Return db.ExecuteDataSet(dbCommand).Tables(0)
    End Function
    Public Function ImpuestoRenta(ByVal p_fechainicio As Decimal) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("pr_SIT_gl_ReportePagoMensualImpuestoRenta")
        db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Int32, p_fechainicio)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarOperacionRentaFijaExcel(ByVal param1 As Decimal, ByVal param2 As Decimal, ByVal portafolio As String, ByVal dataRequest As DataSet) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Using dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Inversiones_Reportes_OperacionesRentaFija")
            db.AddInParameter(dbCommand, "@p_FechaInicio", DbType.Decimal, param1)
            db.AddInParameter(dbCommand, "@p_FechaFin", DbType.Decimal, param2)
            db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, portafolio)
            Using ds As New DataSet
                db.LoadDataSet(dbCommand, ds, "ReporteOperacionRentaFijaExcel")
                Return ds
            End Using
        End Using
    End Function
End Class
