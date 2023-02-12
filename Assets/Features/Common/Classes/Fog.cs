using UnityEngine;

namespace MarchingCubes.Common.Classes 
{
    public class Fog : MonoBehaviour
    {
        public Color overworldColor;
        public Color underworldColor;

        public float underworldDistance;
        public float overworldDistance;

        private void FixedUpdate()
        {
            Color currentFogColor = RenderSettings.fogColor;
            Color fogColor;

            float currentFogDistance = RenderSettings.fogEndDistance;
            float fogDistance;

            if (transform.position.y >= 0f)
            {
                fogColor = Vector4.Lerp(currentFogColor, overworldColor, Time.fixedDeltaTime);
                fogDistance = Mathf.Lerp(currentFogDistance, overworldDistance, Time.fixedDeltaTime);
            }
            else
            {
                fogColor = Vector4.Lerp(currentFogColor, underworldColor, Time.fixedDeltaTime);
                fogDistance = Mathf.Lerp(currentFogDistance, underworldDistance, Time.fixedDeltaTime);
            }

            RenderSettings.fogColor = fogColor;
            Camera.main.backgroundColor = fogColor;
            
            RenderSettings.fogEndDistance = fogDistance;
        }
    }
}
