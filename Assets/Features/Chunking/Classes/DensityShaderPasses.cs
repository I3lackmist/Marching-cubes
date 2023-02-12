using System;
using UnityEngine;
using MarchingCubes.Common.Interfaces;

namespace MarchingCubes.Chunking.Classes
{
	[Serializable]
	public class DensityShaderPasses : NoiseShaderPass, IShaderPass
	{
        public override void SetProperties(ComputeShader shader) 
		{
			base.SetProperties(shader);
		}
	}
}