namespace Pedestrian.Engine.UI
{
    public class VerticalFocusGroup : FocusGroup
    {
        public override void FocusUp()
        {
            SetFocus(focusedNode.Previous);
        }

        public override void FocusDown()
        {
            SetFocus(focusedNode.Next);
        }
    }
}
