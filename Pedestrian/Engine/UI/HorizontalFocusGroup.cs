using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pedestrian.Engine.Input;
using System;
using System.Collections.Generic;

namespace Pedestrian.Engine.UI
{
    public class HorizontalFocusGroup : IFocusGroup
    {
        public IFocusable FocusedItem { get; private set; }
        public Action<IFocusable> OnItemSelected { get; set; }

        LinkedList<IFocusable> nodes;
        LinkedListNode<IFocusable> focusedNode;


        public HorizontalFocusGroup()
        {
            nodes = new LinkedList<IFocusable>();
        }

        public void Update(GameTime gameTime)
        {
            if (GlobalInput.WasCommandEntered(InputCommand.Enter))
            {
                OnItemSelected?.Invoke(FocusedItem);
            }
            else if (GlobalInput.WasCommandEntered(InputCommand.Left))
            {
                FocusLeft();
            }
            else if (GlobalInput.WasCommandEntered(InputCommand.Right))
            {
                FocusRight();
            }
        }

        public void AddItem(IFocusable item)
        {
            var node = nodes.AddLast(item);
            if (FocusedItem == null)
            {
                SetFocus(node);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in nodes)
            {
                item.Draw(spriteBatch);
            }
        }

        public void FocusLeft()
        {
            SetFocus(focusedNode.Previous);
        }

        public void FocusRight()
        {
            SetFocus(focusedNode.Next);
        }

        public void FocusUp() {}
        public void FocusDown() {}

        private void SetFocus(LinkedListNode<IFocusable> node)
        {
            if (node != null)
            {
                focusedNode = node;
                FocusedItem?.OnFocusRemoved?.Invoke();
                FocusedItem = focusedNode.Value;
                FocusedItem.OnFocused?.Invoke();
            }
        }
    }
}
