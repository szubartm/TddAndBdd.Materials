using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NUnit.Framework;

namespace UnitTesting.Databases
{
    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        [UsedImplicitly]
        public BlogContext() { }

        public BlogContext([NotNull] DbContextOptions options) : base(options) { }
    }

    [TestFixture]
    public class EntitiesTests
    {
        #region Entities Test Support

        public static BlogContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase("MyBlogs")
                .Options;

            var context = new BlogContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static void Initialize(BlogContext context, int blogCount = 25, int postCount = 10)
        {
            var rnd = new Random(1);

            for (int i = 1; i <= blogCount; i++)
            {
                var blog = new Blog { Name = $"Blog{i}" };

                context.Blogs.Add(blog);

                for (int j = 1; j <= postCount; j++)
                {
                    var post = new Post
                    {
                        Blog = blog,
                        Content = "My Content",
                        NumberOfReads = rnd.Next(0, 5000),
                        PostDate = DateTime.Today.AddDays(-rnd.Next(0, 100)).AddSeconds(rnd.Next(0, 30000)),
                        Title = $"Blog {blog.Id} - Post {j}",
                    };

                    context.Posts.Add(post);
                }
            }

            context.SaveChanges();
        }

        public static void DisposeContext(ref BlogContext context)
        {
            context?.Database.EnsureDeleted();
            context?.Dispose();
            context = null;
        }

        #endregion

        BlogContext _context;

        [OneTimeSetUp]
        public void BeforeEverything() => Initialize(_context = CreateContext(), 5, 15);

        [OneTimeTearDown]
        public void AfterEverything() => DisposeContext(ref _context);


        [Test]
        public void Entities_Where_SimplePredicate()
        {
            //Arrange
            var expected = new[] { 3, 5 };

            //Act
            var actual = _context.Blogs.Where(blog => blog.Id % 3 == 0 || blog.Id == 5).Select(blog => blog.Id).ToArray();

            //Assert
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        //MORE EXAMPLES IN AdhocLinq !!!
    }
}
