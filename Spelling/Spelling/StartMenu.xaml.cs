using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spelling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartMenu : ContentPage
    {
        public StartMenu()
        {
            InitializeComponent();
        }

        private async void TextToSpeech(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Menu());
        }

        private async void BotAssist(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BotAssistant());
        }

        private async void WebPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WebPage());
        }
    }
}