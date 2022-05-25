using Common;
using Common.ExtensionMethods;
using Common.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services.Recipes
{
    public class RecipesService : IRecipesService
    {
        public async Task<List<INewsFormat>> GetRecipes()
        {
            RecipesModelAPI recipesModelAPI = new RecipesModelAPI();
            try
            {
                string queryResult = await HTTPRequestHelper.DoAsyncQuery("https://totalrecipesnetwork.com/feed/");
    
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Rss));

                using var stringReader = new StringReader(queryResult);
                recipesModelAPI.Rss = (Rss)ser.Deserialize(stringReader);
            }
            catch (Exception e)
            {
                StaticLogger.Logger.Error(e);
            }

            List<INewsFormat> result = new List<INewsFormat>();

            if (recipesModelAPI.Rss?.Channel != null)
            {
                for (int i = 0; i < recipesModelAPI.Rss.Channel.Item.Count; i++)
                {
                    var currentRecipe = recipesModelAPI.Rss.Channel.Item[i];

                    result.Add(new Recipe()
                    {
                        Title = currentRecipe.Title.ShortString(25) ,
                        Body = currentRecipe.Description.ShortString(25),
                        ImagePath = currentRecipe.Image.Url,
                        Url = currentRecipe.Link2,
                    });
                }
            }

            return result;
        }
    }
}
