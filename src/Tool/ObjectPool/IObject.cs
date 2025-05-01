namespace LogicGamer.Core.Tool.ObjectPool
{
    public interface IObject
    {
        public void OnInit(Userdata data);
        public void OnReturn();
    }
}