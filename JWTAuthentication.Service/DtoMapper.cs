using AutoMapper;
using JWTAuthentication.Core.DTOs;
using JWTAuthentication.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Service
{
  internal class DtoMapper : Profile
  {
    public  DtoMapper()
    {
      CreateMap<ProductDTO,Product>().ReverseMap();
      CreateMap<UserAppDto,UserApp>().ReverseMap();
    }
  }
}
