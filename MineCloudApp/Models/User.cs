namespace MineCloudApp.Models
{
    class User : IUser
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }

        public static IUser CurrentUser { get; set; }
    }
}
