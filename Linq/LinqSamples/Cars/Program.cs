using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessCars("fuel.csv");

            var manufacturers = ProcessManufacturers("manufacturers.csv");

            var query =
                from car in cars
                group car by car.Manufacturer;

            foreach (var group in query)
            {
                Console.WriteLine(group.Key);
                foreach (var car in group.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name}: {car.Combined}");
                }
            }
            Console.ReadLine();




            var queryJoin =
                from car in cars
                join manufacturer in manufacturers
                on car.Manufacturer equals manufacturer.Name
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    manufacturer.Headquarters,
                    car.Name,
                    car.Combined
                };

            var queryMultipleJoinConditions =
                from car in cars
                join manufacturer in manufacturers
                on new { car.Manufacturer , car.Year} equals 
                new {Manufacturer = manufacturer.Name, manufacturer.Year }
                orderby car.Combined descending, car.Name ascending
                select new
                {
                    manufacturer.Headquarters,
                    car.Name,
                    car.Combined
                };

            //use the query syntax when doing joins this is fucking atrocious
            var queryMethodSyntax =
                cars.Join(manufacturers, //Inner sequence shold be the smaller of the two
                          car => car.Manufacturer, //outer sequence (in this case cars) join condition
                          manufacturer => manufacturer.Name, //inner sequence join condition
                          (car, manufacturer) => new //projection of new combined object returned (basically the select)
                          {
                              manufacturer.Headquarters,
                              car.Name,
                              car.Combined
                          })
                          .OrderByDescending(c => c.Combined)
                          .ThenBy(c => c.Name);

            var query4 = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);
           
            //query syntax equivalant
            var query1 =
                from car in cars
                where car.Manufacturer == "BMW"
                && car.Year == 2016
                orderby car.Combined descending, car.Name ascending
                select car;

            var query2 =
               from car in cars
               where car.Manufacturer == "BMW"
               && car.Year == 2016
               orderby car.Combined descending, car.Name ascending
               select new
               {
                   Manufacturer = car.Manufacturer,
                   Name = car.Name,
                   Combined = car.Combined
               };

            var top =
                cars.Where(c => c.Manufacturer == "BMW" && c.Year == 2016)
                    .OrderByDescending(c => c.Combined)
                    .ThenBy(c => c.Name)
                    .Select(c => new { c.Manufacturer, c.Name, c.Combined })
                    .First();



            foreach(var car in queryMethodSyntax.Take(10))
            {
                Console.WriteLine($"{car.Headquarters}: {car.Name}: {car.Combined}");
            }

            Console.ReadLine();
        }

        private static List<Car> ProcessCars(string path)
        {
            /*
            return File.ReadAllLines(path) //Returns string array
                        .Skip(1) //Skip the headers
                        .Where(line => line.Length > 1) //there is an empty line at the end of the file we want to ignore
                        .Select(l => Car.ParseFromCsv(l)).ToList();
                        //two different ways of making the same call
                        //.Select(Car.ParseFromCsv).ToList();


   
             //Query Syntax Equivalant
             var query =
                from line in File.ReadAllLines(path).Skip(1)
                where line.Length > 1
                select Car.ParseFromCsv(line)

            return query.ToList();
             
    

            */


            var query = File.ReadAllLines(path)
                .Skip(1)
                .Where(line => line.Length > 1)
                .ToCar(); //define an extension method for Ienumarable of String that converts the filtered string output to a car object


            return query.ToList(); 


        }

        private static List<Manufacturer> ProcessManufacturers(string path)
        {
            var query = File.ReadAllLines(path)
                            .Where(line => line.Length > 1)
                            .Select(line => 
                            {
                                var columns = line.Split(',');
                                return new Manufacturer()
                                {
                                    Name = columns[0],
                                    Headquarters = columns[1],
                                    Year = int.Parse(columns[2])
                                };
                            });

            return query.ToList();
        }

    }
}
