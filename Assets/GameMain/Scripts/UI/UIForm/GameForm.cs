using System;
using System.Collections.Generic;
using Framework;
using Framework.Develop;
using Tencent;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public partial class GameForm : UGuiForm
    {
        [SerializeField] private GameObject _keyIconPrefab;
        [SerializeField] private GameObject _materialItem;

        /// <summary>
        /// UI列表中要显示的材质枚举，包括顺序
        /// </summary>
        private static List<EMaterial> _uiEMaterialList = new List<EMaterial>()
        {
            EMaterial.WhiteError,
            EMaterial.BlackError,
            EMaterial.Jelly,
            EMaterial.Climbable,
            EMaterial.Cloud,
            EMaterial.Dangerous,
            EMaterial.Hover,
        };

        private Dictionary<string, GameObject> _keyDict = new();
        private List<MaterialUIItem> _materialUIItems = new();
        private int _curChooseMaterialIndex = 0;
        private ECameraMode _cameraMode;

        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
            InitMaterialItems();
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Subscribe(OnGetKeyArgs.EventId, OnGetKey);
            GameEntry.Event.Subscribe(OnUseKeyArgs.EventId, OnUseKey);
        }

        public override void OnClose()
        {
            base.OnClose();
            GameEntry.Event.Unsubscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Unsubscribe(OnGetKeyArgs.EventId, OnGetKey);
            GameEntry.Event.Unsubscribe(OnUseKeyArgs.EventId, OnUseKey);
        }

        private void OnGetKey(object sender, GameEventArgs arg)
        {
            var e = (OnGetKeyArgs)arg;
            var keyobj = Instantiate(_keyIconPrefab, m_trans_keys, true);
            if (e.KeySprite is not null)
            {
                keyobj.GetComponent<Image>().sprite = e.KeySprite;
            }

            _keyDict.Add(e.Key, keyobj);
        }

        private void OnUseKey(object sender, GameEventArgs arg)
        {
            var e = (OnUseKeyArgs)arg;
            Destroy(_keyDict[e.Key]);
        }

        private void OnMaterialChange(object sender, GameEventArgs arg)
        {
            var e = (OnGunMaterialChangeArg)arg;
            m_tmp_curMaterial.text = $"当前材质：{e.Material}";
        }

        private void InitMaterialItems()
        {
            for (int i = 0; i < _uiEMaterialList.Count; i++)
            {
                var item = Instantiate(_materialItem).GetComponent<MaterialUIItem>();
                EMaterial eMaterial = _uiEMaterialList[i];
                item.Init(eMaterial, i);
                item.transform.SetParent(m_rect_materials);
                item.gameObject.SetActive(false);
            }

            _curChooseMaterialIndex = _uiEMaterialList.Count / 2;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                int pre = _curChooseMaterialIndex;
                _curChooseMaterialIndex = _curChooseMaterialIndex - 1 < 0
                    ? _uiEMaterialList.Count - 1
                    : _curChooseMaterialIndex - 1;
                ChooseMaterialItem(pre, _curChooseMaterialIndex);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                int pre = _curChooseMaterialIndex;
                _curChooseMaterialIndex = _curChooseMaterialIndex + 1 >= _uiEMaterialList.Count
                    ? 0
                    : _curChooseMaterialIndex + 1;
                ChooseMaterialItem(pre, _curChooseMaterialIndex);
            }
        }

        private void ChooseMaterialItem(int pre, int after)
        {
            MaterialUIItem preItem = _materialUIItems[pre], afterItem = _materialUIItems[after];
            preItem.CancelChoose();
            afterItem.Choose();
        }
    }
}