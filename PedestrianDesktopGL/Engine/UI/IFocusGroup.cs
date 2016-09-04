using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pedestrian.Engine.UI
{
    interface IFocusGroup
    {
        IFocusable FocusedItem { get; }
        Action<IFocusable> OnItemSelected { get; set; }

        void AddItem(IFocusable item);
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
        void FocusRight();
        void FocusLeft();
        void FocusDown();
        void FocusUp();
    }
}
