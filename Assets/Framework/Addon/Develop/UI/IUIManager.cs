namespace Framework
{
    public interface IUIManager : IService
    {
        void OpenUIForm(UIFormId id,object userData=null);

        /// <summary>
        /// 根据UI枚举关闭枚举为Id的UIForm
        /// </summary>
        /// <param name="id">枚举</param>
        void CloseUIForm(UIFormId id);

        void CloseAllUIForms();
    }
}