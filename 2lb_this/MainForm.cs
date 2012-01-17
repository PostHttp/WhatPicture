using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices; //for Sound
using System.Reflection; //for Sound
using System.IO; //for Sound;

namespace _2lb_this
{
    public partial class WhatPicture : Form
    {
        public WhatPicture()
        {
            InitializeComponent();
        }

        const int lengthArrayPicture = 30;
        bool pressPictureFirstBool, pressPictureSecondBool, firstStart = true;
        int[] valueArrayPicture =
            {0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14},
            valueSecuenceArrayPicture = new int[lengthArrayPicture];
        int pressPictureFirstNumber, pressPictureSecondNumber, counterClickPicture;
        Point[] valuePiontLocationArrayPicture = new Point[lengthArrayPicture];

        public class Sound
        {
            private byte[] m_soundBytes;
            private string m_fileName;

            private enum Flags
            {
                SND_SYNC = 0x0000,  /* play synchronously (default) */
                SND_ASYNC = 0x0001,  /* play asynchronously */
                SND_NODEFAULT = 0x0002,  /* silence (!default) if sound not found */
                SND_MEMORY = 0x0004,  /* pszSound points to a memory file */
                SND_LOOP = 0x0008,  /* loop the sound until next sndPlaySound */
                SND_NOSTOP = 0x0010,  /* don't stop any currently playing sound */
                SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
                SND_ALIAS = 0x00010000, /* name is a registry alias */
                SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
                SND_FILENAME = 0x00020000, /* name is file name */
                SND_RESOURCE = 0x00040004  /* name is resource name or atom */
            }

            [DllImport("CoreDll.DLL", EntryPoint = "PlaySound", SetLastError = true)]
            private extern static int WCE_PlaySound(string szSound, IntPtr hMod, int flags);

            [DllImport("CoreDll.DLL", EntryPoint = "PlaySound", SetLastError = true)]
            private extern static int WCE_PlaySoundBytes(byte[] szSound, IntPtr hMod, int flags);

            /// <summary>
            /// Construct the Sound object to play sound data from the specified file.
            /// </summary>
            public Sound(string fileName)
            {
                m_fileName = fileName;
            }

            /// <summary>
            /// Construct the Sound object to play sound data from the specified stream.
            /// </summary>
            public Sound(Stream stream)
            {
                // read the data from the stream
                m_soundBytes = new byte[stream.Length];
                stream.Read(m_soundBytes, 0, (int)stream.Length);
            }

            /// <summary>
            /// Play the sound
            /// </summary>
            public void Play()
            {
                // if a file name has been registered, call WCE_PlaySound,
                //  otherwise call WCE_PlaySoundBytes
                if (m_fileName != null)
                    WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
                else
                    WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_MEMORY));
            }
        }

        int playSound(string nameSound) //воспроизвести выбранную музыку
        {
            //            Sound soundMusic = new Sound(nameSound + ".wav");
            Sound soundMusic = new Sound(nameSound + ".wav");
            soundMusic.Play();
            return 0;
        }

        int newGame()
        {
            allReset();            
            counterClickPicture = 0; //Обнулить количество кликов
            //loadSettings();
            viewNumberClickPicture(); //Показать количество кликов
            generateArrayPicture(); //Сгенерировать расположения картинок
            locationPicture(); //Расставить картинки
            for (int i = 0; i < lengthArrayPicture; i++) //Отобразить все картинки
            {
                visiblePictureHideBox(i, true);
                visiblePictureBox(i, true);
            }
            firstStart = false;
            return 0;
        }

        int allReset()
        {
            if (!firstStart)
                for (int i = 0; i < lengthArrayPicture; i++) //Отобразить все картинки
                {
                    visiblePictureHideBox(i, false);
                    visiblePictureBox(i, false);
                }
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            pressPictureFirstBool = false; //Невыбрана первая картинка
            pressPictureSecondBool = false; //Не выбрана вторая картинка
            return 0;
        }

        int loadSettings()
        {
            counterClickPicture = Properties.Settings.Default.savedScore;
            MessageBox.Show(Convert.ToString(counterClickPicture));
            return 0;
        }

        int generateArrayPicture() //Сгенерировать расположения картинок
        {
            bool endGenerate = false;

            for (int i = 0; i < lengthArrayPicture; i++) //зануление массива перед генерацией
                valueSecuenceArrayPicture[i] = 0;
            for (int i = lengthArrayPicture - 1; i >= 0; i--) //заполнение по случайному индексу
            {
                visibleLabel(1, true);
                Update();
                while (!endGenerate)
                {
                    Random valueRandom = new Random();
                    waitSleep(20);
                    int valueAllRandom = valueRandom.Next(lengthArrayPicture);
                    if (valueSecuenceArrayPicture[valueAllRandom] == 0)
                    {
                        valueSecuenceArrayPicture[valueAllRandom] = i;
                        endGenerate = true;
                    }
                }
                endGenerate = false;
            }
            visibleLabel(1, false);
            return 0;
        }

        int locationPicture() //Расставить картинки
        {
            const int countColumn = 5, countString = 6,
                locationStartPictureX = 12, locationStartPictureY = 27;
            int[] locationArrayPictureX = new int[lengthArrayPicture], locationArrayPictureY = new int[lengthArrayPicture];
            int counterPicture = 0, locationPictureX, locationPictureY;

            locationPictureX = locationStartPictureX;
            locationPictureY = locationStartPictureY;
            for (int i = 0; i < countString; i++)
            {
                for (int j = 0; j < countColumn; j++) //locationArrayPictureX
                {
                    locationArrayPictureX[counterPicture] = locationPictureX;
                    counterPicture++;
                    locationPictureX += 64;
                }
                locationPictureX = locationStartPictureX;
            }
            counterPicture = 0;
            for (int i = 0; i < countString; i++) //locationArraypictureY
            {
                for (int j = 0; j < countColumn; j++)
                {                    
                    locationArrayPictureY[counterPicture] = locationPictureY;
                    counterPicture++;
                }
                locationPictureY += 64;
            }
            for (int i = 0; i < lengthArrayPicture; i++)
            {
                valuePiontLocationArrayPicture[i] = new Point(locationArrayPictureX[i], locationArrayPictureY[i]);
                (this.Controls["pictureBox" + valueSecuenceArrayPicture[i]] as PictureBox).Location = valuePiontLocationArrayPicture[i];
                (this.Controls["pictureHideBox" + valueSecuenceArrayPicture[i]] as PictureBox).Location = valuePiontLocationArrayPicture[i];
            }
            return 0;
        }

        int visiblePictureBox(int numberPicture, bool boolVisiblePicture) //Функция отображения/скрытия любой картинки pictureBox
        {
            (this.Controls["pictureBox" + numberPicture] as PictureBox).Visible = boolVisiblePicture;
            return 0;
        }

        int visiblePictureHideBox(int numberHidePicture, bool boolVisiblePicture) //Функция отображения/скрытия любой картинки pictureHideBox
        {
            (this.Controls["pictureHideBox" + numberHidePicture] as PictureBox).Visible = boolVisiblePicture;
            return 0;
        }

        int visibleLabel(int numberLabel, bool boolVisibleLabel)
        {
            (this.Controls["label" + numberLabel] as Label).Visible = boolVisibleLabel;
            return 0;
        }

        int viewNumberClickPicture() //Показать количество кликов
        {
            textBox1.Text = Convert.ToString(counterClickPicture);
            return 0;
        }

        int waitSleep(int numberMiliSeconds)
        {
            System.Threading.Thread.Sleep(numberMiliSeconds);
            return 0;
        }

        int cmpPictureNumber(int numberPicture) //Сравнение номеров картинок, проверка на последовательность выбора картинок
        {
            counterClickPicture++;
            viewNumberClickPicture();
            if ((pressPictureFirstBool == false) && (pressPictureSecondBool == false)) //нажали первую и до этого не была нажата ни одна
            {                
                pressPictureFirstNumber = numberPicture;
                visiblePictureHideBox(pressPictureFirstNumber, false);
                pressPictureFirstBool = true;
                checkBox1.Checked = true;
            }
            else
            {
                if (pressPictureFirstNumber == numberPicture) //если была нажата одна и та же картинка
                {
                    visiblePictureHideBox(pressPictureFirstNumber, true);
                    pressPictureFirstBool = false;
                    checkBox1.Checked = false;
                }
                else
                {
                    if ((pressPictureFirstBool == true) && (pressPictureSecondBool == false)) //была нажата первая и нажали вторую
                    {
                        pressPictureSecondNumber = numberPicture;
                        visiblePictureHideBox(pressPictureSecondNumber, false);
                        pressPictureSecondBool = true;
                        checkBox2.Checked = true;
                        if (valueArrayPicture[pressPictureFirstNumber] == valueArrayPicture[pressPictureSecondNumber]) //если номера совпали
                        {
                            visiblePictureHideBox(pressPictureFirstNumber, false);
                            visiblePictureHideBox(pressPictureSecondNumber, false);
                            Update();
                            //playSound("yes");
                            waitSleep(400);
                            visiblePictureBox(pressPictureFirstNumber, false);
                            visiblePictureBox(pressPictureSecondNumber, false);
                            pressPictureFirstBool = false;
                            pressPictureSecondBool = false;
                            checkBox1.Checked = false;
                            checkBox2.Checked = false;
                        }
                        else //если номера НЕ совпали
                        {
                            visiblePictureHideBox(pressPictureSecondNumber, false);
                            Update();
                            //playSound("no");
                            waitSleep(400);
                            visiblePictureHideBox(pressPictureFirstNumber, true);
                            visiblePictureHideBox(pressPictureSecondNumber, true);
                            pressPictureFirstBool = false;
                            pressPictureSecondBool = false;
                            checkBox1.Checked = false;
                            checkBox2.Checked = false;
                        }
                    }
                }
            }
            return 0;
        }



        private void pictureBox0_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(1);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(3);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(5);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(12);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(13);
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(14);
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(15);
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(16);
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(17);
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(18);
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(19);
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(20);
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(21);
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(22);
        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(23);
        }

        private void pictureBox24_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(24);
        }

        private void pictureBox25_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(25);
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(26);
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(27);
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(28);
        }

        private void pictureBox29_Click(object sender, EventArgs e)
        {
            //cmpPictureNumber(29);
        }







        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help objectHelp = new Help();
            objectHelp.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About objectAbout = new About();
            objectAbout.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Properties.Settings.Default.savedScore = counterClickPicture;
            //Properties.Settings.Default.Save();
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }







        private void pictureHideBox0_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(0);
        }

        private void pictureHideBox1_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(1);
        }

        private void pictureHideBox2_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(2);
        }

        private void pictureHideBox3_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(3);
        }

        private void pictureHideBox4_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(4);
        }

        private void pictureHideBox5_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(5);
        }

        private void pictureHideBox6_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(6);
        }

        private void pictureHideBox7_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(7);
        }

        private void pictureHideBox8_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(8);
        }

        private void pictureHideBox9_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(9);
        }

        private void pictureHideBox10_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(10);
        }

        private void pictureHideBox11_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(11);
        }

        private void pictureHideBox12_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(12);
        }

        private void pictureHideBox13_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(13);
        }

        private void pictureHideBox14_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(14);
        }

        private void pictureHideBox15_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(15);
        }

        private void pictureHideBox16_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(16);
        }

        private void pictureHideBox17_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(17);
        }

        private void pictureHideBox18_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(18);
        }

        private void pictureHideBox19_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(19);
        }

        private void pictureHideBox20_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(20);
        }

        private void pictureHideBox21_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(21);
        }

        private void pictureHideBox22_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(22);
        }

        private void pictureHideBox23_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(23);
        }

        private void pictureHideBox24_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(24);
        }

        private void pictureHideBox25_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(25);
        }

        private void pictureHideBox26_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(26);
        }

        private void pictureHideBox27_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(27);
        }

        private void pictureHideBox28_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(28);
        }

        private void pictureHideBox29_Click(object sender, EventArgs e)
        {
            cmpPictureNumber(29);
        }
    }
}
