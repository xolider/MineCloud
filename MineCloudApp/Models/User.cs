using System;
using System.Collections.Generic;
using System.Text;

namespace MineCloudApp.Models
{
    class User : IUser
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }
    }
}
