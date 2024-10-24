using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public class MaterialUIItem : MonoBehaviour
    {
        [SerializeField] private Material _materialSelectMat;
        public EMaterial MaterialType;
        public int Index;

        private Animator _animator;
        private RawImage _3dShowRawImage;

        private Transform _bg;
        private Transform _circle;

        public void Init(EMaterial eMaterial, int index)
        {
            MaterialType = eMaterial;
            Index = index;

            _bg = transform.Find("Bg");
            _circle = transform.Find("Circle");
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
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float time = Mathf.Clamp01(stateInfo.normalizedTime);
            _animator.Play("OnSelectMaterial", 0, 1 - time);
            _circle.GetComponent<Image>().material = _materialSelectMat;
        }

        public void CancelChoose()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            float time = Mathf.Clamp01(stateInfo.normalizedTime);
            _animator.Play("OnUnSelectMaterial", 0, 1 - time);
            _circle.GetComponent<Image>().material = null;
        }
    }
}