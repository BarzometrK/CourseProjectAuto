using System;

namespace Course_project
{
    abstract class Info
    {
        public int CarID { get; set; }
    }

    class InfoCars : Info, IComparable
    {
        public string Model { get; set; }
        public string BodyType { get; set; }
        public double Capacity { get; set; }
        public int Power { get; set; }
        public int ProductionYear { get; set; }
        public int Price { get; set; }
        public int CompareTo(object obj)
        {
            if (obj is InfoCars car) return CarID.CompareTo(car.CarID);
            else throw new ArgumentException();
        }

        public InfoCars(string model, string bodyType, double capacity, int power, int productionYear, int price, int carID)
        {
            Model = model;
            BodyType = bodyType;
            Capacity = capacity;
            Power = power;
            ProductionYear = productionYear;
            Price = price;
            CarID = carID;
        }
    }

    class InfoClients : Info
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Date { get; set; }
        public string Phone { get; set; }

        public InfoClients(string firstName, string lastName, string date, string phone, int carID)
        {
            FirstName = firstName;
            LastName = lastName;
            Date = date;
            Phone = phone;
            CarID = carID;
        }
    }
}
