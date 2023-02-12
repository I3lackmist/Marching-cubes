using MarchingCubes.Common.Enums;
using UnityEngine;

namespace MarchingCubes.Common.Interfaces
{
    public interface IShaderPass
    {
        void SetProperties(ComputeShader shader);
    }
}
