using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Service.Services
{
   static class SignService
  {
    public static SecurityKey GetSymetricSecurityKey(string securityKey)
    {
      return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
  }
}
