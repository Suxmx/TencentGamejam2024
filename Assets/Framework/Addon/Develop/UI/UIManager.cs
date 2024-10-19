using System;
using System.Collections.Generic;
using Services;
using Services.Asset;
using UnityEngine;

namespace Framework
{
    public class UIManager : Service, IUIManager
    {
        public UIConfig Config;

        private static int _serialId = 0;
        private List<UGuiForm> _activeUIs = new();
        private Dictionary<int, UGuiForm> _serialDict = new();
        private Canvas _uiRoot;
        private IAssetLoader _resource;

        protected override void Awake()
        {
            base.Awake();
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
        public int OpenUIForm(UIFormId id)
        {
            if (_resource is null)
            {
                _resource = ServiceLocator.Get<IAssetLoader>();
            }
            var uiConfig = Config.GetUIUnit(id);
            if (!uiConfig.CanMultiOpen && _activeUIs.FindIndex(x => x.FormId == id) != -1) return -1;
            //从Config里面读路径加载UI预制体
            var uiGoPrefab = _resource.Load<GameObject>(Config.GetUIPath(uiConfig));
            var uiGo = Instantiate(uiGoPrefab, Vector3.zero, Quaternion.identity);
            var form = uiGo.GetComponent<UGuiForm>();
            //设置FormId和SerialId
            form.FormId = id;
            form.SerialId = _serialId;
            //加入到字典中备用
            _serialDict.Add(form.SerialId, form);
            _activeUIs.Add(form);
            _serialId++;
            form.transform.SetParent(_uiRoot.transform);
            form.transform.localScale=Vector3.one;
            form.OnInit();
            form.OnOpen();
            return form.SerialId;
        }

        /// <summary>
        /// 根据序列化Id关闭Form
        /// </summary>
        /// <param name="serialId">序列化Id</param>
        public void CloseUIForm(int serialId)
        {
            if (serialId < 0) return;
            //若SerialId字典里找不到就返回
            if (!_serialDict.ContainsKey(serialId)) return;
            var form = _serialDict[serialId];
            form.OnClose();
            //从字典中移除
            _activeUIs.Remove(form);
            _serialDict.Remove(serialId);
            Destroy(form.gameObject);
        }

        /// <summary>
        /// 根据UI枚举关闭所有枚举为Id的UIForm
        /// </summary>
        /// <param name="id">枚举</param>
        public void CloseUIForm(UIFormId id)
        {
            var forms = _activeUIs.FindAll(x => x.FormId == id);
            for (int i = forms.Count-1; i>=0;i--)
            {
                var form = _activeUIs[i];
                form.OnClose();
                _activeUIs.Remove(form);
                _serialDict.Remove(form.SerialId);
                Destroy(form.gameObject);
            }
        }


        public void CloseAllUIForm()
        {
            for (int i = _activeUIs.Count - 1; i >= 0; i--)
            {
                var form = _activeUIs[i];
                form.OnClose();
                Destroy(form.gameObject);
            }
            _activeUIs.Clear();
            _serialDict.Clear();
        }
    }
}