using Forum.Web.Models.Base;
using Forum.Web.Models.Reply;
using System.Collections.Generic;

namespace Forum.Web.Models.Post
{
    public class PostIndexModel : PostBase
    { 
        public string Title { get; set; }
        public string PostContent { get; set; }

        public IEnumerable<PostReplyModel> Replies { get; set; }
    }
}
