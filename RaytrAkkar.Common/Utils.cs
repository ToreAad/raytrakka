using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RaytrAkkar.Common
{
    public static class Utils
    {
        public static byte[] Flatten(this byte[,,] data)
        {
            var width = data.GetLength(0);
            var height = data.GetLength(1);
            var depth = data.GetLength(2);

            var result = new byte[width * height * depth];
            for (int x = 0; x< width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for(int z = 0; z < depth; z++)
                    {
                        result[x + width*(y+height*z)] = data[x, y, z];
                    }
                }
            }
            return result;
        }

        public static byte[,,] Unflatten(this byte[] data, int width, int height, int depth)
        {

            var result = new byte[width, height, depth];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        result[x, y, z] = data[x + width * (y + height * z)];
                    }
                }
            }
            return result;
        }

        public static Bitmap ToBitmap(this RenderedScene scene)
        {
            int w = scene.Scene.Width;
            int h = scene.Scene.Height;
            var data = scene.Data.Unflatten(w, h, 3);

            var bmp = new Bitmap(w, h);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(255,
                        data[x, y, 0],
                        data[x, y, 1],
                        data[x, y, 2]));
                }
            }
            return bmp;
        }

        
        public static Bitmap ToBitmap(this RenderedTile tile, Bitmap bmp)
        {
            int w = tile.Tile.Width;
            int h = tile.Tile.Height;
            var data = tile.Data.Unflatten(w, h, 3);

            for (int dx = 0; dx < w; dx++)
            {
                for (int dy = 0; dy < h; dy++)
                {
                    bmp.SetPixel(tile.Tile.X + dx, tile.Tile.Y + dy, Color.FromArgb(255,
                        data[dx, dy, 0],
                        data[dx, dy, 1],
                        data[dx, dy, 2]));
                }
            }
            return bmp;
        }

        public static Bitmap ToBitmap(this RenderedScene scene, Bitmap bmp)
        {
            int w = scene.Scene.Width;
            int h = scene.Scene.Height;
            var data = scene.Data.Unflatten(w, h, 3);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(255,
                        data[x, y, 0],
                        data[x, y, 1],
                        data[x, y, 2]));
                }
            }
            return bmp;
        }
    }
}
