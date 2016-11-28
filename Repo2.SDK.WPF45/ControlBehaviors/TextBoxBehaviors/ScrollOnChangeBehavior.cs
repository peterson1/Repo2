using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Repo2.SDK.WPF45.ControlBehaviors.TextBoxBehaviors
{
    public class ScrollOnChangeBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += (s, e) =>
            {
                AssociatedObject.ScrollToEnd();
            };
        }
    }
}
