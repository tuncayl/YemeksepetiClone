//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace tamircisepeti22.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sepet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sepet()
        {
            this.siparislers = new HashSet<siparisler>();
        }
    
        public int sepetid { get; set; }
        public int hizmetid { get; set; }
        public int userid { get; set; }
        public bool kullanıldı { get; set; }
    
        public virtual hizmetler hizmetler { get; set; }
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<siparisler> siparislers { get; set; }
    }
}
