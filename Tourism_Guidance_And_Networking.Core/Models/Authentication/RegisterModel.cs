using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism_Guidance_And_Networking.Core.Models.Authentication
{
	public  class RegisterModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; } 
		public string Email { get; set; }
		public string Password { get; set; }
		[Compare("Password")]
		public string ConfirmPassword { get; set; }
	}
}
