using Services.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Recipes
{
    public class Recipe : IRecipe
    {
        public string Title
        { get; set; }

        public string ImagePath
        { get; set; }

        public string Body
        { get; set; }

        public string Url
        { get; set; }

        public string Publisher
        { get; set; }
    }
}
