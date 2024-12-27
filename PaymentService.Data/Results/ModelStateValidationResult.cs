using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Results
{
  public class ModelStateValidationResult
  {
    public ModelStateValidationResult()
    {
      Errors = new Dictionary<string?, string?>();
    }
    public bool Success { get; set; }
    public Dictionary<string?, string?> Errors { get; set; }
  }
}
