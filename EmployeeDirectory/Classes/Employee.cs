using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmployeeDirectory.Classes
{
    public class Employee
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public Employee(string fullName, DateTime birthDate, string gender)
        {
            FullName = fullName;
            BirthDate = birthDate;
            Gender = gender;
        }

        public void SendToDatabase()
        {
            var connectionString = AppsettingsParser.GetConnectionString();

            if (!SqlCommander.IsTableExist(connectionString))
            {
                Console.WriteLine("Table does not exist.");
                return;
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Employees (FullName, BirthDate, Gender) VALUES (@FullName, @BirthDate, @Gender)";

                using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FullName", FullName);
                    command.Parameters.AddWithValue("@BirthDate", BirthDate);
                    command.Parameters.AddWithValue("@Gender", Gender);

                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Employee added to database.");
            }
        }

        public static void SendDataToDatabase(List<Employee> employees)
        {
            var connectionString = AppsettingsParser.GetConnectionString();

            if (!SqlCommander.IsTableExist(connectionString))
            {
                Console.WriteLine("Table does not exist.");
                return;
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO Employees (FullName, BirthDate, Gender) VALUES (@FullName, @BirthDate, @Gender)";

                foreach (var employee in employees)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@FullName", employee.FullName);
                            command.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                            command.Parameters.AddWithValue("@Gender", employee.Gender);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                        }

                    }
                }

                Console.WriteLine("Employees added to database.");
            }
        }


        public int CalculateAge()
        {
            int age = DateTime.Now.Year - BirthDate.Year;

            if (DateTime.Now.Month < BirthDate.Month || (DateTime.Now.Month == BirthDate.Month
                && DateTime.Now.Day < BirthDate.Day))
            {
                age--;
            }

            return age;
        }
    }
}
