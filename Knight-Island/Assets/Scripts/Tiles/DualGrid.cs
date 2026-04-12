using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Tilemap), typeof(TilemapRenderer))]
    public class DualGrid : MonoBehaviour
    {
        [SerializeField] private TileBase paintingTile;
        [SerializeField] private DualTileRuleset ruleset;
        [HideInInspector] [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tilemap fakeTilemap;
        [HideInInspector] [SerializeField] private TilemapRenderer fakeTilemapRenderer;
        [Space]
        [SerializeField] private bool showFakeTilemap;

        private void OnValidate()
        {
            if (tilemap == null) TryGetComponent(out tilemap);
            if (fakeTilemap != null)
            {
                transform.position =  fakeTilemap.transform.position +  new Vector3(0.5f, 0.5f);
                
                if (fakeTilemapRenderer == null) fakeTilemap.TryGetComponent(out fakeTilemapRenderer);
            }
        }

        void Start()
        {
            RenderMap();
        }
        
        void Update()
        {
            if (Application.isPlaying) Destroy(this);
            
            RenderMap();
        }

        private void RenderMap()
        {
            tilemap.ClearAllTiles();
            
            if (fakeTilemap == null) return;
            if (paintingTile == null) return;
            
            if(fakeTilemapRenderer != null) fakeTilemapRenderer.enabled = showFakeTilemap;
            
            fakeTilemap.CompressBounds();
            BoundsInt gridBounds = fakeTilemap.cellBounds;
            
            if (gridBounds.size.x < 2 || gridBounds.size.y < 2 ) return;
            
            for (int y = gridBounds.position.y - 1; y < gridBounds.position.y + gridBounds.size.y; y++)
            {
                for (int x = gridBounds.position.x - 1; x < gridBounds.position.x + gridBounds.size.x; x++)
                {
                    TileBase upLeft = fakeTilemap.GetTile(new Vector3Int(x, y+1, 0));
                    TileBase upRight = fakeTilemap.GetTile(new Vector3Int(x+1, y+1, 0));
                    TileBase downLeft = fakeTilemap.GetTile(new Vector3Int(x, y, 0));
                    TileBase downRight = fakeTilemap.GetTile(new Vector3Int(x+1, y, 0));
                    
                    TileBase evaluated = ruleset.Evaluate(
                        upLeft == paintingTile, 
                        upRight == paintingTile, 
                        downLeft == paintingTile, 
                        downRight == paintingTile
                        );
                    
                    tilemap.SetTile(new Vector3Int(x, y, 0), evaluated);
                }
            }
            tilemap.RefreshAllTiles();
        }
    }
}
