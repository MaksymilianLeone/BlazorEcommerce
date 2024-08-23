using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Shared
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ArticleDate { get; set; } = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        public bool Featured { get; set; }
        public bool Visible { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public bool Editing { get; set; } = false;
        public bool IsNew { get; set; }
    }
}
