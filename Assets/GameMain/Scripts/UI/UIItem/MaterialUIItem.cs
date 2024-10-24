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
            // var offsetMax = _bg.GetComponent<RectTransform>().offsetMax;
            // offsetMax.x = 350;
            // var offsetMin = _bg.GetComponent<RectTransform>().offsetMin;
            // offsetMin.x = 350;
            // _bg.GetComponent<RectTransform>().offsetMax = offsetMax;
            // _bg.GetComponent<RectTransform>().offsetMax = offsetMin;
            _animator = GetComponent<Animator>();
            _animator.Play("OnUnSelectMaterial");
            _animator.Update(10);
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