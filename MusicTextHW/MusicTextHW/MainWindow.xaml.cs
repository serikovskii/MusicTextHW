using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicTextHW
{
    public partial class MainWindow : Window
    {
        private MediaPlayer player;
        private Thread thread;

        public MainWindow()
        {
            InitializeComponent();
            player = new MediaPlayer();
        }

        private void PlyaMusic(object sender, RoutedEventArgs e)
        {
            thread = new Thread(ThreadPlay);
            thread.IsBackground = true;
            thread.Start();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";

            if (openFile.ShowDialog()==true)
            {
                player.Open(new Uri(openFile.FileName));
            }
            ThreadPlay();
        }

        public void ThreadPlay()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate { player.Play(); }));
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Thread saveText = new Thread(SaveText);
            saveText.IsBackground = false;
            saveText.Start();
        }

        private void SaveText()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                TextRange doc = new TextRange(note.Document.ContentStart, note.Document.ContentEnd);
                using (FileStream fileStream = File.Create(DateTime.Now.ToLongDateString() + ".txt"))
                {
                    doc.Save(fileStream, DataFormats.Text);
                }

            }));
        }
    }
}
