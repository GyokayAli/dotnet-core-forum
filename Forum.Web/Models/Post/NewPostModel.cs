namespace Forum.Web.Models.Post
{
    public class NewPostModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ForumName { get; set; }
        public int ForumId { get; set; }
        public string AuthorName { get; set; }
        public string ForumImageUrl { get; set; }
    }
}
