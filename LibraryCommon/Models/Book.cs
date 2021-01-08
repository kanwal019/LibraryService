using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Common.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
    }
}
