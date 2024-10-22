using System;
using System.Collections.Generic;
using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public partial class SettingForm : UGuiForm
    {
        [SerializeField] private List<RectTransform> _topItems;

        [SerializeField] private List<SettingPage> _settingPages;

        [SerializeField] private List<string> _titles;
        [SerializeField] private TextMeshProUGUI _titleTxt;

        private int _currentIndex;
        private int _settingCount => _topItems.Count;

        public override void OnInit()
        {
            base.OnInit();
            //初始化各个子页面
            foreach (var page in _settingPages)
            {
                page.Init();
            }

            //初始化各个按钮
            for (int i = 0; i < _topItems.Count; i++)
            {
                var item = _topItems[i];
                var button = item.GetComponentInChildren<Button>();
                var i1 = i;
                button.onClick.AddListener(() => { ChooseIndex(_currentIndex, i1); });
            }
        }

        public override void OnOpen()
        {
            base.OnOpen();
            ChooseIndex(_currentIndex, (_settingPages.Count) / 2, true);
        }

        public override void OnClose()
        {
            base.OnClose();
            foreach (var page in _settingPages)
            {
                page.Close();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                int newIndex = _currentIndex - 1 < 0 ? 0 : _currentIndex - 1;
                ChooseIndex(_currentIndex, newIndex);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                int newIndex = _currentIndex + 1 > _settingCount - 1 ? _currentIndex : _currentIndex + 1;
                ChooseIndex(_currentIndex, newIndex);
            }
        }

        private void ChooseIndex(int previousIndex, int nextIndex, bool force = false)
        {
            if (nextIndex == previousIndex && !force) return;
            
            //关闭之前的
            var label = _topItems[previousIndex].Find("ButtonChooseLabel");
            label.gameObject.SetActive(false);
            _settingPages[previousIndex].Close();
            //设置新的
            _currentIndex = nextIndex;
            label = _topItems[_currentIndex].Find("ButtonChooseLabel");
            label.gameObject.SetActive(true);
            _settingPages[_currentIndex].Open();
            _titleTxt.text = _titles[_currentIndex];
        }
    }
}