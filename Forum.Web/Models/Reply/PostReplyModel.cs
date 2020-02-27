using Forum.Web.Models.Base;

namespace Forum.Web.Models.Reply
{
    public class PostReplyModel : PostBase
    {
        public string ReplyContent { get; set; }
        public int PostId { get; set; }
        public bool IsAuthorAdmin { get; set; }
    }
}
