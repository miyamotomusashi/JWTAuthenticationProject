using JWTAuthentication.Core.Configuration;
using JWTAuthentication.Core.DTOs;
using JWTAuthentication.Core.Models;
using JWTAuthentication.Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Service.Services
{
  public class TokenService : ITokenService
  {
    private readonly UserManager<UserApp> _userManager;
    private readonly CustomTokenOptions _tokenOptions;

    public TokenService(UserManager<UserApp> usernamager, IOptions<CustomTokenOptions> options)
    {
      _userManager = usernamager;
      _tokenOptions = options.Value;

    }
    public TokenDTO Createtoken(UserApp userApp)
    {
      var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
      var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
      var securityKey = SignService.GetSymetricSecurityKey(_tokenOptions.SecurityKey);
      SigningCredentials signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

      JwtSecurityToken jwtSecurtiyToken = new JwtSecurityToken(
        issuer: _tokenOptions.Issuer,
        expires: accessTokenExpiration,
        notBefore: DateTime.Now,
        claims: GetClaim(userApp, _tokenOptions.Audience),
        signingCredentials: signingCredential
        );

      var handler = new JwtSecurityTokenHandler();
      var token = handler.WriteToken(jwtSecurtiyToken);

      var tokenDto = new TokenDTO
      {
        AccessToken = token,
        RefreshToken = CreateRefreshToken(),
        AccessTokenExpiration = accessTokenExpiration,
        RefreshTokenExpiration = refreshTokenExpiration
      };

      return tokenDto;
    }

    public ClientTokenDto CreateTokenByClient(Client client)
    {
      var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
      var securityKey = SignService.GetSymetricSecurityKey(_tokenOptions.SecurityKey);
      SigningCredentials signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

      JwtSecurityToken jwtSecurtiyToken = new JwtSecurityToken(
        issuer: _tokenOptions.Issuer,
        expires: accessTokenExpiration,
        notBefore: DateTime.Now,
        claims: GetClaimsByClient(client),
        signingCredentials: signingCredential
        );

      var handler = new JwtSecurityTokenHandler();
      var token = handler.WriteToken(jwtSecurtiyToken);

      var tokenDto = new ClientTokenDto
      {
        AccessToken = token,
        AccessTokenExpiration = accessTokenExpiration
      };

      return tokenDto;
    }

    private string CreateRefreshToken()
    {
      var numberByte = new byte[32];
      using var rnd = RandomNumberGenerator.Create();
      rnd.GetBytes(numberByte);
      return Convert.ToBase64String(numberByte);
      //return Guid.NewGuid().ToString();
    }

    IEnumerable<Claim> GetClaim(UserApp userApp, List<string> audiences)
    {
      var userlist = new List<Claim>()
      {
        new Claim(ClaimTypes.NameIdentifier,userApp.Id),
        new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
        new Claim(ClaimTypes.Name,userApp.UserName),
        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
      };

      userlist.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

      return userlist;

    }


    IEnumerable<Claim> GetClaimsByClient(Client client)
    {
      var claims = new List<Claim>();
      claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
      new Claim(JwtRegisteredClaimNames.Sub, client.ClientId.ToString());
      return claims;

    }
  }
}
