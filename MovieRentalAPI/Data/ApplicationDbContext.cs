namespace MovieRentalAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using MovieRentalAPI.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookStatus> BookStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookStatus>().HasData(
                new BookStatus { Id = 1, Name = "Na półce" },
                new BookStatus { Id = 2, Name = "Wypożyczona" },
                new BookStatus { Id = 3, Name = "Zwrócona" },
                new BookStatus { Id = 4, Name = "Uszkodzona" }
            );

            //Example data  TODO: remove
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Hobbit", Author="Tolkien", ISBN= "9780345445605", BookStatusId=1 },
                new Book { Id = 2, Title = "LotR: Drużyna pierścienia", Author = "Tolkien", ISBN = "9780345445606", BookStatusId = 2 },
                new Book { Id = 3, Title = "LotR: Dwie wieże", Author = "Tolkien", ISBN = "9780345445607", BookStatusId = 3 },
                new Book { Id = 4, Title = "Lotr: Powrót Króla", Author = "Tolkien", ISBN = "9780345445608", BookStatusId = 4 }
            );

            modelBuilder.Entity<Book>()
                    .HasIndex(b => b.ISBN)
                    .IsUnique();
        }
    }
}
