using Forum.Data;
using Forum.Data.Models;
using Forum.Service;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace Forum.Tests.Services
{
    [TestFixture]
    public class PostServiceShould
    {
        [TestCase("coFFee", 3)]
        [TestCase("teA", 1)]
        [TestCase("water", 0)]
        [TestCase(null, 3)]
        public void ReturnResultsCorrespondingToQuery(string query, int expected)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            // Arrange
            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Forums.Add(new ForumEntity
                {
                    Id = 19
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 1234,
                    Title = "First Posts",
                    Content = "Coffee time"
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = -3214,
                    Title = "Coffee",
                    Content = "Some content"
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 32523,
                    Title = "Tea",
                    Content = "Coffee"
                });

                ctx.SaveChanges();
            }

            // Act
            using (var ctx = new ApplicationDbContext(options))
            {
                var postService = new PostService(ctx);
                var result = postService.GetFilteredPosts(query);

                var postCount = result.Count();

                // Assert
                Assert.AreEqual(expected, postCount);
            }
        }
    }
}
