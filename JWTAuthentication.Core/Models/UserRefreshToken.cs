﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Core.Models
{
  public class UserRefreshToken
  {
        public string UserId { get; set; }
        public decimal Code { get; set; }
        public DateTime Expiration { get; set; }
    }
}
