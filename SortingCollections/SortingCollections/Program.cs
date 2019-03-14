using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingCollections
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Animal> animals = new List<Animal>
            {
                new Animal { Name="Lucy", Color="White", Age=3},
                new Animal { Name="Tiger", Color="Brown", Age=9},
                new Animal { Name="Chubbs", Color="Tan", Age=7},
            };

            Console.WriteLine("Default Order");

            foreach(var animal in animals)
            {
                Console.WriteLine($"{animal.Name} is {animal.Color} and {animal.Age} years old.");
            }
            Console.Write("\n\n");

            Console.WriteLine("Name Order");
            Comparison<Animal> sorter = new Comparison<Animal>(AnimalDelegates.CompareName);
            animals.Sort(sorter);

            foreach (var animal in animals)
            {
                Console.WriteLine($"{animal.Name} is {animal.Color} and {animal.Age} years old.");
            }
            Console.Write("\n\n");

            Console.WriteLine("Age Order");
            sorter = new Comparison<Animal>(AnimalDelegates.CompareAge);
            animals.Sort(sorter);

            foreach (var animal in animals)
            {
                Console.WriteLine($"{animal.Name} is {animal.Color} and {animal.Age} years old.");
            }
            Console.Write("\n\n");

            Console.WriteLine("By Color Using LINQ");
         
            foreach (var animal in animals.OrderBy(a => a.Color.ToLower()).ToList())
            {
                Console.WriteLine($"{animal.Name} is {animal.Color} and {animal.Age} years old.");
            }
            Console.Write("\n\n");



            Console.ReadKey();
        }
    }
}
