using System;

namespace Chroma.Engine.Utilities
{
    public interface IScript
    {
        object Execute(object[] args);

    }
}