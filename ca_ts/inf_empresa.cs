//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ca_ts
{
    using System;
    using System.Collections.Generic;
    
    public partial class inf_empresa
    {
        public System.Guid id_empresa { get; set; }
        public Nullable<int> id_estatus { get; set; }
        public Nullable<int> id_tipo_rfc { get; set; }
        public string rfc { get; set; }
        public string razon_social { get; set; }
        public string telefono { get; set; }
        public string telefono_alt { get; set; }
        public string email { get; set; }
        public string email_alt { get; set; }
        public string calle_num { get; set; }
        public string cp { get; set; }
        public Nullable<int> id_asenta_cpcons { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
    }
}
