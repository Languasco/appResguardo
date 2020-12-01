using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Datos;
using Negocio.Mantenimientos;
using Negocio.Resultados;

namespace WebApi_dsigeResguardo.Controllers.Mantenimiento
{
    [EnableCors("*", "*", "*")]
    public class tblPersonalController : ApiController
    {
        private DSIGE_ResguardoEntities db = new DSIGE_ResguardoEntities();

        // GET: api/tblPersonal
        public IQueryable<tbl_Personal> Gettbl_Personal()
        {
            return db.tbl_Personal;
        }

        
        public object Gettbl_Personal(int opcion, string filtro)
        {
            Resultado res = new Resultado();
            Proveedor_BL obj_negocio = new Proveedor_BL();
            object resul = null;
            try
            {
                if (opcion == 1)
                {
                    res.ok = true;
                    res.data = (from a in db.tbl_Estados
                                select new
                                {
                                    a.id_Estado,
                                    a.descripcion_estado
                                }).ToList();
                    res.totalpage = 0;
                    resul = res;
                }
                else if (opcion == 2)
                {
                    res.ok = true;
                    res.data = (from a in db.tbl_Empresas
                                where a.estado == 1
                                select new
                                {
                                    a.id_Empresa,
                                    a.razonSocial_Empresa
                                }).ToList();
                    res.totalpage = 0;
                    resul = res;
                }
                else if (opcion == 3)
                {
                    string[] parametros = filtro.Split('|');
                    int idEmpresa = Convert.ToInt32(parametros[0].ToString());
                    int idEstado = Convert.ToInt32(parametros[1].ToString());


                    if (idEmpresa == 0)
                    {
                        res.ok = true;
                        res.data = (from a in db.tbl_Personal
                                    join b in db.tbl_Cargo_Personal on a.id_Cargo equals b.id_Cargo
                                    join c in db.tbl_Empresas on a.id_Empresa equals c.id_Empresa
                                    join d in db.tbl_Distritos on a.id_Distrito equals d.id_Distrito
                                    where a.estado == idEstado
                                    select new
                                    {
                                        a.id_Personal,
                                        a.id_Empresa,
                                        c.razonSocial_Empresa,
                                        a.id_TipoDoc,
                                        a.nroDocumento_Personal,
                                        a.apellidos_Personal,
                                        a.nombres_Personal,
                                        a.id_Cargo,
                                        b.nombreCargo,
                                        a.id_Distrito,
                                        d.nombreDistrito,
                                        a.direccion_Personal,
                                        a.estado,
                                        descripcion_estado = a.estado == 0 ? "INACTIVO" : "ACTIVO",
                                        a.usuario_creacion
                                    }).ToList();
                        res.totalpage = 0;
                    }
                    else {
                        res.ok = true;
                        res.data = (from a in db.tbl_Personal
                                    join b in db.tbl_Cargo_Personal on a.id_Cargo equals b.id_Cargo
                                    join c in db.tbl_Empresas on a.id_Empresa equals c.id_Empresa
                                    join d in db.tbl_Distritos on a.id_Distrito equals d.id_Distrito
                                    where a.estado == idEstado && a.id_Empresa == idEmpresa
                                    select new
                                    {
                                        a.id_Personal,
                                        a.id_Empresa,
                                        c.razonSocial_Empresa,
                                        a.id_TipoDoc,
                                        a.nroDocumento_Personal,
                                        a.apellidos_Personal,
                                        a.nombres_Personal,
                                        a.id_Cargo,
                                        b.nombreCargo,
                                        a.id_Distrito,
                                        d.nombreDistrito,
                                        a.direccion_Personal,
                                        a.estado,
                                        descripcion_estado = a.estado == 0 ? "INACTIVO" : "ACTIVO",
                                        a.usuario_creacion
                                    }).ToList();
                        res.totalpage = 0;
                    }






                    resul = res;
                }
                else if (opcion == 4)
                {
                    string[] parametros = filtro.Split('|');
                    int idPersonal = Convert.ToInt32(parametros[0].ToString());

                    tbl_Personal objReemplazar;
                    objReemplazar = db.tbl_Personal.Where(u => u.id_Personal == idPersonal).FirstOrDefault<tbl_Personal>();
                    objReemplazar.estado = 0;

                    db.Entry(objReemplazar).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                        res.ok = true;
                        res.data = "OK";
                        res.totalpage = 0;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        res.ok = false;
                        res.data = ex.InnerException.Message;
                        res.totalpage = 0;
                    }
                    resul = res;

                }
                else if (opcion == 5)
                {
                    string[] parametros = filtro.Split('|');
                    string nroDoc = parametros[0].ToString();

                    if (db.tbl_Personal.Count(e => e.nroDocumento_Personal == nroDoc) > 0)
                    {
                        resul = true;
                    }
                    else
                    {
                        resul = false;
                    }
                }
                else if (opcion == 6)  /// tipo trabajo
                {
                    string[] parametros = filtro.Split('|');
                    int idGrupo =Convert.ToInt32(parametros[0].ToString());

                    res.ok = true;
                    res.data = (from a in db.tbl_GrupoTabla_Det
                                where a.id_grupoTabla == idGrupo && a.estado == 1
                                select new
                                {
                                    a.id_detalleTabla,
                                    a.descripcion_grupoTabla
                                }).ToList();
                    res.totalpage = 0;
                    resul = res;
                }
                else if (opcion == 7)  /// tipo cargo
                {
                     res.ok = true;
                    res.data = (from a in db.tbl_Cargo_Personal
                                where a.estado == 1
                                select new
                                {
                                    a.id_Cargo,
                                    a.nombreCargo
                                }).ToList();
                    res.totalpage = 0;
                    resul = res;
                }
                else if (opcion == 8)
                {
                    string[] parametros = filtro.Split('|');
                    int idUsuario = Convert.ToInt32(parametros[0].ToString());

                    res.ok = true;
                    res.data = obj_negocio.set_grabar_ImportacionPersonal(idUsuario);
                    res.totalpage = 0;
                    resul = res;
                }
                else if (opcion == 9)  /// --- distritos
                {
                    res.ok = true;
                    res.data = (from a in db.tbl_Distritos 
                                where a.estado == 1
                                select new
                                {
                                    a.id_Distrito ,
                                    a.nombreDistrito
                                }).ToList();
                    res.totalpage = 0;
                    resul = res;
                }
                else
                {
                    res.ok = false;
                    res.data = "Opcion seleccionada invalida";
                    res.totalpage = 0;

                    resul = res;
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
                resul = res;
            }
            return resul;
        }

        public object Posttbl_Personal(tbl_Personal tbl_Personal)
        {
            Resultado res = new Resultado();
            try
            {
                tbl_Personal.fecha_creacion = DateTime.Now;
                db.tbl_Personal.Add(tbl_Personal);
                db.SaveChanges();

                res.ok = true;
                res.data = (from a in db.tbl_Personal
                            join b in db.tbl_Cargo_Personal on a.id_Cargo equals b.id_Cargo
                            join c in db.tbl_Empresas on a.id_Empresa equals c.id_Empresa
                            join d in db.tbl_Distritos on a.id_Distrito equals d.id_Distrito
                            where a.id_Personal == tbl_Personal.id_Personal
                            select new
                            {
                                a.id_Personal,
                                a.id_Empresa,
                                c.razonSocial_Empresa,
                                a.id_TipoDoc,
                                a.nroDocumento_Personal,
                                a.apellidos_Personal,
                                a.nombres_Personal,
                                a.id_Cargo,
                                b.nombreCargo,
                                a.id_Distrito,
                                d.nombreDistrito,
                                a.direccion_Personal,
                                a.estado,
                                descripcion_estado = a.estado == 0 ? "INACTIVO" : "ACTIVO",
                                a.usuario_creacion
                            }).ToList();
                res.totalpage = 0;
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                res.totalpage = 0;
            }
            return res;
        }

        public object Puttbl_Personal(int id, tbl_Personal tbl_Personal)
        {
            Resultado res = new Resultado();

            tbl_Personal objReemplazar;
            objReemplazar = db.tbl_Personal.Where(u => u.id_Personal == id).FirstOrDefault<tbl_Personal>();

            objReemplazar.id_Empresa = tbl_Personal.id_Empresa;
            objReemplazar.id_TipoDoc = tbl_Personal.id_TipoDoc;
            objReemplazar.nroDocumento_Personal = tbl_Personal.nroDocumento_Personal;
            objReemplazar.apellidos_Personal = tbl_Personal.apellidos_Personal;
            objReemplazar.nombres_Personal = tbl_Personal.nombres_Personal;

            objReemplazar.id_Cargo = tbl_Personal.id_Cargo;
            objReemplazar.direccion_Personal = tbl_Personal.direccion_Personal;
            objReemplazar.id_Distrito = tbl_Personal.id_Distrito;

            objReemplazar.estado = tbl_Personal.estado;
            objReemplazar.usuario_edicion = tbl_Personal.usuario_creacion;
            objReemplazar.fecha_edicion = DateTime.Now;

            db.Entry(objReemplazar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                res.ok = true;
                res.data = "OK";
                res.totalpage = 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                res.ok = false;
                res.data = ex.InnerException.Message;
                res.totalpage = 0;
            }

            return res;
        }


        // DELETE: api/tblPersonal/5
        [ResponseType(typeof(tbl_Personal))]
        public IHttpActionResult Deletetbl_Personal(int id)
        {
            tbl_Personal tbl_Personal = db.tbl_Personal.Find(id);
            if (tbl_Personal == null)
            {
                return NotFound();
            }

            db.tbl_Personal.Remove(tbl_Personal);
            db.SaveChanges();

            return Ok(tbl_Personal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_PersonalExists(int id)
        {
            return db.tbl_Personal.Count(e => e.id_Personal == id) > 0;
        }
    }
}