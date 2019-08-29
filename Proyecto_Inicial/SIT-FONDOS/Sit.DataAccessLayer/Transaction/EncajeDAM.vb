Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Sit.BusinessEntities
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Data.Sql



''' <summary>
''' Clase para el acceso de los datos para Encaje tabla.
''' </summary>
Public Class EncajeDAM


    Private sqlCommand As String = ""

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Inserta un expediente en Encaje tabla.
    ''' <summary>
    ''' <param name="codigoEncaje"></param>
    ''' <param name="valorMantenido"></param>
    ''' <param name="valorRequerido"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="fechaProceso"></param>
    ''' <param name="codigoNegocio"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <param name="host"></param>
    ''' <returns></returns>
    Public Function Insertar(ByVal codigoEncaje As String, ByVal valorMantenido As Decimal, ByVal valorRequerido As Decimal, ByVal codigoPortafolio As String, ByVal fechaProceso As Decimal, ByVal codigoNegocio As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String) As String
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_Insertar")

        db.AddInParameter(dbCommand, "@p_CodigoEncaje", DbType.String, codigoEncaje)
        db.AddInParameter(dbCommand, "@p_ValorMantenido", DbType.Decimal, valorMantenido)
        db.AddInParameter(dbCommand, "@p_ValorRequerido", DbType.Decimal, valorRequerido)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.Decimal, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.Decimal, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)

        db.ExecuteNonQuery(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona un solo expediente de Encaje tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Seleccionar(ByVal codigoEncaje As String) As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_Seleccionar")

        db.AddInParameter(dbCommand, "@p_CodigoEncaje", DbType.String, codigoEncaje)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Encaje tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoPortafolio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoPortafolio(ByVal codigoPortafolio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_SeleccionarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Selecciona expedientes de Encaje tabla por una llave extranjera.
    ''' <summary>
    ''' <param name="codigoNegocio"></param>
    ''' <returns>DataSet</returns>
    Public Function SeleccionarPorCodigoNegocio(ByVal codigoNegocio As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_SeleccionarPorCodigoNegocio")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)

        Return db.ExecuteDataSet(dbCommand)
    End Function

    ''' <summary>
    ''' Lista todos los expedientes de Encaje tabla.
    ''' <summary>
    ''' <returns>DataSet</returns>
    Public Function Listar() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_Listar")

        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarPorFiltro(ByVal codigoPortafolio As String, ByVal fecha As Date, ByVal dataRequest As DataSet) As EncajeBE
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_SeleccionarPorFiltro")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolioSBS", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaEncaje", DbType.Decimal, DataUtility.ObtenerFecha(fecha))

        Dim objeto As New EncajeBE
        db.LoadDataSet(dbCommand, objeto, "Encaje")
        Return objeto
    End Function
    ''' <summary>
    ''' Midifica un expediente en Encaje tabla.
    ''' <summary>
    ''' <param name="codigoEncaje"></param>
    ''' <param name="valorMantenido"></param>
    ''' <param name="valorRequerido"></param>
    ''' <param name="codigoPortafolio"></param>
    ''' <param name="fechaProceso"></param>
    ''' <param name="codigoNegocio"></param>
    ''' <param name="usuarioCreacion"></param>
    ''' <param name="fechaCreacion"></param>
    ''' <param name="horaCreacion"></param>
    ''' <param name="usuarioModificacion"></param>
    ''' <param name="fechaModificacion"></param>
    ''' <param name="horaModificacion"></param>
    ''' <param name="usuarioEliminacion"></param>
    ''' <param name="fechaEliminacion"></param>
    ''' <param name="horaEliminacion"></param>
    ''' <param name="host"></param>
    Public Function Modificar(ByVal codigoEncaje As String, ByVal valorMantenido As Decimal, ByVal valorRequerido As Decimal, ByVal codigoPortafolio As String, ByVal fechaProceso As Decimal, ByVal codigoNegocio As String, ByVal usuarioCreacion As String, ByVal fechaCreacion As Decimal, ByVal horaCreacion As Decimal, ByVal usuarioModificacion As String, ByVal fechaModificacion As Decimal, ByVal horaModificacion As Decimal, ByVal usuarioEliminacion As String, ByVal fechaEliminacion As Decimal, ByVal horaEliminacion As String, ByVal host As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_Modificar")

        db.AddInParameter(dbCommand, "@p_CodigoEncaje", DbType.String, codigoEncaje)
        db.AddInParameter(dbCommand, "@p_ValorMantenido", DbType.Decimal, valorMantenido)
        db.AddInParameter(dbCommand, "@p_ValorRequerido", DbType.Decimal, valorRequerido)
        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)
        db.AddInParameter(dbCommand, "@p_FechaProceso", DbType.Decimal, fechaProceso)
        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)
        db.AddInParameter(dbCommand, "@p_UsuarioCreacion", DbType.String, usuarioCreacion)
        db.AddInParameter(dbCommand, "@p_FechaCreacion", DbType.Decimal, fechaCreacion)
        db.AddInParameter(dbCommand, "@p_HoraCreacion", DbType.Decimal, horaCreacion)
        db.AddInParameter(dbCommand, "@p_UsuarioModificacion", DbType.String, usuarioModificacion)
        db.AddInParameter(dbCommand, "@p_FechaModificacion", DbType.Decimal, fechaModificacion)
        db.AddInParameter(dbCommand, "@p_HoraModificacion", DbType.Decimal, horaModificacion)
        db.AddInParameter(dbCommand, "@p_UsuarioEliminacion", DbType.String, usuarioEliminacion)
        db.AddInParameter(dbCommand, "@p_FechaEliminacion", DbType.Decimal, fechaEliminacion)
        db.AddInParameter(dbCommand, "@p_HoraEliminacion", DbType.String, horaEliminacion)
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, host)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Encaje table por una llave primaria compuesta.
    ''' <summary>
    Public Function Eliminar(ByVal codigoEncaje As String) As Boolean
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_Eliminar")

        db.AddInParameter(dbCommand, "@p_CodigoEncaje", DbType.String, codigoEncaje)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Encaje table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoPortafolio(ByVal codigoPortafolio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_EliminarPorCodigoPortafolio")

        db.AddInParameter(dbCommand, "@p_CodigoPortafolio", DbType.String, codigoPortafolio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    ''' <summary>
    ''' Elimina un expediente de Encaje table por una llave extranjera.
    ''' <summary>
    Public Function EliminarPorCodigoNegocio(ByVal codigoNegocio As String) As Boolean

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_EliminarPorCodigoNegocio")

        db.AddInParameter(dbCommand, "@p_CodigoNegocio", DbType.String, codigoNegocio)

        db.ExecuteNonQuery(dbCommand)
        Return True
    End Function

    Public Function ListarParametros() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ListarParametros")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function SeleccionarParametro(ByVal secuencial As String) As DataSet

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_SeleccionarParametro")
        db.AddInParameter(dbCommand, "@p_secuencia", DbType.String, secuencial)
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ModificarParametro(ByVal Secuencia As String, ByVal nombre As String, ByVal valor As String) As String

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ModificarParametro")
        db.AddInParameter(dbCommand, "@p_secuencia", DbType.String, Secuencia)
        db.AddInParameter(dbCommand, "@p_nombre", DbType.String, nombre)
        db.AddInParameter(dbCommand, "@p_valor", DbType.String, valor)


        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function CalculoencajePeru(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal gobiernocentral As String, ByVal bancocentral As String, ByVal diaspromediar As Decimal, ByVal porcentajelimite As Decimal, ByVal diasvencimiento As Decimal, ByVal indicadorvalorfondo As String, ByVal valorindicadorfondo As Decimal, ByVal indicadorencajemantenido As String, ByVal valorindicadorencajemantenido As Decimal, ByVal indicadortotalactivos As String, ByVal valorindicadortotalactivos As Decimal, ByVal datarequest As DataSet) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_CalculoEncajePeru")

        db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.Decimal, fechaproceso)
        db.AddInParameter(dbCommand, "@p_gobiernoCentral", DbType.String, gobiernocentral)
        db.AddInParameter(dbCommand, "@p_BancoCentral", DbType.String, bancocentral)
        db.AddInParameter(dbCommand, "@p_diaspromediar", DbType.Decimal, diaspromediar)
        db.AddInParameter(dbCommand, "@p_porcentajelimite", DbType.Decimal, porcentajelimite)
        db.AddInParameter(dbCommand, "@p_diasvencimiento", DbType.Decimal, diasvencimiento)
        db.AddInParameter(dbCommand, "@p_indicadorvalorfondo", DbType.String, indicadorvalorfondo)
        db.AddInParameter(dbCommand, "@p_Valorindicadorfondo", DbType.Decimal, valorindicadorfondo)
        db.AddInParameter(dbCommand, "@p_IndicadorencajeMantenido", DbType.String, indicadorencajemantenido)
        db.AddInParameter(dbCommand, "@p_ValorindicadorencajeMantenido", DbType.Decimal, valorindicadorencajemantenido)
        db.AddInParameter(dbCommand, "@p_IndicadorTotalActivos", DbType.String, indicadortotalactivos)
        db.AddInParameter(dbCommand, "@p_ValorindicadortotalActivos", DbType.Decimal, valorindicadortotalactivos)
        db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
        db.ExecuteNonQuery(dbCommand)
    End Function
    Public Function CalculoencajePeru2(ByVal portafolio As String, ByVal fechaproceso As Decimal, ByVal gobiernocentral As String, ByVal bancocentral As String, ByVal diaspromediar As Decimal, ByVal porcentajelimite As Decimal, ByVal diasvencimiento As Decimal, ByVal indicadorvalorfondo As String, ByVal valorindicadorfondo As Decimal, ByVal indicadorencajemantenido As String, ByVal valorindicadorencajemantenido As Decimal, ByVal indicadortotalactivos As String, ByVal valorindicadortotalactivos As Decimal, ByVal datarequest As DataSet) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_CalculoEncajePeru2")
        dbCommand.CommandTimeout = 1020 'RGF 20081223 salia error de timeout HDG 20111117
        db.AddInParameter(dbCommand, "@p_codigoPortafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.Decimal, fechaproceso)
        db.AddInParameter(dbCommand, "@p_gobiernoCentral", DbType.String, gobiernocentral)
        db.AddInParameter(dbCommand, "@p_BancoCentral", DbType.String, bancocentral)
        db.AddInParameter(dbCommand, "@p_diaspromediar", DbType.Decimal, diaspromediar)
        db.AddInParameter(dbCommand, "@p_porcentajelimite", DbType.Decimal, porcentajelimite)
        db.AddInParameter(dbCommand, "@p_diasvencimiento", DbType.Decimal, diasvencimiento)
        db.AddInParameter(dbCommand, "@p_indicadorvalorfondo", DbType.String, indicadorvalorfondo)
        db.AddInParameter(dbCommand, "@p_Valorindicadorfondo", DbType.Decimal, valorindicadorfondo)
        db.AddInParameter(dbCommand, "@p_IndicadorencajeMantenido", DbType.String, indicadorencajemantenido)
        db.AddInParameter(dbCommand, "@p_ValorindicadorencajeMantenido", DbType.Decimal, valorindicadorencajemantenido)
        db.AddInParameter(dbCommand, "@p_IndicadorTotalActivos", DbType.String, indicadortotalactivos)
        db.AddInParameter(dbCommand, "@p_ValorindicadortotalActivos", DbType.Decimal, valorindicadortotalactivos)
        db.AddInParameter(dbCommand, "@p_usuario", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Usuario"))
        db.AddInParameter(dbCommand, "@p_fecha", DbType.Decimal, DataUtility.ObtenerFecha(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Hora", DbType.String, DataUtility.ObtenerHora(DataUtility.ObtenerValorRequest(datarequest, "Fecha")))
        db.AddInParameter(dbCommand, "@p_Host", DbType.String, DataUtility.ObtenerValorRequest(datarequest, "Host"))
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ObtenerNemonicosError() As DataSet
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_SeleccionarNemonicoError")
        Return db.ExecuteDataSet(dbCommand)
    End Function
    Public Function ExtornoEncajePeru(ByVal portafolio As String, ByVal fechaproceso As Decimal) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ExtornoEncaje")

        db.AddInParameter(dbCommand, "@p_codigoPortafoliosbs", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.Decimal, fechaproceso)
        db.ExecuteNonQuery(dbCommand)
    End Function

    Public Function ExisteEncajePeru(ByVal portafolio As String, ByVal fechaproceso As Decimal) As Integer

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ExisteEncaje")
        dbCommand.CommandTimeout = 1020 'HDG 20111117

        db.AddInParameter(dbCommand, "@p_codigoPortafoliosbs", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.Decimal, fechaproceso)
        Return Convert.ToInt32(db.ExecuteScalar(dbCommand))
    End Function

    Public Function ObtenerFechaUltimoEncaje(ByVal portafolio As String, ByVal fechaproceso As Decimal) As Decimal

        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ObtenerFechaUltimoEncaje")

        db.AddInParameter(dbCommand, "@p_codigoPortafoliosbs", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.Decimal, fechaproceso)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Function ObtenerFechaT1Indicadores(ByVal codigoindicador As String, ByVal portafolio As String, ByVal fechaproceso As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ObtenerFechaIndicadoresT1")
        db.AddInParameter(dbCommand, "@p_codigoindicador", DbType.String, codigoindicador)
        db.AddInParameter(dbCommand, "@p_codigoportafolioSBS", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@p_fechaproceso", DbType.Decimal, fechaproceso)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
    Public Sub ActualizarRentabilidad(ByVal portafolio As String, ByVal fecha As Decimal, ByVal numerocuotast As Decimal, ByVal valorcuotat As Decimal, ByVal numerocuotast1 As Decimal, ByVal valorcuotat1 As Decimal, ByVal compras As Decimal, ByVal ventas As Decimal)
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_ActualizarRentabilidad")
        dbCommand.CommandTimeout = 1020 'HDG 20111117
        db.AddInParameter(dbCommand, "@portafolio", DbType.String, portafolio)
        db.AddInParameter(dbCommand, "@fechaencaje", DbType.Decimal, fecha)
        db.AddInParameter(dbCommand, "@NumerocuotasT", DbType.Decimal, numerocuotast)
        db.AddInParameter(dbCommand, "@ValorCuotaT", DbType.Decimal, valorcuotat)
        db.AddInParameter(dbCommand, "@NumerocuotasT1", DbType.Decimal, numerocuotast1)
        db.AddInParameter(dbCommand, "@ValorCuotaT1", DbType.Decimal, valorcuotat1)
        db.AddInParameter(dbCommand, "@Compras", DbType.Decimal, compras)
        db.AddInParameter(dbCommand, "@Ventas", DbType.Decimal, ventas)
        db.ExecuteNonQuery(dbCommand)
    End Sub
    Public Function ProximaFechaEncaje(ByVal FechaProceso As Decimal) As Decimal
        Dim db As Database = DatabaseFactory.CreateDatabase()
        Dim dbCommand As dbCommand = db.GetStoredProcCommand("Encaje_SeleccionarProximaFecha")
        db.AddInParameter(dbCommand, "@FechaProceso", DbType.String, FechaProceso)
        Return Convert.ToDecimal(db.ExecuteScalar(dbCommand))
    End Function
End Class

