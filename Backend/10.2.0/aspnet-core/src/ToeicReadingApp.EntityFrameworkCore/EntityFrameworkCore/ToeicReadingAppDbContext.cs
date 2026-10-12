using Abp.Zero.EntityFrameworkCore;
using ToeicReadingApp.Authorization.Roles;
using ToeicReadingApp.Authorization.Users;
using ToeicReadingApp.Exams;
using ToeicReadingApp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace ToeicReadingApp.EntityFrameworkCore;

public class ToeicReadingAppDbContext : AbpZeroDbContext<Tenant, Role, User, ToeicReadingAppDbContext>
{
    /* Define a DbSet for each entity of the application */
    public virtual DbSet<Exam> Exams { get; set; }
    public virtual DbSet<ExamPart> ExamParts { get; set; }
    public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<ExamAttempt> ExamAttempts { get; set; }
    public virtual DbSet<ExamAttemptDetail> ExamAttemptDetails { get; set; }

    public ToeicReadingAppDbContext(DbContextOptions<ToeicReadingAppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Prevent Multiple Cascade Paths in SQL Server
        modelBuilder.Entity<Question>(b =>
        {
            b.HasOne(q => q.Exam)
                .WithMany()
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(q => q.ExamPart)
                .WithMany(p => p.Questions)
                .HasForeignKey(q => q.ExamPartId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(q => q.QuestionGroup)
                .WithMany(g => g.Questions)
                .HasForeignKey(q => q.QuestionGroupId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ExamPart>(b =>
        {
            b.HasOne(p => p.Exam)
                .WithMany(e => e.Parts)
                .HasForeignKey(p => p.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<QuestionGroup>(b =>
        {
            b.HasOne(g => g.ExamPart)
                .WithMany(p => p.QuestionGroups)
                .HasForeignKey(g => g.ExamPartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ExamAttempt>(b =>
        {
            b.HasOne(a => a.Exam)
                .WithMany()
                .HasForeignKey(a => a.ExamId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ExamAttemptDetail>(b =>
        {
            b.HasOne(d => d.ExamAttempt)
                .WithMany(a => a.Details)
                .HasForeignKey(d => d.ExamAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(d => d.Question)
                .WithMany()
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

