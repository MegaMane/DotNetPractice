using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CarDb>());
            InsertData();
            QueryData();
        }

        private static void QueryData()
        {
            var db = new CarDb();

            var query = from car in db.Cars
                        orderby car.Combined descending, car.Name ascending
                        select car;

            var queryExt =
                db.Cars.OrderByDescending(c => c.Combined)
                       .ThenBy(c => c.Name)
                       .Take(10);

            foreach (var car in queryExt)
            {
                Console.WriteLine($"{car.Name}: {car.Combined}");
            }

            Console.ReadLine();
        }

        private static void InsertData()
        {

            var cars = ProcessCars("fuel.csv");
            var db = new CarDb();

            var currentRows = db.Cars.Count();

            db.Database.Log = Console.WriteLine;

            if (!db.Cars.Any())
            {
                foreach (var car in cars)
                {
                    db.Cars.Add(car);
                }

                var rowsInserted = db.Cars.Count();
                db.SaveChanges();
            }

            
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
