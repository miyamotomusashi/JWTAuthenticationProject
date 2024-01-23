using JWTAuthentication.Core.DTOs;
using JWTAuthentication.Core.Repositories;
using JWTAuthentication.Core.Service;
using JWTAuthentication.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Service.Services
{
  public class GenericService<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _repository;

    public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
    {
      _unitOfWork = unitOfWork;
      _repository = repository;
    }

    public async Task<ResponseDto<TDto>> AddAsync(TDto entity)
    {
      var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
      await _repository.AddAsync(newEntity);
      await _unitOfWork.CommitAsync();

      var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
      return ResponseDto<TDto>.Success(newDto,200);
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> GetAllAsync()
    {
      var products = ObjectMapper.Mapper.Map<List<TDto>>(await _repository.GetAllAsync());
      return ResponseDto<IEnumerable<TDto>>.Success(products,200);
    }

    public async Task<ResponseDto<TDto>> GetByIdAsync(int id)
    {
      var product = await _repository.GetByIdAsync(id);
      if(product is null)
      {
        return ResponseDto<TDto>.Fail(404, "Id not found",true);
      }

      var productDto = ObjectMapper.Mapper.Map<TDto>(product);
      return ResponseDto<TDto>.Success(productDto, 200);

    }



    public async Task<ResponseDto<NoDataDto>> Remove(int id)
    {
      var isExistEntity = await _repository.GetByIdAsync(id);
      if(isExistEntity is null)
      {
        return ResponseDto<NoDataDto>.Fail(404, "Id not found", true);
      }

      _repository.Remove(isExistEntity);
      await _unitOfWork.CommitAsync();
      return ResponseDto<NoDataDto>.Success(200);
    }

    public async Task<ResponseDto<NoDataDto>> Update(TDto entity,int id)
    {
      var isExistEntity = await _repository.GetByIdAsync(id);
      if (isExistEntity is null)
      {
        return ResponseDto<NoDataDto>.Fail(404, "Id not found", true);
      }

      var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);
      _repository.Update(updateEntity);
      await _unitOfWork.CommitAsync();
      return ResponseDto<NoDataDto>.Success(204);
    }

    public async Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
      var list = _repository.Where(predicate);
      return ResponseDto<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()),200);
    }
  }
}
