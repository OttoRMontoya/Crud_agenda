using System.Data.Entity;
using Crud_agenda.Models;

namespace Crud_agenda.Models
{
    public class AgendaDbContext : DbContext
    {
        public AgendaDbContext() : base("AgendaEntities")
        {
        }

        public DbSet<AgendaEstatus> AgendaEstatus { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<AgendaEmpresa> AgendaEmpresa { get; set; }
        public DbSet<AgendaClinica> AgendaClinica { get; set; }
        public DbSet<AgendaDoctor> AgendaDoctor { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AgendaEstatus>()
                .Property(x => x.IdEstatus)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AgendaEstatus>()
                .Property(x => x.FechaCreacion)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Agenda>()
                .Property(x => x.IdAgendaCita)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Agenda>()
                .Property(x => x.FechaCrea)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Agenda>()
                .Property(x => x.FechaModifica)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<AgendaEmpresa>()
                .Property(x => x.IdEmpresa)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AgendaClinica>()
                .Property(x => x.IdClinica)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AgendaDoctor>()
                .Property(x => x.CodigoDoctor)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Paciente>()
                .Property(x => x.Codigo)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Paciente>()
                .Property(x => x.FechaCrea)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<Paciente>()
                .Property(x => x.FechaModifica)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);
        }
    }
}
