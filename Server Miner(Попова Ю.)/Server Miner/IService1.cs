using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server_Miner
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool EnterName(string name);
        [OperationContract]
        void NewGame(string name,int N, int M, int numBombs);
        [OperationContract]
        int Click(string name, int N, int M);
        [OperationContract]
        void Exit(string name);
        [OperationContract]
        void Record(string name, int time, int level);
        [OperationContract]
        void BinaryAdd(string name, int time, ref string [] mas, int l, int r, int N);
        [OperationContract]
        string[] PrintRec(int level);
    }
}
