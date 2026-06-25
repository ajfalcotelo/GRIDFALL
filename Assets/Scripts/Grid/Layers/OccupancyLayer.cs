using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OccupancyLayer : BaseLayer<IUnitRoot>
{
    public OccupancyLayer(Tilemap ground)
        : base(ground) { }

    public override void BuildLayer()
    {
        var width = ground.cellBounds.size.x;
        var height = ground.cellBounds.size.y;
        cells = new IUnitRoot[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector2Int(x, y);
                SetNode(pos, null);
            }
        }
    }
}
