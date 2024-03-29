﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EmployeeDirectory.Classes
{
    public static class SqlCommander
    {
        public static bool CreateEmployeeTable(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Employees (
                        Id bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ) PRIMARY KEY,
                        FullName character varying(100) NOT NULL,
                        BirthDate timestamp with time zone NOT NULL,
                        Gender character varying(10) NOT NULL)";


                if (IsTableExist(connectionString))
                {
                    return false;
                }

                using (var command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();

                return true;
            }
        }

        public static bool IsTableExist(string connectionString)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string isTableExistQuery = @"SELECT EXISTS (SELECT FROM pg_tables
                    WHERE 
                    schemaname = 'public' AND 
                    tablename  = 'employees')";


                using (var command = new NpgsqlCommand(isTableExistQuery, connection))
                {
                    if ((bool)command.ExecuteScalar() == true)
                    {
                        return true;
                    }
                    return false;
                }

            }
        }

        public static List<Employee> GetUniqueEmployeesSortedByName(string connectionString)
        {
            if (!IsTableExist(connectionString))
            {
                return null;
            }

            HashSet<Tuple<string, DateTime>> uniquePairs = new HashSet<Tuple<string, DateTime>>();

            List<Employee> uniqueEmployees = new List<Employee>();


            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectAllRowsQuery = @"SELECT * FROM Employees ORDER BY fullName";

                NpgsqlCommand queryCommand = new NpgsqlCommand(selectAllRowsQuery, connection);

                NpgsqlDataReader reader = queryCommand.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = reader["fullName"].ToString();
                    DateTime birthDate = Convert.ToDateTime(reader["birthDate"]);
                    string gender = reader["gender"].ToString();

                    if (uniquePairs.Add(new Tuple<string, DateTime>(fullName, birthDate)))
                    {
                        uniqueEmployees.Add(new Employee(fullName, birthDate, gender));
                    } 
                }

            }
            return uniqueEmployees;
        }

        public static List<Employee> FindEmployees(string connectionString, string startingLetter, bool gender)
        {
            if (!IsTableExist(connectionString))
            {
                return null;
            }

            List<Employee> employees = new List<Employee>();


            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string selectAllRowsQuery = @"SELECT * FROM Employees WHERE Gender = " + (gender? "'Male'":"'Female'") +" AND FullName LIKE '" + startingLetter
                    + "%'";

                NpgsqlCommand queryCommand = new NpgsqlCommand(selectAllRowsQuery, connection);

                NpgsqlDataReader reader = queryCommand.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = reader["fullName"].ToString();
                    DateTime birthDate = Convert.ToDateTime(reader["birthDate"]);
                    Employee employee = new Employee(fullName, birthDate, gender? "Male" : "Female");

                    employees.Add(employee);
                }

            }
            return employees;
        }


        public static List<Employee> FindEmployeesUpd(string connectionString, string startingLetter, bool gender)
        {
            if (!IsTableExist(connectionString))
            {
                return null;
            }

            List<Employee> employees = new List<Employee>();


            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string genderString = gender ? "Male" : "Female";

                NpgsqlCommand queryCommand = new NpgsqlCommand(@"SELECT * FROM Employees WHERE GENDER = @Gender  AND FullName >= @StartingLetter AND FullName < @PreviousLetter", connection);
                queryCommand.Parameters.AddWithValue("@Gender", genderString);
                queryCommand.Parameters.AddWithValue("@StartingLetter", startingLetter + "%");
                queryCommand.Parameters.AddWithValue("@PreviousLetter", (char)((int)startingLetter.ToCharArray()[0] + 1) + "%");

                NpgsqlDataReader reader = queryCommand.ExecuteReader();

                while (reader.Read())
                {
                    string fullName = reader["fullName"].ToString();
                    DateTime birthDate = Convert.ToDateTime(reader["birthDate"]);
                    Employee employee = new Employee(fullName, birthDate, gender ? "Male" : "Female");

                    employees.Add(employee);
                }

            }
            return employees;
        }
    }
}
