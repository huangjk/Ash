﻿






using Ash.Core;
using UnityEditor;
using UnityEngine;

namespace Ash.Editor.AssetBundleTools
{
    internal sealed class SourceAsset
    {
        private Texture m_CachedIcon;

        public SourceAsset(string guid, string path, string name, SourceFolder folder)
        {
            if (folder == null)
            {
                throw new AshException("Source asset folder is invalid.");
            }

            Guid = guid;
            Path = path;
            Name = name;
            Folder = folder;
            m_CachedIcon = null;
        }

        public string Guid
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public SourceFolder Folder
        {
            get;
            private set;
        }

        public string FromRootPath
        {
            get
            {
                return (Folder.Folder == null ? Name : string.Format("{0}/{1}", Folder.FromRootPath, Name));
            }
        }

        public int Depth
        {
            get
            {
                return Folder != null ? Folder.Depth + 1 : 0;
            }
        }

        public Texture Icon
        {
            get
            {
                if (m_CachedIcon == null)
                {
                    m_CachedIcon = AssetDatabase.GetCachedIcon(Path);
                }

                return m_CachedIcon;
            }
        }
    }
}
