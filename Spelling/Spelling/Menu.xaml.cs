using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spelling
{
    public partial class Menu : ContentPage
    {

        public Menu()
        {
            InitializeComponent();
        }

        private void ChangeLabel(object sender, ValueChangedEventArgs args)
        {
            double valor = Math.Truncate(args.NewValue);

            if (valor == 0) { valor = 1; }
            Changed.Text = valor.ToString();
        }

        async void AnotherPage(object sender, EventArgs e)
        {
            try
            {
                //Activity_Indicator.IsRunning = true;
                await Navigation.PushAsync(new Spelling(picker.Items[picker.SelectedIndex], Changed.Text, Practice.IsToggled));
            }
            catch (ArgumentOutOfRangeException)
            {
                await DisplayAlert("Wrong values", "Oops!! Try again", "OK");
            }
        }

    }
}