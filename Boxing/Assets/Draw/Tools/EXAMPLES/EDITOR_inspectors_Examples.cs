﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAndEditorGUI;

namespace Playtime_Painter
{

#if !NO_PEGI && UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ColorBleedControllerPlugin))]
    public class ColorBleedControlsEditor : Editor
    {
        public override void OnInspectorGUI() => ((ColorBleedControllerPlugin)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(BrushConfigOnly))]
    public class BrushConfigOnlyEditor : Editor
    {
        public override void OnInspectorGUI() => ((BrushConfigOnly)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(LightCaster))]
    public class BakedShadowsLightProbeEditor : Editor
    {
        public override void OnInspectorGUI() => ((LightCaster)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(MergingTerrainController))]
    public class MergingTerrainEditor : Editor
    {
        public override void OnInspectorGUI() => ((MergingTerrainController)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(MultiBufferProcessing))]
    public class MultiBufferProcessingEditor : Editor
    {
        public override void OnInspectorGUI() => ((MultiBufferProcessing)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(PainterBall))]
    public class PainterBallEditor : Editor
    {
        public override void OnInspectorGUI() => ((PainterBall)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(PaintingReciever))]
    public class PaintingRecieverEditor : Editor
    {
        public override void OnInspectorGUI() => ((PaintingReciever)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(PaintWithoutComponent))]
    public class PaintWithoutComponentEditor : Editor
    {
        public override void OnInspectorGUI() => ((PaintWithoutComponent)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(RaycastOnCollisionPainter))]
    public class PainterCasterEditor : Editor
    {
        public override void OnInspectorGUI() => ((RaycastOnCollisionPainter)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(ShadowVolumeTexture))]
    public class ShadowVolumeTextureEditor : Editor
    {
        public override void OnInspectorGUI() => ((ShadowVolumeTexture)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(SkinnedMeshCaster))]
    public class SkinnedMeshCasterEditor : Editor
    {
        public override void OnInspectorGUI() => ((SkinnedMeshCaster)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(VolumeTexture))]
    public class VolumeTextureEditor : Editor
    {
        public override void OnInspectorGUI() => ((VolumeTexture)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(VolumePaintingPlugin))]
    public class VolumePaintingPluginEditor : Editor
    {
        public override void OnInspectorGUI() => ((VolumePaintingPlugin)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(CombinedMaps.CombinedMapsControllerPlugin))]
    public class CombinedMapsControllerEditor : Editor
    {
        public override void OnInspectorGUI() => ((CombinedMaps.CombinedMapsControllerPlugin)target).Inspect(serializedObject);
    }

    [CustomEditor(typeof(WaterController))]
    public class WaterEditor : Editor
    {
        public override void OnInspectorGUI() => ((WaterController)target).Inspect(serializedObject);
    }

#endif
}