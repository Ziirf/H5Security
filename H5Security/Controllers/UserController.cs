using H5Security.Models;
using H5Security.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace H5Security.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IEncryption _encryption;
        private IMicrosoftSQL _sql;
        public UserController(IEncryption encryption, IMicrosoftSQL microsoftSQL)
        {
            _encryption = encryption;
            _sql = microsoftSQL;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<User>> Get()
        {
            var userList = new List<User>();
            _sql.OpenConnection((connection) =>
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Users", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userList.Add(new User()
                        {
                            Id = (int)reader["ID"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"],
                            Salt = (byte[])reader["Salt"],
                            Iterations = (int)reader["Iterations"]
                        });
                    }
                }
            });

            return userList;
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User user = null;
            _sql.OpenConnection((connection) =>
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            Id = (int)reader["ID"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"],
                            Salt = (byte[])reader["Salt"],
                            Iterations = (int)reader["Iterations"]
                        };
                    }
                }
            });

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost()]
        public ActionResult<UserDTO> CreateUser([FromBody] UserDTO userRequest)
        {
            var salt = _encryption.GenerateSalt();
            var iteration = new Random().Next(100000);
            var hashedPassword = _encryption.Hash(userRequest.Password, salt, iteration);

            _sql.OpenConnection((connection) =>
            {
                SqlCommand command = new SqlCommand("INSERT INTO Users (username,password,salt,iterations) VALUES (@username, @password, @salt, @iterations)", connection);
                command.Parameters.AddWithValue("@username", userRequest.Username);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@salt", salt);
                command.Parameters.AddWithValue("@iterations", iteration);

                command.ExecuteNonQuery();
            });

            return new UserDTO() { Username = userRequest.Username, Password = "*****" };
        }

        [HttpPost("Login")]
        public ActionResult<bool> Login([FromBody] UserDTO userRequest)
        {
            User user = new User() { Username = userRequest.Username };
            string hashedPassword = string.Empty;

            _sql.OpenConnection((connection) =>
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Username = @username", connection);
                command.Parameters.AddWithValue("@username", userRequest.Username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hashedPassword = (string)reader["Password"];
                        user.Salt = (byte[])reader["Salt"];
                        user.Iterations = (int)reader["Iterations"];
                    }
                }
            });

            return _encryption.Hash(userRequest.Password, user.Salt, user.Iterations) == hashedPassword;
        }
    }
}
