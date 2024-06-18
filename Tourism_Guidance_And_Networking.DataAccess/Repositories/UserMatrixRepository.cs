namespace Tourism_Guidance_And_Networking.DataAccess.Repositories;

public class UserMatrixRepository : BaseRepository<UserMatrix>, IUserMatrix
{
    private new readonly ApplicationDbContext _context;
    public UserMatrixRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public List<UserMatrix> CreateAllUserMatrices()
    {
        List<UserMatrix> userMatrixList = new()
        {
            // user1 --> 60728638-96b8-4576-ac14-da785002ee04 (High rating, Low Price, Apartment or Villa)
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(0),"VIEW BOOK"),//10, 2761
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(43),"VIEW"),//10, 13905
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(41),"VIEW"),//7, 15450
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(94),"LIKE"),         // 8553
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(1581),"VIEW BOOK"),  // 8.4 , 1911

            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(1578),"SAVE"),       // 8.5,  3808
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(1552),"VIEW BOOK"),  // 8.6, 3708
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(1520),"BOOK"),       // 8.8, 2781
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(155),"LIKE"),        // 4635
            CreateUserMatrix("60728638-96b8-4576-ac14-da785002ee04",ScaleItemId(198),"SAVE"),        // 8.9 6489

            //User 2 --> 60728638-96c8-4576-ac14-da785002ee04 (High rating, Any Price, Hotel)
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(1437),"VIEW BOOK"),     //9.4, 3980, Hurghada 
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(2700),"VIEW"),     //10, Cairo
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(3039),"SAVE"),     // 8.8, Hur
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(2958),"LIKE"),     //7.1, hur
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(2852),"VIEW BOOK"),     //9.4, hur

             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(1045),"VIEW BOOK"),         //9.4, hur
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(1050),"LIKE"),         // 8.6, port
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(2693),"SAVE"),    //8.6,port
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(2446),"VIEW BOOK"),    //9.4,hur
             CreateUserMatrix("60728638-96c8-4576-ac14-da785002ee04",ScaleItemId(2001),"BOOK"),         //10, hur

             //User 3 --> 60728638-96d8-4576-ac14-da785002ee04 (Luxor and aswan)
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(2010),"LIKE"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(1635),"BOOK"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(1589),"VIEW BOOK"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(1490),"VIEW BOOK"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(1139),"SAVE"),
             //ASWAN           
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(902),"VIEW"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(873),"LIKE"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(599),"BOOK"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(599),"VIEW BOOK"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(1441),"VIEW BOOK"),
             CreateUserMatrix("60728638-96d8-4576-ac14-da785002ee04",ScaleItemId(2042),"SAVE"),

             //User 4 --> 60728638-96e8-4576-ac14-da785002ee04 (3 adult, alexandria)
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(1119),"BOOK"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(1090),"LIKE"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(1090),"VIEW BOOK"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(384),"VIEW"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(18),"VIEW"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(18),"LIKE"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(477),"VIEW"),

              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(498),"VIEW"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(4694),"LIKE"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(922),"VIEW"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(3037),"SAVE"),
              CreateUserMatrix("60728638-96e8-4576-ac14-da785002ee04",ScaleItemId(2369),"VIEW BOOK"),

              //User 5 --> 60728638-96f8-4576-ac14-da785002ee04
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2252),"SAVE"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2252),"BOOK"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2337),"SAVE"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(1916),"LIKE"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(1916),"VIEW BOOK"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2231),"VIEW BOOK"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2478),"SAVE"),

              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2040),"VIEW"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(1968),"VIEW"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(1968),"SAVE"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(2183),"VIEW BOOK"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(1514),"LIKE"),
              CreateUserMatrix("60728638-96f8-4576-ac14-da785002ee04",ScaleItemId(1967),"VIEW")
        };
        return userMatrixList;
    }
    private static UserMatrix CreateUserMatrix(string userId, int itemId, string action)
    {
        UserMatrix userMatrix = new()
        {
            UserID = userId,
            ItemID = itemId,
            Action = action
        };
        return userMatrix;
    }
    private static int ScaleItemId(int id)
    {
        return id + 10_000;
    }
}
