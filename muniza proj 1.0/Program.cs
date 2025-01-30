// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("press 1 to create user");
        Console.WriteLine("press 2 to find user");
        Console.WriteLine("press 3 to delete user");
        Console.WriteLine("press 4 to update user");

        int a = int.Parse(Console.ReadLine());

        if (a == 1)
        {
            Console.WriteLine("enter your username");
            string Username = Console.ReadLine();

            Console.WriteLine("enter your email");
            string Email = Console.ReadLine();

            Console.WriteLine("enter your password");
            string Password = Console.ReadLine();

            string encryptThepass = BCrypt.Net.BCrypt.HashPassword(Password);

            // string connnectionstring ="Data Source = HP\\SQLEXPRESS; Initial Catalog = dummy_info; Integrated Security = True; Encrypt = True; Trust Server Certificate = True\r\n\r\n\r\n";
            string connectionstring = "Data Source=HP\\SQLEXPRESS;Initial Catalog=users_1;Integrated Security=True;Trust Server Certificate=True";

            createuser(connectionstring, Username, Email, encryptThepass);

        }
        else if (a == 2)
        {
            string connectionstring = "Data Source=HP\\SQLEXPRESS;Initial Catalog=users_1;Integrated Security=True;Trust Server Certificate=True"; 

            Console.WriteLine("enter the user_email");
            string Email = Console.ReadLine();

            finduser(connectionstring , Email);

        }
        else if (a == 3)
        {
            Console.WriteLine("Enter email to delete User Account! ");
            string user_email= Console.ReadLine();

            string connectionstring = "Data Source=HP\\SQLEXPRESS;Initial Catalog=users_1;Integrated Security=True;Trust Server Certificate=True";
            delete_user (connectionstring, user_email); 
        }
        else if(a == 4)
        {
            Console.WriteLine("enter your user_email to find the user to be updated");
            string Email = Console.ReadLine();

            Console.WriteLine("enter your new username");
            string NewUsername = Console.ReadLine();

            Console.WriteLine("enter your new password");
            string NewPassword = Console.ReadLine();

            string encryptpass = BCrypt.Net.BCrypt.HashPassword(NewPassword);

            string connectionstring = "Data Source=HP\\SQLEXPRESS;Initial Catalog=users_1;Integrated Security=True;Trust Server Certificate=True";
            edit_user(connectionstring,Email,NewUsername, encryptpass);
        }

    }
    static void createuser(string connectionstring, string Username, string Email, string Password)
    {
        try
        {
            Console.WriteLine("trying to connect with database");

            using SqlConnection connection = new SqlConnection(connectionstring);

            connection.Open();

            string query = "INSERT into Users(Username,Email,Password) " +
                "VALUES( @Username,@Email,@Password)";

            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Password", Password);

            int rowsaffected = command.ExecuteNonQuery();

            if (rowsaffected > 0)
            {
                Console.WriteLine("user saved syccesfully");
            }
            else
            {
                Console.WriteLine("user not saved");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

        }
    }

    static void finduser(string connectionstring, string Email)
    {
        try
        {
          
            string query = "select * from Users where Email=@Email";

            using SqlConnection connection = new(connectionstring);

            connection.Open();

            using SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Email", Email);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine("User Found! Below are the Details of User");
                Console.WriteLine(reader["Username"].ToString());
                Console.WriteLine(reader["Email"].ToString());
                Console.WriteLine(reader["Password"].ToString());
            }
            else
            {
                Console.WriteLine("No User Found With this Email!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("e.message");
        }
    }
    static void delete_user(string connectionstring ,string Email)
    {
        try
        {
            using SqlConnection connection = new(connectionstring);
            var query = "DELETE FROM Users WHERE Email = @Email"; 
           
            connection.Open();

            using  SqlCommand command = new(query, connection);

            command.Parameters.AddWithValue("@Email", Email);   
            int rowsDeleted = command.ExecuteNonQuery();   

            if (rowsDeleted > 0)
            {
                Console.WriteLine("User Deleted Succesfully!");
            }
            else
            {
                Console.WriteLine("User Not Found!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void edit_user(string connectionstring, string Email, string NewUsername, string NewPassword)
    {
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string query = "UPDATE Users SET Username = @NewUsername, Password = @NewPassword WHERE Email = @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@NewUsername", NewUsername);
                        command.Parameters.AddWithValue("@NewPassword", NewPassword);
                        command.Parameters.AddWithValue("@Email", Email);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("User details updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No user found with the provided email.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

    }
}
