namespace QuizEngine.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using QuizEngine.Models;

    public partial class QuizEngineDbContext : DbContext
    {
        static QuizEngineDbContext()
        {
            Database.SetInitializer<QuizEngineDbContext>(null);
        }

        public QuizEngineDbContext()
            : base("name=QuizEngineDb")
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            /*
            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.Quiz)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.Results)
                .WithRequired(e => e.Quiz)
                .WillCascadeOnDelete(false);
             */ 
        }
    }
}
