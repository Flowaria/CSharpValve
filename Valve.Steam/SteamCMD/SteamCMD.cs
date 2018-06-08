using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Valve.Steam
{
    public class SteamCMD : IDisposable
    {
        public const string SteamCMDFileWebURL = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
        public string SteamCMDPath
        {
            get
            {
                return SteamCMDPath;
            }
            private set
            {
                Info.FileName = value;
            }
        }
        private ProcessStartInfo Info { get; set; }
        private Process Process { get; set; }

        private SteamCMD()
        {
            Info = new ProcessStartInfo
            {
                FileName = "steamcmd.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true
            };

            Process = new Process
            {
                StartInfo = Info
            };
        }

        public static SteamCMD InitSteamCMD(string folder)
        {
            if (Directory.Exists(folder))
            {
                var cmdpath = Path.Combine(folder, "steamcmd.exe");
                if (!File.Exists(cmdpath))
                {
                    string tempfile = Path.Combine(Path.GetTempPath(), "steamcmd.tempzip");
                    using (var wc = new WebClient())
                    {
                        try
                        {
                            wc.DownloadFile(SteamCMDFileWebURL, tempfile);
                        }
                        catch
                        {
                            throw new WebException("Unable to Download File");
                        }
                    }
                    if (File.Exists(tempfile))
                    {
                        try
                        {
                            ZipFile.ExtractToDirectory(tempfile, folder);
                            File.Delete(tempfile);
                        }
                        catch
                        {
                            if(File.Exists(tempfile)) File.Delete(tempfile);
                            throw new IOException("Unable to Upzip File");
                        }
                    }
                }

                if (File.Exists(cmdpath))
                {
                    var steamcmd = new SteamCMD();
                    steamcmd.SteamCMDPath = cmdpath;
                    steamcmd.Init();
                    return steamcmd;
                }
                else
                {
                    throw new FileNotFoundException("Unable to Find Steamcmd.exe File");
                }
            }
            else
            {
                throw new DirectoryNotFoundException("Unable to Find Directory");
            }
        }

        private void Init()
        {
            Process.Start();
        }

        public void Login(string id="anonymous", string password="")
        {
            ExecuteCmd(String.Format("login {0}", id));
            if(!id.Equals("anonymous"))
            {
                ExecuteCmd(password);
            }
        }

        public void ExecuteCmd(string cmd)
        {
            Process.StandardInput.WriteLine(cmd);
            Process.WaitForInputIdle();
        }

        public async Task ExecuteCmdAsync(string cmd)
        {
            await Process.StandardInput.WriteLineAsync(cmd);
        }

        public async Task<bool> UpdateApp(int appid, string path, string param=null)
        {
            ExecuteCmd(String.Format("force_install_dir {0}", path));
            ExecuteCmd(String.Format("app_update {0} {1}", appid, param));
            await Task.Run(() => Process.WaitForInputIdle());
            return true;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
