using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using Services.Asset;
using UnityEngine;

namespace Framework
{
    public class UIManager : Service, IUIManager
    {
        public UIConfig Config;

        private Dictionary<UIFormId, UGuiForm> _uiInstanceDict = new();
        private Canvas _uiRoot;
        private IAssetLoader _resource;

        protected override void Awake()
        {
            base.Awake();
            _uiRoot = GetComponentInChildren<Canvas>();
        }

        /// <summary>
        /// for debugging
        /// </summary>
        public void InitImmediately()
        {
            //for debug
            if (_resource is null)
            {
                _resource = GameEntry.Resource;
            }
            _uiRoot = GetComponentInChildren<Canvas>();
        }
        protected override void Start()
        {
            base.Start();
            _resource = ServiceLocator.Get<IAssetLoader>();
        }

        /// <summary>
        /// 打开UI并返回其在内部的序列化Id
        /// </summary>
        /// <param name="id">UI枚举Id</param>
        /// <returns>序列化Id</returns>
        public void OpenUIForm(UIFormId id)
        {
            if (_resource is null)
            {
                _resource = ServiceLocator.Get<IAssetLoader>();
            }

            UGuiForm form = null;

            if (!_uiInstanceDict.ContainsKey(id))
            {
                var uiConfig = Config.GetUIUnit(id);
                //从Config里面读路径加载UI预制体
                var uiGoPrefab = _resource.Load<GameObject>(Config.GetUIPath(uiConfig));
                var uiGo = Instantiate(uiGoPrefab, Vector3.zero, Quaternion.identity);
                form = uiGo.GetComponent<UGuiForm>();
                //设置FormId和SerialId
                form.FormId = id;
                //加入到字典中备用
                _uiInstanceDict.Add(form.FormId, form);
                form.transform.SetParent(_uiRoot.transform);
                form.transform.localScale = Vector3.one;
                form.OnInit();
            }
            else
            {
                form = _uiInstanceDict[id];
                form.gameObject.SetActive(true);
            }

            form.OnOpen();
        }

        /// <summary>
        /// 根据UI枚举关闭枚举为Id的UIForm
        /// </summary>
        /// <param name="id">枚举</param>
        public void CloseUIForm(UIFormId id)
        {
            if (_uiInstanceDict.TryGetValue(id, out var form))
            {
                form.OnClose();
                form.gameObject.SetActive(false);
            }
        }

        public void CloseAllUIForms()
        {
            var keys = _uiInstanceDict.Keys.ToList();
            foreach (var key in keys)
            {
                var form = _uiInstanceDict[key];
                form.OnClose();
                form.gameObject.SetActive(false);
            }
        }
    }
}