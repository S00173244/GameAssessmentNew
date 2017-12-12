using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimatedSprite;
using Microsoft.Xna.Framework;
using Tiling;
using Tiler;

namespace GameAssessment
{
    class StartPositionTile:AnimateSheetSprite
    {
        public StartPositionTile(Game g,Vector2 userPosition,List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth) : base(g,userPosition,sheetRefs,frameWidth,frameHeight,layerDepth)
        {
            DrawOrder = 1;
        }
    }
}
