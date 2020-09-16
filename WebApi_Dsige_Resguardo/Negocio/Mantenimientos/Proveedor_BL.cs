using Negocio.Conexion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using ThoughtWorks.QRCode.Codec;

namespace Negocio.Mantenimientos
{
    public class Proveedor_BL
    {
        public DataTable get_DatosGenerales_Iconos(int idIcon)
        {
            DataTable dt_detalle = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_MANT_EMPRESAS_ICONOS", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idIcono", SqlDbType.Int).Value = idIcon;

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

        public string set_grabar_ImportacionPersonal(int id_usuario)
        {
            string resultado = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_MANT_PERSONAL_TEMPORAL_PERSONAL_GRABAR", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_usuario", SqlDbType.VarChar).Value = id_usuario;

                        cmd.ExecuteNonQuery();
                        resultado = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                resultado = e.Message;
            }
            return resultado;
        }


        public string set_grabar_areasMasivas(int idUsuarioBD, string areasMasivo, int idusuarioLoggin)
        {
            string resultado = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_MANT_USUARIO_AREAS_MASIVO_GRABAR", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idUsuarioBD", SqlDbType.Int).Value = idUsuarioBD;
                        cmd.Parameters.Add("@areasMasivo", SqlDbType.VarChar).Value = areasMasivo;
                        cmd.Parameters.Add("@idusuarioLogin", SqlDbType.Int).Value = idusuarioLoggin;

                        cmd.ExecuteNonQuery();
                        resultado = "OK";
                    }
                }
            }
            catch (Exception e)
            {
                resultado = e.Message;
            }
            return resultado;
        }
        

        public string get_generarDescargar_CodigoQR(int idUsuarioBD)
        {
            string resultado = "";
            try
            {
                DataTable dt_usuarios = new DataTable();
                string rutaQR = System.Web.Hosting.HostingEnvironment.MapPath("~/Archivos/QR/" + idUsuarioBD + ".gif");
                string FileExcel = ConfigurationManager.AppSettings["Archivos"] + "QR/" + idUsuarioBD + ".gif";

                if (File.Exists(rutaQR)) /// verificando si existe el archivo zip
                {
                    System.IO.File.Delete(rutaQR);
                }

                using (SqlConnection cn = new SqlConnection(bdConexion.cadenaBDcx()))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("DSIGE_PROY_W_MANT_USUARIO_CODIGO_QR", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idUsuarioBD", SqlDbType.Int).Value = idUsuarioBD;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_usuarios);
                        }
                    }
                                                         
                    if (dt_usuarios.Rows.Count > 0)
                    {
                        ///------GENERANDO EL CODIGO QR------
                            var Codigo_Has = "";
                                Codigo_Has = dt_usuarios.Rows[0]["codigoQR"].ToString();
 
                            byte[] obj_codQR = GeneraCodigoQR(Codigo_Has);

                            using (FileStream Writer = new System.IO.FileStream(rutaQR, FileMode.Create, FileAccess.Write))
                            {
                                Writer.Write(obj_codQR, 0, obj_codQR.Length);
                            }

                        if (File.Exists(rutaQR)) /// verificando si existe el archivo zip
                        {
                            resultado = FileExcel;
                        }
                        else {
                            throw new System.ArgumentException("No se pudo almacenar el archivo en el servidor");
                        }

              

                        //---- FIN DE GENERACION CODIGO QR---------
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }

        public byte[] GeneraCodigoQR(string TextoCodificar)
        {
            //Instancia del objeto encargado de codificar el codigo QR
            QRCodeEncoder CodigoQR = new QRCodeEncoder();

            CodigoQR.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            CodigoQR.QRCodeScale = 4;
            CodigoQR.QRCodeErrorCorrect = ThoughtWorks.QRCode.Codec.QRCodeEncoder.ERROR_CORRECTION.M;
            CodigoQR.QRCodeVersion = 0;
            CodigoQR.QRCodeBackgroundColor = System.Drawing.Color.White;
            CodigoQR.QRCodeForegroundColor = System.Drawing.Color.Black;

            //Se retorna el Codigo QR codificado en formato de arreglo de bytes.
            return imageToByteArray(CodigoQR.Encode(TextoCodificar, System.Text.Encoding.UTF8));
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }




    }
}
