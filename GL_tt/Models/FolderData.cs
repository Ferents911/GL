using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GL_tt.Models
{
    public class FolderData
    {
        public string Name; 
        public DateTime Date;
        public List<FileData> Files;
        public List<FolderData> Children;

        public FolderData(FileSystemInfo fsi)
        {
           
            Name = fsi.Name;
            Date = fsi.CreationTime;
            Files = new List<FileData>();
            Children = new List<FolderData>();
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            string user = wi.Name;

            foreach (FileSystemInfo f in (fsi as DirectoryInfo).GetFileSystemInfos())
            {
                if (f.Attributes == FileAttributes.Directory)
                {
                    
                    RemoveDirectorySecurity(f.FullName, @user, FileSystemRights.ReadData, AccessControlType.Allow);
                    Children.Add(new FolderData(f));    
                }
                else
                {
                    RemoveFileSecurity(f.FullName, @user, FileSystemRights.ReadData, AccessControlType.Allow);
                    var TempFileInfo = new FileData();
                    TempFileInfo.Name = new FileInfo(f.FullName).Name;
                    TempFileInfo.Size = (new FileInfo(f.FullName).Length + " B").ToString();
                    TempFileInfo.Path = f.FullName;
                    Files.Add(TempFileInfo);
                }
            }
            
        }

        
        public static void RemoveFileSecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            File.SetAccessControl(fileName, fSecurity);
        }

        public static void RemoveDirectorySecurity(string fileName, string account, FileSystemRights rights, AccessControlType controlType)
        {
            DirectorySecurity fSecurity = Directory.GetAccessControl(fileName);
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
            Directory.SetAccessControl(fileName, fSecurity);

        }



        public string TreeToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
  
}
