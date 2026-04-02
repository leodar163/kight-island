using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tiles
{
    [CreateAssetMenu(fileName = "Ruleset", menuName = "Tiles/Dual Tile Ruleset", order = 0)]
    public class DualTileRuleset : ScriptableObject
    {
        [SerializeField] private TileBase r0001;
        [SerializeField] private TileBase r0010;
        [SerializeField] private TileBase r0011;
        [SerializeField] private TileBase r0100;
        [SerializeField] private TileBase r0101;
        [SerializeField] private TileBase r0110;
        [SerializeField] private TileBase r0111;
        [SerializeField] private TileBase r1000;
        [SerializeField] private TileBase r1001;
        [SerializeField] private TileBase r1010;
        [SerializeField] private TileBase r1011;
        [SerializeField] private TileBase r1100;
        [SerializeField] private TileBase r1101;
        [SerializeField] private TileBase r1110;
        [SerializeField] private TileBase r1111;
        
        public TileBase Evaluate(bool upLeft, bool upRight, bool downLeft, bool downRight)
        {
            if (!upLeft && !upRight && !downLeft && !downRight) return null;
            if (!upLeft && !upRight && !downLeft && downRight)  return r0001;
            if (!upLeft && !upRight && downLeft && !downRight)  return r0010;
            if (!upLeft && !upRight && downLeft && downRight)   return r0011;
            if (!upLeft && upRight && !downLeft && !downRight)  return r0100;
            if (!upLeft && upRight && !downLeft && downRight)   return r0101;
            if (!upLeft && upRight && downLeft && !downRight)   return r0110;
            if (!upLeft && upRight && downLeft && downRight)    return r0111;
            if (upLeft && !upRight && !downLeft && !downRight)  return r1000;
            if (upLeft && !upRight && !downLeft && downRight)   return r1001;
            if (upLeft && !upRight && downLeft && !downRight)   return r1010;
            if (upLeft && !upRight && downLeft && downRight)    return r1011;
            if (upLeft && upRight && !downLeft && !downRight)   return r1100;
            if (upLeft && upRight && !downLeft && downRight)    return r1101;
            if (upLeft && upRight && downLeft && !downRight)    return r1110;
            return r1111;
        }
    }
}