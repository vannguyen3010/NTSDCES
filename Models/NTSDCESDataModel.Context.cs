﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTSDCES.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NTSDCESEntities : DbContext
    {
        public NTSDCESEntities()
            : base("name=NTSDCESEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<ForumPost> ForumPosts { get; set; }
        public virtual DbSet<ForumRep> ForumReps { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<SupportForm> SupportForms { get; set; }
        public virtual DbSet<Forum> Fora { get; set; }
    }
}