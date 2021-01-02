using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SEQRenderer
{
    class PixelRaster
    {
        public PixelRaster(int scl_x, int scl_y, int res_x, int res_y)
        {
            _pixelScale = new Vector2(scl_x, scl_y);
            _resolution = new Vector2(res_x, res_y);
            try
            {
                _pixels = new Color[res_x,res_y];
            }
            catch (OutOfMemoryException e)
            {
                Console.Error.WriteLine("Could not allocate pixel raster.");
            }
            catch (Exception e)
            {
            }
        }

        public void DrawRaster(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle();
            rect.Width = (int)_pixelScale.X;
            rect.Height = (int)_pixelScale.Y;
            for (int x = 0; x < _resolution.X; x++)
            {
                for(int y = 0; y < _resolution.Y; y++)
                {
                    rect.Location = new Point(x * (int)_pixelScale.X, y * (int)_pixelScale.Y);
                    sb.Draw(_pixelIMG, rect, _pixels[x, y]);
                }
            }
        }

        public void ClearRaster()
        {
            for (int x = 0; x < _resolution.X; x++)
            {
                for (int y = 0; y < _resolution.Y; y++)
                {
                    _pixels[x, y] = Color.Transparent;
                }
            }
        }

        public void LoadContent(Texture2D pxl_img) { _pixelIMG = pxl_img; }

        public void SetPixel(int x, int y, Color col)
        {
            _pixels[x, y] = col;
        }
        
        //public void DrawPixel(SpriteBatch sprite_batch, int x_, int y_, Color col_)
        //{
        //    Rectangle rect = new Rectangle();
        //    Vector2 vect = new Vector2(x_ * _pixelScale.X, y_ * _pixelScale.Y);
        //    rect.Location = vect.ToPoint();
        //    rect.Width = (int)_pixelScale.X;
        //    rect.Height = (int)_pixelScale.Y;

        //    sprite_batch.Draw(_pixelIMG, rect, col_);
        //}

        public void SetPixelScale(int scl_x, int scl_y)
        {
            _pixelScale.X = scl_x;
            _pixelScale.Y = scl_y;
        }
        public void SetPixelScale(Point scl)
        {
            _pixelScale.X = scl.X;
            _pixelScale.Y = scl.Y;
        }

        //ClearAll


        private Color[,] _pixels;
        private Vector2 _pixelScale;
        private Vector2 _resolution;
        //Vector2 topleft
        private Texture2D _pixelIMG;
    }
}
