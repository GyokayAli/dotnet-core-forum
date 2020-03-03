using System;

namespace Forum.Web.Models.Base
{
    public class PostBase
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        public bool IsAuthorAdmin { get; set; }
        public DateTime Created { get; set; }
    }
}
