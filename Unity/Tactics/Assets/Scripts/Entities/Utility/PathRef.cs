using UnityEngine;

namespace Assets.Scripts.Entities.Utility
{
    public class PathRef
    {
        public PathRef(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }

        public PathRef(int posX, int posY, int height)
        {
            PosX = posX;
            PosY = posY;
            Height = height;
        }

        public PathRef(int posX, int posY, int steps, int height)
        {
            PosX = posX;
            PosY = posY;
            Steps = steps;
            Height = height;
        }

        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Steps { get; set; }
        public int Height { get; set; }
        public Vector3 Position { get; set; }
    }
}
