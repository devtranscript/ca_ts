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
    
    public partial class inf_dispositivos
    {
        public int id_dispositivo { get; set; }
        public string ip { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public Nullable<System.Guid> id_sala { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
    }
}
