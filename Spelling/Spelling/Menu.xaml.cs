using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
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
                if (Skill.IsToggled) // Spelling -> You Listen
                {
                    await Navigation.PushAsync(new Spelling(picker.Items[picker.SelectedIndex], Changed.Text, Practice.IsToggled));
                }
                else // Listening -> You Speak
                {
                    var permissions = await Permissions.CheckStatusAsync<Permissions.Microphone>();
                    if(permissions !=  PermissionStatus.Granted)
                    {
                        permissions = await Permissions.RequestAsync<Permissions.Microphone>();
                    }

                    if(permissions == PermissionStatus.Granted)
                    {
                        await Navigation.PushAsync(new Listening(picker.Items[picker.SelectedIndex], Changed.Text, Practice.IsToggled));
                    }
                    else
                    {
                        string action = await DisplayActionSheet("Enable microphone permission to continue", "Cancel", null);
                    }
                    
                } 
            }
            catch (ArgumentOutOfRangeException)
            {
                await DisplayAlert("Wrong values", "Oops!! Try again", "OK");
            }
        }

    }
}