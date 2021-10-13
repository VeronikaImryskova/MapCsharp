using OSMMapLib;
using System;
using System.Data;

namespace Csharp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map();
            Layer l = new Layer("https://{c}.tile.openstreetmap.org/{z}/{x}/{y}.png", 5);
            map.layer = l;
            //49.8328156, 18.1651111
            map.lat = 49.8328156;
            map.lon = 18.1651111;
            map.zoom = 17;

            map.Render("newMap.png");
        }
    }
}
