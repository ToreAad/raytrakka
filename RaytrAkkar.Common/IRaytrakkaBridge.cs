using RaytrAkkar.Common;

namespace RaytrAkkar.Common
{
    public interface IRaytrakkaBridge
    {
        void AddRenderedScene(RenderedScene scene);
        void AddRenderedTile(RenderedTile tile);
        void AddScene(RenderScene scene);

        void Log(string message);
    }
}