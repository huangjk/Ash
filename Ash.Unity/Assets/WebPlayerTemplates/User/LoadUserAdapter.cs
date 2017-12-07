using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Venipuncture.ManagementPlatform
{
    public class LoadUserAdapter : ILoadAdapter
    {
        public T Load<T>(string path)
        {
            return Ash.Utility.Json.ToObject<T>(LoadB(path));
        }

        public void Save<T>(T t, string path)
        {
            string josnData = Ash.Utility.Json.ToJson(t);

            Ash.Log.Info(josnData);
            //SaveB(josnData, path);
        }

        private void SaveB(string contents, string path)
        {
            byte[] bytes = Ash.Utility.Converter.GetBytes(contents);

            System.IO.File.WriteAllBytes(path, bytes);
        }
        private string LoadB(string path)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return Ash.Utility.Converter.GetString(bytes);
        }
    }
}
