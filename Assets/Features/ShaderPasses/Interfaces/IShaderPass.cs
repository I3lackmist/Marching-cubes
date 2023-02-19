using UnityEngine;

namespace MarchingCubes.ShaderPasses.Interfaces
{
    public interface IShaderPass
    {
        void Execute();
        void SetBuffer(string bufferName, ComputeBuffer computeBuffer);
        public void SetProperty(string propertyName, Vector3Int value);
        public void SetProperty(string propertyName, float value);
        public void SetProperty(string propertyName, int value);

    }
}
