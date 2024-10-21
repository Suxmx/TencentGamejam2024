using UnityEngine;

namespace Framework
{
    public abstract class UGuiForm : MonoBehaviour
    {
        public UIFormId FormId;

        public virtual void OnInit()
        {
            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }

        public virtual void OnOpen()
        {
            
        }

        public virtual void OnClose()
        {
        }
    }
}