using System;
using UnityEngine;

namespace Rendering
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SpriteStackRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite atlas;
        [SerializeField] [Min(1)] private int sliceCount = 8;

        [SerializeField] [HideInInspector] private MeshFilter meshFilter;
        [SerializeField] [HideInInspector] private MeshRenderer meshRenderer;

        private static readonly int Atlas = Shader.PropertyToID("_Atlas");
        private static readonly int SliceCount = Shader.PropertyToID("_SliceCount");
        private static readonly int SliceSpread = Shader.PropertyToID("_SliceSpread");
        private static readonly int SliceHeightUV = Shader.PropertyToID("_SliceHeightUV");

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
            // Une slice = toute la hauteur de l'atlas, largeur = atlas.width / sliceCount
            float sliceWidthPx = (float)atlas.texture.width / sliceCount;
            float sliceHeightPx = atlas.texture.height;

            // On choisit un PPU (pixels per unit) de référence, ex: 16
            float ppu = atlas.pixelsPerUnit;
            float sliceWidth = sliceWidthPx / ppu;
            float sliceHeight = sliceHeightPx / ppu;    

            float totalHeight = sliceHeight + (sliceCount - 1) * (1 / ppu);

            Mesh mesh = new (); 

            Vector3[] vertices =
            {
                new (-sliceWidth / 2f, 0,           0),
                new ( sliceWidth / 2f, 0,           0),
                new (-sliceWidth / 2f, totalHeight, 0),
                new ( sliceWidth / 2f, totalHeight, 0),
            };

            int[] triangles = { 0, 2, 1, 2, 3, 1 };

            Vector2[] uvs =
            {
                new (0, 0),
                new (1, 0),
                new (0, 1),
                new (1, 1),
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;
        }

        private void UpdateMaterialProperties()
        {
            float totalHeightPx = atlas.texture.height + sliceCount - 1;
            
            MaterialPropertyBlock mpb = new ();
            meshRenderer.GetPropertyBlock(mpb);
            mpb.SetTexture(Atlas, atlas.texture);
            mpb.SetInt(SliceCount, sliceCount);
            mpb.SetFloat(SliceSpread, 1f / (atlas.texture.height + sliceCount - 1)); // en UV
            mpb.SetFloat(SliceHeightUV, atlas.texture.height / totalHeightPx);
            meshRenderer.SetPropertyBlock(mpb);
        }
    }
}
