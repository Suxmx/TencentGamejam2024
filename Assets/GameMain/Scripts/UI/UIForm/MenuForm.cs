using System.Collections.Generic;
using DG.Tweening;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameMain
{
    public partial class MenuForm : UGuiForm
    {
        private Tween _selectorTween;

        private void Update()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            GraphicRaycaster raycaster = (GameEntry.UI as UIManager).UIRoot.GetComponent<GraphicRaycaster>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.GetComponent<ButtonHover>())
                {
                    OnHoverButton(result.gameObject.transform as RectTransform);
                }
            }
        }

        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        public override void OnClose()
        {
            base.OnClose();
            RemoveEvents();
        }

        private void RegisterEvents()
        {
            m_btn_newgame.onClick.AddListener(OnClickNewGame);
            m_btn_setting.onClick.AddListener(OnClickSettings);
            m_btn_exit.onClick.AddListener(OnClickExit);
        }

        private void RemoveEvents()
        {
            m_btn_newgame.onClick.RemoveListener(OnClickNewGame);
            m_btn_setting.onClick.RemoveListener(OnClickSettings);

            m_btn_exit.onClick.RemoveListener(OnClickExit);
        }

        private void OnClickNewGame()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureMenu).EnterGame();
        }

        private void OnClickExit()
        {
            Application.Quit();
        }

        public void OnHoverButton(RectTransform rect)
        {
            if (_selectorTween is not null && _selectorTween.active)
            {
                _selectorTween.Kill();
            }

            m_rect_selector.DOMoveY(rect.position.y, 0.3f);
        }

        private void OnClickSettings()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }
    }
}