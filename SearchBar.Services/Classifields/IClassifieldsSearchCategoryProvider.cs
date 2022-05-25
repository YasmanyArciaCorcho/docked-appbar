using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classifields
{
    public interface IClassifieldsSearchCategoryProvider
    {
        public IEnumerable<ClassifieldsCategory> GetClassifieldsCategories();
    }
}
