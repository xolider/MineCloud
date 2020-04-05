using System;
using System.Collections.Generic;
using System.Text;

namespace MineCloudApp.Models
{
    class User : IUser
    {
        public string Username { get; private set; }

        public string Email { get; private set; }

        public int Id { get; private set; }

        public User()
        {

        }
    }
}
