using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Core.DTOs
{
  public class CreateUserDto
  {
    public string UsereName { get; set; }
    public string EMail { get; set; }
    public string Password { get; set; }
  }
}
