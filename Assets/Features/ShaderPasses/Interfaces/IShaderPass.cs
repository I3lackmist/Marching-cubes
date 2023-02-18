using UnityEngine;

namespace MarchingCubes.ShaderPasses.Interfaces
{
    public interface IShaderPass
    {
        void Execute();
        void SetPosition(Vector3Int position);
        void SetBuffer(string bufferName, ComputeBuffer computeBuffer);
    }
}
