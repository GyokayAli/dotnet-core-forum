using Forum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data
{
    public interface IForum
    {
        ForumEntity GetById(int id);
        IEnumerable<ForumEntity> GetAll();
        IEnumerable<ApplicationUser> GetActiveUsers(int id);

        Task Create(ForumEntity forum);
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
        bool HasRecentPost(int id);
    }
}
