using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace esameProva
{
    public class Program
    {
        static string connectionString = @"Server=localhost;Database=DotNetCourseDatabase;Trusted_Connection=false;TrustServerCertificate=True;User Id=SA;Password=SQLConnect1";

        public static void Main(string[] args)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Connection successful with database!");

                    while (true)
                    {
                        Console.WriteLine("Menu: ");
                        Console.WriteLine("1 - All users list");
                        Console.WriteLine("2 - Add new user");
                        Console.WriteLine("3 - Update user con id: ");
                        Console.WriteLine("4 - Delete user");
                        Console.WriteLine("5 - List witch all users male:");
                        Console.WriteLine("6 - List witch all users female:");
                        Console.WriteLine("7 - List witch active users");
                        Console.WriteLine("8 - Escape from app");

                        string choice = Console.ReadLine();


                        switch (choice)
                        {
                            case "1":
                                GetAllUsers();
                                break;
                            case "2":
                                AddNewUser();
                                break;
                        }

                    }
                }

                static void GetAllUsers()
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string query = "SELECT [UserId],[FirstName],[LastName],[Email],[Gender],[Active] FROM TutorialAppSchema.Users";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    Console.WriteLine("Users list:");
                                    while (reader.Read())
                                    {
                                        int userId = Convert.ToInt32(reader["UserId"]);
                                        string firstName = reader["FirstName"].ToString();
                                        string lastName = reader["LastName"].ToString();
                                        string email = reader["Email"].ToString();
                                        string gender = reader["Gender"].ToString();
                                        bool active = Convert.ToBoolean(reader["Active"]);

                                        User user = new User(userId, firstName, lastName, email, gender, active);
                                        Console.WriteLine(user.GetInfo());
                                    }

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error with list of users: {ex.Message}");
                        }
                    }
                }

                static void AddNewUser()
                {
                    try
                    {
                        Console.WriteLine("Enter first name: ");
                        string firstName = Console.ReadLine();
                        Console.WriteLine("Enter last name: ");
                        string lastName = Console.ReadLine();
                        Console.WriteLine("Enter email: ");
                        string email = Console.ReadLine();
                        Console.WriteLine("Enter gender: ");
                        string gender = Console.ReadLine();
                        Console.WriteLine("Is the user active? (true/false)");
                        bool active;
                        bool inputValid = bool.TryParse(Console.ReadLine(), out active);

                        if (!inputValid)
                        {
                            Console.WriteLine("Please insert valid bool: false or true");
                            return;
                        }
                        Console.WriteLine($"Active value: {active}");



                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = @"INSERT INTO TutorialAppSchema.Users(FirstName,LastName, Email,Gender,Active
                                         ) VALUES(@FirstName, @LastName, @Email,@Gender,@Active)";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.Add("@FirstName", SqlDbType.Text).Value = firstName;
                                command.Parameters.Add("@LastName", SqlDbType.Text).Value = lastName;
                                command.Parameters.Add("@Email", SqlDbType.Text).Value = email;
                                command.Parameters.Add("@Gender", SqlDbType.Text).Value = email;
                                command.Parameters.Add("@Active", SqlDbType.Bit).Value = active;

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine($"New user {firstName} {lastName} added con successful!");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to add new user!");
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error with adding new user: {ex.Message}");
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);

            }
        }

    }


}

