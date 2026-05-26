using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vet_domain.Models;

namespace vet_data.Context
{
    public class VetDbContext : DbContext
    {
        public VetDbContext(DbContextOptions<VetDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ClienteTelefono> ClienteTelefonos { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<ClienteMascota> ClienteMascotas { get; set; }
        public DbSet<TypeVacuna> TypeVacunas { get; set; }
        public DbSet<Vacuna> Vacunas { get; set; }
        public DbSet<Desparacitacion> Desparacitaciones { get; set; }
        public DbSet<Cirugia> Cirugias { get; set; }
        public DbSet<Grooming> Groomings { get; set; }
        public DbSet<TypeCita> TypeCitas { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nombres de tablas exactos como están en Neon
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<ClienteTelefono>().ToTable("ClienteTelefono");
            modelBuilder.Entity<Mascota>().ToTable("Mascota");
            modelBuilder.Entity<ClienteMascota>().ToTable("ClienteMascota");
            modelBuilder.Entity<TypeVacuna>().ToTable("TypeVacuna");
            modelBuilder.Entity<Vacuna>().ToTable("Vacuna");
            modelBuilder.Entity<Desparacitacion>().ToTable("Desparacitacion");
            modelBuilder.Entity<Cirugia>().ToTable("Cirugia");
            modelBuilder.Entity<Grooming>().ToTable("Grooming");
            modelBuilder.Entity<TypeCita>().ToTable("TypeCita");
            modelBuilder.Entity<Cita>().ToTable("Cita");

            // PKs
            modelBuilder.Entity<User>().HasKey(u => u.IdUser);
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<ClienteTelefono>().HasKey(ct => ct.IdClienteTelefono);
            modelBuilder.Entity<Mascota>().HasKey(m => m.IdMascota);
            modelBuilder.Entity<ClienteMascota>().HasKey(cm => cm.IdClienteMascota);
            modelBuilder.Entity<TypeVacuna>().HasKey(tv => tv.IdTypeVacuna);
            modelBuilder.Entity<Vacuna>().HasKey(v => v.IdVacuna);
            modelBuilder.Entity<Desparacitacion>().HasKey(d => d.IdDesparacitacion);
            modelBuilder.Entity<Cirugia>().HasKey(c => c.IdCirugia);
            modelBuilder.Entity<Grooming>().HasKey(g => g.IdGrooming);
            modelBuilder.Entity<TypeCita>().HasKey(tc => tc.IdTypeCita);
            modelBuilder.Entity<Cita>().HasKey(c => c.IdCita);

            // Columnas con nombres exactos de Neon
            modelBuilder.Entity<User>()
                .Property(u => u.IdUser).HasColumnName("IdUser");

            modelBuilder.Entity<Cliente>()
                .Property(c => c.IdCliente).HasColumnName("IdCliente");

            modelBuilder.Entity<ClienteTelefono>()
                .Property(ct => ct.Cliente_FK).HasColumnName("Cliente_FK");

            modelBuilder.Entity<Mascota>()
                .Property(m => m.birth_date).HasColumnName("birth_date");

            modelBuilder.Entity<TypeVacuna>()
                .Property(tv => tv.IdTypeVacuna).HasColumnName("IdTypeVacuna");

            modelBuilder.Entity<Vacuna>()
                .Property(v => v.Mascota_FK).HasColumnName("Mascota_FK");
            modelBuilder.Entity<Vacuna>()
                .Property(v => v.TypeVacuna_FK).HasColumnName("TypeVacuna_FK");

            modelBuilder.Entity<Desparacitacion>()
                .Property(d => d.Mascota_FK).HasColumnName("Mascota_FK");

            modelBuilder.Entity<Cirugia>()
                .Property(c => c.Mascota_FK).HasColumnName("Mascota_FK");

            modelBuilder.Entity<Grooming>()
                .Property(g => g.Mascota_FK).HasColumnName("Mascota_FK");

            modelBuilder.Entity<ClienteMascota>()
                .Property(cm => cm.Mascota_FK).HasColumnName("Mascota_FK");
            modelBuilder.Entity<ClienteMascota>()
                .Property(cm => cm.Cliente_FK).HasColumnName("Cliente_FK");

            modelBuilder.Entity<Cita>()
                .Property(c => c.Mascota_FK).HasColumnName("Mascota_FK");
            modelBuilder.Entity<Cita>()
                .Property(c => c.TypeCita_FK).HasColumnName("TypeCita_FK");

            // Relaciones
            modelBuilder.Entity<ClienteTelefono>()
                .HasOne(ct => ct.Cliente)
                .WithMany(c => c.Telefonos)
                .HasForeignKey(ct => ct.Cliente_FK);

            modelBuilder.Entity<ClienteMascota>()
                .HasOne(cm => cm.Cliente)
                .WithMany(c => c.ClienteMascotas)
                .HasForeignKey(cm => cm.Cliente_FK);

            modelBuilder.Entity<ClienteMascota>()
                .HasOne(cm => cm.Mascota)
                .WithMany(m => m.ClienteMascotas)
                .HasForeignKey(cm => cm.Mascota_FK);

            modelBuilder.Entity<Vacuna>()
                .HasOne(v => v.Mascota)
                .WithMany(m => m.Vacunas)
                .HasForeignKey(v => v.Mascota_FK);

            modelBuilder.Entity<Vacuna>()
                .HasOne(v => v.TypeVacuna)
                .WithMany(tv => tv.Vacunas)
                .HasForeignKey(v => v.TypeVacuna_FK);

            modelBuilder.Entity<Desparacitacion>()
                .HasOne(d => d.Mascota)
                .WithMany(m => m.Desparacitaciones)
                .HasForeignKey(d => d.Mascota_FK);

            modelBuilder.Entity<Cirugia>()
                .HasOne(c => c.Mascota)
                .WithMany(m => m.Cirugias)
                .HasForeignKey(c => c.Mascota_FK);

            modelBuilder.Entity<Grooming>()
                .HasOne(g => g.Mascota)
                .WithMany(m => m.Groomings)
                .HasForeignKey(g => g.Mascota_FK);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Mascota)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.Mascota_FK);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.TypeCita)
                .WithMany(tc => tc.Citas)
                .HasForeignKey(c => c.TypeCita_FK);
        }
    }
}
