using JWTAuthentication.Core.DTOs;
using JWTAuthentication.Core.Models;
using JWTAuthentication.Core.Service;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Service.Services
{
  public class UserService : IUserService
  {
    private readonly UserManager<UserApp> _userManager;

    public UserService(UserManager<UserApp> userManager)
    {
      _userManager=userManager;
    }
    public async Task<ResponseDto<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
      var user = new UserApp { Email = createUserDto.EMail, UserName = createUserDto.UsereName };
      var result = await _userManager.CreateAsync(user,createUserDto.Password);

      if(!result.Succeeded)
      {
        var errors = result.Errors.Select(x => x.Description).ToList();
        return ResponseDto<UserAppDto>.Fail(400,new ErrorDto(errors,true));
      }

      return ResponseDto<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
    }

    public async Task<ResponseDto<UserAppDto>> GetUserByNameAsync(string userName)
    {
      var user = await _userManager.FindByNameAsync(userName);
      if(userName is null)
      {
        return ResponseDto<UserAppDto>.Fail(404,"Username not found",true);
      }

      return ResponseDto<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
    }
  }
}
