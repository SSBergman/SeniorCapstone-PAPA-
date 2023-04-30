using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ASP_Test.Models;

namespace ASP_Test.Models;

public partial class PapaDb1Context : DbContext
{
    public PapaDb1Context()
    {
    }

    public PapaDb1Context(DbContextOptions<PapaDb1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<CompletedClass> CompletedClasses { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<DegreeList> DegreeLists { get; set; }

    public virtual DbSet<ListableClass> ListableClasses { get; set; }

    public virtual DbSet<ListedClass> ListedClasses { get; set; }

    public virtual DbSet<ReqChecklist> ReqChecklists { get; set; }

    public virtual DbSet<ReqList> ReqLists { get; set; }

    public virtual DbSet<RequiredForDegree> RequiredForDegrees { get; set; }

    public virtual DbSet<ClassChecklist> ClassChecklists { get; set; }

    public virtual DbSet<Requirement> Requirements { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<SemCount> SemCount { get; set; }

    public virtual DbSet<ListedView> ListedView { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=20.163.97.233,1401;Initial Catalog=PAPA_DB1;Persist Security Info=True;TrustServerCertificate=True; User ID=SrPAPATeam;Password=ListenToPAPA14");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("ClassPK");

            entity.ToTable("Class");

            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.Category)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Class_Name");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
            entity.Property(e => e.Prerequisites)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Prerequisites");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");
            entity.Property(e => e.InFall)
                .IsRequired()
                .HasDefaultValueSql("('FALSE')");
            entity.Property(e => e.InSpring)
                .IsRequired()
                .HasDefaultValueSql("('FALSE')");
            entity.Property(e => e.InSummer)
                .IsRequired()
                .HasDefaultValueSql("('FALSE')");
            entity.Property(e => e.IsOffered)
                .IsRequired()
                .HasDefaultValueSql("('TRUE')");
        });

        modelBuilder.Entity<CompletedClass>(entity =>
        {
            entity.HasKey(e => e.CompleteId).HasName("CompPK");

            entity.ToTable("CompletedClass");

            entity.Property(e => e.CompleteId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Complete_ID");
            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");
            entity.Property(e => e.StudentId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Student_ID");

            entity.HasOne(d => d.Class).WithMany(p => p.CompletedClasses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("CompClassFK");

            entity.HasOne(d => d.Student).WithMany(p => p.CompletedClasses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("CompStuFK");
        });

        modelBuilder.Entity<Degree>(entity =>
        {
            entity.HasKey(e => e.DegreeId).HasName("DegreePK");

            entity.ToTable("Degree");

            entity.Property(e => e.DegreeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Degree_ID");
            entity.Property(e => e.DegreeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Degree_Name");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");
            entity.Property(e => e.VersionYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValueSql("(datepart(year,getdate()))")
                .IsFixedLength()
                .HasColumnName("Version_Year");
        });

        modelBuilder.Entity<DegreeList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DegreeList");

            entity.Property(e => e.DegreeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Degree_ID");
            entity.Property(e => e.DegreeName)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("Degree Name");
        });

        modelBuilder.Entity<ListableClass>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListableClasses");

            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.Category)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Class_Name");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
            entity.Property(e => e.Prerequisites)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Prerequisites");
            entity.Property(e => e.Fall)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Spring)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Summer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Available)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Available");

        });

        modelBuilder.Entity<ListedClass>(entity =>
        {
            entity.HasKey(e => e.ListId).HasName("ListPK");

            entity.ToTable("ListedClass");

            entity.Property(e => e.ListId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("List_ID");
            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");
            entity.Property(e => e.SemesterId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Semester_ID");

            entity.HasOne(d => d.Class).WithMany(p => p.ListedClasses)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("ListClassFK");

            entity.HasOne(d => d.Semester).WithMany(p => p.ListedClasses)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("ListSemFK");
        });

        modelBuilder.Entity<ReqChecklist>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Req_Checklist");

            entity.Property(e => e.DegreeName)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("Degree_Name");
            entity.Property(e => e.Category)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Complete)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
            entity.Property(e => e.StudentId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Student_ID");
        });
        modelBuilder.Entity<ReqList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Req_List");

            entity.Property(e => e.DegreeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Degree_ID");
            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Class_Name");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
        });

        modelBuilder.Entity<RequiredForDegree>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RequiredForDegree");

            entity.Property(e => e.DegreeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Degree_ID");
            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Class_Name");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
            entity.Property(e => e.RequiredBit)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("RequiredBit");
        });
        modelBuilder.Entity<ClassChecklist>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ClassChecklist");

            entity.Property(e => e.StudentId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Student_ID");
            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.Category)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("Category");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Class_Name");
            entity.Property(e => e.Status)
                .HasMaxLength(38)
                .IsUnicode(false)
                .HasColumnName("Status");
            entity.Property(e => e.StatusBit)
                .IsRequired()
                .HasDefaultValueSql("('FALSE')")
                .HasColumnName("StatusBit");
        });
        modelBuilder.Entity<Requirement>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("ReqPK");

            entity.ToTable("Requirement");

            entity.Property(e => e.ReqId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Req_ID");
            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.DegreeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Degree_ID");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");

            entity.HasOne(d => d.Class).WithMany(p => p.Requirements)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReqClaFK");

            entity.HasOne(d => d.Degree).WithMany(p => p.Requirements)
                .HasForeignKey(d => d.DegreeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReqDegFK");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.SemesterId).HasName("SemesterPK");

            entity.ToTable("Semester");

            entity.Property(e => e.SemesterId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Semester_ID");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");
            entity.Property(e => e.Season)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.SemYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Sem_Year");
            entity.Property(e => e.StudentId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Student_ID");

            entity.HasOne(d => d.Student).WithMany(p => p.Semesters)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("SemStuFK");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("StudentPK");

            entity.ToTable("Student");

            entity.Property(e => e.StudentId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Student_ID");
            entity.Property(e => e.DegreeId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Degree_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.GradYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValueSql("(datepart(year,getdate()))")
                .IsFixedLength()
                .HasColumnName("Grad_Year");
            entity.Property(e => e.IdNum)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_Num");
            entity.Property(e => e.StudentPassword)
                .HasMaxLength(ushort.MaxValue)
                .IsUnicode(false)
                .HasColumnName("Student_Password");

            entity.HasOne(d => d.Degree).WithMany(p => p.Students)
                .HasForeignKey(d => d.DegreeId)
                .HasConstraintName("StuDegFK");
        });
        modelBuilder.Entity<SemCount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SemCount");

            entity.Property(e => e.StudentId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Student_ID");
            entity.Property(e => e.Semesters)
                .HasColumnName("Semesters");

        });
        modelBuilder.Entity<ListedView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ListedView");

            entity.Property(e => e.ClassId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Class_ID");
            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Class_Name");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Course_Code");
            entity.Property(e => e.Prerequisites)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Prerequisites");
            entity.Property(e => e.ListId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("List_ID");
            entity.Property(e => e.SemesterId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Semester_ID");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
