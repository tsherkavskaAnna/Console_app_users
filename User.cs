using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace esameProva
{
    internal class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }

        public User(int userId, string firstName, string lastName, string email, string gender, bool active)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Gender = gender;
            Active = active;

        }

        public string GetInfo()
        {
            return $"Information about user: {FirstName} + {LastName} with email - {Email} and gender: {Gender} + {Active}";
        }

    }
}