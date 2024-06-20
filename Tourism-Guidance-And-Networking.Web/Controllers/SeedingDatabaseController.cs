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
    [Authorize(Roles = Roles.Admin)]
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

                    if (namesOfhotels.Contains(propertyData.Name[$"{i}"]))
                        continue;
                    else
                        namesOfhotels.Add(propertyData.Name[$"{i}"]);


                    RegisterModel registerModel = new RegisterModel();
                    registerModel.FirstName = $"Hotel{i}";
                    registerModel.LastName = $"Hotel{i}";
                    registerModel.Username = $"Hotel{i + 40000}";
                    registerModel.Address = propertyData.address[$"{i}"];
                    registerModel.PhoneNumber = "+20123456789";
                    registerModel.Email = $"Hotel{i + 40000}@gmail.com";
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
                   
                }
            }
            return Ok(Hotels.Count);

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

                    Random random = new Random();

                    double rating = 9;
                    int numOfReviews = random.Next(20, 50);

                    if (propertyData.rating[$"{i}"] is not null)
                        rating = double.Parse(propertyData.rating[$"{i}"]);

                    if (propertyData.number_of_reviews[$"{i}"] is not null)
                        numOfReviews = ExtractNumberFromString(propertyData.number_of_reviews[$"{i}"]);

                    

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
            return Ok(Rooms.Count);
        }

        [HttpGet("SeedingAccommdations")]
        public async Task<IActionResult> Accommdations()
        {// Path to your JSON file
         // Path to your JSON file
            string filePath = "D:\\Visual studio setup\\TourismGaudinceProject\\Tourism-Guidance-And-Networking\\Tourism-Guidance-And-Networking.Web\\wwwroot\\Data\\stays.json";

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

                    Random random = new Random();

                    double rating = 9;
                    int numOfReviews = random.Next(20, 50);

                    if (propertyData.rating[$"{i}"] is not null)
                        rating = double.Parse(propertyData.rating[$"{i}"]);

                    if (propertyData.number_of_reviews[$"{i}"] is not null)
                        numOfReviews = ExtractNumberFromString(propertyData.number_of_reviews[$"{i}"]);

                   

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
                        CompanyId = 3 // To Be Changed 
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
            return Ok(Accommodations.Count);
        }

        [HttpGet("SeedingCategoriesOfTouristPlaces")]
        public async Task<IActionResult> SeedingCategories()
        {

            List<Category> Categories = new()
            {
                new Category
                {
                    Name="Natural Attractions"
                },
                new Category
                {
                    Name = "Cultural and Historical Sites"
                },
                new Category
                {
                    Name="Entertainment"
                },
                new Category
                {
                    Name="Religional"
                }, 
                
            };

            foreach (var item in Categories)
            {
                await _unitOfWork.Categories.AddAsync(item);
            }
            _unitOfWork.Complete();

            return Ok(Categories);
        }

        [HttpGet("SeedingTouristPlaces")]
        public async Task<IActionResult> SeedingTouristPlaces()
        {
            var natural = await _unitOfWork.Categories.FindAsync(c => c.Name == "Natural Attractions");
            int naturalId = natural.Id;

            var Cultural = await _unitOfWork.Categories.FindAsync(c => c.Name == "Cultural and Historical Sites");
            int CulturalId = Cultural.Id;

            var Entertainment = await _unitOfWork.Categories.FindAsync(c => c.Name == "Entertainment");
            int EntertainmentId = Entertainment.Id;

            var Religional = await _unitOfWork.Categories.FindAsync(c => c.Name == "Religional");
            int ReligionalId = Religional.Id;

            List<TouristPlace> TouristPlaces = new()
            {
                new TouristPlace
                {
                    Name="Great Pyramid of Gizas",
                    Description = "Counted among the most popular places to visit in Egypt, the Great Pyramid of Giza is the last attraction among the Seven Wonders of the Ancient World that have survived the rigorous test of time. The Great Pyramid of Giza happens to be the largest and the oldest of the 3 intriguing pyramids present in the Giza pyramid complex.\r\n\r\nAs per the beliefs of the Egyptologists, this pyramid is the tomb of pharaoh Khufu, the Fourth Dynasty Egyptian. The Great Pyramid of Giza remained as the world’s tallest man-made building for over 3,800 years.\r\n",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/574/original/1614240790_shutterstock_266356247.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Luxor Temple",
                    Description = "Constructed majorly by the pharaohs Ramses II (1279–1213 BC) and Amenhotep III (1390–1352 BC), the Luxor Temple is one of the most strikingly graceful monumental places to see in Egypt.\r\n\r\nAlso referred to as the Southern Sanctuary, this temple has received some modifications by King Tutankhamun, the Romans, and Alexander the Great over the passage of time. Declared as an UNESCO World Heritage Site, features beautiful sandstone walls that glow gorgeously during the sunset.\r\n",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/597/original/1614241809_shutterstock_1218880864.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Nile River Cruise",
                    Description = "Defining the real beauty of Egypt, the Nile river is one of the most photogenic places to visit in Egypt. The view of the deep blue waters and the breathtaking sunrise and sunset can leave any nature lover totally awestruck. The Nile River also offers the visitors excellent cruising opportunities, including multi-day cruise tours.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/569/original/1614240585_shutterstock_665374996.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Valley of the Kings",
                    Description = "Nestled on the west bank of the Nile and once known as the Place of Truth or the Great Necropolis of Millions of Years of Pharaoh, the Valley of the Kings is the primary place of burial of the Pharaohs of the 11th to 16th century BC.\r\n\r\nThis place is home to over 63 chambers and tombs that are adorned with intriguing artworks related to Egyptian mythology.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/593/original/1614241632_shutterstock_359457191.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Egyptian Antiquities Museum",
                    Description = "Also known as the Museum of Cairo and the Egyptian Museum, the Egyptian Antiquities Museum lies in downtown Cairo. Offering an insight of the Egyptian golden era, when the country was ruled by kings, this beautiful museum is home to many unique and rare artefacts, dating back to the prehistoric era.\r\n\r\nPacked with more than 120,000 historic items the Egyptian Antiquities Museum is one of the most recommended Egypt places to go for all history buffs.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/563/original/1614240295_shutterstock_153273740.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Siwa Oasis at Libyan Border",
                    Description = "Located in the western part of Egypt, far away from the hustle and bustle of the city, Siwa Oasis is one of the most serene places to visit in Egypt. Flanked by towering date palms and encompassed by several picturesque water springs, this charming little oasis makes for one of the most beautiful tourist spots in the Western Desert.\r\n\r\nThis place is surrounded by the remnants of a magnificent mud-brick fortification. Siwa Oasis can be the perfect base from where you can plan your adventure tours to the vast surrounding desert.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/656/original/1614250045_shutterstock_625870298.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="The White Desert in Farafra",
                    Description = "One of the most offbeat places to visit in Egypt is the White Desert, which features majestic chalk mountains that create the look of a snowy expanse amidst the arid land. The entire landscape is packed with stunning iceberg-shaped pinnacles and huge white boulders.\r\n\r\nIf you are done with exploring the tombs and temples of Egypt, pay a visit to the unique White Desert and soak in its scenic natural beauty",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/658/original/1614250193_shutterstock_124162276.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Sakkara Pyramids",
                    Description = "You must have heard about the Pyramids of Giza, but you will be glad to know that Egypt also has many other majestic pyramids in store for you. Just a few kilometers away from Cairo city, Sakkara Pyramids offer an insight of the architectural talent that flourished among the ancient Egyptians.\r\n\r\nThe Red Pyramid, Bent Pyramid, and Step Pyramid, together form the breathtaking Sakkara Pyramids that are standing tall to treat your eyes. There are also various old tombs with intricately adorned interior walls that will catch your attention, alongside the pyramids.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/564/original/1614240380_shutterstock_386088181.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Mosque of Mohamed Ali",
                    Description = "Better known as “Alabaster Mosque”, the Mosque of Mohamed Ali is an Islamic shrine, lying in the Citadel of Cairo city. Dating back to the 19th century, this mosque is believed to be one of the best places in Egypt not only for spiritual enthusiasts, but also for architecture buffs.\r\n\r\nFlaunting its twin minarets and animated silhouette, this mosque is considered to be the largest built structure in the 19th century. The Mosque of Mohamed Ali boasts an Ottoman style architectural design with impressively adorned interiors.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/566/original/1614240486_shutterstock_197480876.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Gezira Island",
                    Description = "Lying towards the western side of downtown Cairo and floating on the blue Nile river, the Gezira Island is one of the most romantic Egypt places to go. The northern part of this island features the Zamalek district and the southern part features the Gezira district.\r\n\r\nThe Gezira Island is connected by 4 bridges, namely the 15th of May Bridge, 6th October Bridge, Al-Galaa Bridge, and Qasr EL Nil Bridge. During the 19th century, the island was known as \"Jardin des Plantes (translating to \"Garden of Plants'' in English), owing to its bewildering collection of imported plants.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/572/original/1614240688_shutterstock_744354067.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Great Sphinx of Giza",
                    Description = "Featuring the face of a human being and the body of a lion, the 66 feet tall Great Sphinx of Giza stands on the western bank of the Nile river. Cut out from limestone, the Great Sphinx of Giza is considered to be Egypt’s oldest known monument.\r\n\r\nAlthough its original age and meaning has still not been decoded, the Sphinx remains as one of the best places to visit in Egypt for history as well as mythology enthusiasts.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/577/original/1614240910_shutterstock_1456340747.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Tiran Island",
                    Description = "Tiran Island is a stunning and almost ethereal piece of uninhabited land amidst the blue waters of the Red Sea, which is located on the maritime border of Saudi.\r\n\r\nAs the island is under the Ras Mohammed National Park, it is a very well protected Biosphere where you can enjoy some of the best underwater natural treasures like marine flora and fauna and naturally colorful reefs. It is one of the most recommended places to visit in Egypt, especially if snorkeling and diving is something you love.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/579/original/1614241036_shutterstock_1643503174.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="King Tutankhamun Museum",
                    Description = "The world’s most famous mummy is that of the Egyptian ruler King Tutankhamun. His coffin has been kept in a very carefully preserved state in the Grand Egyptian Museum of Cairo, which is famously referred to as King Tutankhamun Museum.\r\n\r\nSpread over 480,00 square meters of land just a few miles south of the Pyramid of Giza and considered to be one of the top-rated Egypt places to visit. King Tutankhamun Museum is one of the most intricate accounts of the Ancient Egyptian reign of Pharaohs.\r\n\r\nThe archeologists have also restored the throne of the king and kept it at the conservation center of the museum since August 2019.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/585/original/1614241231_shutterstock_1877541589.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Ras Muhammad National Park",
                    Description = "Ras Muhammad National Park is not only the most popular national park of the country, but is also one of the best places to visit in Egypt for diving. This park is nestled amidst the breathtaking mangroves and flamboyant coral reefs of the Red Sea and the inland Sinai desert.\r\n\r\nThe clear waters here offer divers the chance to easily witness the colourful corals and flourishing reef fishes. The Ras Muhammad National Park is also home to a myriad of White Storks and Risso’s Dolphins.\r\n",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/583/original/1614241127_shutterstock_1057585814.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Aqua Blue Water Park",
                    Description = "Packed with a total of 9 huge pools and 44 exhilarating water slides, Aqua Blue Water Park is one of the most recommended places to visit in Egypt for adventure enthusiasts. Some of the popular rides that are enjoyed by all adrenaline seekers visiting the park include the Tube Free Fall, the Twisters, the Rafting Slide, the Black Hole, and the Kamikaze.\r\n\r\nThe Aqua Blue Water Park is also equipped with a dedicated kid’s club, a basketball court, a tennis court, a massage room, and a billiards room.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/587/original/1614241342_shutterstock_95209888.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = EntertainmentId,
                },
                new TouristPlace
                {
                    Name="Old Market of Sharm",
                    Description = "The quaint streets of the Old Market of Sharm are packed with a myriad of colourful and lively souvenir shops, which makes this open-air market one of the best places in Egypt for shoppaholics.\r\n\r\nTourists visiting this flamboyant market can enjoy shopping for traditional textiles, ornaments, trinkets, spices, shisha pipes, traditional ahwa (coffee) joints, and saffron. After your shopping spree is over, you can relish your taste buds with some authentic delicacies available here, including seafood meals and  Egyptian barbecue.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/589/original/1614241435_shutterstock_254911135.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = EntertainmentId,
                },
                new TouristPlace
                {
                    Name="Karnak Temple",
                    Description = "Counted among the top historical tourist attractions in Egypt, the Karnak Temple is home to a number of popular buildings, including the Temple of Khons, the Festival Temple of Tuthmosis III, and the Great Temple of Amun.\r\n\r\nThe complex of this renowned temple also shelters the famous Konark Open Air Museum. Visitors can also find a number of age-old pylons, chapels, and remnants of decayed temples here. The Karnak Temple is also considered to be the world’s second largest ancient spiritual site.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/591/original/1614241536_shutterstock_1017481156.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Temple of Medinat Habu",
                    Description = "Also referred to as the “Mortuary Temple of Ramses III”, the Temple of Medinat Habu is one of the most famous religious places to visit in Egypt’s Luxor city. Although King Ramses III was buried in the Valley of Kings, the Medinet Habu was constructed in his honour.\r\n\r\nThe southeast corner of this monumental structure offers the best view of the whole complex. Some of the important attractions of the Medinet Habu include the Chapels of the Votaresses, the Second Pylon, Sacred Lake, the First Pylon, Nilometer, &  Hypostyle Hall.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/595/original/1614241714_shutterstock_170841761.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Khan-el-Khalili",
                    Description = "Laid down in the 14th century and considered to be one of the best places to visit in Egypt for shoppaholics, Khan-el-Khalili has its name among the largest markets of the world. This colourful open market has a very chirpy and vibrant atmosphere, with vendors selling a number of local items and customers bargaining at their best.\r\n\r\nSpread across a huge area, the stores and shops here offer all kinds of souvenirs, including semi precious stoneworks, miniature pyramids, toy camels, silverware, gold artifacts, stained glass lamps, antiques, copperware, handmade carpets, and incense.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/598/original/1614241894_shutterstock_1087783190.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="The Egyptian Museum",
                    Description = "The Egyptian Museum in Cairo (EMC) is the oldest archaeological museum in the Middle East. It is famous for its mind-boggling collection of more than 170,000 artefacts collected from the Predynastic Period to the Greco-Roman Era. The museum has the highest number of treasured pieces of ancient Egyptian history including Tutankhamun's treasures.",
                    Image = "https://media1.thrillophilia.com/filestore/5444t0h3olcye63mjz5pe4u65fbr_1613991867_shutterstock_482978608.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Al Azhar Mosque",
                    Description = "Immerse yourself in the rich history and architectural magnificence of the Al Azhar mosque in Cairo. The Al Azhar mosque is one of the oldest mosques in the world and was built in the 10th century. You can marvel at the towering minarets, sprawling courtyards, and sacred prayer halls. The Al Azhar University within the mosque is one of the best institutions in the world to obtain Islamic scholarship.\r\n\r\nDiscover the magic of Al Azhar Mosque and explore other attractions of Egypt with our cu",
                    Image = "https://media1.thrillophilia.com/filestore/676tysj5ft0rc3muyezwg70k2yws_1613992253_shutterstock_745082155.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = ReligionalId,
                },
                new TouristPlace
                {
                    Name="The Hanging Church",
                    Description = "Also referred to as the “Church of Saint Virgin Mary”, the iconic Hanging Church of Cairo happens to be a stunning stone facade, flaunting intricate inscriptions with Arabic and Coptic marks. Established back in the 3rd century AD, the Hanging Church is one of the oldest churches in the world.\r\n\r\nThe church is named as the “Hanging Church” as it is constructed at the top of the gatehouse of the Babylon Fortress and seems to be hanging mid-air.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/607/original/1614246038_shutterstock_1900092145.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = ReligionalId,
                },
                new TouristPlace
                {
                    Name="Museum Of Modern Egyptian Art",
                    Description = "Nestled on the Gezira Island and better known as the Gezira Center for Modern Art, the Museum Of Modern Egyptian Art is an integral part of the National Cultural Center. Featuring an impressive collection of contemporary Egyptian arts crafted between the 20th and 21st centuries, this magnificent museum is one of the favorite places to see in Egypt among all art lovers.\r\n\r\nSome of the popular iconic artworks sheltered in the museum include “Bride of the Nile” by Mahmoud Mukhtar and “Al Madina” by Mahmoud Said. Visitors can also spot brilliant artworks by other legendary artists, including Gauguin, Monet, and Van Gogh.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/608/original/1614246290_shutterstock_1261105237.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Cairo Tower",
                    Description = "Located at Sharia Hadayek Al Zuhreya Gezira, the 187 meters tall Cairo Tower is known to be one of the most visited tourist attractions in Egypt. Showcasing a stunning architectural design, this towering structure offers mesmerizing panoramic views of the entire Cairo city from its top floor.\r\n\r\nBuilt between 1954 and 1961, the tower flaunts brilliant latticework that offers an insight into the excellent Arab craftsmanship of the times gone by. At present, Cairo Tower is home to many elegant nightclubs and restaurants.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/610/original/1614246455_shutterstock_1125265670.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = EntertainmentId
                },
                new TouristPlace
                {
                    Name="Grand Egyptian Museum",
                    Description = "Also referred to as the “Giza Museum”, the Grand Egyptian Museum is a magnificent archaeological museum, which is under construction in the city of Giza. Supposed to be the world’s largest archaeological museum, this museum will be home to ancient Egyptian artefacts, including collections of King Tutankhamun.\r\n\r\nTo be constructed over a land of around 480,000 square metres, the architecture of the museum will resemble a chamfered triangular shape.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/612/original/1614246690_June5_gem11500x1000.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Pyramid of Khafre",
                    Description = "Considered to be the 2nd largest and 2nd tallest among the three Pyramids of Giza, Pyramid of Khafre is the tomb of pharaoh Khafre of the Fourth-Dynasty, who ruled between 2558 and 2532 BC. Spanning over a base of 215 meters and rising to a height of 136 meters, this majestic pyramid is coated with Tura limestone.\r\n\r\nThe structure’s inner portions feature intriguing chambers that have been cut out of limestones. The Pyramid of Khafre has 2 entrances, one lying at the ground level and the other lying at a height of 11 meters.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/613/original/1614246787_shutterstock_1057017011.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Giza Solar Boat Museum",
                    Description = "Built in 1985, Giza Solar Boat Museum was made to shelter the boat, which was placed there after the death of King Cheops. The boat was disassembled and buried, only to be found again and restored.\r\n\r\nThis 44 meters long boat made of Cedar wood is believed to have been used by him to sail down river Nile in his afterlife.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/615/original/1614246876_shutterstock_1099599260.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Red and Bent Pyramid",
                    Description = "One of the earliest pyramids to be built, the Bent Pyramid was a creation of one of the Founding figureheads of the Pharaohs known as Sneferu. The very uniquely designed pyramid is a stark contrast to the many other pyramids constructed during later years.\r\n\r\nAlso known as the Red Bent Pyramid of Dahshur, this architectural marvel has 3 funeral chambers and the 3rd chamber has a very unusual feature. The red stones of the roof are designed with gradual overhangs, which results in a very unique pattern not visible in any of the other Pyramids.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/616/original/1614247022_shutterstock_245214079.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Egypt Papyrus Museum",
                    Description = "Exhibiting and selling brilliant artworks crafted on a parchment derived from the cyperus papyrus plant, the Egypt Papyrus Museum is one of the most quintessential places to visit in Egypt for every tourist craving to buy some unique souvenirs before leaving Egypt.\r\n\r\nThe brilliant handmade paintings drawn on the barks of the papyrus trees will leave you awestruck at the excellent local craftsmanship.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/617/original/1614247129_shutterstock_1414780067.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Pyramid of Djoser",
                    Description = "Considered to be one of the less explored places to visit in Egypt, Pyramid of Djoser offers you the chance to explore a typical Egyptian pyramid minus the regular crowd. Lying south of Cairo, in Saqqara town, Djoser Pyramid is known to be the most ancient stone-cut monuments of the earth.\r\n\r\nDating back to the 27th century BC, this pyramid was constructed by leveraging a unique step-style architectural design, which predates the Pyramids of Giza that feature a smooth-sided architecture.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/620/original/1614247215_shutterstock_1771907882.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Luxor Museum",
                    Description = "The Luxor Museum exhibits a brilliant collection of ancient antiquities that date back to the period between the Old Kingdom and the Mamluk period. Collected majorly from the Theban necropolis and temples, this museum rewards your eyes with a myriad of masterpieces.\r\n\r\nSome of the impressive collections of the Luxor Museum include a carved figurine of Tuthmosis III, a limestone relief of Tuthmosis III,  and an alabaster statue of Amenhotep III.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/621/original/1614247296_shutterstock_1284196573.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Mortuary Temple of Hatshepsut - Deir el-Bahari",
                    Description = "The beautiful Mortuary temple of Hatshepsut is nestled on the western banks of river Nile, at the foothills of the Libyan plateau. Dedicated to the sun god, Amon-Ra, this temple is made of limestone and showcases classical architectural design.\r\n\r\nThe Mortuary temple of Hatshepsut was built by a very skilled architect of that time, Senimut in honour of the glorious achievements of Queen Hatsheput. The temple features 3 impressive terraces, a hypostyle hall, courts, and pylons.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/622/original/1614247426_shutterstock_561452176.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Valley of Artisans",
                    Description = "Valley of Artisans is one of the top places to visit in Egypt for seeing the iconic Egyptian tombs. Also known as Dier el-Medina, it is beautifully crafted with workers' dwellings and magnificently illustrated tombs. Located along the western bank of River Nile, it is just a few minutes away from the Colossi of Memnon and Medinet Habu.\r\n\r\nYou will find beautifully decorated, vivid colors and scenes from daily life. You can plan to visit various tombs like Shuroy, Roy, Anker Ka, and Sennutem within close distance of each other while exploring the Valley of Artisans.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/624/original/1614247521_shutterstock_1383777110.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Avenue of Sphinxes",
                    Description = "Nestled between the Luxor Temple and the Karnak Temple complex, the Avenue of Sphinxes is the place where various commercial processions were once held. Dating back to 380 BC, the Avenue of Sphinxes is about 2.7 kilometers long, lined by a total of 1,350 sphinxes.\r\n\r\nThe most popular among all the sphinxes is the Great Sphinx at Giza, which bears cat-like features. The other sphinxes lining this avenue feature rams’ heads.\r\n",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/627/original/1614247627_shutterstock_1302362185.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Colossi of Memnon",
                    Description = "Making for one of the popular historical tourist attractions in Egypt, Colossi of Memnon refers to 2 magnificent statues that guarded the temple of King Amenhotep III in the times gone by. These 59 feet tall statues approximately weigh 700 tons and depict the king.\r\n\r\nMade of quartz sandstone, the statues feature fine carvings near their legs, which represent the mother and the wife of King Amenhotep III. Although the faces of both the statues have been damaged, yet these figurines remain as a major attraction, owing to their imposing size.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/631/original/1614247784_shutterstock_1627612756.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Temple of Seti",
                    Description = "Lying on the outskirts of the city of Luxor, the Temple of Seti is one of the least explored attractions of the country. This temple has been named after Seti I, the Egyptian ruler, who began constructing the building, but could not complete it.\r\n\r\nAlthough the temple is a bit damaged at the present day, yet its walls retain the beautiful carvings featuring Seti I and Ramses II and the gods.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/634/original/1614247961_shutterstock_1655150488.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Coptic Orthodox Church of Alexandria",
                    Description = "The Coptic Orthodox Church of Alexandria happens to be an Oriental Orthodox Christian church, which once served as the seat of the head of the Coptic Orthodox Church, the Pope of Alexandria.\r\n\r\nThis church is believed to be standing on the same place where the evangelist and apostle, Saint Mark had founded a church during the 1st century. The church showcases impressive basilique style architectural design with stunning Coptic engravings.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/636/original/1614248059_shutterstock_1894504417.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = ReligionalId,
                },
                new TouristPlace
                {
                    Name="Bibliotheca Alexandrina - Library in Alexandria",
                    Description = "Established as recently as in 2002, Bibliotheca Alexandrina is a very inspiring hub of knowledge. Currently featuring over 1.5 million educational items, the library of Bibliotheca Alexandrina is one of the best Egypt places to visit for readers and students.\r\n\r\nThis huge educational gallery is home to millions of important books, a planetarium, 4 museums, and 6 specialist libraries.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/639/original/1614248153_shutterstock_229036063.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = EntertainmentId,
                },
                new TouristPlace
                {
                    Name="Citadel of Qaitbay or The Fort of Qaitbay",
                    Description = "Nestled on the coast of the Mediterranean Sea, the Citadel of Qaitbay was built as a defensive fortress back in the 15th century. Constructed over the remnants of the iconic Pharos lighthouse built by Mamluk sultan Qaitbey, the citadel features bulky walls and a robust architecture.\r\n\r\nTransformed into a maritime museum back in 1952, the fortification not only offers an insight of the rich architecture and history of Egypt, but also exhibits fossilized marine creatures.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/640/original/1614248247_shutterstock_1621723867.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Alexandria National Museum",
                    Description = "Spread across three floors, the stunning Alexandria National Museum offers an insight of the rich heritage and glorious past of the Egyptian city of Alexandria. Inaugurated in 2003, this museum is home to around 1,800 artefacts that speak about Alexandria through the Islamic, Roman, Pharaonic, and Coptic ages.\r\n\r\nThe museum also has precious jewels, glassware, chinaware, and silverware, dating back to the 19th century.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/641/original/1614248356_shutterstock_1793043298.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="Stanley Bridge in Alexandria",
                    Description = "Built over the Stanley Bay, the picture-perfect Stanley Bridge makes for an important landmark in the city of Alexandria. Featuring 6 spans, the bridge extends up to a length of about 400 meters.\r\n\r\nConsidered to be the country’s first bridge to be built into the sea, the Stanley Bridge is an engineering masterpiece, showcasing the modern Egyptian style architectural design. Tourists visiting the Stanley Bridge can spend time soaking in the breathtaking views of the gorgeous bay and the pristine beaches of Alexandria.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/642/original/1614248451_shutterstock_106437776.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = EntertainmentId,
                },
                new TouristPlace
                {
                    Name="The Blue Hole of Dahab",
                    Description = "Notably one of the most daredevil places to visit in Egypt, The Blue Hole of Dahab is not for the faint-hearted individuals. The Blue Hole happens to be the world’s most challenging diving location situated a few kilometers north of Dahab in Southeast Sina.\r\n\r\nKnown for its breathtaking underwater and aerial view of the Red Sea, it is not easy to miss out. Originally a marine sinkhole with a depth of over 328 feet or 100 meters, it is a common free diving spot for adventure and thrill-seeking travelers visiting Egypt. It is also a famous spot for snorkeling and diving if you want to spot rare turtle species, corals, and reef sharks.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/643/original/1614248585_shutterstock_643683376.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Mount Sinai",
                    Description = "Mount Sinai is known popularly for being the spot where God granted Moses the ten commandments. Considered to be one of the most suggested Egypt places to visit for adventure junkies, this majestic, rugged mountain offers excellent hiking opportunities.\r\n\r\nOnce they reach the summit, the hikers can soak in the panoramic views of the picturesque South Sinai mountain range. Mount Sinai is also known to offer breathtaking views of sunrise and sunset.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/644/original/1614248753_shutterstock_1018991782.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = CulturalId,
                },
                new TouristPlace
                {
                    Name="St. Catherine's Monastery",
                    Description = "Constructed between 527 and 565 AD, St. Catherine's Monastery is one of the oldest spiritual places to visit in Egypt. Nestled at the foothills of Mt. Sinai, this monastery has been named after St. Catherine, who had been tortured and killed for her belief in Christianity.\r\n\r\nThis beautiful desert monastery shelters an amazing collection of spiritual iconography, manuscripts, art, and the iconic burning bush. You can also climb Mt. Sinai and enjoy the breathtaking sunset or sunrise views, after paying homage at St. Catherine's Monastery.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/645/original/1614248834_shutterstock_416863516.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = ReligionalId,
                },
                new TouristPlace
                {
                    Name="Dahab's Eel Garden",
                    Description = "Dahab’s Eel Garden is one of the most alluring Egypt places to visit, especially for the marine enthusiasts. It has derived its name from many garden eels that accommodate seafloor just a few meters away from the diving entry point of Assalah Shore at around 10 to 20-meter depth.\r\n\r\nA very unusual phenomenon, the garden eels in this diving spot are a rare beauty one cannot miss out on. In addition to the eels, it is also the perfect place to spot a huge number of barracudas that live permanently here.. You can also enjoy rare seagrass, ghost pipefish, hard and soft corals, and even huge coral boulders.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/646/original/1614248958_shutterstock_90241177.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = EntertainmentId,
                },
                new TouristPlace
                {
                    Name="Coloured Canyon",
                    Description = "Lying near Nuweiba town, on the Sinai peninsula, the Coloured Canyon is a thin slot canyon. The Coloured Canyon got its name from its gorgeous array of colours that make it look unique and fascinating. This 800 meters long canyon evolved as a result of water erosion over millions of years.\r\n\r\nThe canyon is flanked by 40 meters high sandstone walls that flaunt various shades of colours, starting from straw yellow and red to dark brown and black. The hues are a result of the presence of iron oxides and magnesium oxides.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/647/original/1614249039_shutterstock_468457970.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Great Sand Sea in Siwa",
                    Description = "Nestled between eastern Libya and western Egypt, the Great Sand Sea is a huge area of sand dunes. Covering a gigantic area of around 72,000 sq. km., the Great Sand Sea makes for one of the largest dune fields in the world.\r\n\r\nThis place is home to some of the world’s largest dunes, including barchan, seif, and crescent dunes. These hypnotic sand dunes offer visitors the chance to indulge in a number of fun activities, such as fossil hunting, dune drives, and sand sledding.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/648/original/1614249168_shutterstock_1532842865.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Magic Lake in Fayoum",
                    Description = "Lying in Wadi El Hitan in Fayoum, the picturesque Magic Lake offers a truly gorgeous sight to behold. The lake is named as “Magic Lake'' because of the fact that it tends to alter its colour multiple times a day, based upon the amount of sun rays it receives and the time of the day it is.\r\n\r\nEncompassed by a sandy desert, the Magic Lake is also believed to contain essential minerals. Going for a swim in this lake can not only offer you the much-needed rejuvenation, but can also treat rheumatism",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/652/original/1614249727_shutterstock_774137044.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                
                new TouristPlace
                {
                    Name="Fjord Bay",
                    Description = "Located around 15 km in the south of Taba, Fjord Bay is one of the popular Egypt places to visit for water activities, including swimming and snorkeling. The bay is surrounded by well-protected coral reefs and spectacularly colorful underworld scenery.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/655/original/1614249968_shutterstock_571874752.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                
                new TouristPlace
                {
                    Name="Fortress of Shali in Siwa Oasis",
                    Description = "Lying in central Siwa, the Fortress of Shali is a beautiful mud-brick fortification, dating back to the 13th century. Made of kershef (an amalgamation of clay and salt rock) and once sheltered hundreds of people, the fortress is in its ruins now.\r\n\r\nBuilt for defending the region against foreign invasions, the fortress originally was a four to five storeyed building. The Fortress of Shali invites many tourists at the present day to soak in its mesmerizing panoramic views of the Nile river.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/657/original/1614250125_shutterstock_778951327.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                
                new TouristPlace
                {
                    Name="Philae Island",
                    Description = "Philae Island is a famous temple island that is situated on the serene Nile River. Located between Aswan High Dam and the old Aswan Dam this is a very important archeological site of various ancient temples and shrines of Egypt.\r\n",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/659/original/1614250265_shutterstock_1240835554.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                },
                new TouristPlace
                {
                    Name="Mahmya Island",
                    Description = "Situated in Hurghada, Mahmya Island is one of the most amazing places to visit in Egypt for beach lovers. The white sandy beaches and crystal clear water are a crowd favorite of every tourist visiting this exotic island. You will find various bars and restaurants along with a chance to enjoy the stunning nightlife while spending time in Mahmya.\r\n\r\nAnother popular reason for visiting Mahmya Island is it is one of the perfect places for snorkel and swimming. Being a protected bio reserve, this island has some of the best underwater scenic beauty of Egypt.",
                    Image = "https://media2.thrillophilia.com/images/photos/000/367/660/original/1614250357_shutterstock_314403725.jpg?w=753&h=450&dpr=1.0",
                    CategoryId = naturalId,
                }
            };

            foreach (var item in TouristPlaces)
            {
                await _unitOfWork.TouristPlaces.AddAsync(item);
            }
            _unitOfWork.Complete();

            return Ok(TouristPlaces);
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
