using System;
using System.ComponentModel;

namespace Tizen.NUI
{
    public interface IDataTemplate
    {
        Func<object> LoadTemplate { get; set; }
    }
}
