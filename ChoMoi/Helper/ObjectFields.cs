using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoMoi.Helper
{
    public class ObjectFields
    {
        public readonly static string BOOK_NAME_INC = "name";
        public readonly static string BOOK_NAME_DESC = "nameDesc";
        public readonly static string BOOK_FILTER_BY_AUTHORID = "authorId";
        public readonly static string BOOK_FILTER_BY_CATEGORYID = "categoryId";
        public readonly static string TITLE = "Title";
        public readonly static string PUBLISHER_ID = "publisherId";
        public readonly static string CATEGORY_ID = "CategoryId";
        public readonly static string ONLINE_ID = "BookBuyOnlineId";
        public readonly static string OFFLINE_ID = "BookBuyOnlineId";

        public static readonly string[] ROLES = { "Admin", "Manager", "Member", "Author" };

    }
}
