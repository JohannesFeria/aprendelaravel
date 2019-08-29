Imports System
Imports System.Data
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql
Public Class VaxInterfazDAM

    Public Function SeleccionarVaxCustman(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxCustman(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxCustman_SeleccionarPorCartera")
        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, cartera)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, fechaProceso)

        Dim oVaxCustman As New DataSet
        db.LoadDataSet(dbCommand, oVaxCustman, "VaxRegaux")
        Return oVaxCustman
    End Function


    Public Sub llenarTabla_VaxCustman(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxCusman")
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    Public Function SeleccionarVaxDvdos(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxDvdos(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxDvdos_SeleccionarPorCartera")
        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, cartera)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, 0)

        Dim oVaxDvdos As New DataSet
        db.LoadDataSet(dbCommand, oVaxDvdos, "VaxRegaux")
        Return oVaxDvdos
    End Function

    Public Sub llenarTabla_VaxDvdos(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxDvdos")
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    Public Function SeleccionarVaxInfodia(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxInfodia(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxInfodia_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, cartera)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, 0)
        Dim oVaxInfodia As New DataSet
        db.LoadDataSet(dbCommand, oVaxInfodia, "VaxRegaux")
        Return oVaxInfodia
    End Function


    Public Sub llenarTabla_VaxInfodia(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxInfodia")
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    Public Function SeleccionarVaxTransac(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxTransac(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxTransac_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, cartera)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, 0)

        Dim oVaxTransac As New DataSet
        db.LoadDataSet(dbCommand, oVaxTransac, "VaxRegaux")
        Return oVaxTransac
    End Function


    Public Sub llenarTabla_VaxTransac(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxTransac")
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub



    Public Function SeleccionarVaxComision(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxComision(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxComision_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, cartera)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, 0)

        Dim oVaxComision As New DataSet
        db.LoadDataSet(dbCommand, oVaxComision, "VaxRegaux")
        Return oVaxComision
    End Function


    Public Sub llenarTabla_VaxComision(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxComision")
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    Public Function SeleccionarVaxAuxbcr(ByVal cartera As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        Me.llenarTabla_VaxAuxbcr(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("VaxAuxbcr_SeleccionarPorCartera")

        db.AddInParameter(dbCommand, "@p_Cartera", DbType.String, cartera)
        db.AddInParameter(dbCommand, "@p_FechaVax", DbType.Decimal, 0)
        Dim oVaxAuxbcr As New DataSet
        db.LoadDataSet(dbCommand, oVaxAuxbcr, "VaxRegaux")
        Return oVaxAuxbcr
    End Function


    Public Sub llenarTabla_VaxAuxbcr(ByVal portafolio As String, ByVal fecha As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("llenarTabla_VaxAuxbcr")
        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fecha)
        db.ExecuteNonQuery(dbCommand)
    End Sub


    '-------------------------------  Interfase LBTR -------------------------------------


    Public Function Seleccionar_LBTR(ByVal portafolio As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        'Me.llenarTabla_LBTR(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Interfaz_LBTR")

        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        Dim oVaxAuxbcr As New DataSet
        db.LoadDataSet(dbCommand, oVaxAuxbcr, "VaxRegaux")
        Return oVaxAuxbcr
    End Function


    '------------------------------ Interfase de cuentas personales ---------------------------------

    Public Function Seleccionar_CuentasPersonales(ByVal portafolio As String, ByVal fechaProceso As Decimal, ByVal dataRequest As DataSet) As DataSet

        'Me.llenarTabla_LBTR(cartera, fechaProceso)

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Gestion_Interfaz_CuentasPersonales")

        db.AddInParameter(dbCommand, "@p_Portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_Secuencial", DbType.String, "01") 'este secuencial obtenerlo de parametros generales
        Dim oVaxAuxbcr As New DataSet
        db.LoadDataSet(dbCommand, oVaxAuxbcr, "VaxRegaux")
        Return oVaxAuxbcr
    End Function

End Class
