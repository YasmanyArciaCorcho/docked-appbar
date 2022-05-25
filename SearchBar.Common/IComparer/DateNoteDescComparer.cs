using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IComparer
{
    public class DateNoteDescComparer : IComparer<Note>
    {
        public int Compare(Note x, Note y)
        {
            if (string.IsNullOrEmpty(y.UpdateDate))
                return -1;
            if (string.IsNullOrEmpty(x.UpdateDate))
                return 1;

            DateTime xDate = Convert.ToDateTime(x.UpdateDate);
            DateTime yTime = Convert.ToDateTime(y.UpdateDate);

            return yTime.CompareTo(xDate);
        }
    }
}
