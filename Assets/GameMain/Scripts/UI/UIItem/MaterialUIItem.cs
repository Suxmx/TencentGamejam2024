using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MaterialUIItem : MonoBehaviour
    {
        public EMaterial MaterialType;
        public int Index;

        private Animator _animator;
        private RawImage _3dShowRawImage;

        private Transform _bg;

        public void Init(EMaterial eMaterial, int index)
        {
            MaterialType = eMaterial;
            Index = index;

            _bg = transform.Find("Bg");
            var offsetMax = _bg.GetComponent<RectTransform>().offsetMax;
            offsetMax.x = 350;
            _bg.GetComponent<RectTransform>().offsetMax = offsetMax;
            _animator = GetComponent<Animator>();
        }
        
        public void Choose()
        {
            _animator.Play("OnSelectMaterial");
        }

        public void CancelChoose()
        {
            _animator.Play("OnUnSelectMaterial");
        }
    }
}