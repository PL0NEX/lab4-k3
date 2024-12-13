using FinancePlannerAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerApi.Infrastructure
{
    public partial class FinancePlannerContext : DbContext
    {
        public FinancePlannerContext()
        {
        }

        public FinancePlannerContext(DbContextOptions<FinancePlannerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<FinancialGoal> FinancialGoals { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e=> e.Id).HasName("users_pkey");

                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("login");

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("pass_hash");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("email");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("categories_pkey");

                entity.ToTable("categories");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("name");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_categories_user");
            });

            modelBuilder.Entity<FinancialGoal>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("financial_goals_pkey");

                entity.ToTable("financial_goals");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("name");

                entity.Property(e => e.TargetAmount)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("target_amount");

                entity.Property(e => e.CurrentAmount)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("current_amount");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FinancialGoals)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_financial_goals_user");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("transactions_pkey");

                entity.ToTable("transactions");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("type");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("numeric(12, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.TransactionDate)
                    .HasColumnType("date")
                    .HasColumnName("transaction_date");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_transactions_user");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_transactions_category");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
