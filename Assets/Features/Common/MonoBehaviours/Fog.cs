using UnityEngine;

namespace MarchingCubes.Common.MonoBehaviours 
{
    public class Fog : MonoBehaviour
    {
        public Color overworldColor;
        public Color underworldColor;

        public float underworldDistance;
        public float overworldDistance;
        public float changeSpeed;
        public float changeHeight;

        public bool turnOffFog;

        private void FixedUpdate()
        {
            if (turnOffFog) {
                RenderSettings.fog = false;
                return;
            }

            Color currentFogColor = RenderSettings.fogColor;
            Color fogColor;

            float currentFogDistance = RenderSettings.fogEndDistance;
            float fogDistance;

            float change = Time.fixedDeltaTime * changeSpeed;
            
            if (transform.position.y >= changeHeight)
            {
                fogColor = Vector4.Lerp(currentFogColor, overworldColor, change);
                fogDistance = Mathf.Lerp(currentFogDistance, overworldDistance, change);
            }
            else
            {
                fogColor = Vector4.Lerp(currentFogColor, underworldColor, change);
                fogDistance = Mathf.Lerp(currentFogDistance, underworldDistance, change);
            }

            RenderSettings.fogColor = fogColor;
            Camera.main.backgroundColor = fogColor;
            
            RenderSettings.fogEndDistance = fogDistance;
        }
    }
}
