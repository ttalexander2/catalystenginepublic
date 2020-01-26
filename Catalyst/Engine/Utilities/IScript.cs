using System;

namespace Catalyst.Engine.Utilities
{
    public interface IScript
    {
        object Execute(object[] args);

    }
}