﻿using System.ComponentModel.DataAnnotations;

namespace Library_Management_App.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
