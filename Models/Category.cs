using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeAppApi.Models
{
    public partial class Category
    {
        public Category()
        {
            Articles = new HashSet<Article>();
        }

        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryImageName { get; set; }
        public DateTime? CategoryPublishDate { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
