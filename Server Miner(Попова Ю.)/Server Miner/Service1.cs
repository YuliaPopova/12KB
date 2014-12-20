using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server_Miner
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1
    {
        public bool EnterName(string name)
        {
            Console.WriteLine("EnterName");
          if (!File.Exists(@"UserGame\" + name))
          {
              StreamWriter sw = new StreamWriter(@"UserGame\" + name,false);
              sw.Write("q");
              sw.Close();
            return true;
          }
          return false;
        }
        public void NewGame(string name, int N, int M, int numBombs)
        {
            Console.WriteLine("NewGame");
            var Mas = new int[N, M];
            SetBombs(Mas,N,M,numBombs);
            CountBombs(Mas,N,M);
            var a = new StreamWriter(@"UserGame\" + name,false);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    a.Write(Mas[i,j]+" ");
                }
                a.WriteLine();
            }
            a.Close();
        }
        public int Click(string name, int N, int M)
        {
            Console.WriteLine("Click");
            var a = File.ReadAllLines(@"UserGame\" + name).Select(i => i.Split(' ')).ToArray();
            return int.Parse(a[N][M]);
        }
        public void Exit(string name)
        {
            Console.WriteLine("Exit");
            File.Delete(@"UserGame\" + name);
        }

        public void Record(string name, int time, int level)
        {
            string[] str = File.ReadAllLines(@"Records\" + level + ".txt").ToArray();
            int N = str.Count(i => i.Split(' ').ToArray().Length > 1);
            if (N < 10 || time < int.Parse(str[9].Split(' ').ToArray()[2])) //надо дописать наш рекорд
            {
                BinaryAdd(name, time, ref str, 0, 9, N);
            }
            StreamWriter sw = new StreamWriter(@"Records\" + level + ".txt");
            foreach (var s in str)
            {
                sw.WriteLine(s);
            }
            sw.Close();
        }

        public void BinaryAdd(string name, int time, ref string [] mas, int l, int r, int N)
        {
            if (r - l <= 1) //пишем в найденное место
            {
                if (N == 0 || int.Parse(mas[0].Split(' ').ToArray()[2]) > time)
                    r = l;
                N++;
                for (int i = N - 1; i > r; i--)
                {
                    mas[i] = i + 1 + ". " + mas[i - 1].Split(' ').ToArray()[1] + ' ' +
                        mas[i - 1].Split(' ').ToArray()[2] + ' ' + mas[i - 1].Split(' ').ToArray()[3];
                }
                mas[r] = r + 1 + ". " + name + " " + time + " c.";
                return;
            }
            int m = (l + r) / 2;
            if (N < m + 1 || time < int.Parse(mas[m].Split(' ').ToArray()[2]))
                BinaryAdd(name, time, ref mas, l, m, N);
            else
            {
                BinaryAdd(name, time, ref mas, m, r, N);
            }

        }

        public string[] PrintRec(int level)
        {
            return File.ReadAllLines(@"Records\" + level + ".txt").ToArray();
            
        }

        public void SetBombs(int[,] Mas, int N, int M, int NumBombs)
        {
            for (int i = 0; i < NumBombs; i++)
            {
                Random rand = new Random();
                int x = rand.Next(0, N);
                int y = rand.Next(0, M);
                while (Mas[x, y] == -1)
                {
                    x = rand.Next(0, N);
                    y = rand.Next(0, M);
                }
                Mas[x, y] = -1;

            }
        }
        public void CountBombs(int[,] Mas, int N, int M)
        {

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if (Mas[i, j] != -1)
                    {
                        if (i - 1 >= 0)
                        {
                            if (Mas[i - 1, j] == -1)
                                Mas[i, j]++;
                        }
                        if (i + 1 < N)
                        {
                            if (Mas[i + 1, j] == -1)
                                Mas[i, j]++;
                        }
                        if (j - 1 >= 0)
                        {
                            if (Mas[i, j - 1] == -1)
                                Mas[i, j]++;
                        }
                        if (j + 1 < M)
                        {
                            if (Mas[i, j + 1] == -1)
                                Mas[i, j]++;
                        }
                        if (j + 1 < M && i + 1 < N)
                        {
                            if (Mas[i + 1, j + 1] == -1)
                                Mas[i, j]++;
                        }
                        if (j - 1 >= 0 && i + 1 < N)
                        {
                            if (Mas[i + 1, j - 1] == -1)
                                Mas[i, j]++;
                        }
                        if (j + 1 < M && i - 1 >= 0)
                        {
                            if (Mas[i - 1, j + 1] == -1)
                                Mas[i, j]++;
                        }
                        if (j - 1 >= 0 && i - 1 >= 0)
                        {
                            if (Mas[i - 1, j - 1] == -1)
                                Mas[i, j]++;
                        }

                    }

                }
            }
        }
    }
}
