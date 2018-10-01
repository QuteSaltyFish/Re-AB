﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAndEditorGUI;
using SharedTools_Stuff;

namespace Playtime_Painter {
    
    [System.Serializable]
    public class MaterialLightManager : PainterStuff, IPEGI {

        public int[] probes;
        public float[] bounceCoefficient = new float[3];

        public LightCaster GetLight (int number) => LightCaster.allProbes[probes[number]];
        
        public MaterialLightManager() {
            
            if (probes == null) 
                probes = new int[3];
            if (bounceCoefficient == null) {
                bounceCoefficient = new float[3];
                for (int i = 0; i < 3; i++)
                    bounceCoefficient[i] = 0.3f;
            }

        }
#if !NO_PEGI
        public int browsedNode = -1;

        public static int probeChanged = -1;

        public virtual bool PEGI() {

            bool changed = false;

            probeChanged = -1;

            if (probes == null)
                probes = new int[3];
            if (bounceCoefficient == null)
                bounceCoefficient = new float[3];


            for (int c = 0; c < 3; c++) {

                int ind = probes[c];

                if (ind < 0)
                {
                    pegi.write(((ColorChanel)c).getIcon());
                    if (icon.Add.Click().nl())
                    {
                        probes[c] = 0;
                        probeChanged = c;
                    }

                }
                else
                {
                    
                    var prb = LightCaster.allProbes[ind];

                    if (prb == null)
                        pegi.write("Probe " + ind, 50);
                    else
                    {
                        if (icon.Delete.Click())
                        {
                            changed = true;
                            probes[c] = -1;
                            probeChanged = c;
                        }
                    }
                        
                    if ("Light:".select_iGotIndex("Select Light Source" ,50, ref ind, LightCaster.allProbes.GetAllObjsNoOrder()).nl()) {
                        probes[c] = ind;
                        probeChanged = c;
                        changed = true;
                    }
                        
                }
                    
                changed |= "Bounce Coefficient".edit(ref bounceCoefficient[c]).nl();
            }
                pegi.Space();
                pegi.newLine();
        
            return changed;
        }

#endif

        public void UpdateLightOnMaterials(List<Material> materials)
        {

            if (materials.Count > 0)
                for (int c = 0; c < 3; c++)
                {

                    Color col = Color.black;
                    Vector3 pos = Vector3.zero;

                    var l = GetLight(c);

                    if (l != null)
                    {
                        col = l.ecol * l.brightness;
                        pos = l.transform.position;
                    }

                    col.a = bounceCoefficient[c];

                    foreach (var m in materials)
                        if (m != null)
                        {
                            m.SetVector("l" + c + "col", col.ToVector4());
                            m.SetVector("l" + c + "pos", pos);
                        }
                }
        }
    }
}