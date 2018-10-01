﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAndEditorGUI;
using SharedTools_Stuff;

namespace Playtime_Painter
{


    public class PaintingCollision
    {
        public StrokeVector vector;
        public PlaytimePainter painter;

        public PaintingCollision(PlaytimePainter p)
        {
            painter = p;
            vector = new StrokeVector();
        }
    }

    [ExecuteInEditMode]
    public class PainterBall : MonoBehaviour, IPEGI

    {

        public MeshRenderer rendy;
        public Rigidbody rigid;
        public SphereCollider _collider;

        public List<PaintingCollision> paintingOn = new List<PaintingCollision>();
        public BrushConfig brush = new BrushConfig();

        PaintingCollision TryAddPainterFrom(GameObject go)
        {
            PlaytimePainter target = go.GetComponent<PlaytimePainter>();

            if (target != null && !target.LockTextureEditing)
            {
                PaintingCollision col = new PaintingCollision(target);
                paintingOn.Add(col);
                col.vector.posFrom = transform.position;
                col.vector.firstStroke = true;
                target.UpdateOrSetTexTarget(TexTarget.RenderTexture);

                return col;
            }

            return null;
        }

        public void OnCollisionEnter(Collision collision)
        {
            //var pcol = 
            TryAddPainterFrom(collision.gameObject);
            /*  if (pcol != null) {

                      var cp = collision.contacts[0];
                      RaycastHit hit;
                      Ray ray = new Ray(cp.point - cp.normal * 0.05f, cp.normal);
                      if (cp.otherCollider.Raycast(ray, out hit, 0.1f))
                          pcol.vector.uvFrom = hit.textureCoord;


              }*/

        }

        public void OnTriggerEnter(Collider collider)
        {
            TryAddPainterFrom(collider.gameObject);

        }

        void TryRemove(GameObject go)
        {
            foreach (PaintingCollision p in paintingOn)
                if (p.painter.gameObject == go)
                {
                    paintingOn.Remove(p);
                    return;
                }
        }

        public void OnTriggerExit(Collider collider)
        {

            TryRemove(collider.gameObject);

        }

        public void OnCollisionExit(Collision collision)
        {
            TryRemove(collision.gameObject);
        }

        public void ChangeColor(Color c)
        {
            brush.BrushConfigColor(c);
            Material m = GetComponent<Renderer>().material;
            m.color = c;
            GetComponent<Renderer>().material = m;
        }

        public void SubmitPaint()
        {
            PlaytimePainter painter = gameObject.GetComponent<PlaytimePainter>();
            ImageData image = painter.ImgData;

            if (!painter.IsTerrainControlTexture())
            {
                string Orig = "";

                if (image.texture2D != null)
                {
                    Orig = image.texture2D.GetPathWithout_Assets_Word();
                    painter.ForceReimportMyTexture(Orig);
                    image.SaveName = image.texture2D.name;
                    GUI.FocusControl("dummy");
                    if (painter.terrain != null)
                        painter.UpdateShaderGlobals();
                }
            }
        }

        public void OnEnable()
        {
            brush.TypeSet(false, BrushTypeSphere.Inst);
            if (rendy == null)
                rendy = GetComponent<MeshRenderer>();
            if (rigid == null)
                rigid = GetComponent<Rigidbody>();
            if (_collider == null)
                _collider = GetComponent<SphereCollider>();

            //brush.BrushConfigColor(Color.blue);

            rendy.sharedMaterial.color = brush.colorLinear.ToGamma();
            brush.TargetIsTex2D = false;
        }

        private void Update()
        {

            brush.Brush3D_Radius = transform.lossyScale.x * 0.7f;

            foreach (PaintingCollision col in paintingOn)
            {
                PlaytimePainter p = col.painter;
                if (brush.IsA3Dbrush(p))
                {
                    StrokeVector v = col.vector;
                    v.posTo = transform.position;
                    brush.Paint(v, p);

                }

            }
        }

#if !NO_PEGI
        public bool PEGI()
        {
            ("Painting on " + paintingOn.Count + " objects").nl();

            if ((_collider.isTrigger) && ("Make phisical".Click().nl()))
            {
                _collider.isTrigger = false;
                rigid.isKinematic = false;
                rigid.useGravity = true;
            }

            if ((!_collider.isTrigger) && ("Make Trigger".Click().nl()))
            {
                _collider.isTrigger = true;
                rigid.isKinematic = true;
                rigid.useGravity = false;
            }



            float size = transform.localScale.x;
            if ("Size:".edit("Size of the ball", 50, ref size, 0.1f, 10).nl())
                transform.localScale = Vector3.one * size;



            pegi.writeOneTimeHint("Painter ball made for World Space Brushes only", "PaintBall_brushHint");

            if ((brush.Targets_PEGI().nl()) || (brush.Mode_Type_PEGI().nl()))
            {
                if ((brush.TargetIsTex2D) || (!brush.IsA3Dbrush(null)))
                {
                    brush.TargetIsTex2D = false;
                    brush.TypeSet(false, BrushTypeSphere.Inst);

                    pegi.resetOneTimeHint("PaintBall_brushHint");
                }
            }

            if (brush.ColorSliders_PEGI())
                rendy.sharedMaterial.color = brush.colorLinear.ToGamma();

            return false;
        }
#endif
    }
}
