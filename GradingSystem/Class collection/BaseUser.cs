using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradingSystem.Class_collection
{
    public abstract class BaseUser : IUser
    {
        protected string connectionString;

        private string role = "none";
        public string RoleName => role;

        protected BaseUser(string connectionString, string role)
        {
            this.connectionString = connectionString;
            this.role = role;
        }

        public virtual int Create(string username, string password, string email)
        {
            int result = 0;

            if (!User.UserExists(connectionString, username))
            {
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    string query = "insert into Users (username, password, email, role) values (@username, @password, @email, @role)";
                    using (SqlCommand command = new(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@role", RoleName);

                        result = command.ExecuteNonQuery();
                    }
                }
            }

            return result;
        }

        public virtual bool Update(string userId, string? username = null, string? password = null, string? email = null)
        {
            // MUST have equal length
            bool[] shouldUpdateTheseParams = new bool[3]
            {
                username != null, password != null, email != null
            };
            string[] paramsToUpdate = new string[3]
            {
                "username = @username", "password = @password", "email = @email"
            };

            StringBuilder queryBuilder = new StringBuilder();
            string separator = "";
            for (int i = 0; i < shouldUpdateTheseParams.Length; i++)
            {
                bool shouldUpdate = shouldUpdateTheseParams[i];
                if (shouldUpdate)
                {
                    queryBuilder.Append(separator);
                    queryBuilder.Append(paramsToUpdate[i]);
                    separator = ", ";
                }
            }

            if (queryBuilder.Length > 0)
            {
                int result = 0;
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    string query = "update Users set " + queryBuilder.ToString() + " where user_id = @ID";
                    using (SqlCommand command = new(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", userId);
                        if (username != null) command.Parameters.AddWithValue("@username", username);
                        if (password != null) command.Parameters.AddWithValue("@password", password);
                        if (email != null) command.Parameters.AddWithValue("@email", email);
                        result = command.ExecuteNonQuery();
                    }
                }

                if (result == 1) return true;
            }
            else
            {
                Console.WriteLine("A User.Update() method was called with no parameters. Nothing was updated.");
            }
            return false;
        }

        public virtual void Delete(string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE user_id = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
