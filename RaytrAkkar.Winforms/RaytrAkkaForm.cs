using Akka.Actor;
using Akka.Configuration;
using RaytrAkkar.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaytrAkkar.Winforms
{
    public partial class RaytrAkkaForm : Form
    {
        private readonly ActorSystem _system;
        private ActorSelection _raytracer;
        private readonly Dictionary<int, Bitmap> _sceneIdToImg = new Dictionary<int, Bitmap>();

        public RaytrAkkaForm(ActorSystem system)
        {
            InitializeComponent();
            _system = system;
            var imageWriter = _system.ActorOf(RaytrakkaListenerActor.Props(RaytrakkaBridge.Instance), "winforms-listener");
            _raytracer = _system.ActorSelection("akka.tcp://raytracer-system@localhost:8081/user/raytracer");

            RaytrakkaBridge.Instance.AddedScene += ReactToSceneAdded;
            RaytrakkaBridge.Instance.RenderedScene += ReactoToSceneRendered;
            RaytrakkaBridge.Instance.RenderedTile += ReactToTileRendered;
            RaytrakkaBridge.Instance.AddedLog += ReactoToLogMessage;
        }

        public void ReactToSceneAdded(object sender, RenderScene scene)
        {
            Console.WriteLine("SceneAdded called");
            AddToCombBox($"{scene.Scene.SceneId}");
            _sceneIdToImg[scene.Scene.SceneId] = new Bitmap(scene.Scene.Width, scene.Scene.Height);
        }

        public void ReactToTileRendered(object sender, RenderedTile tile)
        {
            Console.WriteLine("ReactToTileRendered called");
            var action = new Action(() =>
            {
                var img = tile.ToBitmap(_sceneIdToImg[tile.Tile.Scene.SceneId]);
                _sceneIdToImg[tile.Tile.Scene.SceneId] = img;
                pictureBoxRender.Image = img;
            });
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }

            SetComboBox(tile.Tile.Scene.SceneId.ToString());
        }

        public void ReactoToSceneRendered(object sender, RenderedScene scene)
        {
            Console.WriteLine("ReactoToSceneRendered called");
            var action = new Action(() =>
            {
                var img = scene.ToBitmap(_sceneIdToImg[scene.Scene.SceneId]);
                _sceneIdToImg[scene.Scene.SceneId] = img;
                pictureBoxRender.Image = img;
            });
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
            SetComboBox(scene.Scene.SceneId.ToString());
        }

        public void ReactoToLogMessage(object sender, string message)
        {
            Console.WriteLine("ReactoToSceneRendered called");
            //AddToCombBox($"Message logged: {message}");
        }

        private void SetComboBox(string id)
        {
            if (!comboBoxScene.Items.Contains(id)) return;
            int index = comboBoxScene.Items.IndexOf(id);

            if (comboBoxScene.InvokeRequired)
            {
                comboBoxScene.Invoke(new Action(() => comboBoxScene.SelectedIndex = index));
            }
            else
            {
                comboBoxScene.SelectedIndex = index;
            }
        }

        private void SetImage(Bitmap img)
        {
            if (comboBoxScene.InvokeRequired)
            {
                comboBoxScene.Invoke(new Action(() => pictureBoxRender.Image = img));
            }
            else
            {
                pictureBoxRender.Image = img;
            }
        }

        private void AddToCombBox(string msg)
        {
            if (comboBoxScene.Items.Contains(msg)) return;
            if (comboBoxScene.InvokeRequired)
            {
                comboBoxScene.Invoke(new Action(() => comboBoxScene.Items.Add(msg)));
            }
            else
            {
                comboBoxScene.Items.Add(msg);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var scene = new Scene(0, 1024, 768);
            _raytracer.Tell(new RenderScene(scene));
        }

        private void comboBoxScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            var action = new Action(() =>
            {
                var img = _sceneIdToImg[int.Parse(comboBoxScene.SelectedItem as string)];
                pictureBoxRender.Image = img;
            });
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
