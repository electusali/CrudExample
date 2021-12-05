using System;
using System.Collections.Generic;

#nullable disable

namespace CrudExampleProject.Models
{
    public partial class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
