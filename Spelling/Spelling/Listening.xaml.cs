using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.CognitiveServices.Speech;

namespace Spelling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listening : ContentPage
    {
        // ATRIBUTOS
        List<string> randomWords = new List<string>();      // ESTA ES LA LISTA DEL VOCABULARIO 
        List<string> givenAnswers = new List<string>();     // AQUI SE GUARDAN LAS RESPUESTAS DEL USUARIO

        int correct = 0;
        int incorrect = 0;

        string[] palabra = new string[] { }; // ENTRA EL STRING PARA SEPARAR ING/ESP
        int control = 0; // LUGAR EN LISTA DE LA PALABRA QUE SONARÁ

        public Listening(string cefr, string level, bool practice)
        {
            InitializeComponent();
            ListenData.Text = "LEVEL: " + cefr + " - " + level;
            randomWords = Vocabulary.obtainVocabulary(cefr, level);
            palabra = divideWord(randomWords[0]);
            TheWord.Text = palabra[0];
        }

        static async Task<string> RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription("8592e9dcfeab4bd5980dae76bfac78af", "southcentralus");
            var recognizer = new SpeechRecognizer(config);
            var result = await recognizer.RecognizeOnceAsync();
            return result.Text;
        }

        private async void SpeakTesting(object sender, EventArgs e)
        {
            Vibration.Vibrate();
            string UserSaid = await RecognizeSpeechAsync();
            palabra = divideWord(randomWords[control]);

            if (UserSaid.EndsWith("."))
            {
                UserSaid = UserSaid.Remove(UserSaid.Length - 1, 1);
            }

            if (palabra[0].ToUpper() == UserSaid.ToUpper())
            {
                correct++;
                givenAnswers.Add(control.ToString() + ". " + UserSaid.ToLower());
            }
            else
            {
                incorrect++;
                givenAnswers.Add(control.ToString() + ". " + palabra[0] + " - " + UserSaid.ToLower());
            }

            control++;

            if (control >= randomWords.Count)
            {
                await Navigation.PushAsync(new Results(givenAnswers, correct, incorrect));
            }
            else
            {
                palabra = divideWord(randomWords[control]);
                TheWord.Text = palabra[0];
            }
        }

        public string[] divideWord(string word)
        {
            string[] words = word.Split(',');
            return words;
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Are you sure?", "Cancel", null, "Yes", "No");
            if (action == "Yes")
            {
                await Navigation.PushAsync(new Results(givenAnswers, correct, incorrect));
            }

        }
    }
}