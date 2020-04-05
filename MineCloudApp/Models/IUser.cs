﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MineCloudApp.Models
{
    public interface IUser
    {
        public string Username { get; }

        public string Email { get; }

        public int Id { get; }
    }
}
