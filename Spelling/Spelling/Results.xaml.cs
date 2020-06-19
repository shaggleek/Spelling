using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spelling
{
    public partial class Results : ContentPage
    {
        public Results(List<string> resultados, int correctas, int incorrectas)
        {
            InitializeComponent();

            Correctas.Text = "Correct: " + correctas.ToString();
            Incorrectas.Text = "Incorrect: " + incorrectas.ToString();

            foreach (var user in resultados)
            {
                ResultadosList.Children.Add(new Label { Text = user, FontSize = 20 });
            }
        }

        async void GoHome_Btn(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}