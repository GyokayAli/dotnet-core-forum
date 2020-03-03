using Forum.Data.Models;
using System;

namespace Forum.Service.Common
{
    public class Helpers
    {

        /// <summary>
        /// Calculate the user rating.
        /// </summary>
        /// <param name="type">The type of action.</param>
        /// <param name="userRating">The current user rating.</param>
        /// <returns></returns>
        public static int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;

            if (type == typeof(Post))
                inc = 1;

            if (type == typeof(PostReply))
                inc = 3;

            return userRating + inc;
        }
    }
}
