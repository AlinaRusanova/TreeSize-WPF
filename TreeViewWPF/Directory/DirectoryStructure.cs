using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using TreeViewWPF.Directory.Data;
using TreeViewWPF.Directory.ViewModels;
using TreeViewWPF.Helpers;

namespace TreeViewWPF.Directory
{
    public class DirectoryStructure
    {
        public static  List<DirectoryItem> GetLogicalDrives()
        {
            // Get every logical drive on the machine

            var drivesInfo = DriveInfo.GetDrives();
            var drives = new List<DirectoryItem>();

            foreach (var driveInfo in drivesInfo)
            {
                if (driveInfo.IsReady)
                {
                    var data = new DirectoryItem
                    {
                        FullPath = driveInfo.Name,
                        Type = DirectoryItemType.Drive,
                        Size = Converter.ConvertBytes(driveInfo.TotalSize - driveInfo.TotalFreeSpace)
                    };
                    drives.Add(data);
                }                                            
            }              
            return drives;

        }

        public static List<DirectoryItem> GetDirectoryContents(string fullPath)
        {
            
            var items = new List<DirectoryItem>();

            var directoryInfo = new DirectoryInfo(fullPath);

            foreach (var folderInfo in directoryInfo.EnumerateDirectories())
            {
                try
                {
                    if (!folderInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        items.Add(new DirectoryItem { FullPath = folderInfo.FullName, Type = DirectoryItemType.Folder, Size = "0" });
                    }
                }
                catch (UnauthorizedAccessException) { }
            }


            // Get Files

            foreach (var fileInfo in directoryInfo.EnumerateFiles())
                {
                    try 
                    {
                            items.Add(new DirectoryItem { FullPath = fileInfo.Name, Type = DirectoryItemType.File, Size = Converter.ConvertBytes(fileInfo.Length) });                  
                    }
                   
                    catch (UnauthorizedAccessException) { }
                }

            return items;
        }


        private static Task GetSize(string fullPath, DirectoryItemViewModel folder)
        {
            if (fullPath == null)
            {
                throw new ArgumentNullException("Can't be null", nameof(fullPath));
            }

            var directoryInfo = new DirectoryInfo(fullPath);
            var enumerationOptions = new EnumerationOptions();

            enumerationOptions.IgnoreInaccessible = true;
            enumerationOptions.RecurseSubdirectories = true;


            try
            {
                return Task.Run(() =>
                {
                    var size = 0l;
                    foreach (var fileInfo in directoryInfo.EnumerateFiles("*", enumerationOptions))
                    {
                        size += fileInfo.Length;
                        folder.Size = Converter.ConvertBytes(size);
                    }
                });
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

        }

        public static Task UpdateSize(ObservableCollection<DirectoryItemViewModel> folders)
        {
            if (folders == null)
            {
                throw new ArgumentNullException("Can't be null", nameof(folders));
            }

            return Task.Run(() =>
            {
                foreach (DirectoryItemViewModel folder in folders)
                {
                    if (folder.Type == DirectoryItemType.Folder)
                    {
                        GetSize(folder.FullPath, folder);
                    }
                }
            });
        }

    }
}