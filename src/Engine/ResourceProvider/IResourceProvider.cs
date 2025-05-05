using LogicGamer.Core.Tool;

namespace LogicGamer.Core.Engine.ResourceProvider
{
    public interface IResourceProvider
    {
        public IResourceLoader GetLoader(string location,Userdata args);

        public bool CheckLocation(string location);

        public void Release();
    }
}