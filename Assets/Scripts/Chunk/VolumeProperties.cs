using System;
using UnityEngine;

[Serializable]
public class VolumeProperties {
	[SerializeField]
    public Vector3Int volumeSize;

	[SerializeField]
	public float distanceBetweenPoints;

	[SerializeField]
    public float isoLevel;

	[SerializeField]
	public bool drawGizmos;

	[SerializeField]
    public ComputeShader marchingCubesShader;

	[SerializeField]
    public ComputeShader densityShader;

	[SerializeField]
	public DensityShaderProperties densityShaderProperties;

}
