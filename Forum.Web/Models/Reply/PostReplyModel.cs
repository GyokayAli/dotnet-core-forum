using Forum.Web.Models.Base;

namespace Forum.Web.Models.Reply
{
    public class PostReplyModel : PostBase
    {
        public string ReplyContent { get; set; }

        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
        public int ForumId { get; set; }
    }
}
