﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diplom
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DiplomEntities : DbContext
    {
        public DiplomEntities()
            : base("name=DiplomEntities")
        {
            ZakazList = Set<ZakazList>();
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<Log_Worker> Log_Worker { get; set; }
        public virtual DbSet<LogZak> LogZak { get; set; }
        public virtual DbSet<Postavka> Postavka { get; set; }
        public virtual DbSet<Prodaja> Prodaja { get; set; }
        public virtual DbSet<Sklad> Sklad { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tovar> Tovar { get; set; }
        internal virtual DbSet<ZakazList> ZakazList { get; set; }
    }
}