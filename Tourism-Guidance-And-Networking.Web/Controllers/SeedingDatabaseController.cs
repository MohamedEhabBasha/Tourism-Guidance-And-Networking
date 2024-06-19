using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Helper;
using Tourism_Guidance_And_Networking.Core.Models.AI_Integration;
using Tourism_Guidance_And_Networking.Core.Models.Authentication;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;
using Tourism_Guidance_And_Networking.Web.Services;

namespace Tourism_Guidance_And_Networking.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = Roles.Admin)]
    public class SeedingDatabaseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public SeedingDatabaseController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IAuthService authService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        [HttpGet("SeedingUsers")]
        public async Task<IActionResult> SeedingUsers()
        {
            var user = await _unitOfWork.ApplicationUsers.FindAsync(u => u.Id == "60728638-96b8-4576-ac14-da785002ee04");

            if (user is not null)
                return BadRequest("Users Already Exists");

            List<ApplicationUser> applicationUsers = new()
            {
                new ApplicationUser()
                {
                    Id="60728638-96b8-4576-ac14-da785002ee04",
                    UserName = "user1",
                    Email = "user1@gmail.com",
                    FirstName = "user1",
                    LastName = "user1",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
                new ApplicationUser()
                {
                   Id="60728638-96c8-4576-ac14-da785002ee04",
                    UserName = "user2",
                    Email = "user2@gmail.com",
                    FirstName = "user2",
                    LastName = "user2",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"

                },
                new ApplicationUser()
                {
                     Id="60728638-96d8-4576-ac14-da785002ee04",
                    UserName = "user3",
                    Email = "user3@gmail.com",
                    FirstName = "user3",
                    LastName = "user3",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
                new ApplicationUser()
                {
                    Id="60728638-96e8-4576-ac14-da785002ee04",
                    UserName = "user4",
                    Email = "user4@gmail.com",
                    FirstName = "user4",
                    LastName = "user4",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
                new ApplicationUser()
                {
                     Id="60728638-96f8-4576-ac14-da785002ee04",
                    UserName = "user5",
                    Email = "user5@gmail.com",
                    FirstName = "user5",
                    LastName = "user5",
                    PhoneNumber = "1112223333",
                    Address = "10th of ramadan"
                },
            };

            foreach (var item in applicationUsers)
            {
                await _userManager.CreateAsync(item, "User@2001");
                _userManager.AddToRoleAsync(item, Roles.Tourist).GetAwaiter().GetResult();
            }

            return Ok();
        }

        [HttpGet("SeedingHotels")]
        public async Task<IActionResult> SeedingHotels()
        {
            // Path to your JSON file
            string filePath = "D:\\University\\Graduation-Project\\Phase1\\Tourism-Guidance-And-Networking\\Tourism-Guidance-And-Networking.Web\\wwwroot\\Data\\stays.json";

            // Read the JSON file asynchronously
            string jsonData;
            using (var reader = new StreamReader(filePath))
            {
                jsonData = await reader.ReadToEndAsync();
            }
            // Deserialize the JSON data into the PropertyData class
            Data propertyData = JsonConvert.DeserializeObject<Data>(jsonData);

            List<string> namesOfhotels = new List<string>();
            List<Hotel> Hotels = new List<Hotel>();

            for (int i = 0; i < 5048; i++)
            {

                if (propertyData.property_type[$"{i}"] == "Hotel" || propertyData.property_type[$"{i}"] == "Suit")
                {
                    RegisterModel registerModel = new RegisterModel();
                    registerModel.FirstName = $"Hotel{i}";
                    registerModel.LastName = $"Hotel{i}";
                    registerModel.Username = $"Hotel{i + 32000}";
                    registerModel.Address = propertyData.address[$"{i}"];
                    registerModel.PhoneNumber = "+20123456789";
                    registerModel.Email = $"Hotel{i + 32000}@gmail.com";
                    registerModel.Password = "Hotel@2001";
                    registerModel.ConfirmPassword = "Hotel@2001";

                   var result = await _authService.RegisterAsync(registerModel, Roles.Hotel);

                    if (!result.IsAuthenticated)
                        return BadRequest(result.Message);

                    SetRefreshTokenInCookies(result.RefreshToken, result.RefrshTokenExpiration);

                    Random random = new Random();

                    double rating = 9;
                    int numOfReviews = random.Next(20, 150);

                    if (propertyData.rating[$"{i}"] is not null)
                        rating = double.Parse(propertyData.rating[$"{i}"]);

                    if (propertyData.number_of_reviews[$"{i}"] is not null)
                        numOfReviews = ExtractNumberFromString(propertyData.number_of_reviews[$"{i}"]);

                    if (propertyData.governorate[$"{i}"] is null ||
                        propertyData.Name[$"{i}"] is null ||
                        propertyData.address[$"{i}"] is null ||
                        propertyData.img[$"{i}"] is null ||
                        propertyData.location[$"{i}"] is null ||
                        propertyData.type[$"{i}"] is null ||
                        propertyData.price[$"{i}"] is null ||
                        propertyData.taxes_and_charges[$"{i}"] is null ||
                        propertyData.info[$"{i}"] is null ||
                        propertyData.img[$"{i}"] is null ||
                        propertyData.num_adults[$"{i}"] is null ||
                        propertyData.description[$"{i}"] is null)
                        continue;
                       


                    Hotel hotel = new()
                    {
                        Name = propertyData.Name[$"{i}"],
                        Address = propertyData.address[$"{i}"],
                        Rating = rating,
                        Image= propertyData.img[$"{i}"].Replace(@"\/", "/"),
                        Reviews = numOfReviews,
                        ApplicationUserId = result.UserId,
                        Location = propertyData.location[$"{i}"],
                        Governorate = propertyData.governorate[$"{i}"]
                    };

                    Hotel myHotel = await _unitOfWork.Hotels.AddAsync(hotel);
                    _unitOfWork.Complete();
                    Hotels.Add(myHotel);

                    if (namesOfhotels.Contains(propertyData.Name[$"{i}"]))
                        continue;
                    else
                        namesOfhotels.Add(propertyData.Name[$"{i}"]);
                }
            }
            return Ok(Hotels);

        }

        [HttpGet("SeedingRooms")]
        public async Task<IActionResult> SeedingRooms()
        {
            // Path to your JSON file
            string filePath = "D:\\University\\Graduation-Project\\Phase1\\Tourism-Guidance-And-Networking\\Tourism-Guidance-And-Networking.Web\\wwwroot\\Data\\stays.json";

            // Read the JSON file asynchronously
            string jsonData;
            using (var reader = new StreamReader(filePath))
            {
                jsonData = await reader.ReadToEndAsync();
            }
            // Deserialize the JSON data into the PropertyData class
            Data propertyData = JsonConvert.DeserializeObject<Data>(jsonData);

            List<Room> Rooms = new List<Room>();

            for (int i = 0; i < 5048; i++)
            {
                if (propertyData.property_type[$"{i}"] == "Hotel" || propertyData.property_type[$"{i}"] == "Suit")
                {

                    Random random = new Random();

                    double rating = 9;
                    int numOfReviews = random.Next(20, 50);

                    if (propertyData.rating[$"{i}"] is not null)
                        rating = double.Parse(propertyData.rating[$"{i}"]);

                    if (propertyData.number_of_reviews[$"{i}"] is not null)
                        numOfReviews = ExtractNumberFromString(propertyData.number_of_reviews[$"{i}"]);

                    if (propertyData.governorate[$"{i}"] is null ||
                        propertyData.Name[$"{i}"] is null ||
                        propertyData.address[$"{i}"] is null ||
                        propertyData.img[$"{i}"] is null || 
                        propertyData.location[$"{i}"] is null ||
                        propertyData.type[$"{i}"] is null ||
                        propertyData.price[$"{i}"] is null ||
                        propertyData.taxes_and_charges[$"{i}"] is null ||
                        propertyData.info[$"{i}"] is null ||
                        propertyData.img[$"{i}"] is null ||
                        propertyData.num_adults[$"{i}"] is null ||
                        propertyData.description[$"{i}"] is null)
                           continue;

                    var hotel = await _unitOfWork.Hotels.FindAsync(h => h.Name == propertyData.Name[$"{i}"]);

                    if (hotel is null)
                        return BadRequest($"Hotel with name {propertyData.Name[$"{i}"]} DOES NOT EXIST");

                    Room room = new Room()
                    {
                        Type = propertyData.type[$"{i}"],
                        Price = int.Parse(propertyData.price[$"{i}"]),
                        Taxes= int.Parse(propertyData.taxes_and_charges[$"{i}"]),
                        Info = propertyData.info[$"{i}"],
                        Description = propertyData.description[$"{i}"],
                        Image = propertyData.img[$"{i}"].Replace(@"\/", "/"),
                        Capicity = int.Parse(propertyData.num_adults[$"{i}"]),
                        Count = random.Next(5,15),
                        CountOfReserved = 0,
                        HotelId = hotel.Id
                    };

                    Room roomDb =  await _unitOfWork.Rooms.AddAsync(room);
                    _unitOfWork.Complete();
                    RoomMapping roomMapping = new()
                    {
                        Room = roomDb.Id,
                        Item = i
                    };
                    await _unitOfWork.RoomMappings.AddAsync(roomMapping);
                    _unitOfWork.Complete();
                    Rooms.Add(room);

                }
            }
            return Ok(Rooms);
        }

        [HttpGet("SeedingAccommdations")]
        public async Task<IActionResult> Accommdations()
        {// Path to your JSON file
         // Path to your JSON file
            string filePath = "D:\\University\\Graduation-Project\\Phase1\\Tourism-Guidance-And-Networking\\Tourism-Guidance-And-Networking.Web\\wwwroot\\Data\\stays.json";

            // Read the JSON file asynchronously
            string jsonData;
            using (var reader = new StreamReader(filePath))
            {
                jsonData = await reader.ReadToEndAsync();
            }
            // Deserialize the JSON data into the PropertyData class
            Data propertyData = JsonConvert.DeserializeObject<Data>(jsonData);

            List<Accommodation> Accommodations = new List<Accommodation>();

            for (int i = 0; i < 5048; i++)
            {
                if (!(propertyData.property_type[$"{i}"] == "Hotel" || propertyData.property_type[$"{i}"] == "Suit"))
                {

                    Random random = new Random();

                    double rating = 9;
                    int numOfReviews = random.Next(20, 50);

                    if (propertyData.rating[$"{i}"] is not null)
                        rating = double.Parse(propertyData.rating[$"{i}"]);

                    if (propertyData.number_of_reviews[$"{i}"] is not null)
                        numOfReviews = ExtractNumberFromString(propertyData.number_of_reviews[$"{i}"]);

                    if (propertyData.governorate[$"{i}"] is null ||
                        propertyData.Name[$"{i}"] is null ||
                        propertyData.address[$"{i}"] is null ||
                        propertyData.img[$"{i}"] is null ||
                        propertyData.location[$"{i}"] is null ||
                        propertyData.type[$"{i}"] is null ||
                        propertyData.price[$"{i}"] is null ||
                        propertyData.taxes_and_charges[$"{i}"] is null ||
                        propertyData.info[$"{i}"] is null ||
                        propertyData.img[$"{i}"] is null ||
                        propertyData.num_adults[$"{i}"] is null ||
                        propertyData.description[$"{i}"] is null)
                        continue;

                    Accommodation accomdation = new Accommodation()
                    {
                        Name = propertyData.Name[$"{i}"],
                        Address = propertyData.address[$"{i}"],
                        Rating = rating,
                        Image = propertyData.img[$"{i}"].Replace(@"\/", "/"),
                        Reviews = numOfReviews,
                        Location = propertyData.location[$"{i}"],
                        Governorate = propertyData.governorate[$"{i}"],
                        Type = propertyData.type[$"{i}"],
                        Price = int.Parse(propertyData.price[$"{i}"]),
                        Taxes = int.Parse(propertyData.taxes_and_charges[$"{i}"]),
                        Info = propertyData.info[$"{i}"],
                        Description = propertyData.description[$"{i}"],
                        Capicity = int.Parse(propertyData.num_adults[$"{i}"]),
                        Count = random.Next(5, 15),
                        CountOfReserved = 0,
                        CompanyId =5 // To Be Changed 
                    };

                    Accommodation accomdationDb = await _unitOfWork.Accommodations.AddAsync(accomdation);
                    _unitOfWork.Complete();
                    AccomdationMapping accomdationMapping = new()
                    {
                        Accomdation = accomdationDb.Id,
                        Item = i
                    };
                    await _unitOfWork.AccomdationMappings.AddAsync(accomdationMapping);
                    _unitOfWork.Complete();
                    Accommodations.Add(accomdationDb);

                }
            }
            return Ok(Accommodations);
        }
        private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(), // important as time in postman diffrent from local
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None

            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public static  int ExtractNumberFromString(string input)
        {
            string numberString = "";
            foreach (char c in input)
            {
                if (char.IsSeparator(c))
                    break;
                if (char.IsDigit(c))
                {
                    numberString += c;
                }
            }
            return int.Parse(numberString);
            
            
        }


    }
}
