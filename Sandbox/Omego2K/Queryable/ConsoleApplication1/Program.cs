using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestLinq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryableTerraServerData<Place> terraPlaces = new QueryableTerraServerData<Place>();

            var query = from place in terraPlaces
                        where place.Name == "Johannesburg"
                        select place.PlaceType;

            foreach (PlaceType placeType in query)
                Console.WriteLine(placeType);
        }
    }
}
