﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.DTOs.CustomerDTOs
{
    // DTO for changing password
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }


        [Required(ErrorMessage = "Current Password is required.")]
        [MinLength(8, ErrorMessage = "New Password must be at least 8 characters.")]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "New Password is required.")]
        [MinLength(8, ErrorMessage = "New Password must be at least 8 characters.")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm New Password is required.")]
        [Compare("NewPassword", ErrorMessage = "New Password and confirm New Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
