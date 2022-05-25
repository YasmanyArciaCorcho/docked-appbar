using Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Recipes
{
    public interface IRecipesService
    {
        Task<List<INewsFormat>> GetRecipes();
    }
}
