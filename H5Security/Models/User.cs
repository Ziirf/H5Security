using System;

namespace H5Security.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
        public int Iterations { get; set; }
    }
}
