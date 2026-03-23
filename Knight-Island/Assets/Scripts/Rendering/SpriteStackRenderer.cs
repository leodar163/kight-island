using System;
using UnityEngine;

namespace Rendering
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SpriteStackRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite atlas;
        [SerializeField] [Min(1)] private int sliceCount = 8;
        [SerializeField] private float angle;

        [SerializeField] [HideInInspector] private MeshFilter meshFilter;
        [SerializeField] [HideInInspector] private MeshRenderer meshRenderer;

        private static readonly int Atlas = Shader.PropertyToID("_Atlas");
        private static readonly int SliceCount = Shader.PropertyToID("_SliceCount");
        private static readonly int SliceSpread = Shader.PropertyToID("_SliceSpread");
        private static readonly int SliceHeightUV = Shader.PropertyToID("_SliceHeightUV");
        private static readonly int Angle = Shader.PropertyToID("_Angle");
        private static readonly int SliceSizePx = Shader.PropertyToID("_SliceSizePx");
        private static readonly int PaddingRatioX = Shader.PropertyToID("_PaddingRatioX");
        private static readonly int PaddingRatioY = Shader.PropertyToID("_PaddingRatioY");
        private static readonly int PaddingYuv = Shader.PropertyToID("_PaddingYUV");

        private void OnValidate()
        {
            if (meshFilter == null) TryGetComponent(out meshFilter);
            if (meshRenderer == null) TryGetComponent(out meshRenderer);
            if (atlas)
            {   
                GenerateMesh();
                UpdateMaterialProperties();
            }
        }

        private void GenerateMesh()
        {
            float sliceWidthPx = (float)atlas.texture.width / sliceCount;
            float sliceHeightPx = atlas.texture.height;

            float ppu = atlas.pixelsPerUnit;
            float sliceWidth = sliceWidthPx / ppu;
            float sliceHeight = sliceHeightPx / ppu;

            float totalHeight = sliceHeight + (sliceCount - 1) * (1f / ppu);

            // Padding dû à la diagonale, en unités monde
            float diagonal = Mathf.Sqrt(sliceWidthPx * sliceWidthPx + sliceHeightPx * sliceHeightPx);
            float paddingX = (diagonal - sliceWidthPx) / ppu;
            float paddingY = (diagonal - sliceHeightPx) / ppu;

            float quadWidth = sliceWidth + paddingX;
            float quadHeight = totalHeight + paddingY;

            Mesh mesh = new();

            Vector3[] vertices =
            {
                new(-quadWidth / 2f, -paddingY / 2f,            0),
                new( quadWidth / 2f, -paddingY / 2f,            0),
                new(-quadWidth / 2f, -paddingY / 2f + quadHeight, 0),
                new( quadWidth / 2f, -paddingY / 2f + quadHeight, 0),
            };

            int[] triangles = { 0, 2, 1, 2, 3, 1 };

            Vector2[] uvs =
            {
                new(0, 0),
                new(1, 0),
                new(0, 1),
                new(1, 1),
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;
        }

        private void UpdateMaterialProperties()
        {
            float sliceWidthPx = (float)atlas.texture.width / sliceCount;
            float sliceHeightPx = atlas.texture.height;

            float diagonal = Mathf.Sqrt(sliceWidthPx * sliceWidthPx + sliceHeightPx * sliceHeightPx);
            float paddingYPx = diagonal - sliceHeightPx;
            float paddingXPx = diagonal - sliceWidthPx;

            // Hauteur totale du quad en pixels, padding inclus
            float totalQuadHeightPx = sliceHeightPx + (sliceCount - 1) + paddingYPx;
            float paddingYUV = (diagonal - sliceHeightPx) / 2f / totalQuadHeightPx;

            MaterialPropertyBlock mpb = new();
            meshRenderer.GetPropertyBlock(mpb);
            mpb.SetTexture(Atlas, atlas.texture);
            mpb.SetInt(SliceCount, sliceCount);
            mpb.SetFloat(SliceSpread, 1f / totalQuadHeightPx);
            mpb.SetFloat(SliceHeightUV, sliceHeightPx / totalQuadHeightPx);
            mpb.SetFloat(Angle, angle * Mathf.Deg2Rad);
            mpb.SetVector(SliceSizePx, new Vector2(sliceWidthPx, sliceHeightPx));
            mpb.SetFloat(PaddingRatioX, diagonal / sliceWidthPx);
            mpb.SetFloat(PaddingRatioY, diagonal / sliceHeightPx);
            mpb.SetFloat(PaddingYuv, paddingYUV);
            meshRenderer.SetPropertyBlock(mpb);
        }
    }
}
