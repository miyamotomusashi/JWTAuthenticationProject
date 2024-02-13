using JWTAuthentication.Core.Configuration;
using JWTAuthentication.Core.DTOs;
using JWTAuthentication.Core.Models;
using JWTAuthentication.Core.Repositories;
using JWTAuthentication.Core.Service;
using JWTAuthentication.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Service.Services
{
  public class AuthenticationService : IAuthenticationService
  {

    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserApp> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

    public AuthenticationService(IOptions<List<Client>> clients, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
    {
      _clients = clients.Value;
      _tokenService = tokenService;
      _userManager = userManager;
      _unitOfWork = unitOfWork;
      _userRefreshTokenRepository = userRefreshTokenRepository;
    }

    public async Task<ResponseDto<TokenDTO>> CreateTokenAsync(LoginDto loginDto)
    {
      if(loginDto is null)
      {
        throw new ArgumentException(nameof(loginDto));
      }

      var user = await _userManager.FindByEmailAsync(loginDto.Email);
      if(user is null)
      {
        return ResponseDto<TokenDTO>.Fail(400,"Email or password is wrong.",true);
      }
      if(!(await _userManager.CheckPasswordAsync(user,loginDto.Password)))
      {
        return ResponseDto<TokenDTO>.Fail(400, "Email or password is wrong.", true);
      }

      var token = _tokenService.Createtoken(user);

      var userrefreshtoken = await _userRefreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
      if(userrefreshtoken is null)
      {
        await _userRefreshTokenRepository.AddAsync(new UserRefreshToken() { UserId=user.Id,Code = token.RefreshToken,Expiration=token.RefreshTokenExpiration });
      }
      else
      {
        userrefreshtoken.Code = token.RefreshToken;
        userrefreshtoken.Expiration = token.RefreshTokenExpiration;
      }

      await _unitOfWork.CommitAsync();

      return ResponseDto<TokenDTO>.Success(token,200);
    }

    public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
      var client = _clients.SingleOrDefault(x => x.ClientId == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
      if(client is null)
      {
        return ResponseDto<ClientTokenDto>.Fail(404,"ClientId or clientsecret not found",true);
      }

      var token = _tokenService.CreateTokenByClient(client);
      return ResponseDto<ClientTokenDto>.Success(token,200);
    }

    public async Task<ResponseDto<TokenDTO>> CreateTokenByRefreshTokenAsync(string refreshToken)
    {
     var existrefreshToken = await _userRefreshTokenRepository.Where(x=>x.Code==refreshToken).SingleOrDefaultAsync();
      if(existrefreshToken is null)
      {
        return ResponseDto<TokenDTO>.Fail(404,"Refresh token not found", true);
      }

      var user = await _userManager.FindByIdAsync(existrefreshToken.UserId);
      if(user is null)
      {
        return ResponseDto<TokenDTO>.Fail(404, "Userid not found", true);
      }

      var token = _tokenService.Createtoken(user);
      existrefreshToken.Code = token.RefreshToken;
      existrefreshToken.Expiration = token.RefreshTokenExpiration;

      await _unitOfWork.CommitAsync();
      return ResponseDto<TokenDTO>.Success(token, 200);

    }

    public async Task<ResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
      var existrefreshToken = await _userRefreshTokenRepository.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
      if(existrefreshToken is null)
      {
        return ResponseDto<NoDataDto>.Fail(404, "Refresh token not found", true);
      }

      _userRefreshTokenRepository.Remove(existrefreshToken);
      _unitOfWork.CommitAsync();

      return ResponseDto<NoDataDto>.Success(200);

    }
  }
}
