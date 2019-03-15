﻿using System;
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
            var cars = ProcessFile("fuel.csv");

            var query = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);

            var anon = new
            {
                Name ="Jon"
            };

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



            foreach(var car in query.Take(10))
            {
                Console.WriteLine($"{car.Name}: {car.Combined}");
            }

            Console.ReadLine();
        }

        private static List<Car> ProcessFile(string path)
        {
            /*
            return File.ReadAllLines(path) //Returns string array
                        .Skip(1) //Skip the headers
                        .Where(line => line.Length > 1) //there is an empty line at the end of the file we want to ignore
                        .Select(l => Car.ParseFromCsv(l))


            */

            var query = File.ReadAllLines(path) //Returns string array
                .Skip(1) //Skip the headers
                .Where(line => line.Length > 1) 
                .ToCar();
            //.Select(Car.ParseFromCsv).ToList();

            return query.ToList();

            /*
   
             //Query Syntax Equivalant
             var query =
                from line in File.ReadAllLines(path).Skip(1)
                where line.Length > 1
                select Car.ParseFromCsv(line)

            return query.ToList();
             
             */
        }


    }
}
