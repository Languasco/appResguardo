//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_ParteDiario_Cab
    {
        public int id_ParteDiario { get; set; }
        public Nullable<int> id_Empresa { get; set; }
        public Nullable<int> id_Servicios { get; set; }
        public Nullable<System.DateTime> fechaAsignacion_PD { get; set; }
        public Nullable<System.DateTime> fechaRegistro_PD { get; set; }
        public string horaInicio_PD { get; set; }
        public string horaTermino_PD { get; set; }
        public string totalHoras_PD { get; set; }
        public Nullable<decimal> cantidadHoras_PD { get; set; }
        public Nullable<decimal> precio_PD { get; set; }
        public Nullable<decimal> totalSoles_PD { get; set; }
        public Nullable<int> id_UsuarioEfectivoPolicial { get; set; }
        public Nullable<int> id_PersonalCoordinar { get; set; }
        public Nullable<int> id_PersonalJefeCuadrilla { get; set; }
        public string lugarTrabajo_PD { get; set; }
        public string nroObraTD_PD { get; set; }
        public string observaciones_PD { get; set; }
        public string latitud_PD { get; set; }
        public string longitud_PD { get; set; }
        public string firma_EfectivoPolicial { get; set; }
        public string firma_JefeCuadrilla { get; set; }
        public Nullable<int> estado { get; set; }
        public Nullable<int> usuario_creacion { get; set; }
        public Nullable<System.DateTime> fecha_creacion { get; set; }
        public Nullable<int> usuario_edicion { get; set; }
        public Nullable<System.DateTime> fecha_edicion { get; set; }
    }
}
