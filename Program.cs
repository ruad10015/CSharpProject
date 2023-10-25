using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Json;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public int Age { get; set; }
}

public class Worker : User
{
    public List<CV> CVs { get; set; }

    public Worker()
    {
        CVs = new List<CV>();
    }

    public void ViewCV()
    {
        if (CVs.Count > 0)
        {
            Console.WriteLine("Your CVs:");
            foreach (var cv in CVs)
            {
                Console.WriteLine($"CV ID: {cv.Id}");
                Console.WriteLine($"Expertise: {cv.Expertise}");
                Console.WriteLine($"School: {cv.School}");
                Console.WriteLine($"University: {cv.University}");
                Console.WriteLine("Skills:");
                foreach (var skill in cv.Skills)
                {
                    Console.WriteLine($"- {skill}");
                }
                Console.WriteLine($"Companies: {string.Join(", ", cv.Companies)}");
                Console.WriteLine($"Start Date: {cv.StartDate}");
                Console.WriteLine($"End Date: {cv.EndDate}");
                Console.WriteLine("Languages:");
                foreach (var language in cv.Languages)
                {
                    Console.WriteLine($"- {language.Name}");
                }
                Console.WriteLine($"Has Diploma: {cv.HasDiploma}");
                Console.WriteLine($"GitLink: {cv.GitLink}");
                Console.WriteLine($"LinkedIn: {cv.LinkedIn}");
                Console.WriteLine("------------------------------");
            }
        }
        else
        {
            Console.WriteLine("You have no CVs. You can create a new one.");
        }
    }

    public void CreateCV()
    {
        CV newCV = new CV();

        Console.Write("Enter Expertise: ");
        newCV.Expertise = Console.ReadLine();

        Console.Write("Enter School: ");
        newCV.School = Console.ReadLine();

        Console.Write("Enter University: ");
        newCV.University = Console.ReadLine();

        Console.WriteLine("Enter Skills (comma-separated, e.g., C#,Java,Python): ");
        string skillsInput = Console.ReadLine();
        newCV.Skills = skillsInput.Split(',').Select(s => s.Trim()).ToList();

        Console.WriteLine("Enter Companies (comma-separated, e.g., Company1,Company2): ");
        string companiesInput = Console.ReadLine();
        newCV.Companies = companiesInput.Split(',').Select(c => c.Trim()).ToList();

        Console.Write("Enter Start Date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            newCV.StartDate = startDate;
        }

        Console.Write("Enter End Date (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
        {
            newCV.EndDate = endDate;
        }

        Console.WriteLine("Enter Languages (comma-separated, e.g., English:Intermediate,French:Beginner): ");
        string languagesInput = Console.ReadLine();
        newCV.Languages = languagesInput.Split(',').Select(lang =>
        {
            var parts = lang.Split(':');
            return new Language
            {
                Name = parts[0].Trim(),
            };
        }).ToList();

        Console.Write("Do you have a diploma? (true/false): ");
        if (bool.TryParse(Console.ReadLine(), out bool hasDiploma))
        {
            newCV.HasDiploma = hasDiploma;
        }

        Console.Write("Enter GitLink: ");
        newCV.GitLink = Console.ReadLine();

        Console.Write("Enter LinkedIn: ");
        newCV.LinkedIn = Console.ReadLine();

        newCV.Id = CVs.Count + 1; // Assign a unique ID

        CVs.Add(newCV);
        Console.WriteLine("CV created successfully.");
    }

    public void DeleteCV()
    {
        Console.Write("Enter the ID of the CV you want to delete: ");
        if (int.TryParse(Console.ReadLine(), out int cvId))
        {
            var cv = CVs.FirstOrDefault(c => c.Id == cvId);
            if (cv != null)
            {
                CVs.Remove(cv);
                Console.WriteLine("CV deleted successfully.");
            }
            else
            {
                Console.WriteLine("Invalid CV ID. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }
}

public class CV
{
    public int Id { get; set; }
    public string? Expertise { get; set; }
    public string? School { get; set; }
    public string? University { get; set; }
    public List<string?> Skills { get; set; }
    public List<string?> Companies { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Language> Languages { get; set; }
    public bool HasDiploma { get; set; }
    public string? GitLink { get; set; }
    public string? LinkedIn { get; set; }
}

public class Language
{
    public string? Name { get; set; }
}

public class Employer : User
{
    public List<Vacancy> Vacancies { get; set; }
}

public class Vacancy
{
    public int Id { get; set; }
    public string? JobTitle { get; set; }
    public string? Description { get; set; }
    public int Salary { get; set; }
}

public class Program
{
    private static List<Worker> workers = new List<Worker>();
    private static List<Employer> employers = new List<Employer>();
    private static List<User> loggedInUsers = new List<User>();

    static void Main(string[] args)
    {
        LoadData();

        while (true)
        {
            Console.WriteLine("Welcome to the Job Advertisement System");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                if (choice == 1)
                {
                    RegisterUser();
                }
                else if (choice == 2)
                {
                    Login();
                }
                else if (choice == 3)
                {
                    SaveData();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }

    private static void RegisterUser()
    {
        Console.WriteLine("Register as:");
        Console.WriteLine("1. Worker");
        Console.WriteLine("2. Employer");
        Console.Write("Enter your choice: ");

        int userTypeChoice;
        if (int.TryParse(Console.ReadLine(), out userTypeChoice))
        {
            if (userTypeChoice == 1)
            {
                Worker worker = new Worker();
                Console.Write("Enter Username: ");
                worker.Username = Console.ReadLine();
                Console.Write("Enter Password: ");
                worker.Password = Console.ReadLine();

                CV blankCV = new CV
                {
                    Id = 1,
                    Skills = new List<string?>(),
                    Companies = new List<string?>(),
                    Languages = new List<Language>()
                };

                worker.CVs = new List<CV> { blankCV };
                workers.Add(worker);
                loggedInUsers.Add(worker);
            }
            else if (userTypeChoice == 2)
            {
                Employer employer = new Employer();
                Console.Write("Enter Username: ");
                employer.Username = Console.ReadLine();
                Console.Write("Enter Password: ");
                employer.Password = Console.ReadLine();
                employers.Add(employer);
                loggedInUsers.Add(employer);
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }

    private static void Login()
    {
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        User user = loggedInUsers.FirstOrDefault(u => u.Username == username && u.Password == password);

        if (user != null)
        {
            Console.WriteLine($"Welcome, {user.Username}!");

            if (user is Worker)
            {
                WorkerMenu(user as Worker);
            }
            else if (user is Employer)
            {
                EmployerMenu(user as Employer);
            }
        }
        else
        {
            Console.WriteLine("Invalid username or password. Please try again.");
        }
    }

    private static void WorkerMenu(Worker worker)
    {
        while (true)
        {
            Console.WriteLine("Worker Menu:");
            Console.WriteLine("1. View CV");
            Console.WriteLine("2. Create CV");
            Console.WriteLine("3. Delete CV");
            Console.WriteLine("4. Log Out");
            Console.Write("Enter your choice: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                if (choice == 1)
                {
                    worker.ViewCV();
                }
                else if (choice == 2)
                {
                    worker.CreateCV();
                }
                else if (choice == 3)
                {
                    worker.DeleteCV();
                }
                else if (choice == 4)
                {
                    SaveData();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }

    private static void EmployerMenu(Employer employer)
    {
        while (true)
        {
            Console.WriteLine("Employer Menu:");
            Console.WriteLine("1. Create Vacancy");
            Console.WriteLine("2. View Vacancies");
            Console.WriteLine("3. Log Out");
            Console.Write("Enter your choice: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                if (choice == 1)
                {
                    CreateVacancy(employer);
                }
                else if (choice == 2)
                {
                    ViewVacancies(employer);
                }
                else if (choice == 3)
                {
                    SaveData();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }

    private static void CreateVacancy(Employer employer)
    {
        Vacancy vacancy = new Vacancy();

        Console.Write("Enter Job Title: ");
        vacancy.JobTitle = Console.ReadLine();

        Console.Write("Enter Description: ");
        vacancy.Description = Console.ReadLine();

        Console.Write("Enter Salary: ");
        if (int.TryParse(Console.ReadLine(), out int salary))
        {
            vacancy.Salary = salary;
        }

        int v =vacancy.Id + 1;
        vacancy.Id = v;

        if (employer.Vacancies == null)
        {
            employer.Vacancies = new List<Vacancy>();
        }

        employer.Vacancies.Add(vacancy);
        Console.WriteLine("Vacancy created successfully.");
    }



    private static void ViewVacancies(Employer employer)
    {
        if (employer.Vacancies.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Your Vacancies:");
            foreach (var vacancy in employer.Vacancies)
            {
                Console.WriteLine($"Vacancy ID: {vacancy.Id}");
                Console.WriteLine($"Job Title: {vacancy.JobTitle}");
                Console.WriteLine($"Description: {vacancy.Description}");
                Console.WriteLine($"Salary: {vacancy.Salary}");
                Console.WriteLine("------------------------------");
            }
        }
        else
        {
            Console.WriteLine("You have no vacancies. You can create a new one.");
        }
    }

    private static void SaveData()
    {
        try
        {
            string workersJson = JsonSerializer.Serialize(workers);
            File.WriteAllText("workers.json", workersJson);

            string employersJson = JsonSerializer.Serialize(employers);
            File.WriteAllText("employers.json", employersJson);

            string loggedInUsersJson = JsonSerializer.Serialize(loggedInUsers);
            File.WriteAllText("loggedInUsers.json", loggedInUsersJson);

            Console.WriteLine("Data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while saving data: " + ex.Message);
        }
    }

    private static void LoadData()
    {
        try
        {
            if (File.Exists("workers.json"))
            {
                string workersJson = File.ReadAllText("workers.json");
                workers = JsonSerializer.Deserialize<List<Worker>>(workersJson);
            }

            if (File.Exists("employers.json"))
            {
                string employersJson = File.ReadAllText("employers.json");
                employers = JsonSerializer.Deserialize<List<Employer>>(employersJson);
            }

            if (File.Exists("loggedInUsers.json"))
            {
                string loggedInUsersJson = File.ReadAllText("loggedInUsers.json");
                loggedInUsers = JsonSerializer.Deserialize<List<User>>(loggedInUsersJson);
            }


            Console.WriteLine("Data loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while loading data: " + ex.Message);
        }
    }
}

