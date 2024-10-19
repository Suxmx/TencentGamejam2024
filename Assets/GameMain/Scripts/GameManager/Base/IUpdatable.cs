namespace Framework
{
    public interface IUpdatable
    {
        public void OnUpdate(float deltaTime);
        public void OnFixedUpdate(float deltaTime);
        public void OnLateUpdate(float deltaTime);
    }
}