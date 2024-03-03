using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using EmployeeDirectory.Classes;
using System.Diagnostics;

namespace EmployeeDirectory
{
    internal class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Specify operating mode.");
                return;
            }

            int mode;
            if (!int.TryParse(args[0], out mode))
            {
                Console.WriteLine("Wrong operating mode.");
                return;
            }

            var connectionString = AppsettingsParser.GetConnectionString();

            switch (mode)
            {
                case 1:
                    if (SqlCommander.IsTableExist(connectionString))
                    {
                        Console.WriteLine("Table already exists.");
                        break;
                    }

                    SqlCommander.CreateEmployeeTable(connectionString);
                    Console.WriteLine("Table created successfully.");
                    break;

                case 2:
                    {
                        if (!SqlCommander.IsTableExist(connectionString))
                        {
                            Console.WriteLine("Table does not exist.");
                            break;
                        }


                        if (args.Length != 4)
                        {
                            Console.WriteLine("Some entity properties are missing.");
                            break;
                        }
                        string fullName = args[1];
                        DateTime birthDate;
                        if (!DateTime.TryParse(args[2], out birthDate))
                        {
                            Console.WriteLine("Invalid date format. Please provide a valid date in the format yyyy-mm-dd.");
                            break;
                        }
                        string gender = args[3];


                        var employee = new Employee(fullName, birthDate, gender);
                        employee.SendToDatabase();
                        break;
                    }

                case 3:
                    {
                        if (!SqlCommander.IsTableExist(connectionString))
                        {
                            Console.WriteLine("Table does not exist.");
                            break;
                        }
                        List<Employee> uniqueEmployeesList = SqlCommander.GetUniqueEmployeesSortedByName(connectionString);
                        foreach (var employee in uniqueEmployeesList)
                        {
                            Console.WriteLine(employee.FullName + " " + employee.BirthDate.ToString("dd/MM/yyyy")
                                + " " + employee.Gender + " " + employee.CalculateAge());
                        }
                        break;
                    }

                case 4:
                    {
                        if (!SqlCommander.IsTableExist(connectionString))
                        {
                            Console.WriteLine("Table does not exist.");
                            break;
                        }

                        var dataGenerator = new DataGenerator();
                        List<Employee> listOfEmployees = new List<Employee>();
                        bool gender;
                        Random random = new Random();

                        for (int i = 0; i < 1000000; i++)
                        {
                            gender = random.Next(2) == 0 ? true : false;

                            Employee employee = new Employee(dataGenerator.GenerateFullName(gender),
                                dataGenerator.GenerateBirthDate(),
                                gender ? "Male" : "Female");

                            listOfEmployees.Add(employee);
                        }

                        for (int i = 0; i < 100; i++)
                        {
                            Employee employee = new Employee(dataGenerator.GenerateFullNameOnlyF(true),
                                dataGenerator.GenerateBirthDate(),
                                "Male");
                            listOfEmployees.Add(employee);
                        }

                        Employee.SendDataToDatabase(listOfEmployees);

                        break;
                    }

                case 5:
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        if (!SqlCommander.IsTableExist(connectionString))
                        {
                            Console.WriteLine("Table does not exist.");
                            break;
                        }
                        List<Employee> employeesList = SqlCommander.FindEmployees(connectionString, "F", true);
                        stopwatch.Stop();
                        foreach (var employee in employeesList)
                        {
                            Console.WriteLine(employee.FullName + " " + employee.BirthDate.ToString("dd/MM/yyyy")
                                + " " + employee.Gender + " " + employee.CalculateAge());
                        }
                        Console.WriteLine(employeesList.Count + " employees were found.");
                        Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");
                        break;
                    }

                case 6:
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();

                        if (!SqlCommander.IsTableExist(connectionString))
                        {
                            Console.WriteLine("Table does not exist.");
                            break;
                        }
                        List<Employee> employeesList = SqlCommander.FindEmployeesUpd(connectionString, "F", true);
                        stopwatch.Stop();
                        foreach (var employee in employeesList)
                        {
                            Console.WriteLine(employee.FullName + " " + employee.BirthDate.ToString("dd/MM/yyyy")
                                + " " + employee.Gender + " " + employee.CalculateAge());
                        }
                        Console.WriteLine(employeesList.Count + " employees were found.");
                        Console.WriteLine($"Time: {stopwatch.ElapsedMilliseconds} ms");
                        break;
                    }
                default:
                    Console.WriteLine("This operation mode does not exist. Type 1 - 6.");
                    break;
            }
        }



    }
}