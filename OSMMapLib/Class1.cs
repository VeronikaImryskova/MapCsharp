using MapRendererLib;
using System;
using System.Text;



namespace OSMMapLib
{
    public class Tile
    {
        private int X;
        private int Y;
        private int zoom;
        private string Url;

        private int x { get { return X; } set { X = value; } }
        private int y { get { return Y; } set { Y = value; } }
        public string url { get { return Url; } set { Url = value; } }

        public int Zoom
        {
            get { return zoom; }
            set
            {
                if (value > 1)
                {
                    zoom = value;
                }
                else
                {
                    zoom = 1;
                }
            }
        }

        public Tile(int X, int Y, int zoom, string url)
        {
            x = X;
            y = Y;
            Zoom = zoom;
            Url = url;
        }

        public override string ToString()
        {
            StringBuilder build = new StringBuilder();

            build.AppendFormat("[{0}, {1}, {2}]: {3}", X, Y, Zoom, Url);

            return build.ToString();
        }
    }
    public class Layer
    {
        private string UrlTemplate = "https://{c}.tile.openstreetmap.org/{z}/{x}/{y}.png";
        public int MaxZoom = 100;

        public Layer(){}

        public Layer(string Url, int Mz)
        {
            UrlTemplate = Url;
            MaxZoom = Mz;
        }

        public string FormatUrl(int x, int y, int zoom)
        {
            Random ran = new Random();

            string[] random = { "a", "b", "c" };
            int index = ran.Next(0, random.Length);

            string Url = UrlTemplate.Replace("{z}", zoom.ToString());
            Url = Url.Replace("{x}", x.ToString());
            Url = Url.Replace("{y}", y.ToString());
            Url = Url.Replace("{c}", random[index]);

            return Url;
        }

        public Tile this[int x, int y, int zoom]
        {
            get { return new Tile(x,y,zoom, FormatUrl(x,y,zoom)); }
        }

    }
    public class Map
    {
        public Layer layer { get; set; }

        private double Lat;
        private double Lon;
        private int Zoom;

        private int centerTileX;
        private int centerTileY;

        public int CenterTileX{ get { return (int)((Lon + 180.0) / 360.0 * (1 << Zoom));}}

        public int CenterTileY { get { return (int)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) + 1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));}}

        public double lat
        {
            get { return Lat; }
            set
            {
                Lat = (value + 90.0) % 180 - 90.0;
            }
        }

        public double lon
        {
            get { return Lon; }
            set
            {
                this.Lon = (value + 180.0) % 360 - 180.0;
            }
        }

        public int zoom
        {
            get { return Zoom; }
            set
            {
                if (value < layer.MaxZoom && value > 1)
                {
                    Zoom = value;
                }
                else
                {
                    Zoom = 10;
                }
            }
        }

        public void Render(string fileName)
        {
            MapRenderer mapRenderer = new MapRenderer(4, 4);
            for (int x = -2; x < 2; x++)
            {
                for (int y = -2; y < 2; y++)
                {
                    Tile tile = this.layer[this.CenterTileX + x, this.CenterTileY + y, this.Zoom];

                    Console.WriteLine(tile);

                    mapRenderer.Set(x + 2, y + 2, tile.url);
                }
            }
            mapRenderer.Flush();
            mapRenderer.Render(fileName);
        }
    }
}
