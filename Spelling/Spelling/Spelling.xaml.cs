using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Reflection;

namespace Spelling
{
    public partial class Spelling : ContentPage
    {
        // ATRIBUTOS
        List<string> randomWords = new List<string>();      // ESTA ES LA LISTA DEL VOCABULARIO 
        List<string> givenAnswers = new List<string>();     // AQUI SE GUARDAN LAS RESPUESTAS DEL USUARIO
        
        int correct = 0;
        int incorrect = 0;
        
        string[] palabra = new string[] { }; // ENTRA EL STRING PARA SEPARAR ING/ESP
        string wordTurn = ""; // PALABRA QUE SONARÁ
        int wordNumber = 1; // LUGAR EN LISTA DE LA PALABRA QUE SONARÁ

        bool practicing = false;
        bool isOn = false;

        // CONSTRUCTOR
        public Spelling(string cefr, string level, bool practice)
        {
            InitializeComponent();
            SpellData.Text = "LEVEL: " + cefr + " - " + level;
            randomWords = Vocabulary.obtainVocabulary(cefr, level);
            palabra = divideWord(randomWords[0]);
            wordTurn = palabra[0];
            practicing = practice;
        }

        // MÉTODOS
        private async void Speak(object sender, EventArgs e)
        {

            if (isOn)
            {
                await TextToSpeech.SpeakAsync( wordTurn );
            }
            else
            {
                isOn = true;
                Btn_S.BackgroundColor = Color.FromHex("#FFEE07");
                Activity_Indicator.IsRunning = true;
                await TextToSpeech.SpeakAsync( wordTurn );
                Activity_Indicator.IsRunning = false;
                EntryWord.IsReadOnly = false;
            }
            
        }

        public void Practice(bool IsAPractice)
        {
            if (IsAPractice)
            {   
                try
                {
                    string[] InEs = divideWord( randomWords[wordNumber] );
                    CorrectWord.Text = InEs[0] + " - " + InEs[1];
                }
                catch (IndexOutOfRangeException)
                {
                    //CorrectWord.Text = randomWords[wordNumber - 1];
                    throw;
                }

            }
            else
            {
                CorrectWord.Text = "";
            }
        }

        private void Test(object sender, EventArgs e)
        {
            // REVISAR ESTA PARTE DEL CÓDIGO
            string word1 = wordTurn.Trim();
            string word2 = EntryWord.Text.Trim();

            Practice(practicing);

            if (word1.ToUpper() == word2.ToUpper())
            {
                correct++;
                word2.ToLower();
                givenAnswers.Add(wordNumber.ToString() + ". " + word2);
            }
            else
            {
                incorrect++;
                givenAnswers.Add(wordNumber.ToString() + ". " + word1 + " - " + word2);
                Vibration.Vibrate();
            }
            
            EntryWord.Text = "";
            Finished();
        }

        public async void Finished()
        {
            int totalWords = randomWords.Count();

            if (totalWords == wordNumber)
            {
                await Navigation.PushAsync(new Results(givenAnswers, correct, incorrect));
            }
            else
            {
                CountingWords.Text = "Word: " + wordNumber.ToString() + " of " + totalWords.ToString();
                palabra = divideWord(randomWords[wordNumber]);
                wordTurn = palabra[0];
                wordNumber++;
                await TextToSpeech.SpeakAsync(wordTurn);
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