using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMplayer
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        LessonsNames lName = new LessonsNames();

        FirstLessonSentences firstLessonSent = new FirstLessonSentences();
        SecondLessonSentences secondLessonSent = new SecondLessonSentences();
        ThirdLessonSentences thirdLessonSent = new ThirdLessonSentences();
        FourthLessonSentences fourthLessonSent = new FourthLessonSentences();


        int nextSentenceAudio = 0;
        int numberOfSelectedSentences = 0;
        int lectionIndex;
        int sentenceIndex;
        int sentecesRepetition = 0;

        static double speed = 1;
     
        string[] sentence = new string[3];
        string slovakSentence, firstSentence, secondSentence;

        bool newSelection = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chbSK.Enabled = false;
            chbDE1.Checked = false;
            chbDE1.Enabled = false;

            richTextBox1.Font = new Font("Arial", 16f, FontStyle.Bold);
            richTextBox1.BackColor = Color.White;

            richTextBox2.Font = new Font("Arial", 14f);
            richTextBox2.BackColor = Color.White;

            for (int f = 1; f < 19; f++)
            {
                listBox1.Items.Add(f.ToString() + ". lekcia");
            }
        }

        private void Play(string[] sentence, int sentenceNumber)
        {
            wplayer.URL = @sentence[sentenceNumber];
            wplayer.settings.rate = speed;
            wplayer.controls.play();

            sentecesRepetition++;

            if (chBWiederholung.Checked == true)    // if number of selected sentences is even to audio sentence repetition switch to next sentence
            {
                if (numberOfSelectedSentences == (sentecesRepetition - 1))
                {
                    sentecesRepetition = 0;

                    sentenceIndex++;
                    listBox2.SelectedIndex++;

                    UpdateSentenceLabel();

                    PlaySentence(sentenceIndex);                    
                }

            }
        }

        private void GetSentences(string wordsSK, string wordsDE)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();

            richTextBox1.SelectionColor = Color.Black;
            richTextBox2.SelectionColor = Color.Black;

            richTextBox1.Text = wordsSK;
            richTextBox2.Text = wordsDE;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = wplayer.status;

            if (wplayer.status.Contains("Stopped"))
            {
                nextSentenceAudio++;

                int sentenceNumber = SwapSentence2(nextSentenceAudio);

                Play(sentence, sentenceNumber);
            }           
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)    // https://stackoverflow.com/questions/22788625/winforms-get-key-pressed
        {
            if (keyData == Keys.Left)
            {
                GetPreviousSentence();
                return true;
            }
            if (keyData == Keys.Right)
            {
                GetNextSentence();
                return true;
            }
            if (keyData == Keys.Space)
            {
                if (btnPause.Enabled)
                {
                    wplayer.controls.pause();

                    btnPause.Enabled = false;
                    btnPlay.Enabled = true;

                    return true;
                }
                if (btnPlay.Enabled)
                {
                    wplayer.controls.play();

                    btnPause.Enabled = true;
                    btnPlay.Enabled = false;

                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private int SwapSentence2(int nextSentenceAudio)
        {
            int nextSentence = 0;

            switch (numberOfSelectedSentences)
            {
                case 1:
                    nextSentence = 0;
                    break;
                case 2:
                    nextSentence = nextSentenceAudio % 2;
                    break;
                case 3:
                    nextSentence = nextSentenceAudio % 3;
                    break;
            }

            return nextSentence;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            lectionIndex = listBox1.IndexFromPoint(e.Location);

            lectionIndex = lectionIndex + 1;

            label11.Text = "Lekcia " + lectionIndex;

            FillSentenceList(lectionIndex);

            GetLessonsName(lectionIndex);
        }

        private void GetLessonsName(int lectionIndex)
        {
            switch (lectionIndex)
            {
                case 1:
                    label15.Text = lName.LessonName[lectionIndex - 1];
                    break;
                case 2:
                    label15.Text = lName.LessonName[lectionIndex - 1];
                    break;
                case 3:
                    label15.Text = lName.LessonName[lectionIndex - 1];
                    break;
            }
        }
        int sentecesCount = 0;
        private void FillSentenceList(int lectionIndex)
        {
            listBox2.Items.Clear();

            switch (lectionIndex)
            {
                case 1:
                    sentecesCount = firstLessonSent.SentencesDE.Count;
                    for (int b = 1; b <= sentecesCount; b++)
                    {
                        listBox2.Items.Add("Veta " + b);
                    }
                    break;
                case 2:
                    sentecesCount = secondLessonSent.SentencesDE.Count;
                    for (int b = 1; b <= sentecesCount; b++)
                    {
                        listBox2.Items.Add("Veta " + b);
                    }
                    break;
                case 3:
                    sentecesCount = thirdLessonSent.SentencesDE.Count;
                    for (int b = 1; b <= sentecesCount; b++)
                    {
                        listBox2.Items.Add("Veta " + b);
                    }
                    break;
                case 4:
                    sentecesCount = fourthLessonSent.SentencesDE.Count;
                    for (int b = 1; b <= sentecesCount; b++)
                    {
                        listBox2.Items.Add("Veta " + b);
                    }
                    break;

            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            newSelection = true;

            sentecesRepetition = 0;

            if (newSelection)
            {
                EnableAssets();
            }

            sentenceIndex = this.listBox2.SelectedIndex;

            sentenceIndex = sentenceIndex + 1;

            UpdateSentenceLabel();

            PlaySentence(sentenceIndex);

            btnPlay.Enabled = false;
            btnPause.Enabled = true;
        }

        private void PlaySentence(int index)
        {
            String baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            slovakSentence = baseDirectory + "L" + lectionIndex + "\\L" + lectionIndex + "V" + sentenceIndex + "s.mp3";

            firstSentence = baseDirectory + "L" + lectionIndex + "\\L" + lectionIndex + "V" + sentenceIndex + "n1.mp3";
            secondSentence = baseDirectory + "L" + lectionIndex + "\\L" + lectionIndex + "V" + sentenceIndex + "n2.mp3";

            LoadSoundSentences2(slovakSentence, firstSentence, secondSentence);

            PrepareSenteces(lectionIndex);

            Play(sentence, 0);
        }

        private void LoadSoundSentences2(string slovakSentence, string firstSentence, string secondSentence)
        {
            if (newSelection) // need to allocate all interval variable in array when swapping on new sentence, because of overflow, when not all sentence checkboxes were checked and swapping from shorter sentence to longer
            {
                int inc = 0;

                sentence[inc] = slovakSentence;
               
                inc++;
                sentence[inc] = firstSentence;
              
                inc++;
                sentence[inc] = secondSentence;              

                newSelection = false;
            }

            sentecesRepetition = 0;

            numberOfSelectedSentences = 0;

            if (chbSK.Checked)
            {
                sentence[numberOfSelectedSentences] = slovakSentence;               

                numberOfSelectedSentences++;
            }
            if (chbDE1.Checked)
            {
                sentence[numberOfSelectedSentences] = firstSentence;              

                numberOfSelectedSentences++;
            }
            if (chbDE2.Checked)
            {
                sentence[numberOfSelectedSentences] = secondSentence;                

                numberOfSelectedSentences++;
            }
        }

        private void EnableAssets()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            btnPause.Enabled = true;
            //btnPlay.Enabled = true;

           // chbSK.Enabled = true;
           // chbDE1.Enabled = true;
            chbDE2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sentecesRepetition = 0;

            GetPreviousSentence();            
        }

        private void GetPreviousSentence()
        {
            newSelection = true;

            if (sentenceIndex == 1) // are we at first sentence? Then play last. If not play next previous. 
            {
                sentenceIndex = sentecesCount;
                listBox2.SelectedIndex = sentecesCount - 1;
            }
            else
            {
                sentenceIndex = sentenceIndex - 1;
                listBox2.SelectedIndex--;
            }

            UpdateSentenceLabel();

            PlaySentence(sentenceIndex);

            btnPlay.Enabled = false;
            btnPause.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sentecesRepetition = 0;

            GetNextSentence();           
        }

        private void GetNextSentence()
        {
            newSelection = true;

            if (sentenceIndex == sentecesCount) // are we at last sentence? Then play first. If not play next sentence. 
            {
                sentenceIndex = 1;
                listBox2.SelectedIndex = 0;
            }
            else
            {
                sentenceIndex = sentenceIndex + 1;
                listBox2.SelectedIndex++;
            }


            UpdateSentenceLabel();

            PlaySentence(sentenceIndex);

            btnPlay.Enabled = false;
            btnPause.Enabled = true;
        }

        private void UpdateSentenceLabel()
        {
            label12.Text = "Veta " + sentenceIndex;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                richTextBox1.Visible = false;
            else
                richTextBox1.Visible = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                richTextBox2.Visible = false;
            else
                richTextBox2.Visible = true;
        }

        private void PrepareSenteces(int lectionIndex)
        {
            switch (lectionIndex)
            {
                case 1:
                    string wordsDE = firstLessonSent.SentencesDE[sentenceIndex - 1];
                    string wordsSK = firstLessonSent.SentencesSK[sentenceIndex - 1];

                    GetSentences(wordsSK, wordsDE);

                    PreparePersonas(lectionIndex, firstLessonSent.Persona[sentenceIndex - 1]);
                    PrepareVerbs(lectionIndex, firstLessonSent.Verb[sentenceIndex - 1]);

                    break;
                case 2:
                    wordsDE = secondLessonSent.SentencesDE[sentenceIndex - 1];
                    wordsSK = secondLessonSent.SentencesSK[sentenceIndex - 1];

                    GetSentences(wordsSK, wordsDE);

                    PreparePersonas(lectionIndex, secondLessonSent.Persona[sentenceIndex - 1]);
                    PrepareVerbs(lectionIndex, secondLessonSent.Verb[sentenceIndex - 1]);

                    break;
                case 3:
                    wordsDE = thirdLessonSent.SentencesDE[sentenceIndex - 1];
                    wordsSK = thirdLessonSent.SentencesSK[sentenceIndex - 1];

                    GetSentences(wordsSK, wordsDE);

                    PreparePersonas(lectionIndex, thirdLessonSent.Persona[sentenceIndex - 1]);
                    PrepareVerbs(lectionIndex, thirdLessonSent.Verb[sentenceIndex - 1]);

                    break;
                case 4:
                    wordsDE = fourthLessonSent.SentencesDE[sentenceIndex - 1];
                    wordsSK = fourthLessonSent.SentencesSK[sentenceIndex - 1];

                    GetSentences(wordsSK, wordsDE);

                    PreparePersonas(lectionIndex, fourthLessonSent.Persona[sentenceIndex - 1]);
                    PrepareVerbs(lectionIndex, fourthLessonSent.Verb[sentenceIndex - 1]);

                    break;



            }
        }

        private void PreparePersonas(int lectionIndex, string firstLessonSentPersona)
        {
            label1.Text = firstLessonSentPersona;
            label14.Text = firstLessonSentPersona;
        }
        private void PrepareVerbs(int lectionIndex, string firstLessonSentVerb)
        {
            lblVerbs.Text = firstLessonSentVerb;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            wplayer.controls.pause();

            btnPause.Enabled = false;
            btnPlay.Enabled = true;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            wplayer.controls.play();

            btnPause.Enabled = true;
            btnPlay.Enabled = false;
        }

        private void chbSK_CheckedChanged(object sender, EventArgs e)
        {
            LoadSoundSentences2(slovakSentence, firstSentence, secondSentence);
        }

        private void chbDE1_CheckedChanged(object sender, EventArgs e)
        {
            LoadSoundSentences2(slovakSentence, firstSentence, secondSentence);
        }

        private void chbDE2_CheckedChanged(object sender, EventArgs e)
        {
            LoadSoundSentences2(slovakSentence, firstSentence, secondSentence);
        }

        private void chBWiederholung_CheckedChanged(object sender, EventArgs e)
        {
            sentecesRepetition = 0;
        }

        private void listBox2_MouseHover(object sender, EventArgs e)
        {
            listBox2.Focus();
        }

        private void numUDSpeed_ValueChanged(object sender, EventArgs e)
        {
            speed = (double)numUDSpeed.Value;
        }
    }
}
