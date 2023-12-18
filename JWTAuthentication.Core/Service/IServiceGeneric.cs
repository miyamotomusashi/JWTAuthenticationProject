using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Core.Service
{
  public interface IServiceGeneric<TEntity,TDto> where TEntity : class where TDto: class
  {
    Task<ResponseDto<TDto>> GetByIdAsync(int id);
    Task<ResponseDto<IEnumerable<TEntity>>> GetAllAsync();
    Task<ResponseDto<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
    Task <ResponseDto<TDto>> AddAsync(TEntity entity);

    Task<ResponseDto<NoDataDto>> Remove(TEntity entity);
    Task<ResponseDto<NoDataDto>> Update(TEntity entity);
  }
}
