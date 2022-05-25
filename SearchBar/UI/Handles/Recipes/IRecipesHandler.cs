using SearchBar.UI.Base;
using Services.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Handles.Recipes
{
    public interface IRecipesHandler: IControlHandler
    {
        IRecipesService RecipesService { get; set; }
    }

    public interface IRecipesHandler<T> : IRecipesHandler, IDashboardHandler<T> where T : IDashboard
    {

    }
}
