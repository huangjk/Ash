using System;
using System.Collections.Generic;
using System.IO;
using Ash;

namespace Ash
{
    public interface IConfigAdapter<T> where T : ConfigDataBase
    {
        T SearchByID(int id);

        T SearchByNickName(string nickname);

        void InitConfigsFromChache();

        void UpdateConfig(T item);

        void Delete(int id);

        void DeleteFromDisk(string configPath );

        void SaveToDisk(string configPath, object obj);
    }
}

