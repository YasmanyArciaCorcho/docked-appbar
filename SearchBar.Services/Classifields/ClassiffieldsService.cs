using Services.Location.Cities;
using Services.Location.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Classifields
{
    public class ClassifieldsService : IClassifieldsService
    {
        readonly IStateProvider _stateProvider;
        readonly ICitiesProvider _citiesProvider;
        readonly IClassifieldsSearchCategoryProvider _classifieldsSearchCategoryService;

        public ClassifieldsService(IStateProvider stateProvider, ICitiesProvider citiesProvider, IClassifieldsSearchCategoryProvider classifieldsSearchCategoryService)
        {
            _stateProvider = stateProvider;
            _citiesProvider = citiesProvider;
            _classifieldsSearchCategoryService = classifieldsSearchCategoryService;
        }

        public IEnumerable<City> GetCities(string stateCode)
         => _citiesProvider.GetCities(stateCode);

        public IEnumerable<State> GetStates()
        => _stateProvider.GetStates();

        public IEnumerable<ClassifieldsCategory> GetSearchCategory()
        {
            return _classifieldsSearchCategoryService.GetClassifieldsCategories();
        }
    }
}
