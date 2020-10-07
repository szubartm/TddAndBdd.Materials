using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using NUnit.Framework;

namespace UnitTesting.Databases
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        private sealed class BlogRelationalComparer : IComparer<Blog>
        {
            public int Compare(Blog x, Blog y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (y is null) return 1;
                if (x is null) return -1;
                var blogIdComparison = x.Id.CompareTo(y.Id);
                if (blogIdComparison != 0) return blogIdComparison;
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }
        }

        public static IComparer<Blog> BlogComparer { get; } = new BlogRelationalComparer();

        public override string ToString() => $"[{Id}=>{Name}] #Posts: {Posts.Count}";
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public virtual Blog Blog { get; set; }
        public int BlogId => Blog.Id;

        public DateTime PostDate { get; set; }

        public int NumberOfReads { get; set; }

        private sealed class PostRelationalComparer : IComparer<Post>
        {
            public int Compare(Post x, Post y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (y is null) return 1;
                if (x is null) return -1;
                var postIdComparison = x.PostId.CompareTo(y.PostId);
                if (postIdComparison != 0) return postIdComparison;
                var titleComparison = string.Compare(x.Title, y.Title, StringComparison.Ordinal);
                if (titleComparison != 0) return titleComparison;
                var contentComparison = string.Compare(x.Content, y.Content, StringComparison.Ordinal);
                if (contentComparison != 0) return contentComparison;
                var postDateComparison = x.PostDate.CompareTo(y.PostDate);
                if (postDateComparison != 0) return postDateComparison;
                return x.NumberOfReads.CompareTo(y.NumberOfReads);
            }
        }

        public static IComparer<Post> PostComparer { get; } = new PostRelationalComparer();

        public override string ToString() => $"[{PostId}=>'{Title}'] @Blog {BlogId}, Posted: {PostDate:o}, read {NumberOfReads}";
    }

    public class TestContext
    {
        public SqlConnection Connection;
        public List<Blog> Blogs;
        public List<Post> Posts;

        public TestContext()
        {
            Blogs = new List<Blog>();
            Posts = new List<Post>();
            string connectionString = @"Data Source=(LocalDb)\TddDemoDb;Initial Catalog=Tdd;Integrated Security=SSPI";
            Connection = new SqlConnection(connectionString);
        }

        public void CleanUp()
        {
            Connection.Execute($"DELETE FROM BlogPosts");
            Connection.Execute($"DELETE FROM Posts");
            Connection.Execute($"DELETE FROM Blogs");
        }
    }
    /// <summary>
    /// Summary description for EntitiesTests
    /// </summary>
    [TestFixture]
    public class DapperEntitiesTests
    {

        #region Entities Test Support
        public static void Initialize(TestContext context, int blogCount = 25, int postCount = 10)
        {
            var rnd = new Random(1);

            for (int i = 1; i <= blogCount; i++)
            {
                var blog = new Blog { Id = i, Name = $"Blog{i}" };

                context.Blogs.Add(blog);
                context.Connection.Execute($"INSERT INTO Blogs (Id, Name) VALUES ({i}, '{blog.Name}')");

                var posts = new List<Post>();
                for (int j = 1; j <= postCount; j++)
                {
                    var post = new Post
                    {
                        PostId = j,
                        Blog = blog,
                        Content = "My Content",
                        NumberOfReads = rnd.Next(0, 5000),
                        PostDate = DateTime.Today.AddDays(-rnd.Next(0, 100)).AddSeconds(rnd.Next(0, 30000)),
                        Title = $"Blog {blog.Id} - Post {j}",
                    };
                    posts.Add(post);
                    context.Posts.Add(post);
                    context.Connection.Execute($"INSERT INTO Posts (Id, Content, Title) VALUES ({j}, '{post.Content}', '{post.Title}')");
                    context.Connection.Execute($"INSERT INTO BlogPosts (BlogId, PostId) VALUES ({i}, {j})");
                }

                blog.Posts = posts;
            }

        }

        public static void DisposeContext(ref TestContext context)
        {
            context.CleanUp();
            context = null;
        }

        #endregion

        private TestContext _context;

        [OneTimeSetUp]
        public void BeforeEverything() => Initialize(_context = new TestContext(), 5, 15);

        [OneTimeTearDown]
        public void AfterEverything() => DisposeContext(ref _context);


        #region Select Tests

        [Test]
        public void Entities_Where_SimplePredicate()
        {
            //Arrange
            Blog[] expected = _context.Blogs.Where(blog => blog.Id % 3 == 0 || blog.Id == 5).ToArray();

            //Act
            string query = "SELECT * FROM Blogs WHERE Id % 3 = 0 OR Id = 5";
            Blog[] actual = _context.Connection.Query<Blog>(query).ToArray();

            //Assert
            Assert.That(actual, Is.EquivalentTo(expected).Using(Blog.BlogComparer));
        }

        [Test]
        public void Entities_Select_SingleColumn()
        {
            //Arrange
            int[] expected = _context.Blogs.Select(x => x.Id).ToArray();

            //Act
            string query = "SELECT Id FROM Blogs";
            int[] actual = _context.Connection.Query<int>(query).ToArray();

            //Assert
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        [Test]
        public void Entities_Select_MultipleColumn()
        {
            //Arrange
            var expected = _context.Blogs.Select(blog => new { BlogId = blog.Id, Name = blog.Name }).ToArray();

            //Act

            string query = "SELECT Id, Name FROM Blogs";
            var actual = _context.Connection.Query(query).Select(blog => new { BlogId = (int)blog.Id, Name = (string)blog.Name }).ToArray();

            //Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Entities_Select_BlogPosts()
        {
            //Arrange
            int[] expected = _context.Blogs.Where(blog => 2 <= blog.Id && blog.Id <= 3).SelectMany(blog => blog.Posts).Select(post => post.PostId).ToArray();

            //Act
            string query = "SELECT PostId FROM BlogPosts WHERE BlogId IN (2,3)";
            int[] actual = _context.Connection.Query<int>(query).ToArray();

            //Assert
            Assert.That(actual, Is.EquivalentTo(expected));
        }

        #endregion
    }
}