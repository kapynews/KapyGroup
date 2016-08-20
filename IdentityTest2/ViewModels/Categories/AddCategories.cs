using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityTest2.ViewModels.Categories
{
    public class AddCategories
    {

        public List<Models.CheckBoxListItem> Genres { get; set; }

        public AddCategories()
        {
            Genres = new List<Models.CheckBoxListItem>();
        }
    }
}