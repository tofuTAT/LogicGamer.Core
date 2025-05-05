namespace LogicGamer.Core.Tool.ObjectPool
{
    public interface IObject
    {
        public void OnReset(Userdata data);
        public void OnReturn();
    }
}