using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Lakiernia.View.Behaviors
{
    public class Wyszukiwanie : Behavior<TextBox>
    {
        private static readonly DependencyProperty TekstZachecajacyProperty = DependencyProperty.Register("TekstZachecajacy", typeof(string), typeof(Wyszukiwanie));

        public string TekstZachecajacy
        {
            get
            {
                return GetValue(TekstZachecajacyProperty) as string;
            }
            set
            {
                SetValue(TekstZachecajacyProperty, value);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Text = TekstZachecajacy;
            AssociatedObject.GotFocus += Wejscie;
            AssociatedObject.LostFocus += Wyjscie;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= Wejscie;
            AssociatedObject.LostFocus -= Wyjscie;
            base.OnDetaching();
        }

        public void Wejscie(object sender, EventArgs e)
        {
            if (AssociatedObject.Text == TekstZachecajacy) AssociatedObject.Text = "";
        }

        public void Wyjscie(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text)) AssociatedObject.Text = TekstZachecajacy;
        }
    }
}
