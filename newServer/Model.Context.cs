﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities10 : DbContext
    {
        public Entities10()
            : base("name=Entities10")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<BLOOD_BANK> BLOOD_BANK { get; set; }
        public DbSet<CHAT> CHAT { get; set; }
        public DbSet<CLINICAL> CLINICAL { get; set; }
        public DbSet<DEPARTMENT> DEPARTMENT { get; set; }
        public DbSet<DOCTOR> DOCTOR { get; set; }
        public DbSet<DRUG> DRUG { get; set; }
        public DbSet<DRUG_INVENTORY> DRUG_INVENTORY { get; set; }
        public DbSet<EXAM_ITEM> EXAM_ITEM { get; set; }
        public DbSet<FOREGROUND_INFORMATION> FOREGROUND_INFORMATION { get; set; }
        public DbSet<INFUSION> INFUSION { get; set; }
        public DbSet<INVENTORY> INVENTORY { get; set; }
        public DbSet<INVENTORY_EXAMPLE> INVENTORY_EXAMPLE { get; set; }
        public DbSet<MANAGER> MANAGER { get; set; }
        public DbSet<MEDICAL_EXAM> MEDICAL_EXAM { get; set; }
        public DbSet<MEDICAL_INSTRUMENT> MEDICAL_INSTRUMENT { get; set; }
        public DbSet<MEDICAL_RECORD> MEDICAL_RECORD { get; set; }
        public DbSet<MEDICAL_TREATEMENT> MEDICAL_TREATEMENT { get; set; }
        public DbSet<NURSE> NURSE { get; set; }
        public DbSet<OPERATION> OPERATION { get; set; }
        public DbSet<OPPOINTMENT> OPPOINTMENT { get; set; }
        public DbSet<PATIENT> PATIENT { get; set; }
        public DbSet<PRESCRIBE> PRESCRIBE { get; set; }
        public DbSet<REGISTRATION_RECORD> REGISTRATION_RECORD { get; set; }
        public DbSet<SCHEDULE> SCHEDULE { get; set; }
        public DbSet<TEST1> TEST1 { get; set; }
    }
}
