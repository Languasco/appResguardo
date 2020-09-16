using Entidades.Reportes;
using Negocio.Conexion;
using Negocio.Resultados;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
 
using System.Threading;
using Excel = OfficeOpenXml;
using Style = OfficeOpenXml.Style;


namespace Negocio.Reportes
{
    public class Reportes_BL
    {
        public DataTable get_ubicacionesPorPersonal( int idServicio, string fechaGps, int idTipoOT, int  idProveedor, int idUsuario)
        {
            DataTable dt_detalle = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_Ubicacion_Personal", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fechaGps;
                        cmd.Parameters.Add("@Servicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@TipoOrden", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@Proveedor", SqlDbType.Int).Value = idProveedor;
                        cmd.Parameters.Add("@TipoRepor", SqlDbType.VarChar).Value = "W";

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt_detalle;
        }

        public DataTable get_eventosCelular(int idServicio, string fechaGps, int idTipoOT, int idProveedor, int idUsuario)
        {
            DataTable dt_detalle = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_Eventos_Personal", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fechaGps;
                        cmd.Parameters.Add("@Servicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@TipoOrden", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@Proveedor", SqlDbType.Int).Value = idProveedor; 

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt_detalle;
        }

        //------
        // REPORTE DETALLE OT
        ///

        //public DataTable get_detalleOt(int idServicio, int idTipoOT, int idProveedor, string fechaIni, string fechaFin, int  idEstado, int idUsuario)
        //{
        //    //DataTable dt_detalle = new DataTable();
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
        //        {
        //            cn.Open();
        //            using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_Detallado", cn))
        //            {
        //                cmd.CommandTimeout = 0;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
        //                cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = idTipoOT;
        //                cmd.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
        //                cmd.Parameters.Add("@FecIni", SqlDbType.VarChar).Value = fechaIni;
        //                cmd.Parameters.Add("@FecFin", SqlDbType.VarChar).Value = fechaFin;

        //                cmd.Parameters.Add("@idEstado", SqlDbType.Int).Value = idEstado;
        //                cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

        //                //using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //                //{
        //                //    da.Fill(dt_detalle);
        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return dt_detalle;
        //}


        public object get_detalleOt(int idServicio, int idTipoOT, int idProveedor, string fechaIni, string fechaFin, int idEstado, int idUsuario)
        {
            Resultado res = new Resultado();
            List<detalleOT_E> obj_List = new List<detalleOT_E>();
            //DataTable dt_detalle = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_Detallado", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
                        cmd.Parameters.Add("@FecIni", SqlDbType.VarChar).Value = fechaIni;
                        cmd.Parameters.Add("@FecFin", SqlDbType.VarChar).Value = fechaFin;
                        cmd.Parameters.Add("@idEstado", SqlDbType.Int).Value = idEstado;
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                detalleOT_E Entidad = new detalleOT_E();

                                Entidad.checkeado = false;

                                Entidad.id_OT = Convert.ToInt32(dr["id_OT"]);
                                Entidad.descripcionEstado = dr["descripcionEstado"].ToString();
                                Entidad.tipoOT = dr["tipoOT"].ToString();
                                Entidad.nroObra = dr["nroObra"].ToString();
                                Entidad.direccion = dr["direccion"].ToString();
                                Entidad.distrito = dr["distrito"].ToString();

                                Entidad.FechaAsignacion = dr["FechaAsignacion"].ToString();
                                Entidad.FechaMovil = dr["FechaMovil"].ToString();


                                Entidad.latitud = dr["latitud"].ToString();
                                Entidad.longitud = dr["longitud"].ToString();

                                Entidad.empresaContratista = dr["empresaContratista"].ToString();
                                Entidad.jefeCuadrilla = dr["jefeCuadrilla"].ToString();

                                Entidad.fechaRegistro = dr["fechaRegistro"].ToString();

                                Entidad.generaVolumen = dr["generaVolumen"].ToString();
                                Entidad.volumenDesmonte = dr["volumenDesmonte"].ToString();
                                Entidad.volumenDesmonteRecoger = dr["volumenDesmonteRecoger"].ToString();

                                Entidad.fechaAprobacion = dr["fechaAprobacion"].ToString();
                                Entidad.fechaCancelacion = dr["fechaCancelacion"].ToString();

                                Entidad.totalVolumen = dr["totalVolumen"].ToString();
                                Entidad.totalDesmonte = dr["totalDesmonte"].ToString();
                                Entidad.totalDesmonteRecoger = dr["totalDesmonteRecoger"].ToString();
                                Entidad.diasVencimiento = dr["diasVencimiento"].ToString();


                                Entidad.id_tipoTrabajo = Convert.ToInt32(dr["id_tipoTrabajo"]);
                                Entidad.id_Distrito = dr["id_Distrito"].ToString();
                                Entidad.referencia = dr["referencia"].ToString();
                                Entidad.descripcion_OT = dr["descripcion_OT"].ToString();
                                Entidad.id_estado = Convert.ToInt32(dr["id_estado"]);
                                Entidad.descripcionServicio = dr["descripcionServicio"].ToString();
                                Entidad.color = dr["color"].ToString();

                                obj_List.Add(Entidad);
                            }

                            res.ok = true;
                            res.data = obj_List;
                            res.totalpage = 0;
                        }


                        //using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        //{
                        //    da.Fill(dt_detalle);

                        //    res.ok = true;
                        //    res.data = dt_detalle;
                        //    res.totalpage = 0;
                        //}


                    }
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }
               
        public object get_descargarDetalleOT(int idServicio, int idTipoOT, int idProveedor,string fechaIni, string fechaFin, int idEstado, int idUsuario)
        {
            Resultado res = new Resultado();
            List<detalleOT_E> obj_List = new List<detalleOT_E>();
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_Detallado", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
                        cmd.Parameters.Add("@FecIni", SqlDbType.VarChar).Value = fechaIni;
                        cmd.Parameters.Add("@FecFin", SqlDbType.VarChar).Value = fechaFin;

                        cmd.Parameters.Add("@idEstado", SqlDbType.Int).Value = idEstado;
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                detalleOT_E Entidad = new detalleOT_E();
                                                               
                                Entidad.id_OT = Convert.ToInt32(dr["id_OT"]);
                                Entidad.descripcionEstado = dr["descripcionEstado"].ToString();
                                Entidad.tipoOT = dr["tipoOT"].ToString();
                                Entidad.nroObra = dr["nroObra"].ToString();
                                Entidad.direccion = dr["direccion"].ToString();
                                Entidad.distrito = dr["distrito"].ToString();

                                Entidad.FechaAsignacion = dr["FechaAsignacion"].ToString();
                                Entidad.FechaMovil = dr["FechaMovil"].ToString();

                                Entidad.empresaContratista = dr["empresaContratista"].ToString();
                                Entidad.jefeCuadrilla = dr["jefeCuadrilla"].ToString();

                                Entidad.fechaRegistro = dr["fechaRegistro"].ToString();                                

                                Entidad.generaVolumen = dr["generaVolumen"].ToString();
                                Entidad.volumenDesmonte = dr["volumenDesmonte"].ToString();
                                Entidad.volumenDesmonteRecoger = dr["volumenDesmonteRecoger"].ToString();

                                Entidad.fechaAprobacion = dr["fechaAprobacion"].ToString();
                                Entidad.fechaCancelacion = dr["fechaCancelacion"].ToString();
 
                                Entidad.totalVolumen = dr["totalVolumen"].ToString();
                                Entidad.totalDesmonte = dr["totalDesmonte"].ToString();
                                Entidad.totalDesmonteRecoger = dr["totalDesmonteRecoger"].ToString(); 
                                Entidad.diasVencimiento = dr["diasVencimiento"].ToString(); 

                                obj_List.Add(Entidad);
                            }

                            if (obj_List.Count == 0)
                            {
                                res.ok = false;
                                res.data = "No hay informacion disponible";
                                res.totalpage = 0;
                            }
                            else
                            {
                                res.ok = true;
                                 res.data = GenerarArchivoExcel_detalle(obj_List, idUsuario);
                                res.totalpage = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }

        public string GenerarArchivoExcel_detalle(List<detalleOT_E> listDetalle, int id_usuario)
        {
            string Res = "";
            int _fila = 4;
            string FileRuta = "";
            string FileExcel = "";

            try
            {

                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Archivos/Excel/" + id_usuario + "_detalleOT.xlsx");
                string rutaServer = ConfigurationManager.AppSettings["Archivos"];

                FileExcel = rutaServer + "Excel/" + id_usuario + "_detalleOT.xlsx";

                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }

                Thread.Sleep(1);

                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("DetalleOT");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 8));


                    oWs.Cells[1, 1].Style.Font.Size = 24; //letra tamaño  2
                    oWs.Cells[1, 1].Value = "DETALLE DE ORDENES TRABAJO";
                    oWs.Cells[1, 1, 1, 19].Merge = true;  // combinar celdaS


                    oWs.Cells[3, 1].Value = "ESTADO";
                    oWs.Cells[3, 2].Value = "TIPO DE ORDEN";
                    oWs.Cells[3, 3].Value = "NRO OBRA/ TD";
                    oWs.Cells[3, 4].Value = "DIRECCION";
                    oWs.Cells[3, 5].Value = "DISTRITO";

                    oWs.Cells[3, 6].Value = "FECHA ASIGNACION ";
                    oWs.Cells[3, 7].Value = "FECHA ENVIO MOVIL";

                    oWs.Cells[3, 8].Value = "EMPRESA CONTRATISTA";
                    oWs.Cells[3, 9].Value = "JEFE CUADRILLA";

                    oWs.Cells[3, 10].Value = "FECHA DE REGISTRO";

                    oWs.Cells[3, 11].Value = "GENERO UN VOLUMEN";
                    oWs.Cells[3, 12].Value = "VOLUMEN DE DESMONTE";
                    oWs.Cells[3, 13].Value = "VOLUMEN DE DESMONTE POR RECOGER";

                    oWs.Cells[3, 14].Value = "FECHA APROBACION";
                    oWs.Cells[3, 15].Value = "FECHA CANCELACION";

                    oWs.Cells[3, 16].Value = "TOTAL EN VOLUMEN ";
                    oWs.Cells[3, 17].Value = "TOTAL EN DESMONTE ";
                    oWs.Cells[3, 18].Value = "TOTAL EN DESMONTE POR RECOGER";
 
                    oWs.Cells[3, 19].Value = "DIAS DE VENCIMIENTO";


                    foreach (var item in listDetalle)
                    {
                        oWs.Cells[_fila, 1].Value = item.descripcionEstado.ToString();
                        oWs.Cells[_fila, 2].Value = item.tipoOT.ToString();
                        oWs.Cells[_fila, 3].Value = item.nroObra.ToString(); 
                        oWs.Cells[_fila, 4].Value = item.direccion.ToString();
                        oWs.Cells[_fila, 5].Value = item.distrito.ToString();

                        oWs.Cells[_fila, 6].Value = item.FechaAsignacion.ToString();
                        oWs.Cells[_fila, 7].Value = item.FechaMovil.ToString();

                        oWs.Cells[_fila, 8].Value = item.empresaContratista.ToString();
                        oWs.Cells[_fila, 9].Value = item.jefeCuadrilla.ToString();

                        oWs.Cells[_fila, 10].Value = item.fechaRegistro.ToString();

                        oWs.Cells[_fila, 11].Value = item.generaVolumen.ToString();
                        oWs.Cells[_fila, 12].Value = item.volumenDesmonte.ToString();
                        oWs.Cells[_fila, 13].Value = item.volumenDesmonteRecoger.ToString();

                        oWs.Cells[_fila, 14].Value = item.fechaAprobacion.ToString();
                        oWs.Cells[_fila, 15].Value = item.fechaCancelacion.ToString();

                        oWs.Cells[_fila, 16].Value = item.totalVolumen.ToString();
                        oWs.Cells[_fila, 16].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right;

                        oWs.Cells[_fila, 17].Value = item.totalDesmonte.ToString();
                        oWs.Cells[_fila, 17].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right;

                        oWs.Cells[_fila, 18].Value = item.totalDesmonteRecoger.ToString();
                        oWs.Cells[_fila, 18].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right;
 
                        oWs.Cells[_fila, 19].Value = item.diasVencimiento.ToString();

                        _fila++;
                    }


                    oWs.Row(1).Style.Font.Bold = true;
                    oWs.Row(1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center;
                    oWs.Row(1).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center;

                    oWs.Row(3).Style.Font.Bold = true;
                    oWs.Row(3).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center;
                    oWs.Row(3).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center;

                    for (int k = 1; k <= 19; k++)
                    {
                        oWs.Column(k).AutoFit();
                    }
                    oEx.Save();
                }

                Res = FileExcel;
            }
            catch (Exception)
            {
                throw;
            }
            return Res;
        }
                
        public object get_fueraPlazoOT(int idServicio, int idTipoOT, int idProveedor, int idUsuario)
        {
            Resultado res = new Resultado();
            List<fueraPlazoOT_E> obj_List = new List<fueraPlazoOT_E>();
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_FueraPlazo", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                fueraPlazoOT_E Entidad = new fueraPlazoOT_E();
                                                               
                                Entidad.id_OT = Convert.ToInt32(dr["id_OT"]);
                                Entidad.descripcionEstado = dr["descripcionEstado"].ToString();
                                Entidad.tipoOT = dr["tipoOT"].ToString();
                                Entidad.nroObra = dr["nroObra"].ToString();
                                Entidad.direccion = dr["direccion"].ToString();
                                Entidad.distrito = dr["distrito"].ToString();

                                Entidad.latitud = dr["latitud"].ToString();
                                Entidad.longitud = dr["longitud"].ToString();

                                Entidad.FechaAsignacion = dr["FechaAsignacion"].ToString();
                                Entidad.FechaMovil = dr["FechaMovil"].ToString();

                                Entidad.empresaContratista = dr["empresaContratista"].ToString();
                                Entidad.jefeCuadrilla = dr["jefeCuadrilla"].ToString();

                                Entidad.fueraPlazoHoras = dr["fueraPlazoHoras"].ToString();
                                Entidad.fueraPlazoDias = dr["fueraPlazoDias"].ToString();

                                Entidad.id_tipoTrabajo = Convert.ToInt32(dr["id_tipoTrabajo"]);
                                Entidad.id_Distrito = dr["id_Distrito"].ToString();
                                Entidad.referencia = dr["referencia"].ToString();
                                Entidad.descripcion_OT = dr["descripcion_OT"].ToString();
                                Entidad.id_estado = Convert.ToInt32(dr["id_estado"]);

                                obj_List.Add(Entidad);
                            }

                            res.ok = true;
                            res.data = obj_List;
                            res.totalpage = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }
        
        public object get_descargarFueraPlazoOT(int idServicio, int idTipoOT, int idProveedor, int idUsuario)
        {
            Resultado res = new Resultado();
            List<fueraPlazoOT_E> obj_List = new List<fueraPlazoOT_E>();
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_Reporte_FueraPlazo", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@idTipoOT", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = idUsuario;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                fueraPlazoOT_E Entidad = new fueraPlazoOT_E();

                                Entidad.id_OT = Convert.ToInt32(dr["id_OT"]);
                                Entidad.descripcionEstado = dr["descripcionEstado"].ToString();
                                Entidad.tipoOT = dr["tipoOT"].ToString();
                                Entidad.nroObra = dr["nroObra"].ToString();
                                Entidad.direccion = dr["direccion"].ToString();
                                Entidad.distrito = dr["distrito"].ToString();

                                Entidad.FechaAsignacion = dr["FechaAsignacion"].ToString();
                                Entidad.FechaMovil = dr["FechaMovil"].ToString();
                                Entidad.empresaContratista = dr["empresaContratista"].ToString();
                                Entidad.jefeCuadrilla = dr["jefeCuadrilla"].ToString();

                                Entidad.fueraPlazoHoras = dr["fueraPlazoHoras"].ToString();
                                Entidad.fueraPlazoDias = dr["fueraPlazoDias"].ToString();

                                obj_List.Add(Entidad);
                            }

                            if (obj_List.Count == 0)
                            {
                                res.ok = false;
                                res.data = "No hay informacion disponible";
                                res.totalpage = 0;
                            }
                            else
                            {
                                res.ok = true;
                                res.data = GenerarArchivoExcel_fueraPlazoOT(obj_List, idUsuario);
                                res.totalpage = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }

        public string GenerarArchivoExcel_fueraPlazoOT(List<fueraPlazoOT_E> listDetalle, int id_usuario)
        {
            string Res = "";
            int _fila = 4;
            string FileRuta = "";
            string FileExcel = "";

            try
            {
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Archivos/Excel/" + id_usuario + "_fueraPlazoOT.xlsx");
                string rutaServer = ConfigurationManager.AppSettings["Archivos"];

                FileExcel = rutaServer + "Excel/" + id_usuario + "_fueraPlazoOT.xlsx";

                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }

                Thread.Sleep(1);

                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("OTFueraPlazo");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 8));

                    oWs.Cells[1, 1].Style.Font.Size = 24; //letra tamaño  2
                    oWs.Cells[1, 1].Value = "ORDEN DE TRABAJO FUERA DE PLAZO";
                    oWs.Cells[1, 1, 1, 11].Merge = true;  // combinar celdaS


                    oWs.Cells[3, 1].Value = "ESTADO";
                    oWs.Cells[3, 2].Value = "TIPO DE ORDEN";
                    oWs.Cells[3, 3].Value = "NRO OBRA/ TD";
                    oWs.Cells[3, 4].Value = "DIRECCION";
                    oWs.Cells[3, 5].Value = "DISTRITO";

                    oWs.Cells[3, 6].Value = "FECHA ASIGNACION ";
                    oWs.Cells[3, 7].Value = "FECHA ENVIO MOVIL";

                    oWs.Cells[3, 8].Value = "EMPRESA CONTRATISTA";
                    oWs.Cells[3, 9].Value = "JEFE CUADRILLA";
                    oWs.Cells[3, 10].Value = "FUERA DE PLAZO EN HORAS";
                    oWs.Cells[3, 11].Value = "FUERA DE PLAZO EN DIAS";
 


                    foreach (var item in listDetalle)
                    {
                        oWs.Cells[_fila, 1].Value = item.descripcionEstado.ToString();
                        oWs.Cells[_fila, 2].Value = item.tipoOT.ToString();
                        oWs.Cells[_fila, 3].Value = item.nroObra.ToString();
                        oWs.Cells[_fila, 4].Value = item.direccion.ToString();
                        oWs.Cells[_fila, 5].Value = item.distrito.ToString();

                        oWs.Cells[_fila, 6].Value = item.FechaAsignacion.ToString();
                        oWs.Cells[_fila, 7].Value = item.FechaMovil.ToString();

                        oWs.Cells[_fila, 8].Value = item.empresaContratista.ToString();
                        oWs.Cells[_fila, 9].Value = item.jefeCuadrilla.ToString();

                        oWs.Cells[_fila, 10].Value = item.fueraPlazoHoras.ToString();
                        oWs.Cells[_fila, 11].Value = item.fueraPlazoDias.ToString();
                        _fila++;
                    }


                    oWs.Row(1).Style.Font.Bold = true;
                    oWs.Row(1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center;
                    oWs.Row(1).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center;

                    oWs.Row(3).Style.Font.Bold = true;
                    oWs.Row(3).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center;
                    oWs.Row(3).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center;

                    for (int k = 1; k <= 11; k++)
                    {
                        oWs.Column(k).AutoFit();
                    }
                    oEx.Save();
                }

                Res = FileExcel;
            }
            catch (Exception)
            {
                throw;
            }
            return Res;
        }

        public DataTable get_ubicacionesOT(int idServicio, string fechaGps, int idTipoOT, int idProveedor, int idEstado, int idUsuario)
        {
            DataTable dt_detalle = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_REPORTE_UBICACION_OT", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = fechaGps;
                        cmd.Parameters.Add("@Servicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@TipoOrden", SqlDbType.Int).Value = idTipoOT;
                        cmd.Parameters.Add("@Proveedor", SqlDbType.Int).Value = idProveedor;
                        cmd.Parameters.Add("@Estado", SqlDbType.Int).Value = idEstado;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dt_detalle;
        }

        public object get_tareoCab(int idServicio, string fechaIni, string fechaFin)
        {
            Resultado res = new Resultado();
            List<Tareo_E> obj_List = new List<Tareo_E>();
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_REPORTE_TAREO_CAB", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@FecIni", SqlDbType.VarChar).Value = fechaIni;
                        cmd.Parameters.Add("@FecFin", SqlDbType.VarChar).Value = fechaFin;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Tareo_E Entidad = new Tareo_E();

                                Entidad.dia = dr["dia"].ToString();
                                Entidad.personal = dr["personal"].ToString();
                                Entidad.jefeCuadrilla = dr["jefeCuadrilla"].ToString();
                                Entidad.direccion = dr["direccion"].ToString();
                                Entidad.nroOrden = dr["nroOrden"].ToString();

                                Entidad.horaInicio = dr["horaInicio"].ToString();
                                Entidad.horaTermino = dr["horaTermino"].ToString();
                                Entidad.totalHoras = dr["totalHoras"].ToString();
                                Entidad.precio = dr["precio"].ToString();
                                Entidad.totalSoles = dr["totalSoles"].ToString();

                                obj_List.Add(Entidad);
                            }

                            res.ok = true;
                            res.data = obj_List;
                            res.totalpage = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }
        
        public object get_descargarTareoCab(int idServicio, string fechaIni, string fechaFin, int idUsuario)
        {
            Resultado res = new Resultado();
            List<Tareo_E> obj_List = new List<Tareo_E>();
            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion.bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_REPORTE_TAREO_CAB", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idServicio", SqlDbType.Int).Value = idServicio;
                        cmd.Parameters.Add("@FecIni", SqlDbType.VarChar).Value = fechaIni;
                        cmd.Parameters.Add("@FecFin", SqlDbType.VarChar).Value = fechaFin;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Tareo_E Entidad = new Tareo_E();

                                Entidad.dia = dr["dia"].ToString();
                                Entidad.personal = dr["personal"].ToString();
                                Entidad.jefeCuadrilla = dr["jefeCuadrilla"].ToString();
                                Entidad.direccion = dr["direccion"].ToString();
                                Entidad.nroOrden = dr["nroOrden"].ToString();

                                Entidad.horaInicio = dr["horaInicio"].ToString();
                                Entidad.horaTermino = dr["horaTermino"].ToString();
                                Entidad.totalHoras = dr["totalHoras"].ToString();
                                Entidad.precio = dr["precio"].ToString();
                                Entidad.totalSoles = dr["totalSoles"].ToString();

                                obj_List.Add(Entidad);
                            }

                            if (obj_List.Count == 0)
                            {
                                res.ok = false;
                                res.data = "No hay informacion disponible";
                                res.totalpage = 0;
                            }
                            else
                            {
                                res.ok = true;
                                res.data = GenerarArchivoExcel_tareoCab(obj_List, idUsuario, idServicio,  fechaIni, fechaFin);
                                res.totalpage = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }
               
        public string GenerarArchivoExcel_tareoCab(List<Tareo_E> listDetalle, int id_usuario, int idServicio, string fechaIni, string fechaFin)
        {
            string Res = "";
            int _fila = 7;
            string FileRuta = "";
            string FileExcel = "";

            try
            {
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Archivos/Excel/" + id_usuario + "_tareoOT.xlsx");
                string rutaServer = ConfigurationManager.AppSettings["Archivos"];

                FileExcel = rutaServer + "Excel/" + id_usuario + "_tareoOT.xlsx";

                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }

                Thread.Sleep(1);

                string areas = "";

                if (idServicio == 1)
                {
                    areas = "Obras";
                }
                if (idServicio == 2)
                {
                    areas = "Emergencia Baja Tension";
                }
                if (idServicio == 3)
                {
                    areas = "Emergencia Media Tension";
                }
                if (idServicio == 4)
                {
                    areas = "Alumbrado Publico";
                }
                if (idServicio == 5)
                {
                    areas = "Ule";
                }              

                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("tareoOT");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 8));

                    oWs.Cells[1, 1].Style.Font.Size = 24; //letra tamaño  2
                    oWs.Cells[1, 1].Value = "TAREO DE EFECTIVO POLICIAL";
                    oWs.Cells[1, 1, 1, 11].Merge = true;  // combinar celdaS


                    oWs.Cells[3, 1].Value = "FECHA : " + fechaIni + " al " + fechaFin;
                    oWs.Cells[4, 1].Value = "AREA : " + areas;


                    oWs.Cells[6, 1].Value = "ITEM";
                    oWs.Cells[6, 2].Value = "DIA";
                    oWs.Cells[6, 3].Value = "PERSONAL";
                    oWs.Cells[6, 4].Value = "JEFE CUADRILLA";
                    oWs.Cells[6, 5].Value = "DIRECCION";
                    oWs.Cells[6, 6].Value = "NRO ORDEN ";
                    oWs.Cells[6, 7].Value = "HORA INICIO";
                    oWs.Cells[6, 8].Value = "HORA TERMINO ";
                    oWs.Cells[6, 9].Value = "TOTAL HORAS";
                    oWs.Cells[6, 10].Value = "PRECIO";
                    oWs.Cells[6, 11].Value = "TOTAL SOLES";

                    int cont = 0;
                    foreach (var item in listDetalle)
                    {
                        cont += 1;
                        oWs.Cells[_fila, 1].Value = cont;
                        oWs.Cells[_fila, 2].Value = item.dia.ToString();
                        oWs.Cells[_fila, 3].Value = item.personal.ToString();
                        oWs.Cells[_fila, 4].Value = item.jefeCuadrilla.ToString();
                        oWs.Cells[_fila, 5].Value = item.direccion.ToString();

                        oWs.Cells[_fila, 6].Value = item.nroOrden.ToString();
                        oWs.Cells[_fila, 7].Value = item.horaInicio.ToString();
                        oWs.Cells[_fila, 8].Value = item.horaTermino.ToString();
                        oWs.Cells[_fila, 9].Value = item.totalHoras.ToString();
                        oWs.Cells[_fila, 9].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right;

                        oWs.Cells[_fila, 10].Value = item.precio.ToString();
                        oWs.Cells[_fila, 10].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right;

                        oWs.Cells[_fila, 11].Value = item.totalSoles.ToString();
                        oWs.Cells[_fila, 11].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right;
                        _fila++;
                    }

                    oWs.Row(1).Style.Font.Bold = true;
                    oWs.Row(1).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center;
                    oWs.Row(1).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center;

                    oWs.Row(6).Style.Font.Bold = true;
                    oWs.Row(6).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center;
                    oWs.Row(6).Style.VerticalAlignment = Style.ExcelVerticalAlignment.Center;

                    for (int k = 1; k <= 11; k++)
                    {
                        oWs.Column(k).AutoFit();
                    }
                    oEx.Save();
                }

                Res = FileExcel;
            }
            catch (Exception)
            {
                throw;
            }
            return Res;
        }


    }
}
