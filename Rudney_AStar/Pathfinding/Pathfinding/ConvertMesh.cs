using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pathfinding
{
    public class ConvertMesh
    {
        private Color color;
        private VertexPositionColor[] myVerts;

        public ConvertMesh(Color color)
        {
            this.color = color;
        }
        public ConvertMesh()
        {
  
        }

        public VertexPositionColor[] returnTriangle(Vector3[] verts,int[] triangles) 
        {
            myVerts = new VertexPositionColor[triangles.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                myVerts[i] = new VertexPositionColor(verts[triangles[i]],this.color);
            }

            return myVerts;
        }

        public VertexPositionTexture[] returnTriangle(Vector3[] verts, int[] triangles,Vector2[] uvCord,int[] uv)
        {
            VertexPositionTexture[] myNewVerts = new VertexPositionTexture[triangles.Length];

            for (int i = 0; i < triangles.Length; i++)
            {
                myNewVerts[i] = new VertexPositionTexture(verts[triangles[i]],uvCord[uv[i]]);
            }

            return myNewVerts;
        }
    }
}
