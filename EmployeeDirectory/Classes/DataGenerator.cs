using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDirectory.Classes
{
    internal class DataGenerator
    {

        public string GenerateFullName(bool gender, string startingLetter = "")
        {
            Random random = new Random();
            string[] firstNamesMale = { "Artem", "Anton", "Aleksei", "Andrei", "Anatoliy", "Artur",
                "Aleksandr", "Albert", "Arkadiy", "Boris", "Bogdan", "Valeriy", "Valentin", "Vladislav"};


            string[] secondNamesMale = { "Avdeev", "Agapov", "Bajenov", "Borisov","Vavilov",
                "Vtorov", "Gavrilov", "Gushin", "Davidov", "Dubov", "Evdokimov", "Efremov",
                "Jarov", "Juravlev", "Zimin", "Zaicev", "Ivanov", "Isakov", "Kazakov",
                "Kurochkin", "Lavrov", "Lubimov", "Maiorov", "Myasnikov", "Naumov", "Nosov",
                "Ovsyannikov", "Ostrovskiy", "Pavlov", "Prohorov","Rakov", "Ryabov", "Savin",
                "Suhov", "Tarasov", "Tumanov", "Uvarov", "Utkin", 
                "Haritonov", "Hudyakov", "Carev", "Cvetkov", "Yakovlev", "Yudin"};

            string[] patronymicMale = { "Ivanovich", "Petrovich", "Alekseevich", "Antovich", "Maximovich",
                "Andreevich", "Borisovich"};


            string[] firstNamesFemale = { "Anna", "Yulya", "Snejanna", "Zoya", "Elena", "Galina",
                "Valentina", "Janna" };

            string[] secondNamesFemale = { "Avdeeva", "Agapova", "Bajenova", "Borisova","Vavilova",
                "Vtorova", "Gavrilova", "Gushina", "Davidova", "Dubova", "Evdokimova", "Efremova",
                "Jarova", "Juravleva", "Zimina", "Zaiceva", "Ivanova", "Isakova", "Kazakova",
                "Kurochkina", "Lavrova", "Lubimova", "Maiorova", "Myasnikova", "Naumova", "Nosova",
                "Ovsyannikova", "Ostrovskiya", "Pavlova", "Prohorova","Rakova", "Ryabova", "Savina",
                "Suhova", "Tarasova", "Tumanova", "Uvarova", "Utkina", 
                "Haritonova", "Hudyakova", "Careva", "Cvetkova", "Yakovleva", "Yudina"};

            string[] patronymicFemale = { "Ivanovna", "Petrovna", "Alekseevna", "Antonovna", "Maximovna",
                "Andreevna", "Borisovna" };

            string firstName, secondName, patronymic;


            if (gender)
            {
                firstName = firstNamesMale[random.Next(firstNamesMale.Length)];

                
                List<string> neededNames = new List<string>();
                if (!String.IsNullOrEmpty(startingLetter))
                {

                    foreach (var name in secondNamesMale)
                    {
                        if (name.StartsWith(startingLetter))
                        {
                            neededNames.Add(name);
                        }
                    }
                }

                if (neededNames.Count != 0)
                {
                    secondName = neededNames[random.Next(neededNames.Count)];
                }
                else
                {
                    secondName = secondNamesMale[random.Next(secondNamesMale.Length)];
                }

                patronymic = patronymicMale[random.Next(patronymicMale.Length)];
            }
            else
            {
                firstName = firstNamesFemale[random.Next(firstNamesFemale.Length)];


                List<string> neededNames = new List<string>();
                if (!String.IsNullOrEmpty(startingLetter))
                {

                    foreach (var name in secondNamesFemale)
                    {
                        if (name.StartsWith(startingLetter))
                        {
                            neededNames.Add(name);
                        }
                    }
                }

                if (neededNames.Count != 0)
                {
                    secondName = neededNames[random.Next(neededNames.Count)];
                }
                else
                {
                    secondName = secondNamesFemale[random.Next(secondNamesFemale.Length)];
                }

                patronymic = patronymicFemale[random.Next(patronymicFemale.Length)];
            }

            return secondName + " " + firstName + " " + patronymic;
        }

        public string GenerateFullNameOnlyF(bool gender)
        {
            Random random = new Random();
            string[] firstNamesMale = { "Artem", "Anton", "Aleksei", "Andrei", "Anatoliy", "Artur",
                "Aleksandr", "Albert", "Arkadiy", "Boris", "Bogdan", "Valeriy", "Valentin", "Vladislav"};


            string[] secondNamesMale = { "Fadeev", "Frolov" };

            string[] patronymicMale = { "Ivanovich", "Petrovich", "Alekseevich", "Antovich", "Maximovich",
                "Andreevich", "Borisovich"};


            string[] firstNamesFemale = { "Anna", "Yulya", "Snejanna", "Zoya", "Elena", "Galina",
                "Valentina", "Janna" };

            string[] secondNamesFemale = { "Fadeev", "Frolov" };

            string[] patronymicFemale = { "Ivanovna", "Petrovna", "Alekseevna", "Antonovna", "Maximovna",
                "Andreevna", "Borisovna" };

            string firstName, secondName, patronymic;


            if (gender)
            {
                firstName = firstNamesMale[random.Next(firstNamesMale.Length)];
                
                secondName = secondNamesMale[random.Next(secondNamesMale.Length)];

                patronymic = patronymicMale[random.Next(patronymicMale.Length)];
            }
            else
            {
                firstName = firstNamesFemale[random.Next(firstNamesFemale.Length)];
                
                
                secondName = secondNamesFemale[random.Next(secondNamesFemale.Length)];


                patronymic = patronymicFemale[random.Next(patronymicFemale.Length)];
            }

            return secondName + " " + firstName + " " + patronymic;
        }
       
        public DateTime GenerateBirthDate()
        {
            Random random = new Random();
            DateTime start = new DateTime(1970, 1, 1);
            DateTime end = new DateTime(2006, 1, 1);
            int range = (end - start).Days;

            return start.AddDays(random.Next(range));
        }
    }
}
