using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Common.Models
{
    public class AddBooksInput
    {
        public string BookName { get; set; }
        public string BookAuthor { get; set; }
        public bool IsAvailable { get; set; }
    }
}
