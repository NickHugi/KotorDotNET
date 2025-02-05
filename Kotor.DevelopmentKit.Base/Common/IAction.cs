using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Common;

public interface IAction<T> where T : class
{
    public void Apply(T data);

    public void Undo(T data);
}
