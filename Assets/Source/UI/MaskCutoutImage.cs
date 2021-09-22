using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Balloondle.UI
{
    public class MaskCutoutImage : Image
    {
        private Material _customMaterial;
        
        public override Material materialForRendering
        {
            get
            {
                if (_customMaterial == null)
                {
                    _customMaterial = new Material(base.materialForRendering);
                }
                
                _customMaterial.SetInt("_StencilComp", (int) CompareFunction.NotEqual);
                return _customMaterial;
            }
        }
    }
}