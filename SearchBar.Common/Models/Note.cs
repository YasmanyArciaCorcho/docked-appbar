using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Note
    {
        public int Id
        { get; set; }

        public string Text
        { get; set; } = "";

        public string UpdateDate
        { get; set; }

        public Note()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is Note other)
                return Id == other.Id;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
