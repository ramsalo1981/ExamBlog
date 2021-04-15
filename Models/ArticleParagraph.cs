using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeAppApi.Models
{
    public partial class ArticleParagraph
    {
        public int ArticleParagraphId { get; set; }
        public string ArticleParagraphTitle { get; set; }
        public string ArticleParagraphImageName { get; set; }
        public string Content { get; set; }
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}
