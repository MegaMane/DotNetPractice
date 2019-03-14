using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace TupleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var myToople = Tuple.Create(1,"two", 3.4, new {Name="Jon", Age=36 });

            Console.WriteLine(myToople.Item1);
            Console.WriteLine(myToople.Item4.Name);
            

            var results = returnTuple();
            var namedResults = returnNamedTuple();

            Console.WriteLine(results.Item4);
            Console.WriteLine(namedResults.person.name);

            Console.ReadKey();

        }

        static Tuple<string, string, string, object> returnTuple()
        {
            var anotherTuple = Tuple.Create("One", "two", "Buckle my shoe", (object)new { Name = "Amy", Age = 31 });
            return anotherTuple;
        }

        static (string one, string two, string three, (string name, int age) person) returnNamedTuple()
        {
            return (one:"One", two:"two", three:"Buckle my shoe", person:( name:"Amy", age: 31) );
            
        }

        static public (string name, int age) GetStudentInfo(string id)
        {
            // Search by ID and find the student.
            return (name: "Annie", age: 25);
        }
    }
}
