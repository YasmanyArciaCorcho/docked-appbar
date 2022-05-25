using SearchBar.UI.Controls.Dashboad.Classifieds;
using Services.Classifields;
using Services.Location.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Handles.Classifields
{
    public class ClassifieldHandler : IClassifieldHandler<ClassifiedsDashboard>
    {
        public ClassifiedsDashboard Dashboard { get; set; }
        readonly IClassifieldsService _classifieldsService;

        public ClassifieldHandler(IClassifieldsService classifieldsService)
        {
            _classifieldsService = classifieldsService;
        }

        public void UpdateStateList()
        {
            Dashboard.States = new SortedList<string, string>();
            foreach (var state in _classifieldsService.GetStates())
            {
                Dashboard.States.Add(state.Name, state.Code);
            }

            Dashboard.StateComboBox.ItemsSource = Dashboard.States.Keys;

            if (Dashboard.States.Keys.Count > 0)
            {
                Dashboard.StateComboBox.SelectedIndex = 0;
                UpdateCitiesList(Dashboard.States[(string)Dashboard.StateComboBox.SelectedItem]);
            }
        }

        public void UpdateCitiesList(string stateCode)
        {
            Dashboard.Cities = new SortedList<string, string>();
            foreach (var city in _classifieldsService.GetCities(stateCode))
            {
                Dashboard.Cities.Add(city.Name, city.Code);
            }

            Dashboard.LocationComboBox.ItemsSource = Dashboard.Cities.Keys;

            if (Dashboard.Cities.Keys.Count > 0) 
                Dashboard.LocationComboBox.SelectedIndex = 0;
        }

        public void UpdateSearchCategory()
        {
            Dashboard.SearchCategory = new SortedList<string, string>();
            foreach (var category in _classifieldsService.GetSearchCategory())
            {
                Dashboard.SearchCategory.Add(category.Name, category.Code);
            }
            Dashboard.SearchCategoryComboBox.ItemsSource = Dashboard.SearchCategory.Keys;

            if (Dashboard.SearchCategory.Keys.Count > 0) 
                Dashboard.SearchCategoryComboBox.SelectedIndex = 0;
        }
    }
}
