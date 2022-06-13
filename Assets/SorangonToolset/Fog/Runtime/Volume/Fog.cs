using UnityEngine;
using UnityEngine.Rendering;

namespace SorangonToolset.UrpFog {
    [System.Serializable]
    [VolumeComponentMenu("URP/Environment/Fog")]
    public class Fog : VolumeComponent {
        public ClampedFloatParameter density = new ClampedFloatParameter(0f, 0f, 1f);
        
        [Header("Distance")]
        public FloatParameter startDistance = new FloatParameter(0f);
        public FloatParameter endDistance = new FloatParameter(100f);
        
        [Space]
        [Header("Height")]
        public FloatParameter heightStart = new FloatParameter(0f);
        public FloatParameter heightEnd = new FloatParameter(100f);

        [Space]
        [Header("Colors")]
        public ColorParameter startColor = new ColorParameter(Color.white,true, false, true);
        public ColorParameter endColor = new ColorParameter(Color.white,true, false, true);
    }
}
