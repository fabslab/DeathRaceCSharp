namespace Pedestrian.Engine.UI
{
    public class HorizontalFocusGroup : FocusGroup
    {
        public override void FocusLeft()
        {
            SetFocus(focusedNode.Previous);
        }

        public override void FocusRight()
        {
            SetFocus(focusedNode.Next);
        }
    }
}
