//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HWapi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CatValue
    {
        public int ValorId { get; set; }
        public string Valor { get; set; }
        public Nullable<int> CatalogoId { get; set; }
        public Nullable<bool> Activo { get; set; }
    
        public virtual CatCatalog CatCatalog { get; set; }
    }
}
