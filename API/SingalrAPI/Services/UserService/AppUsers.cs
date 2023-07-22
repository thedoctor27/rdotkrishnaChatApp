namespace SingalrAPI.Services.UserService
{
    public class AppUser
    {
        //user1 / user2 / user3 / user4

        public string Id { get; set; }
        public string UserName { get; set; }

        public static readonly List<AppUser> appUsers = new List<AppUser> {
            new AppUser
            {
                Id="7786160377",
                UserName="user1"
            },
            new AppUser
            {
                Id="2558687734",
                UserName="user2"
            },
            new AppUser
            {
                Id="2481080908",
                UserName="user3"
            },
            new AppUser
            {
                Id="8195803519",
                UserName="user4"
            },
        };
    }
}
