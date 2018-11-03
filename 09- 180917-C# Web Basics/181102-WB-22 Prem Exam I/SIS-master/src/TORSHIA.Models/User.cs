﻿using System;
using TORSHIA.Models.Enums;

namespace TORSHIA.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }
}
