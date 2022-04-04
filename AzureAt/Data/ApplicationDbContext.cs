using AzureAt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = STOREPROCEDURES15; Trusted_Connection = True; MultipleActiveResultSets = true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amigo>(entity =>
            {
                entity.HasOne(d => d.Estado).WithMany(p => p.ListaAmigos).HasForeignKey(d => d.EstadoId)
                      .OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Amigo_Estado");
            });
            modelBuilder.Entity<Amiguinhos>(entity =>
            {            
                entity.HasOne(d => d.Amigo).WithMany(p => p.ListaAmiguinhos).HasForeignKey(d => d.Id)
                      .OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Amiguinhos_Amigo");
            });
            modelBuilder.Entity<Pais>(entity =>
            {
            });
            modelBuilder.Entity<Estado>(entity =>
            {           
                entity.HasOne(d => d.Pais).WithMany(p => p.ListaEstados).HasForeignKey(d => d.PaisId)
                      .OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_Estado_Pais");
            });
        }
        public DbSet<Amigo> Amigo { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<AzureAt.Models.FileData> FileData { get; set; }
        public DbSet<AzureAt.Models.Amiguinhos> Amiguinhos { get; set; }

    }
}
