using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RaytrAkkar.Web
{
    public class RaytrakkaBridge : IRaytrakkaBridge
    {
        public RaytrakkaBridge() { }

        public event EventHandler<RenderedScene> RenderedScene;


        public void AddRenderedScene(RenderedScene scene)
        {
            RenderedScene?.Invoke(this, scene);
        }


        public event EventHandler<RenderedTile> RenderedTile;
        public void AddRenderedTile(RenderedTile tile)
        {
            RenderedTile?.Invoke(this, tile);
        }


        public event EventHandler<RenderScene> AddedScene;
        public void AddScene(RenderScene scene)
        {
            AddedScene?.Invoke(this, scene);
        }

        public event EventHandler<string> AddedLog;
        public void Log(string message)
        {
            Debug.WriteLine(message);
            AddedLog?.Invoke(this, message);
        }
    }
}
