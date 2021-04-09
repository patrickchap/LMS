using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Models.LMSModels
{
    public partial class Team1LMSContext : DbContext
    {
        public Team1LMSContext()
        {
        }

        public Team1LMSContext(DbContextOptions<Team1LMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Classes> Classes { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Enrolled> Enrolled { get; set; }
        public virtual DbSet<Professors> Professors { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Submission> Submission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("Server=atr.eng.utah.edu;User Id=u0780207;Password=Basket#1;Database=Team1LMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrators>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.Dob)
                    .IsRequired()
                    .HasColumnName("DOB")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<AssignmentCategories>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.ClassId, e.Name })
                    .HasName("ClassID")
                    .IsUnique();

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClassId)
                    .HasColumnName("ClassID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GradingWeight).HasColumnType("int(3)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.AssignmentCategories)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssignmentCategories_ibfk_1");
            });

            modelBuilder.Entity<Assignments>(entity =>
            {
                entity.HasKey(e => e.AssignmentId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.CategoryId, e.Name })
                    .HasName("CategoryID")
                    .IsUnique();

                entity.Property(e => e.AssignmentId)
                    .HasColumnName("AssignmentID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("CategoryID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasColumnType("varchar(8192)");

                entity.Property(e => e.DueDate).HasColumnType("DateTime");

                entity.Property(e => e.MaxPoint).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Assignments_ibfk_1");
            });

            modelBuilder.Entity<Classes>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.TaughtBy)
                    .HasName("TaughtBy");

                entity.HasIndex(e => new { e.CourseCatalogId, e.Semester })
                    .HasName("CourseCatalogID_2")
                    .IsUnique();

                entity.Property(e => e.ClassId)
                    .HasColumnName("ClassID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Year)
                    .HasColumnName("Year")
                    .HasColumnType("uint");

                entity.Property(e => e.CourseCatalogId)
                    .IsRequired()
                    .HasColumnName("CourseCatalogID")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasColumnType("DateTime");

                entity.Property(e => e.Semester)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasColumnType("DateTime");

                entity.Property(e => e.TaughtBy)
                    .IsRequired()
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.HasOne(d => d.CourseCatalog)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseCatalogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_1");

                entity.HasOne(d => d.TaughtByNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.TaughtBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Classes_ibfk_2");

                
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.HasKey(e => e.CatalogId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => new { e.DeptAbbr, e.Number })
                    .HasName("DeptAbbr")
                    .IsUnique();

                entity.Property(e => e.CatalogId)
                    .HasColumnName("CatalogID")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.DeptAbbr)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Number).HasColumnType("int(4)");

                entity.HasOne(d => d.DeptAbbrNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.DeptAbbr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Courses_ibfk_1");
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.Abbreviation)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Name)
                    .HasName("Name")
                    .IsUnique();

                entity.Property(e => e.Abbreviation).HasColumnType("varchar(4)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Enrolled>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ClassId })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ClassId)
                    .HasName("ClassID");

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.ClassId)
                    .HasColumnName("ClassID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Grade)
                    .IsRequired()
                    .HasColumnType("varchar(2)");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Enrolled)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrolled)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrolled_ibfk_1");
            });

            modelBuilder.Entity<Professors>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DeptWorksIn)
                    .HasName("DeptWorksIn");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.DeptWorksIn)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.Property(e => e.Dob)
                    .IsRequired()
                    .HasColumnName("DOB")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.DeptWorksInNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.DeptWorksIn)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Professors_ibfk_1");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.UId)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.DeptMajor)
                    .HasName("DeptMajor");

                entity.Property(e => e.UId)
                    .HasColumnName("uID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.DeptMajor)
                    .IsRequired()
                    .HasColumnType("varchar(4)");

                entity.Property(e => e.Dob)
                    .IsRequired()
                    .HasColumnName("DOB")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.DeptMajorNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.DeptMajor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("DeptMajor");
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => new { e.AssignmentId, e.StudentId, e.Time })
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.StudentId)
                    .HasName("StudentID");

                entity.Property(e => e.AssignmentId)
                    .HasColumnName("AssignmentID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasColumnType("varchar(8)");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasColumnType("varchar(8192)");

                entity.Property(e => e.Score).HasColumnType("int(11)");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submission_ibfk_1");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Submission)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Submission_ibfk_2");
            });
        }
    }
}
