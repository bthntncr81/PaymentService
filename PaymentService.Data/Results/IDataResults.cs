using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.Results
{
    public interface IDataResults<T>: IResults
    {
        T Data { get; }
    }
}
