﻿using System.ComponentModel.DataAnnotations;
using Test.DataAccess.Models;

namespace Test.WebAPI.Models.User
{
    public class NewUserDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public int UserRoleId { get; set; }

        public List<int>? UserGroupIds { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}