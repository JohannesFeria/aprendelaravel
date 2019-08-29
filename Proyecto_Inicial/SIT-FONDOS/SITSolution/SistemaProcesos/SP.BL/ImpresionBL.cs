/*
 * Fecha de Creación    : 30/05/2016
 * Modificado por           : Irene Reyes
 * Numero de OT             : 8829
 * Descripción del cambio   : Creación
 *                            
 * */
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Procesos.DA;

namespace Procesos.BL
{
       
    public class ImpresionBL
    {

		private decimal idFondo;
		private string fondo;
		private string moneda;
		private DateTime fechaInicio;
		private DateTime fechaFin;
		private Decimal valorCuotaInicial;
		private Decimal valorCuotaFinal;
		private Decimal idCliente;

		public decimal IdFondo
		{
			set
			{
				idFondo = value;
			}
			get
			{
				return idFondo;
			}
		}

		public decimal IdCliente
		{
			set
			{
				idCliente = value;
			}
			get
			{
				return idCliente;
			}
		}

		public string Fondo
		{
			set 
			{
				fondo = value;
			}
		}
		public string Moneda
		{
			set
			{
				moneda = value;
			}
		}
		public DateTime FechaInicio 
		{
			set 
			{
				fechaInicio = value;
			}
		}

		public DateTime FechaFin
		{
			set 
			{
				fechaFin = value;
			}
		}
		
		public Decimal ValorCuotaInicial
		{
			set
			{
				valorCuotaInicial = value;
			}
		}

		public Decimal ValorCuotaFinal
		{
			set
			{
				valorCuotaFinal = value;
			}
		}

		public ImpresionBL() {}

		private void GoTo(int line, ref int lineIndex, ref int linesLeft, StreamWriter sw)
		{
			for (int i = lineIndex; i < line; i ++)
			{
				sw.WriteLine(@"\par");
				lineIndex += 1;
				linesLeft -= 1;
			}
		}

		private void Write(string line, int startPosition, ref int lineIndex, ref int linesLeft, StreamWriter sw)
		{
				sw.WriteLine("".PadLeft(startPosition, ' ') + line + @"\par");
				lineIndex += 1;
				linesLeft -= 1;
		}

		private void WriteRTFHeader(StreamWriter sw)
		{
			sw.WriteLine(@"{\rtf1\ansi\deff0{\fonttbl{\f0\fmodern\fprq1\fcharset0 Courier New;}}");
			sw.WriteLine(@"{\colortbl ;\red0\green0\blue0;}");
			sw.WriteLine(@"{\*\generator Msftedit 5.41.15.1503;}\viewkind4\uc1\pard\cf1\lang3082\f0\fs16\par");
		}

		private void WriteRTFFooter(StreamWriter sw)
		{
			sw.WriteLine("}");
		}

		private void CompletePage(int lineIndex, int linesLeft, StreamWriter sw)
		{
			while (linesLeft > 0)
			{
				sw.WriteLine(@"\par");
				lineIndex += 1;
				linesLeft -= 1;
			}
		}

		public void SaveToFile(string path)
		{
			StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
			try
			{
				WriteRTFHeader(sw);
				 
                EECCIndividualDA da = new EECCIndividualDA();

				DataTable dtParticipes = da.ObtenerParticipeIndividual(idFondo, fechaInicio, fechaFin,idCliente);

				StringBuilder sb = new StringBuilder();

				foreach(DataRow drParticipe in dtParticipes.Rows)
				{
					decimal totalCuentasInicial = 0;
					decimal totalFondoInicial = 0;
					decimal totalCuentasFinal = 0;
					decimal totalFondoFinal = 0;
					bool unicaCuenta = false;

					decimal idParticipe = Convert.ToDecimal(drParticipe["ID"]);

					//sw.WriteLine();
					int linesLeft = 92;
					int lineIndex = 1;


					#region Datos Generales
					GoTo(11, ref lineIndex, ref linesLeft, sw);
					Write(fondo, 16, ref lineIndex, ref linesLeft, sw);
                    Write(drParticipe["CODIGO"].ToString(), 16, ref lineIndex, ref linesLeft, sw);
					Write(drParticipe["NOMBRE"].ToString(), 16, ref lineIndex, ref linesLeft, sw);
					if (drParticipe["TIPO_PERSONA"].Equals("MAN"))
					{
						DataTable dtMancomunos = da.ObtenerMacomunos(Convert.ToInt32(drParticipe["CODIGO"]));
						foreach (DataRow drMancomuno in dtMancomunos.Rows)
							if (drMancomuno["ESTADO"].Equals("ACT"))
								Write(drMancomuno["PARTICIPE"].ToString(), 16, ref lineIndex, ref linesLeft, sw);
					}
					Write(drParticipe["DIRECCION"].ToString(), 16, ref lineIndex, ref linesLeft, sw);
					if (!drParticipe["TIPO_UBICACION"].ToString().Equals("-1"))
						Write(drParticipe["TIPO_UBICACION"].ToString() + " " + drParticipe["UBICACION"].ToString(), 16, ref lineIndex, ref linesLeft, sw);
					Write(string.Format("{0} {1} {2}", drParticipe["DISTRITO"].ToString(), drParticipe["CIUDAD"].ToString(), drParticipe["DEPARTAMENTO"].ToString()), 16, ref lineIndex, ref linesLeft, sw);
					Write(drParticipe["PAIS"].ToString(), 16, ref lineIndex, ref linesLeft, sw);
					#endregion

					#region Saldo Inicial
					GoTo(26, ref lineIndex, ref linesLeft, sw);
					decimal saldoInicial = da.ObtenerSaldoEnFondo(idFondo, idParticipe, fechaInicio);

					sb.Append(@"\b ");
					sb.Append(fechaInicio.ToString("dd/MM/yyyy").PadRight(16, ' '));
					sb.Append("SALDO INICIAL".PadRight(18, ' '));
                    sb.Append(saldoInicial.ToString(Constants.ConstantesING.FormatoCuotas).PadLeft(18, ' '));
                    sb.Append(valorCuotaInicial.ToString(Constants.ConstantesING.FormatoValorCuota).PadLeft(19, ' '));
					sb.Append(string.Format("{1} {0}", (saldoInicial * valorCuotaInicial).ToString("N"), moneda).PadLeft(18, ' '));
					sb.Append(@" \b0");
					Write(sb.ToString(), 18, ref lineIndex, ref linesLeft, sw);					
					sb.Remove(0, sb.Length);

					totalFondoInicial = saldoInicial;
					#endregion 

					#region Movimientos
					GoTo(34, ref lineIndex, ref linesLeft, sw);
					DataTable dtCuentas = da.ObtenerCuentas(idParticipe);
				
					// Una sola cuenta
					if (dtCuentas.Rows.Count == 1)
					{
						decimal idCuentaParticipacion = Convert.ToDecimal(dtCuentas.Rows[0]["ID"]);
						DataTable dtMovimientos = da.ObtenerMovimientos(idFondo, idCuentaParticipacion, fechaInicio, fechaFin);
						foreach (DataRow drMovimiento in dtMovimientos.Rows)
						{
							decimal numeroCuotas = Convert.ToDecimal(drMovimiento["NUMERO_CUOTAS"]);
							decimal valorCuota = (drMovimiento["VALOR_CUOTA"] == DBNull.Value) ? 0 : Convert.ToDecimal(drMovimiento["VALOR_CUOTA"]);
							sb.Append(Convert.ToDateTime(drMovimiento["FECHA_PROCESO"]).ToString("dd/MM/yyyy").PadRight(16, ' '));
							sb.Append(drMovimiento["MOVIMIENTO"].ToString().PadRight(18, ' '));
                            sb.Append(numeroCuotas.ToString(Constants.ConstantesING.FormatoCuotas).PadLeft(18, ' '));
							if (valorCuota == 0)
							{
								sb.Append("".PadLeft(19, ' '));
								sb.Append("".PadLeft(18, ' '));
							}
							else
							{
                                sb.Append(valorCuota.ToString(Constants.ConstantesING.FormatoValorCuota).PadLeft(19, ' '));
								sb.Append(string.Format("{1} {0}", (numeroCuotas * valorCuota).ToString("N"), moneda).PadLeft(18, ' '));
							}
							Write(sb.ToString(), 18, ref lineIndex, ref linesLeft, sw);
							sb.Remove(0, sb.Length);
						}

						unicaCuenta = true;
					}
					// Varias cuentas
					else
					{
						foreach (DataRow drCuenta in dtCuentas.Rows)
						{
							decimal idCuentaParticipacion = Convert.ToDecimal(drCuenta["ID"]);
							bool cuentaEliminada = drCuenta["FLAG_ELIMINADO"].ToString().Equals("S");
							decimal saldoInicialCuenta = da.ObtenerSaldoEnCuenta(idFondo, idCuentaParticipacion, fechaInicio);

							if (!cuentaEliminada || saldoInicialCuenta > 0)
							{
								//Cabecera
								Write(string.Format(@"\b {0} {1} \b0", drCuenta["CODIGO"], drCuenta["ETIQUETA"]), 18, ref lineIndex, ref linesLeft, sw);

								//Saldo inicial cuenta
								sb.Append(fechaInicio.ToString("dd/MM/yyyy").PadRight(16, ' '));
								sb.Append("SALDO INICIAL".PadRight(18, ' '));
                                sb.Append(saldoInicialCuenta.ToString(Constants.ConstantesING.FormatoCuotas).PadLeft(18, ' '));
                                sb.Append(valorCuotaInicial.ToString(Constants.ConstantesING.FormatoValorCuota).PadLeft(19, ' '));
								sb.Append(string.Format("{1} {0}", (saldoInicialCuenta * valorCuotaInicial).ToString("N"), moneda).PadLeft(18, ' '));
								Write(sb.ToString(), 18, ref lineIndex, ref linesLeft, sw);					
								sb.Remove(0, sb.Length);

								totalCuentasInicial += saldoInicialCuenta;

								//Movimientos
								DataTable dtMovimientos = da.ObtenerMovimientos(idFondo, idCuentaParticipacion, fechaInicio, fechaFin);
								foreach (DataRow drMovimiento in dtMovimientos.Rows)
								{
									decimal numeroCuotas = Convert.ToDecimal(drMovimiento["NUMERO_CUOTAS"]);
									decimal valorCuota = (drMovimiento["VALOR_CUOTA"] == DBNull.Value) ? 0 : Convert.ToDecimal(drMovimiento["VALOR_CUOTA"]);
									sb.Append(Convert.ToDateTime(drMovimiento["FECHA_PROCESO"]).ToString("dd/MM/yyyy").PadRight(16, ' '));
									sb.Append(drMovimiento["MOVIMIENTO"].ToString().PadRight(18, ' '));
                                    sb.Append(numeroCuotas.ToString(Constants.ConstantesING.FormatoCuotas).PadLeft(18, ' '));
									if (valorCuota == 0)
									{
										sb.Append("".PadLeft(19, ' '));
										sb.Append("".PadLeft(18, ' '));
									}
									else
									{
                                        sb.Append(valorCuota.ToString(Constants.ConstantesING.FormatoValorCuota).PadLeft(19, ' '));
										sb.Append(string.Format("{1} {0}", (numeroCuotas * valorCuota).ToString("N"), moneda).PadLeft(18, ' '));
									}
									Write(sb.ToString(), 18, ref lineIndex, ref linesLeft, sw);
									sb.Remove(0, sb.Length);
								}

								//Saldo final cuenta
								decimal saldoFinalCuenta = da.ObtenerSaldoEnCuenta(idFondo, idCuentaParticipacion, fechaFin);
								sb.Append(fechaFin.ToString("dd/MM/yyyy").PadRight(16, ' '));
								sb.Append("SALDO FINAL".PadRight(18, ' '));
                                sb.Append(saldoFinalCuenta.ToString(Constants.ConstantesING.FormatoCuotas).PadLeft(18, ' '));
                                sb.Append(valorCuotaFinal.ToString(Constants.ConstantesING.FormatoValorCuota).PadLeft(19, ' '));
								sb.Append(string.Format("{1} {0}", (saldoFinalCuenta * valorCuotaFinal).ToString("N"), moneda).PadLeft(18, ' '));
								Write(sb.ToString(), 18, ref lineIndex, ref linesLeft, sw);					
								sb.Remove(0, sb.Length);

								totalCuentasFinal += saldoFinalCuenta;
								Write("", 18, ref lineIndex, ref linesLeft, sw);	
							}
						}
					}
					#endregion

					#region Saldo Final
					GoTo(69, ref lineIndex, ref linesLeft, sw);
					decimal saldoFinal = da.ObtenerSaldoEnFondo(idFondo, idParticipe, fechaFin);

					sb.Append(@"\b ");
					sb.Append(fechaFin.ToString("dd/MM/yyyy").PadRight(16, ' '));
					sb.Append("SALDO FINAL".PadRight(18, ' '));
                    sb.Append(saldoFinal.ToString(Constants.ConstantesING.FormatoCuotas).PadLeft(18, ' '));
                    sb.Append(valorCuotaFinal.ToString(Constants.ConstantesING.FormatoValorCuota).PadLeft(19, ' '));
					sb.Append(string.Format("{1} {0}", (saldoFinal * valorCuotaFinal).ToString("N"), moneda).PadLeft(18, ' '));
					sb.Append(@" \b0");
					Write(sb.ToString(), 18, ref lineIndex, ref linesLeft, sw);
					sb.Remove(0, sb.Length);

					totalFondoFinal = saldoFinal;
					#endregion 

					if ((!unicaCuenta) && (totalCuentasInicial != totalFondoInicial)) Write("@@INICIAL", 18, ref lineIndex, ref linesLeft, sw);
					if ((!unicaCuenta) && (totalCuentasFinal != totalFondoFinal)) Write("@@FINAL", 18, ref lineIndex, ref linesLeft, sw);

					CompletePage(lineIndex, linesLeft, sw);
				}
				WriteRTFFooter(sw);
			}
			finally
			{
				sw.Close();
			}
		}
	}
}
