using JWTAuthentication.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Core.Service
{
  public  interface IAuthenticationService
  {
    Task<ResponseDto<TokenDTO>> CreateTokenAsync(LoginDto loginDto);
    Task<ResponseDto<TokenDTO>> CreateTokenByRefreshTokenAsync(string refreshToken);
    Task<ResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken);
    Task<ResponseDto<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLoginDto);
  }
}
