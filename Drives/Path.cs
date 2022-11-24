using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drives
{
    public static class Explorer
    {
        public const string ROOT = "\\";

        public static string CurDir = ROOT;
        internal static List<Fso> Up()
        {
            if (CurDir == ROOT)
            {
                return Dir();
            }
            var parent = Directory.GetParent(CurDir);
            if (parent == null)
            {
                CurDir = ROOT;
            }
            else
            {
                CurDir = parent.FullName;
            }
            return Dir();
        }

        public static List<Fso> Dir()
        {
            if (CurDir == ROOT)
            {
                return DriveInfo.GetDrives().Select(d => new Fso(d.Name, FsoType.Drive, d.TotalSize, d.Name)).ToList();
            }

            var d = new DirectoryInfo(CurDir);
            var folders = d.GetDirectories().Select(d => new Fso(d.Name, FsoType.Folder, -1, d.FullName));
            var files = d.GetFiles().Select(d => new Fso(d.Name, FsoType.File, d.Length, d.FullName));
            return folders.Concat(files).ToList();

        }

        internal static List<Fso> Cd(string folder)
        {
            CurDir = Path.Combine(CurDir, folder);
            return Dir();
        }
        public static void OpenWithDefaultProgram(string path)
        {
            using Process fileopener = new Process();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }
    }
}
