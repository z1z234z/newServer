//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace newServer
{
    using System;
    using System.Collections.Generic;
    
    public partial class MEDICAL_RECORD
    {
        public MEDICAL_RECORD()
        {
            this.CLINICAL = new HashSet<CLINICAL>();
            this.INFUSION = new HashSet<INFUSION>();
            this.MEDICAL_EXAM = new HashSet<MEDICAL_EXAM>();
            this.MEDICAL_TREATEMENT = new HashSet<MEDICAL_TREATEMENT>();
            this.PRESCRIBE = new HashSet<PRESCRIBE>();
        }
    
        public decimal ID { get; set; }
        public string TREAT_STATE { get; set; }
        public System.DateTime TIME { get; set; }
        public decimal DOCTOR_ID { get; set; }
        public decimal PATIENT_ID { get; set; }
        public string DISEASE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DIAGNOSIS { get; set; }
        public string CLIN_STATE { get; set; }
        public string INFU_STATE { get; set; }
        public string DRUG_STATE { get; set; }
    
        public virtual ICollection<CLINICAL> CLINICAL { get; set; }
        public virtual DOCTOR DOCTOR { get; set; }
        public virtual ICollection<INFUSION> INFUSION { get; set; }
        public virtual ICollection<MEDICAL_EXAM> MEDICAL_EXAM { get; set; }
        public virtual PATIENT PATIENT { get; set; }
        public virtual ICollection<MEDICAL_TREATEMENT> MEDICAL_TREATEMENT { get; set; }
        public virtual ICollection<PRESCRIBE> PRESCRIBE { get; set; }
    }
}
