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

                    Console.WriteLine("Push one button for open menu.");
                    Console.ReadKey();

                    while (true)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Menu: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("1 - All users list");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("2 - Add new user");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("3 - Update user: ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("4 - Delete user");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("5 - List with all users male:");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("6 - List with all users female:");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("7 - List with active users");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("8 - Enter from app");
                        Console.ResetColor();

                        string choice = Console.ReadLine();


                        switch (choice)
                        {
                            case "1":
                                GetAllUsers();
                                break;
                            case "2":
                                AddNewUser();
                                break;
                            case "3":
                                UpdateUser();
                                break;
                            case "4":
                                DeleteUser();
                                break;
                            case "5":
                                GetMaleUsers();
                                break;
                            case "6":
                                GetFemaleUsers();
                                break;
                            case "7":
                                GetActiveUsers();
                                break;
                            case "8":
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Choice is not correct. Try again!");
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
                                command.Parameters.Add("@Gender", SqlDbType.Text).Value = gender;
                                command.Parameters.Add("@Active", SqlDbType.Bit).Value = active;

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"New user {firstName} {lastName} added con successful!");
                                    Console.ResetColor();
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

                static void UpdateUser()
                {
                    try
                    {
                        Console.WriteLine("Enter id of user for update");

                        if (int.TryParse(Console.ReadLine(), out int userId))
                        {
                            Console.WriteLine("Enter new first name: ");
                            string firstName = Console.ReadLine();
                            Console.WriteLine("Enter new last name: ");
                            string lastName = Console.ReadLine();
                            Console.WriteLine("Enter new email: ");
                            string email = Console.ReadLine();
                            Console.WriteLine("Enter new gender: ");
                            string gender = Console.ReadLine();
                            Console.WriteLine("Is the user active? (true/false)");
                            bool inputValid = bool.TryParse(Console.ReadLine(), out bool active);

                            if (!inputValid)
                            {
                                Console.WriteLine("Please insert valid bool: false or true");
                                return;
                            }

                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                string sql = "UPDATE TutorialAppSchema.Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email,Gender = @Gender, Active = @Active WHERE UserId = @UserId";

                                using (SqlCommand command = new SqlCommand(sql, connection))
                                {
                                    command.Parameters.AddWithValue("@UserId", userId);
                                    command.Parameters.AddWithValue("@FirstName", firstName);
                                    command.Parameters.AddWithValue("@LastName", lastName);
                                    command.Parameters.AddWithValue("@Email", email);
                                    command.Parameters.AddWithValue("@Gender", gender);
                                    command.Parameters.AddWithValue("@Active", SqlDbType.Bit).Value = active;

                                    int rowsAffected = command.ExecuteNonQuery();
                                    if (rowsAffected > 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Yellow;
                                        Console.WriteLine($"User {firstName} {lastName} with id {userId} updated with successful!");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.WriteLine($"User with id {userId} not found.");
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error with updating of user: {ex.Message}");
                    }
                }
                static void DeleteUser()
                {
                    try
                    {
                        Console.WriteLine("Enter id of user for delete");
                        if (int.TryParse(Console.ReadLine(), out int userId))
                        {
                            if (CheckIfExistId(userId))
                            {
                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();
                                    string sql = "DELETE FROM TutorialAppSchema.Users WHERE UserId = @UserId";

                                    using (SqlCommand command = new SqlCommand(sql, connection))
                                    {
                                        command.Parameters.AddWithValue("@UserId", userId);

                                        int rowsAffected = command.ExecuteNonQuery();
                                        if (rowsAffected > 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine($"User with id {userId} deleted with success!");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Failed to delete user with id {userId}");
                                        }
                                    }
                                }

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"User with id {userId} does not exist");
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Invalid user ID. Please enter valid ID!");
                            Console.ResetColor();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed deleting of user: {ex.Message}");
                    }

                }

                static void GetMaleUsers()
                {
                    try
                    {
                        using SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();
                        string sql = "SELECT * FROM MaleUsersView";

                        using SqlCommand command = new SqlCommand(sql, connection);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                static void GetFemaleUsers()
                {
                    try
                    {
                        using SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();
                        string sql = "SELECT * FROM FemaleUsersView";

                        using SqlCommand command = new SqlCommand(sql, connection);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                }
                static void GetActiveUsers()
                {
                    try
                    {
                        using SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();
                        string sql = "SELECT * FROM ActiveUsers";

                        using SqlCommand command = new SqlCommand(sql, connection);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);

            }
        }
        private static bool CheckIfExistId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT COUNT(*) FROM TutorialAppSchema.Users WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    int userIdCount = (int)command.ExecuteScalar();

                    return userIdCount > 0;
                }

            }
        }
    }


}

