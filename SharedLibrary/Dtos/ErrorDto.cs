using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
  public class ErrorDto
  {
    public List<string> Errors { get; private set; }
    public bool IsShow { get; private set; }

    public ErrorDto()
    {
      Errors = new List<string>();
    }
    public ErrorDto(string error, bool isshown)
    {
      Errors.Add(error);
      IsShow = isshown;
    }

    public ErrorDto(List<string> errors, bool isshown)
    {
      Errors = errors;
      IsShow = isshown;
    }

  }
}
