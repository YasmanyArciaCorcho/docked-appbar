using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Location.States;

namespace Services.Location.Cities
{
    public class USACitiesProvider : ICitiesProvider
    {
        static Dictionary<string, City[]> _cities;

        public USACitiesProvider()
        {
            _cities = new Dictionary<string, City[]>();

            _cities.Add("AL", new City[]
            {
                new City("auburn", "Auburn"),
                new City("bham", "Birmingham"),
                new City("dothan", "Dothan"),
                new City("shoals", "Florence / Muscle Shoals"),
                new City("gadsden", "Gadsden-Anniston"),
                new City("huntsville", "Huntsville"),
                new City("mobile", "Mobile"),
                new City("montgomery", "Montgomery"),
                new City("tuscaloosa", "Tuscaloosa")
            });

            _cities.Add("AK", new City[]
            {
                new City("anchorage", "Anchorage"),
                new City("fairbanks", "Fairbanks"),
                new City("kenai", "Kenai Peninsula"),
                new City("juneau", "Juneau"),
             });

            _cities.Add("AZ", new City[]
            {
                new City("flagstaff", "Flagstaff / Sedona"),
                new City("mohave", "Mohave County"),
                new City("phoenix", "Phoenix"),
                new City("prescott", "Prescott"),
                new City("showlow", "Show Low"),
                new City("sierravista", "Sierra Vista"),
                new City("tucson", "Tucson"),
                new City("yuma", "Yuma"),
            });

            _cities.Add("AR", new City[]
           {
                new City("fayar", "Fayetteville"),
                new City("fortsmith", "Fort Smith"),
                new City("jonesboro", "Jonesboro"),
                new City("littlerock", "Little Rock"),
                new City("memphis", "Memphis"),
                new City("texarkana", "Texarkana"),
           });

            _cities.Add("CA", new City[]
            {
                new City("bakersfield", "Bakersfield"),
                new City("chico", "Chico"),
                new City("fresno", "Fresno / Madera"),
                new City("goldcountry", "Gold Country"),
                new City("hanford", "Hanford-Corcoran"),
                new City("humboldt", "Humboldt County"),
                new City("imperial", "Imperial County"),
                new City("inlandempire", "Inland Empire"),
                new City("losangeles", "Los Angeles"),
                new City("mendocino", "Mendocino County"),
                new City("merced", "Merced"),
                new City("modesto", "Modesto"),
                new City("monterey", "Monterey Bay"),
                new City("orangecounty", "Orange County"),
                new City("palmsprings", "Palm Springs"),
                new City("redding", "Redding"),
                new City("reno", "Reno / Tahoe"),
                new City("sacramento", "Sacramento"),
                new City("sandiego", "San Diego"),
                new City("slo", "San Luis Obispo"),
                new City("santabarbara", "Santa Barbara"),
                new City("santamaria", "Santa Maria"),
                new City("sfbay", "SF Bay Area"),
                new City("siskiyou", "Siskiyou county"),
                new City("stockton", "SanStockton"),
                new City("susanville", "Ventura County"),
                new City("visalia", "Visalia-Tulare"),
                new City("yubasutter", "Yuba-Sutter"),
            });

            _cities.Add("CO", new City[]
            {
                new City("boulder", "Boulder"),
                new City("cosprings", "Colorado Springs"),
                new City("denver", "Denver"),
                new City("fortcollins", "Fort Collins / North CO"),
                new City("rockies", "High Rockies"),
                new City("pueblo", "Pueblo"),
                new City("westslope", "Western Slope"),
            });

            _cities.Add("CT", new City[]
             {
                 new City("newlondon","Eastern CT"),
                 new City("hartford","Hartford"),
                  new City("newhaven","New Haven"),
                  new City("nwct","Northwest CT"),
             });

            _cities.Add("DE", new City[]
             {
                 new City("delaware","Delaware"),
             });

            _cities.Add("DC", new City[]
            {
                 new City("washingtondc","Washington, DC"),
            });

            _cities.Add("FL", new City[]
            {
                new City("washingtondc","Washington, DC"),
                 new City("daytona","Daytona Beach"),
                new City("keys","Florida Keys"),
                new City("fortmyers","Ft Myers / SW Florida"),
                new City("gainsville","Gainsville"),
                new City("cfl","Heartland"),
                new City("jacksonville","Jacksonville"),
                new City("lakeland","Lakeland"),
                new City("lakecity","North Central FL"),
                new City("ocala","Ocala"),
                new City("okaloosa","Okaloosa / Walton"),
                new City("orlando","Orlando"),
                new City("panamacity","Panama City"),
                new City("pensacola","Pensacola"),
                new City("sarasota","Sarasota / Bradenton"),
                new City("miami","South Florida"),
                new City("spacecoast","Space Coast"),
                new City("staugustine","St Augustine"),
                new City("tallahassee","Tallahassee"),
                new City("tampa","Tampa Bay Area"),
                new City("treasure","Treasure Coast"),
          });

            _cities.Add("GA", new City[]
            {
                new City("albanyga","Albany"),
                new City("athensga","Athens"),
                new City("atlanta","Atlanta"),
                new City("augusta","Augusta"),
                new City("brunswick","Brunswick"),
                new City("columbusga","Columbus"),
                new City("macon","Macon / Warner Robins"),
                new City("nwga","Northwest GA"),
                new City("savannah","avannah / Hinesville"),
                new City("statesboro","Statesboro"),
                new City("valdosta","Valdosta")
        });

            _cities.Add("HI", new City[]
            {
               new City("honolulu","Hawaii"),
            });

            _cities.Add("ID", new City[]
            {
               new City("boise","Boise"),
               new City("eastidaho","East Idaho"),
               new City("lewiston","Lewiston / Clarkston"),
               new City("twinfalls","Twin Falls"),
            });


            _cities.Add("IL", new City[]
            {
              new City("bn","Bloomington-Normal"),
              new City("chambana","Champaign Urbana"),
              new City("chicago","Chicago"),
              new City("decatur","Decatur"),
              new City("lasalle","La Salle Co"),
              new City("mattoon","Mattoon-Charleston"),
              new City("peoria","Peoria"),
              new City("rockford","Rockford"),
              new City("carbondale","Southern Illinois"),
              new City("springfieldil","Springfield"),
              new City("quincy","Western IL"),
            });

            _cities.Add("IN", new City[]
            {
               new City("bloomington","Bloomington"),
               new City("evansville","Evansville"),
               new City("fortwayne","Fort Wayne"),
               new City("indianapolis","Indianapolis"),
               new City("kokomo","Kokomo"),
               new City("tippecanoe","Lafayette / West Lafayette"),
               new City("muncie","Muncie / Anderson"),
               new City("richmondin","Richmond"),
               new City("southbend","Southbend / Michiana"),
               new City("terrehaute","Terre Haute"),
            });

            _cities.Add("IA", new City[]
           {
                new City("ames","Ames"),
                new City("cedarrapids","Cedar Rapids"),
                new City("desmoines","Des Moines"),
                new City("dubuque","Dubuque"),
                new City("fortdodge","Fort Dodge"),
                new City("iowacity","Iowa City"),
                new City("masoncity","Mason City"),
                new City("quadcities","Quad Cities"),
                new City("siouxcity","Sioux City"),
                new City("ottumwa","Southeast IA"),
                new City("waterloo","Waterloo / Cedar Falls"),
           });

            _cities.Add("KS", new City[]
            {
              new City("lawrence","Lawrence"),
              new City("ksu","Manhatten"),
              new City("nwks","Northweast KS"),
              new City("salina","Salina"),
              new City("seks","Southeast KS"),
              new City("swks","Southwest KS"),
              new City("topeka","Topeka"),
              new City("wichita","Wichita"),
            });

            _cities.Add("KY", new City[]
            {
              new City("bgky","Bowling Green"),
              new City("eastky","Eastern Kentucky"),
              new City("lexington","Lexington"),
              new City("louisville","Louisville"),
              new City("owensboro","Owensboro"),
              new City("westky","Western KY"),
            });

            _cities.Add("LA", new City[]
            {
              new City("batonrouge","Baton Rouge"),
              new City("cenla","Central Louisiana"),
              new City("houma","Houma"),
              new City("lafayette","Lafayette"),
              new City("lakecharles","Lake Charles"),
              new City("monroe","Monroe"),
              new City("neworleans","New Orleans"),
              new City("shreveport","Shreveport"),
            });

            _cities.Add("ME", new City[]
            {
                 new City("maine","Maine"),
            });

            _cities.Add("MD", new City[]
            {
                new City("annapolis","Annapolis"),
                new City("baltimore","Baltimore"),
                new City("easternshore","Eastern Shore"),
                new City("frederick","Frederick"),
                new City("smd","Southern Maryland"),
                new City("westmd","Western Maryland"),
            });

            _cities.Add("MA", new City[]
             {
                new City("boston","Boston"),
                new City("capecod","Cape Cod / Islands"),
                new City("southcoast","South Coast"),
                new City("westernmass","Western Massachusettes"),
                new City("worcester","Worcester / Central MA"),
             });

            _cities.Add("MI", new City[]
            {
                new City("annarbor","Ann Arbor"),
                new City("battlecreek","Battle Creek"),
                new City("centralmich","Central Michigan"),
                new City("detroit","Detroit Metro"),
                new City("flint","Flint"),
                new City("grandrapids","Grand Rapids"),
                new City("holland","Holland"),
                new City("jxn","Jackson"),
                new City("kalamazoo","Kalamazoo"),
                new City("lansing","Lansing"),
                new City("monroemi","Monroe"),
                new City("muskegon","Muskegon"),
                new City("nmi","Northern Michigan"),
                new City("porthuron","Port Huron"),
                new City("saginaw","Saginaw-Midland-Baycity"),
                new City("swmi","Southwest Michigan"),
                new City("thumb","The Thumb"),
                new City("up","Upper Peninsula"),
             });

            _cities.Add("MN", new City[]
            {
               new City("bemidji","Bemidji"),
               new City("brainerd","Brainerd"),
               new City("duluth","Duluth / Superior"),
               new City("mankato","Mankato"),
               new City("minneapolis","Minneapolis / St Paul"),
               new City("rmn","Rochester"),
               new City("marshall","Southwest MN"),
               new City("stcloud","St Cloud"),
            });

            _cities.Add("MS", new City[]
            {
                new City("gulfport","Gulfport / Biloxi"),
                new City("hattiesburg","Hattiesburg"),
                new City("jackson","Jackson"),
                new City("meridian","Meridian"),
                new City("northmiss","North Mississippi"),
                new City("natchez","Southwest MS"),
            });


            _cities.Add("MO", new City[]
            {
                new City("columbiamo","Columbia / Jeff City"),
                new City("joplin","Joplin"),
                new City("kansascity","Kansas City"),
                new City("kirksville","Kirksville"),
                new City("loz","Lake of The Ozarks"),
                new City("semo","Southeast Missouri"),
                new City("springfield","Springfield"),
                new City("stjoseph","St Joseph"),
                new City("stlouis","St Louis"),
            });

            _cities.Add("NE", new City[]
            {
               new City("grandisland","Grand Island"),
               new City("lincoln","Lincoln"),
               new City("northplatte","North Platte"),
               new City("omaha","Omaha / Council Bluffs"),
               new City("scottsbluff","Scottsbluff / Panhandle"),
            });

            _cities.Add("NV", new City[]
           {
              new City("elko","Elko"),
              new City("lasvegas","Las Vegas"),
              new City("reno","Reno / Tahoe"),
           });

            _cities.Add("NJ", new City[]
            {
              new City("cnj","Central NJ"),
              new City("jerseyshore","Jersey Shore"),
              new City("newjersey","North Jersey"),
              new City("southjersey","South Jersey"),
            });

            _cities.Add("NM", new City[]
           {
               new City("albuquerque","Albuquerque"),
               new City("clovis","Clovis / Portales"),
               new City("farmington","Farmington"),
               new City("lascruces","Las Cruces"),
               new City("roswell","Roswell / Carlsbad"),
               new City("santafe","Santa Fe / Taos"),
            });

            _cities.Add("NC", new City[]
            {
                new City("asheville","Asheville"),
                new City("boone","Boone"),
                new City("charlotte","Charlotte"),
                new City("eastnc","Eastern NC"),
                new City("fayetteville","Fayetteville"),
                new City("greensboro","Greensboro"),
                new City("hickory","Hickory / Lenoir"),
                new City("onslow","Jacksonville"),
                new City("outerbanks","Outer Banks"),
                new City("raleigh","Raleigh / Durham / CH"),
                new City("wilmington","Wilmington"),
                new City("winstonsalem","Winston-Salem"),
            });

            _cities.Add("NY", new City[]
           {
                new City("albany","Albany"),
                new City("binghamton","Binghamton"),
                new City("buffalo","Buffalo"),
                new City("catskills","Catskills"),
                new City("chautauqua","Chautauqua"),
                new City("elmira","Elmira-Corning"),
                new City("fingerlakes","Finger Lakes"),
                new City("glensfalls","Glens Falls"),
                new City("hudsonvalley","Hudson Valley"),
                new City("ithaca","Ithaca"),
                new City("longisland","Long Island"),
                new City("newyork","New York City"),
                new City("oneonta","Oneonta"),
                new City("plattsburgh","Plattsburgh-Adirondacks"),
                new City("potsdam","Potsdam-Canton-Massena"),
                new City("rochester","Rochester"),
                new City("syracuse","Syracuse"),
                new City("twintiers","Twin Tiers NY/PA"),
                new City("utica","Utica-Rome-Onieda"),
                new City("watertown","Watertown"),
           });

            _cities.Add("ND", new City[]
            {
                new City("bismarck","Bismarck"),
                new City("fargo","Fargo / Moorhead"),
                new City("grandforks","Grand Forks"),
                new City("nd","North Dakota"),
            });

            _cities.Add("OH", new City[]
           {
                 new City("akroncanton","Akron / Canton"),
                new City("ashtabula","Ashtabula"),
                new City("athensohio","Athens"),
                new City("chillicothe","Chillicothe"),
                new City("cincinnati","Cincinnati"),
                new City("cleveland","Cleveland"),
                new City("columbus","Columbus"),
                new City("dayton","Dayton / Springfield"),
                new City("limaohio","Lima / Findlay"),
                new City("mansfield","Mansfield"),
                new City("sandusky","Sandusky"),
                new City("toledo","Toledo"),
                new City("tuscarawas","Tuscarawas co"),
                new City("youngstown","Youngstown"),
                new City("zanesville","Zanesville / Cambridge"),
           });

            _cities.Add("OK", new City[]
            {
                new City("lawton","Lawton"),
                new City("enid","Northwest OK"),
                new City("oklahomacity","Oklahoma City"),
                new City("stillwater","Stillwater"),
                new City("tulsa","Tulsa"),
            });

            _cities.Add("OR", new City[]
            {
                new City("bend","Bend"),
                new City("corvallis","Corvallis / Albany"),
                new City("eastoregon","East Oregon"),
                new City("eugene","Eugene"),
                new City("klamath","Klamath Falls"),
                new City("medford","Medford-Ashland"),
                new City("oregoncoast","Oregon Coast"),
                new City("portland","Portland"),
                new City("roseburg","Roseburg"),
                new City("salem","Salem"),
            });

            _cities.Add("PA", new City[]
            {
                new City("altoona","Altoona-Johnstown"),
                new City("chambersburg","Cumberland Valley"),
                new City("erie","Erie"),
                new City("harrisburg","Harrisburg"),
                new City("lancaster","Lancaster"),
                new City("allentown","Lehigh Valley"),
                new City("meadville","Meadville"),
                new City("philadelphia","Philadelphia"),
                new City("pittsburgh","Pittsburgh"),
                new City("poconos","Poconos"),
                new City("reading","Reading"),
                new City("scranton","Scranton / Wilkes-barre"),
                new City("pennstate","State College"),
                new City("williamsport","Williamsport"),
                new City("york","York"),
            });

            _cities.Add("SC", new City[]
            {
                new City("charleston","Charleston"),
                new City("columbia","Columbia"),
                new City("florencesc","Florence"),
                new City("greenville","Greenville / Upstate"),
                new City("hiltonhead","Hilton Head"),
                new City("myrtlebeach","Myrtle Beach"),
            });

            _cities.Add("RI", new City[]
            {
                new City("providence","Providence"),
            });

            _cities.Add("SD", new City[]
           {
                new City("nesd","Northeast SD"),
                new City("csd","Pierre / Central SD"),
                new City("rapidcity","Rapid City / West SD"),
                new City("siouxfalls","Sioux Falls / SE SD"),
                new City("sd","South Dakota"),
           });

            _cities.Add("TN", new City[]
          {
                new City("chattanooga","Chattanooga"),
                new City("clarksville","Clarksville"),
                new City("cookeville","Cookeville"),
                new City("jacksontn","Jackson"),
                new City("knoxville","Knoxville"),
                new City("memphis","Memphis"),
                new City("nashville","Nashville"),
                new City("tricities","Tri-Cities"),
          });

            _cities.Add("TX", new City[]
            {
                 new City("abilene","Abilene"),
                 new City("amarillo","Amarillo"),
                 new City("austin","Austin"),
                 new City("beaumont","Beaumont / Port Arthur"),
                 new City("brownsville","Brownsville"),
                 new City("collegestation","College Station"),
                 new City("corpuschristi","Corpus Christi"),
                 new City("dallas","Dallas / Fort Worth"),
                 new City("nacogdoches","Deep East Texas"),
                 new City("delrio","Del Rio / Eagle Pass"),
                 new City("elpaso","El Paso"),
                 new City("galveston","Galveston"),
                 new City("houston","Houston"),
                 new City("killeen","Killeen / Temple / Ft Hood"),
                 new City("laredo","Laredo"),
                 new City("lubbock","Lubbock"),
                 new City("mcallen","Mcallen / Edinburg"),
                 new City("odessa","Odessa / Midland"),
                 new City("sanangelo","San Angelo"),
                 new City("sanantonio","San Antonio"),
                 new City("sanmarcos","San Marcos"),
                 new City("bigbend","Southwest TX"),
                 new City("texoma","Texoma"),
                 new City("easttexas","Tyler / East TX"),
                 new City("victoriatx","Victoria"),
                 new City("waco","Waco"),
                 new City("wichitafalls","Wichita Falls"),
          });

            _cities.Add("UT", new City[]
            {
                new City("logan","Logan"),
                new City("ogden","Ogden-Clearfield"),
                new City("provo","Provo / Orem"),
                new City("saltlakecity","Salt Lake City"),
                new City("stgeorge","St George"),
            });

            _cities.Add("VT", new City[]
           {
                new City("vermont","Vermont"),
           });

            _cities.Add("VA", new City[]
            {
                new City("charlottesville","Charlottesville"),
                new City("danville","Danville"),
                new City("fredericksburg","Fredericksburg"),
                new City("norfolk","Norfolk / Hampton Roads"),
                new City("harrisonburg","Harrisonburg"),
                new City("lynchburg","Lynchburg"),
                new City("blacksburg","New River Valley"),
                new City("richmond","Richmond"),
                new City("roanoke","Roanoke"),
                new City("swva","Southwest VA"),
                new City("winchester","Winchester"),
            });


            _cities.Add("WA", new City[]
           {
                new City("bellingham","Bellingham"),
                new City("kpr","Kennewick-Pasco-Richland"),
                new City("moseslake","Moses Lake"),
                new City("olympic","Olympic Peninsula"),
                new City("pullman","Pullman / Moscow"),
                new City("seattle","Seattle-Tacoma"),
                new City("skagit","Skagit / Island / SJI"),
                new City("spokane","Spokane / Coeur d'alene"),
                new City("wenatchee","Wenatchee"),
                new City("yakima","Yakima"),
           });

            _cities.Add("WV", new City[]
             {
                 new City("charlestonwv","Charleston"),
                 new City("martinsburg","Eastern Panhandle"),
                 new City("huntington","Huntington-Ashland"),
                 new City("morgantown","Morgantown"),
                 new City("wheeling","Northern Panhandle"),
                 new City("parkersburg","Parkersburg-Marietta"),
                 new City("swv","Southern WV"),
                 new City("wv","West Virginia (old)"),
             });

            _cities.Add("WI", new City[]
            {
                 new City("appleton","Appleton-Oshkosk-FDL"),
                 new City("eauclaire","Eau Claire"),
                 new City("greenbay","Green Bay"),
                 new City("janesville","Janesville"),
                 new City("racine","Kenosha-Racine"),
                 new City("lacrosse","La Crosse"),
                 new City("madison","Madison"),
                 new City("milwaukee","Milwaukee"),
                 new City("northernwi","Northern WI"),
                 new City("sheboygan","Sheboygan"),
                 new City("wausau","Wausau"),
            });

            _cities.Add("WY", new City[]
         {
                 new City("wyoming","Wyoming"),
         });
        }

        public City[] GetCities(string stateCode)
        {
            if (_cities.ContainsKey(stateCode))
                return _cities[stateCode];
            return new City[0];
        }
    }
}
