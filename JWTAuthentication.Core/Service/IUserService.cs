using JWTAuthentication.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Core.Service
{
  public interface IUserService
  {
    Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    Task<ResponseDto<UserAppDto>> GetUserByNameAsync(string userName);
  }
}
