using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MinerClient.ServiceReference1;

namespace MinerClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private new string Name = "qq";

        private Button[,] btns; //массив кнопок
        private DispatcherTimer timer;

        public MainWindow()
        {
          InitializeComponent();
          //
          MI.IsEnabled = false;
          gridPanel.Visibility = Visibility.Hidden;
          //

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += timer_Tick;
        }
        int N, M, NumBombs;

        void timer_Tick(object sender, EventArgs e)
        {
            int tim;
            int.TryParse(lblTimer.Content.ToString(), out tim);
            tim++;
            lblTimer.Content = tim.ToString();
        }

       private void Window_Loaded_easy(object sender, RoutedEventArgs e)
        {
            N = 9;
            M = 9;
            NumBombs = 10;
            Miner.Height = 360;
            Miner.Width = 290;
            gridPanel.Width = Miner.Width;
            Window_Loaded(sender, e);
        }

        private void Window_Loaded_normal(object sender, RoutedEventArgs e)
        {
            N = 16;
            M = 16;
            NumBombs = 40;
            Miner.Height = 570;
            Miner.Width = 500;
            gridPanel.Width = Miner.Width;
            Window_Loaded(sender, e);
        }

        private void Window_Loaded_hard(object sender, RoutedEventArgs e)
        {
            N = 30;
            M = 16;
            NumBombs = 99;
            Miner.Height = 570;
            Miner.Width = 920;
            gridPanel.Width = Miner.Width;
            Window_Loaded(sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var Image = new BitmapImage();
            Image.BeginInit();
            Image.UriSource = new Uri(@"/Images/new_game.jpg", UriKind.RelativeOrAbsolute); //загружаем картинку
            Image.EndInit();
            Image game = new Image(); //картинка для кнопки
            game.Source = Image; //Загруженная картинка
            btnMiner.Content = game;

            timer.Stop();
            lblTimer.Content = "0";
            lblFlags.Content = NumBombs;

            try
            {
                var MinerClient = new Service1Client();
                MinerClient.NewGame(Name, N, M, NumBombs);


                btns = new Button[N, M];
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < M; j++)
                    {
                        btns[i, j] = new Button();
                        NewPoint p = new NewPoint(i, j);
                        btns[i, j].Tag = p;
                        btns[i, j].Content = "";
                        btns[i, j].Width = 30;
                        btns[i, j].Height = 30;

                        btns[i, j].Click += btnNew_Click;
                        btns[i, j].MouseRightButtonDown += btnRight_Button_Click;
                        Canvas.SetLeft(btns[i, j], i*btns[i, j].Width);
                        Canvas.SetTop(btns[i, j], j*btns[i, j].Height);
                        cnvMain.Width = btns[i, j].Width*N;
                        cnvMain.Height = btns[i, j].Height*M;
                        cnvMain.Children.Add(btns[i, j]);
                    }
            }
            catch
            {
                nnn.Content += "Window_Loaded";
            }
        }


        public void ClickButton(Button button)
        {
            int fl;
            int.TryParse(lblFlags.Content.ToString(), out fl);
            if (button.Content != "" && button.IsEnabled)
            {
                fl++;
                lblFlags.Content = fl.ToString();
            }

            var x = (button.Tag as NewPoint).x;
            var y = (button.Tag as NewPoint).y;

            try
            {
                var MinerClient = new Service1Client();
                int Bombs = MinerClient.Click(Name, x, y);

                if (Bombs == 0 && button.IsEnabled)
                {
                    button.Content = "";
                    button.IsEnabled = false;
                    if (x > 0)
                        ClickButton(btns[x - 1, y]); //вызов соседней кнопки
                    if ((x > 0) && (y > 0))
                        ClickButton(btns[x - 1, y - 1]);
                    if (y > 0)
                        ClickButton(btns[x, y - 1]);
                    if ((x < N - 1) && (y > 0))
                        ClickButton(btns[x + 1, y - 1]);
                    if (x < N - 1)
                        ClickButton(btns[x + 1, y]);
                    if ((x < N - 1) && (y < M - 1))
                        ClickButton(btns[x + 1, y + 1]);
                    if (y < M - 1)
                        ClickButton(btns[x, y + 1]);
                    if ((x > 0) && (y < M - 1))
                        ClickButton(btns[x - 1, y + 1]);

                }

                else
                {
                    var loadImage = new BitmapImage();
                    loadImage.BeginInit();
                    loadImage.UriSource = new Uri(@"/Images/" + Bombs.ToString() + ".jpg", UriKind.RelativeOrAbsolute);
                        //загружаем картинку
                    loadImage.EndInit();
                    var test = new Image {Source = loadImage}; //картинка для кнопки
                    button.Content = test;
                    button.IsEnabled = false;
                }
            }
            catch
            {
                nnn.Content += "ClickButton";
            }
        }

        void btnNew_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();

            var x = ((sender as Button).Tag as NewPoint).x;
            var y = ((sender as Button).Tag as NewPoint).y;

            try
            {
                var MinerClient = new Service1Client();
                int Bombs = MinerClient.Click(Name, x, y);
                if (Bombs == -1)
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(@"/Images/lost.jpg", UriKind.RelativeOrAbsolute); //загружаем картинку
                    img.EndInit();
                    Image lost = new Image(); //картинка для кнопки
                    lost.Source = img; //Загруженная картинка
                    btnMiner.Content = lost;

                    timer.Stop();
                    for (int i = 0; i < N; i++)
                        for (int j = 0; j < M; j++)
                        {
                            Bombs = MinerClient.Click(Name, i, j);
                            if (Bombs == -1)
                            {
                                Image image = new Image();
                                BitmapImage bombImage = new BitmapImage();
                                bombImage.BeginInit();
                                bombImage.UriSource = new Uri(@"/Images/bomb.jpg", UriKind.RelativeOrAbsolute);
                                    //загружаем картинку
                                bombImage.EndInit();
                                Image bomb = new Image(); //картинка для кнопки
                                bomb.Source = bombImage; //Загруженная картинка
                                btns[i, j].Content = bomb;
                            }
                            btns[i, j].IsEnabled = false;
                        }

                    if (
                        MessageBox.Show("You lost!!! Play again?", "The game is over", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                        Window_Loaded(sender, e);
                    else
                    {
                        Close();
                        timer.Stop();
                    }

                }
                else
                {
                    ClickButton(sender as Button);
                    int n = 0;
                    for (int i = 0; i < N; i++)
                        for (int j = 0; j < M; j++)
                        {
                            if (btns[i, j].IsEnabled)
                                n++;
                        }
                    if (n == NumBombs)
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri(@"/Images/win.jpg", UriKind.RelativeOrAbsolute); //загружаем картинку
                        img.EndInit();
                        Image win = new Image(); //картинка для кнопки
                        win.Source = img; //Загруженная картинка
                        btnMiner.Content = win;
                        for (int i = 0; i < N; i++)
                            for (int j = 0; j < N; j++)
                            {
                                if (btns[i, j].IsEnabled && btns[i, j].Content == "")
                                {
                                    BitmapImage bombImage = new BitmapImage();
                                    bombImage.BeginInit();
                                    bombImage.UriSource = new Uri(@"/Images/bomb.jpg", UriKind.RelativeOrAbsolute);
                                        //загружаем картинку
                                    bombImage.EndInit();
                                    Image bomb = new Image(); //картинка для кнопки
                                    bomb.Source = bombImage; //Загруженная картинка
                                    btns[i, j].Content = bomb;
                                }
                                btns[i, j].IsEnabled = false;
                            }
                        timer.Stop();
                        if (
                            MessageBox.Show("You won!!! Your time is " + lblTimer.Content + " seconds. Play again?",
                                "Congratulations!", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                            MessageBoxResult.Yes)
                            Window_Loaded(sender, e);
                        else
                        {
                            Close();
                            timer.Stop();
                        }
                    }
                }
            }
            catch
            {
                nnn.Content += "btnNew_Click";
            }
        }

        void btnRight_Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage flagImage = new BitmapImage();
            flagImage.BeginInit();
            flagImage.UriSource = new Uri(@"/Images/flag.jpg", UriKind.RelativeOrAbsolute); //загружаем картинку флаг
            flagImage.EndInit();
            var flag = new Image(); //картинка для кнопки
            flag.Source = flagImage; //Загруженная картинка

            int fl;
            int.TryParse(lblFlags.Content.ToString(), out fl);
            if ((sender as Button).Content == "")
            {
                (sender as Button).Content = flag;
                fl--;
            }
            else
            {
                (sender as Button).Content = "";
                fl++;
            }
            lblFlags.Content = fl.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
            if (
                MessageBox.Show("Do you really want to quit?", "Confirm Closing", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                e.Cancel = true;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                    {
                        if (btns[i, j].IsEnabled)
                        {
                            timer.Start();
                            return;
                        }
                    }
                    return;
                }
            }
            else
            {
                try
                {
                    
                    var MinerClient = new Service1Client();
                    MinerClient.Exit(Name);
                }
                catch
                {
                    nnn.Content += "asdasd";
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          var minerClient = new Service1Client();
          if (minerClient.EnterName(NamePlayer.Text))
          {
              Name = NamePlayer.Text;
              MI.IsEnabled = true;
              ButExi.Visibility = gridPanel.Visibility = Visibility.Visible;
              NamePlayer.IsEnabled = false;
              Window_Loaded_easy(sender, e);
          }
          else
          {
              MessageBox.Show( "Take another name!","Is already taken!");
          }
        }

        private void ButExi_Click(object sender, RoutedEventArgs e)
        {
          var minerClient = new Service1Client();
          minerClient.Exit(NamePlayer.Text);
          MI.IsEnabled = false;
            NamePlayer.Text = "";
          ButExi.Visibility = gridPanel.Visibility = Visibility.Hidden;
          NamePlayer.IsEnabled = true;
        }

    }
}
