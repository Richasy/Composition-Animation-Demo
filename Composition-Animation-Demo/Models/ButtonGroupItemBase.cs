namespace CompositionAnimationDemo.Models
{
    public class ButtonGroupItemBase : IButtonGroupItem
    {
        public string DisplayText { get; set; }
        public int SelectionIndex { get; set; }

        internal ButtonGroupItemBase(){}

        public ButtonGroupItemBase(string text)
        {
            this.DisplayText = text;
            SelectionIndex = -1;
        }
    }
}
