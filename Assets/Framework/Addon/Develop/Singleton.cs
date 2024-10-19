﻿namespace Framework
{
    public class Singleton<T> where T : new()
    {
        public static T Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new T();
                return _instance;
            }
        }

        private static T _instance;
    }
}