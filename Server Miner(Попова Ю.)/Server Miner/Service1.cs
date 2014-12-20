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
