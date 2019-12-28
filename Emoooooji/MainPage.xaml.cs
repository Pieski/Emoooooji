using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EmojiTranslator
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnEntryCompleted(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("the key \"ba\" refers to:"+Database.SecondaryDatabase["ba"]);
            string input = this.EntryBox.Text;
            this.ShowBox.Text = null;
            List<string> words = Translator.SplitString(input);
            foreach(string word in words)
            {
                string translated = Translator.GetEmojiFromWord(word);
                if (translated != word)
                    this.ShowBox.Text += translated;
                else
                {
                    string sec_result = Translator.GetEmojiFromChar(word);
                    if (sec_result != null)
                        this.ShowBox.Text += sec_result;
                    else
                        this.ShowBox.Text += word;
                }
            }
        }
    }
}
