using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiling;
using AnimatedSprite;
using Tiler;

namespace GameAssessment
{
    class EndPositionTile:AnimateSheetSprite
    {
        public EndPositionTile(Game g, Vector2 userPosition, List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth) : base(g,userPosition,sheetRefs,frameWidth,frameHeight,layerDepth)
        {
            DrawOrder = 1;
        }

        public override void Update(GameTime gametime)
        {
            TilePlayer p = Game.Services.GetService<TilePlayer>();
            if (p == null) return;
            else
            {

                if (p.BoundingRectangle.Intersects(this.BoundingRectangle) && p.objectivesComplete == false)
                    p.PixelPosition = p.previousPosition;
            }

            
            base.Update(gametime);
        }
    }
}
