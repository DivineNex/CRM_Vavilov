using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    internal static class DatabaseService
    {
        public static NpgsqlConnection? Connection { get; private set; }

        public static void OpenConnection(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }

        public static void CloseConnection()
        {
            Connection?.Close();
        }

        public static string RegisterUser(string name, string email, string password)
        {
            if (Connection == null)
                return "Регистрация в данный момент недоступна";

            var query = "SELECT COUNT(*) FROM users WHERE email = @email";
            using (var command = new NpgsqlCommand(query, Connection))
            {
                command.Parameters.AddWithValue("email", email);
                var count = (long)command.ExecuteScalar();

                if (count > 0)
                    return "Пользователь с такой электронной почтой уже зарегистрирован";
            }

            query = "INSERT INTO users (name, email, password) VALUES (@name, @email, @password)";
            using (var command = new NpgsqlCommand(query, Connection))
            {
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("email", email);
                command.Parameters.AddWithValue("password", password);
                command.ExecuteNonQuery();
            }

            return "Регистрация прошла успешно";
        }
    }
}
