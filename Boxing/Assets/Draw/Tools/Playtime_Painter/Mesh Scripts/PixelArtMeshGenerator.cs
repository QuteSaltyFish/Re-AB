﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAndEditorGUI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Playtime_Painter
{


    public class PixelArtMeshGenerator : MonoBehaviour, IPEGI
    {
        static int width = 8;
        static float halfPix = 0;
        static Vert[] verts;// = new List<vert>();
        static List<int> tris = new List<int>();

        public int testWidth = 8;
        public float thickness = 0.2f;


        public MeshFilter mFilter;

        enum PicV { lup = 0, rup = 1, rdwn = 2, ldwn = 3 };

        class Vert
        {
            public Vector3 pos;// = new Vector3();
            public Vector4 uv;// = new Vector2();

            public Vert(int x, int y, PicV p, float borderPercent)
            {
                uv = new Vector4(halfPix + (float)x / (float)width, halfPix + (float)y / (float)width                                                     // normal coordinate

                    , halfPix + (float)x / (float)width, halfPix + (float)y / (float)width); // with center coordinate

                pos = new Vector3(uv.x - 0.5f, uv.y - 0.5f, 0);

                float off = halfPix * (1 - borderPercent);

                Vector3 offf = Vector3.zero;

                switch (p)
                {
                    case PicV.ldwn: offf += new Vector3(-off, off, 0); break;
                    case PicV.lup: offf += new Vector3(-off, -off, 0); break;
                    case PicV.rdwn: offf += new Vector3(off, off, 0); break;
                    case PicV.rup: offf += new Vector3(off, -off, 0); break;
                }

                pos += offf;

                uv.x += offf.x;
                uv.y += offf.y;

            }
        }

        static int GetIndOf(int x, int y, PicV p)
        {
            return (y * width + x) * 4 + (int)p;
        }

        void JoinDiagonal(int x, int y)
        {
            tris.Add(GetIndOf(x, y, PicV.rdwn));
            tris.Add(GetIndOf(x + 1, y, PicV.ldwn));
            tris.Add(GetIndOf(x, y + 1, PicV.rup));

            tris.Add(GetIndOf(x, y + 1, PicV.rup));
            tris.Add(GetIndOf(x + 1, y, PicV.ldwn));
            tris.Add(GetIndOf(x + 1, y + 1, PicV.lup));
        }

        void JoinDown(int x, int y)
        {
            tris.Add(GetIndOf(x, y, PicV.ldwn));
            tris.Add(GetIndOf(x, y, PicV.rdwn));
            tris.Add(GetIndOf(x, y + 1, PicV.lup));

            tris.Add(GetIndOf(x, y, PicV.rdwn));
            tris.Add(GetIndOf(x, y + 1, PicV.rup));
            tris.Add(GetIndOf(x, y + 1, PicV.lup));

        }

        void JoinRight(int x, int y)
        {
            tris.Add(GetIndOf(x, y, PicV.rup));
            tris.Add(GetIndOf(x + 1, y, PicV.lup));
            tris.Add(GetIndOf(x, y, PicV.rdwn));

            tris.Add(GetIndOf(x + 1, y, PicV.lup));
            tris.Add(GetIndOf(x + 1, y, PicV.ldwn));
            tris.Add(GetIndOf(x, y, PicV.rdwn));
        }

        void FillPixel(int x, int y)
        {
            tris.Add(GetIndOf(x, y, PicV.lup));
            tris.Add(GetIndOf(x, y, PicV.rup));
            tris.Add(GetIndOf(x, y, PicV.ldwn));

            tris.Add(GetIndOf(x, y, PicV.rup));
            tris.Add(GetIndOf(x, y, PicV.rdwn));
            tris.Add(GetIndOf(x, y, PicV.ldwn));
        }

        public Mesh GenerateMesh(int w)
        {
            width = w;
            halfPix = 0.5f / width;

            Mesh m = new Mesh();

            int pixls = width * width;

            verts = new Vert[pixls * 4];
            tris.Clear();

            Vector3[] fverts = new Vector3[verts.Length];
            List<Vector4> uvs = new List<Vector4>();//[verts.Length];

            for (int i = 0; i < verts.Length; i++)
                uvs.Add(new Vector4());

            for (int x = 0; x < width; x++)
                for (int y = 0; y < width; y++)
                {

                    for (int p = 0; p < 4; p++)
                    {
                        int ind = GetIndOf(x, y, (PicV)p);
                        verts[ind] = new Vert(x, y, (PicV)p, thickness);
                        fverts[ind] = verts[ind].pos;
                        uvs[ind] = verts[ind].uv;
                    }

                    FillPixel(x, y);
                    if (thickness > 0)
                    {
                        thickness = Mathf.Min(thickness, 0.9f);
                        if (x < width - 1) JoinRight(x, y);
                        if (y < width - 1) JoinDown(x, y);
                        if ((x < width - 1) && (y < width - 1))
                            JoinDiagonal(x, y);
                    }
                }

            m.vertices = fverts;
            m.SetUVs(0, uvs);
            m.triangles = tris.ToArray();





            return m;
        }



        void Save()
        {

#if UNITY_EDITOR
            string meshFilePath = "Assets/" + mFilter.transform.name + ".asset";
            Mesh meshToSave = mFilter.sharedMesh;
            AssetDatabase.CreateAsset(meshToSave, meshFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }
#if !NO_PEGI
        public bool PEGI()
        {
            bool changed = false;
            changed |= "Mesh Filter".edit(ref mFilter).nl();
            changed |= "Width: ".edit(ref testWidth).nl();
            changed |= "Thickness ".edit(ref thickness).nl();
            if ("Generate".Click())
                mFilter.mesh = GenerateMesh(testWidth * 2);

            if (mFilter && mFilter.sharedMesh && "Save".Click())
                Save();





            return changed;
        }
#endif

    }
}