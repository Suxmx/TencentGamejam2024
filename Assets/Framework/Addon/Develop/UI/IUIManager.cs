namespace Framework
{
    public interface IUIManager : IService
    {
        int OpenUIForm(UIFormId id);
        void CloseUIForm(int serialId);
        void CloseUIForm(UIFormId id);
        void CloseAllUIForm();
    }
}