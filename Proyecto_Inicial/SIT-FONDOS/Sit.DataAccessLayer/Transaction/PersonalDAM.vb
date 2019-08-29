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

Public Class PersonalDAM

#Region "/* Constructor */"

    Public Sub New()

    End Sub

#End Region
    Private oPersonalRow As PersonalBE.PersonalRow
    Private decimalVacio As String = ""

    Public Function Listar(ByVal dataRequest) As PersonalBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim objeto As New PersonalBE
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Personal_Listar")
        db.LoadDataSet(dbCommand, objeto, "Personal")
        Return objeto
    End Function



    Public Function Insertar(ByVal ob As PersonalBE, ByVal dataRequest As DataSet)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim sqlCommand As String = "Personal_Insertar"
        Dim dbCommand As DbCommand = db.GetStoredProcCommand(sqlCommand)
        oPersonalRow = CType(ob.Personal.Rows(0), PersonalBE.PersonalRow)
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oPersonalRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, oPersonalRow.CodigoUsuario)
        db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, oPersonalRow.CodigoCentroCosto)
        db.AddInParameter(dbCommand, "@p_ApellidoPaterno", DbType.String, oPersonalRow.ApellidoPaterno)
        db.AddInParameter(dbCommand, "@p_ApellidoMaterno", DbType.String, oPersonalRow.ApellidoMaterno)
        db.AddInParameter(dbCommand, "@p_PrimerNombre", DbType.String, oPersonalRow.PrimerNombre)
        db.AddInParameter(dbCommand, "@p_SegundoNombre", DbType.String, oPersonalRow.SegundoNombre)
        db.AddInParameter(dbCommand, "@p_CodigoCargo", DbType.String, oPersonalRow.CodigoCargo)
        db.AddInParameter(dbCommand, "@p_FechaCese", DbType.Decimal, IIf(oPersonalRow.FechaCese = decimalVacio, DBNull.Value, oPersonalRow.FechaCese))
        db.AddInParameter(dbCommand, "@p_FechaIngreso", DbType.String, oPersonalRow.FechaIngreso)
        db.AddInParameter(dbCommand, "@p_LibretaElectoral", DbType.String, oPersonalRow.LibretaElectoral)
        db.AddInParameter(dbCommand, "@p_email_trabajo", DbType.String, oPersonalRow.email_trabajo)
        db.AddInParameter(dbCommand, "@p_email_personal", DbType.String, oPersonalRow.email_personal)
        db.AddInParameter(dbCommand, "@p_matricula", DbType.String, oPersonalRow.matricula)
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function Modificar(ByVal ob As PersonalBE, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Personal_Modificar")
        oPersonalRow = CType(ob.Personal.Rows(0), PersonalBE.PersonalRow)
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, oPersonalRow.CodigoInterno)
        db.AddInParameter(dbCommand, "@p_CodigoUsuario", DbType.String, oPersonalRow.CodigoUsuario)
        db.AddInParameter(dbCommand, "@p_CodigoCentroCosto", DbType.String, oPersonalRow.CodigoCentroCosto)
        db.AddInParameter(dbCommand, "@p_ApellidoPaterno", DbType.String, oPersonalRow.ApellidoPaterno)
        db.AddInParameter(dbCommand, "@p_ApellidoMaterno", DbType.String, oPersonalRow.ApellidoMaterno)
        db.AddInParameter(dbCommand, "@p_PrimerNombre", DbType.String, oPersonalRow.PrimerNombre)
        db.AddInParameter(dbCommand, "@p_SegundoNombre", DbType.String, oPersonalRow.SegundoNombre)
        db.AddInParameter(dbCommand, "@p_CodigoCargo", DbType.String, oPersonalRow.CodigoCargo)
        db.AddInParameter(dbCommand, "@p_FechaCese", DbType.Decimal, IIf(oPersonalRow.FechaCese = decimalVacio, DBNull.Value, oPersonalRow.FechaCese))
        db.AddInParameter(dbCommand, "@p_FechaIngreso", DbType.String, oPersonalRow.FechaIngreso)
        db.AddInParameter(dbCommand, "@p_LibretaElectoral", DbType.Decimal, oPersonalRow.LibretaElectoral)
        db.AddInParameter(dbCommand, "@p_email_trabajo", DbType.String, oPersonalRow.email_trabajo)
        db.AddInParameter(dbCommand, "@p_email_personal", DbType.String, oPersonalRow.email_personal)
        db.AddInParameter(dbCommand, "@p_matricula", DbType.String, oPersonalRow.matricula)
        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function
    Public Function Eliminar(ByVal codigoPersonal As String, ByVal dataRequest As DataSet) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Personal_Eliminar")
        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoPersonal)
        db.ExecuteNonQuery(dbCommand)
       Return True
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoInterno As String, ByVal nombreCompleto As String) As PersonalBE

        Dim oPersonalBE As New PersonalBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Personal_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
        db.AddInParameter(dbCommand, "@p_NombreCompleto", DbType.String, nombreCompleto)

        db.LoadDataSet(dbCommand, oPersonalBE, "Personal")

        Return oPersonalBE

    End Function

    Public Function SeleccionarPorCodigoInterno(ByVal codigoInterno As String) As String
        Dim nombre As String

        Dim db As Database = DatabaseFactory.CreateDatabase()

        Dim dbCommand As DbCommand = db.GetStoredProcCommand("Personal_SeleccionarPorCodigoInterno")

        db.AddInParameter(dbCommand, "@p_CodigoInterno", DbType.String, codigoInterno)
        db.AddOutParameter(dbCommand, "@p_Nombre", DbType.String, 200)

        db.ExecuteNonQuery(dbCommand)

        nombre = CType(db.GetParameterValue(dbCommand, "@p_Nombre"), String)

        Return nombre
    End Function

    'RGF 20110505 OT 63063 REQ 01
    Public Function VerificaPermisoNegociacion(ByVal LoginUsuario As String) As Integer
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_VerificaPermisoNegociacion")

        db.AddInParameter(dbCommand, "@p_LoginUsuario", DbType.String, LoginUsuario)

        Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)

        Return dt.Rows(0)("Permitido")
    End Function

    'CMB OT 65473 20120625
    Public Function SeleccionarCodigoInterno(ByVal codigoUsuario As String) As String
        Dim codigoInterno As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Personal_SeleccionarCodigoInterno")
        db.AddInParameter(dbCommand, "@p_codigoUsuario", DbType.String, codigoUsuario)

        Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)
        For Each dr As DataRow In dt.Rows
            codigoInterno = CType(dr("CodigoInterno"), String)
        Next
        Return codigoInterno
    End Function

    ''' <summary>
    ''' Busca por usuario el mail asignado
    ''' </summary>
    ''' <param name="codigoUsuario">Código del usuario</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SeleccionarMail(ByVal codigoUsuario As String) As String
        Dim s_Mail As String = ""
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As DbCommand = db.GetStoredProcCommand("pr_SIT_gl_Personal_Selecionar_Mail")
        db.AddInParameter(dbCommand, "@CodigoUsuario", DbType.String, codigoUsuario)

        Dim dt As DataTable = db.ExecuteDataSet(dbCommand).Tables(0)
        For Each dr As DataRow In dt.Rows
            s_Mail = Convert.ToString(dr("email_trabajo"))
            Exit For
        Next
        Return s_Mail
    End Function

End Class
