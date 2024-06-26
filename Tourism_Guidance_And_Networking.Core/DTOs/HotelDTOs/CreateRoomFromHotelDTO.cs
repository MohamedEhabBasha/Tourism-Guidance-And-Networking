﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Attribute;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class CreateRoomFromHotelDTO
    {

        [MaxLength(250)]
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose an image"),
         AllowedExtenstions(FileSettings.AllowedExtensions)]
        public IFormFile ImagePath { get; set; } = default!;
        [Required]
        public int Capicity { get; set; }

    }
}
