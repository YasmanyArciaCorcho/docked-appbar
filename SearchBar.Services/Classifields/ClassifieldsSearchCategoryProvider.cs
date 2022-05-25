using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classifields
{
    public class ClassifieldsSearchCategoryProvider : IClassifieldsSearchCategoryProvider
    {
        public IEnumerable<ClassifieldsCategory> GetClassifieldsCategories()
        {
            return new ClassifieldsCategory[]
            {
                new ClassifieldsCategory("pet","Pets"),
                new ClassifieldsCategory("act","Activity Partners"),
                new ClassifieldsCategory("laf","Lost + Found"),
                new ClassifieldsCategory("ats","Artists"),
                new ClassifieldsCategory("mis","Missed Connections"),
                new ClassifieldsCategory("kid","Child Care"),
                new ClassifieldsCategory("cls","Classes"),
                new ClassifieldsCategory("muc","Musicians"),
                new ClassifieldsCategory("eve","Events"),
                new ClassifieldsCategory("ccc","General Community"),
                new ClassifieldsCategory("pol","Politics"),
                new ClassifieldsCategory("grp","Groups"),
                new ClassifieldsCategory("rnr","Rants & Raves"),
                new ClassifieldsCategory("rid","Rideshare"),
                new ClassifieldsCategory("vnn","Local News"),
                new ClassifieldsCategory("vol","Volunteers"),
                new ClassifieldsCategory("apa","Apts / Housing"),
                new ClassifieldsCategory("swp","Housing Swap"),
                new ClassifieldsCategory("rew","Housing Wanted"),
                new ClassifieldsCategory("off","Office / Commercial"),
                new ClassifieldsCategory("prk","Parking / Storage"),
                new ClassifieldsCategory("reo","Real Estate For Sale"),
                new ClassifieldsCategory("roo","Rooms / Shared"),
                new ClassifieldsCategory("sha","Rooms Wanted"),
                new ClassifieldsCategory("sub","Sublets / Temporary"),
                new ClassifieldsCategory("aos","Automotive Services"),
                new ClassifieldsCategory("lbs","Labor / Move"),
                new ClassifieldsCategory("bts","Beauty"),
                new ClassifieldsCategory("lgs","Legal"),
            };
        }
    }
}
