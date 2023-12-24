using JWTAuthentication.Core.Configuration;
using JWTAuthentication.Core.DTOs;
using JWTAuthentication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Core.Service
{
  public interface ITokenService
  {
    TokenDTO Createtoken(UserApp userApp);
    ClientTokenDto CreateTokenByClient(Client client);
  }
}
