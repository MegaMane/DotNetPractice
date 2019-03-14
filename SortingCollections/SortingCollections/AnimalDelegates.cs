using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingCollections
{
    public static class AnimalDelegates
    {
        public static int CompareName(Animal am1, Animal am2)
        {
            return String.Compare(am1.Name, am2.Name, ignoreCase:true);
   
        }

        public static int CompareAge(Animal am1, Animal am2)
        {
            if(am1.Age > am2.Age)
            {
                return 1;
            }

            else if (am1.Age < am2.Age)
            {
                return -1;
            }

            return 0;

        }
    }
}
