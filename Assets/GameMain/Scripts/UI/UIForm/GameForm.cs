using System;
using System.Collections.Generic;
using Framework;
using Framework.Develop;
using MyTimer;
using Tencent;
using UnityEngine;
using UnityEngine.UI;

namespace GameMain
{
    public partial class GameForm : UGuiForm
    {
        [SerializeField] private GameObject _keyIconPrefab;
        [SerializeField] private GameObject _materialItem;

        private bool _inited=false;

        /// <summary>
        /// UI列表中要显示的材质枚举，包括顺序
        /// </summary>
        private static List<EMaterial> _uiEMaterialList = new List<EMaterial>()
        {
            EMaterial.WhiteError,
            EMaterial.Jelly,
            EMaterial.Climbable,
            EMaterial.Cloud,
            EMaterial.Hover,
        };

        private Dictionary<string, GameObject> _keyDict = new();
        private List<MaterialUIItem> _materialUIItems = new();
        private int _curChooseMaterialIndex = 0;
        private ECameraMode _cameraMode;
        private TimerOnly _scrollTimer = new();

        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
            InitMaterialItems();
            _scrollTimer.Initialize(0.1f);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Event.Subscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Subscribe(OnGetKeyArgs.EventId, OnGetKey);
            GameEntry.Event.Subscribe(OnUseKeyArgs.EventId, OnUseKey);
            GameEntry.Event.Subscribe(OnBulletNumChangeArg.EventId, OnMaterialBulletNumChange);
            _inited = true;
        }

        public override void OnClose()
        {
            base.OnClose();
            GameEntry.Event.Unsubscribe(OnGunMaterialChangeArg.EventId, OnMaterialChange);
            GameEntry.Event.Unsubscribe(OnGetKeyArgs.EventId, OnGetKey);
            GameEntry.Event.Unsubscribe(OnUseKeyArgs.EventId, OnUseKey);
            GameEntry.Event.Unsubscribe(OnBulletNumChangeArg.EventId, OnMaterialBulletNumChange);
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
                var localScale = item.transform.localScale;
                EMaterial eMaterial = _uiEMaterialList[i];
                item.Init(eMaterial, i);
                item.transform.SetParent(m_rect_materials);
                item.transform.localScale = localScale;

                _materialUIItems.Add(item);
            }

            _curChooseMaterialIndex = _uiEMaterialList.Count / 2;
            _materialUIItems[_curChooseMaterialIndex].Choose();
            AGameManager.Player.ChangeMaterialGunMat(_materialUIItems[_curChooseMaterialIndex].MaterialType);
        }

        private void Update()
        {
            if(!_inited) return;
            float scroll = Input.GetAxis("Mouse ScrollWheel") * -10;
            scroll = scroll < 0 ? -1 : (scroll > 0) ? 1 : 0;
            int pre = _curChooseMaterialIndex;
            _curChooseMaterialIndex += Mathf.FloorToInt(scroll);
            while (_curChooseMaterialIndex >= _uiEMaterialList.Count) _curChooseMaterialIndex -= _uiEMaterialList.Count;
            while (_curChooseMaterialIndex < 0) _curChooseMaterialIndex += _uiEMaterialList.Count;
            if (pre != _curChooseMaterialIndex && _scrollTimer.Completed)
            {
                ChooseMaterialItem(pre, _curChooseMaterialIndex);
                _scrollTimer.Restart();
            }
            else
            {
                _curChooseMaterialIndex = pre;
            }
        }

        private void ChooseMaterialItem(int pre, int after)
        {
            MaterialUIItem preItem = _materialUIItems[pre], afterItem = _materialUIItems[after];
            preItem.CancelChoose();
            afterItem.Choose();
            AGameManager.Player.ChangeMaterialGunMat(_materialUIItems[after].MaterialType);
        }

        private void OnMaterialBulletNumChange(object sender, GameEventArgs arg)
        {
            var e = (OnBulletNumChangeArg)arg;
            _materialUIItems.Find(x => x.MaterialType == e.MaterialType).OnBulletNumChange(e.BulletNum);
        }
    }
}