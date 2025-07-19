using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Pure3D;

namespace CircusSetup
{
    public class AssetExporter
    {
        public int FilesLeft = 0;
        public int TotalFiles = 0;
        public bool Exporting = true;
        public bool isISO = false;
        public bool isPS2 = false;
        public string InputPath = string.Empty; // folder path to game files
        public string OutputPath = string.Empty; // should end in a file name
        BackgroundWorker Worker;
        Dictionary<string, List<string>> FilePaths;
        List<string> XMVPaths;
        public XISO ISO;
        public ISO9660 ISO_PS2;
        public string ISOpath = string.Empty;
        public string GodotPath = string.Empty;
        public string ZipPath = AppDomain.CurrentDomain.BaseDirectory + "\\Packs\\CircusData.pcz";
        public bool PackingAssets = true;
        public string ISO_Extract_Path = AppDomain.CurrentDomain.BaseDirectory + "Packs\\ISO\\";
        string AudioBasePath;

        public int VideosLeft = 0;
        public int TotalVideos = 0;
        public int LevelsLeft = 0;
        public int TotalLevels = 0;
        public int PacksLeft = 0;
        public int TotalPacks = 0;
        public ProcessStages Stage = ProcessStages.Prepare;

        public event EventHandler<int> WorkerProgressChanged;
        public event EventHandler WorkerFinished;

        public enum ProcessStages
        {
            Prepare = 0,
            ExtractISO = 1,
            ExtractAssets = 2,
            InstallMods = 3,
            PackAssets = 4,
            End = 5,
        }

        public AssetExporter()
        {
            FilePaths = new Dictionary<string, List<string>>();
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.DoWork += Worker_DoWork;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            Worker.ProgressChanged += Worker_ProgressChanged;
            XMVPaths = new List<string>();
            ISO = new XISO();
            ISO_PS2 = new ISO9660();
            OutputPath = AppDomain.CurrentDomain.BaseDirectory + "\\import\\out.tscn";
            AudioBasePath = $"{System.IO.Path.GetDirectoryName(OutputPath)}\\Sounds\\";
            Directory.CreateDirectory(OutputPath);
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Packs\\");
        }

        public void StartWorker(string inPath, string outPath)
        {
            InputPath = inPath;
            OutputPath = outPath;
            Worker.RunWorkerAsync();
        }
        public void StartWorker(string inPath)
        {
            InputPath = inPath;
            Worker.RunWorkerAsync();
        }

        void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (WorkerProgressChanged != null)
            {
                WorkerProgressChanged.Invoke(this, 100);
            }
            if (WorkerFinished != null)
            {
                WorkerFinished.Invoke(this, null);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (WorkerProgressChanged != null)
            {
                WorkerProgressChanged.Invoke(this, e.ProgressPercentage);
            }
        }

        void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            Exporting = true;
            StartExport();

            while (Exporting)
            {
                Thread.Sleep(100);
                if (TotalFiles != 0)
                {
                    Worker.ReportProgress((int)((1f - (FilesLeft / (float)TotalFiles)) * 100));
                }
            }
            
        }

        async void StartExport()
        {
            string DirPath = InputPath;
            Stage = ProcessStages.Prepare;
            Console.WriteLine("Preparing...");

            Stage = ProcessStages.ExtractISO;

            if (isISO)
            {
                Console.WriteLine("Extracting ISO...");
                DirPath = ISO_Extract_Path;
                if (isPS2)
                {
                    await ISO_PS2.ExportISO(ISOpath, ISO_Extract_Path);
                }
                else
                {
                    await ISO.ExportISO(ISOpath, ISO_Extract_Path);
                }
            }

            Stage = ProcessStages.ExtractAssets;
            Console.WriteLine("Extracting assets...");

            #region Extract Assets
            DirectoryInfo Dir = new DirectoryInfo(DirPath);
            FilePaths = new Dictionary<string, List<string>>();
            Recursive_Batch(Dir, FilePaths);

            IList<Task> TaskList = new List<Task>();
            foreach (string Path in FilePaths[".rcf"])
            {
                if (Path.Contains("movies"))
                {
                    TaskList.Add(ExportRCF(Path));
                    PacksLeft++;
                    TotalPacks++;
                }
            }
            await Task.WhenAll(TaskList);
            TaskList.Clear();
            
            Dir = new DirectoryInfo(DirPath);
            FilePaths = new Dictionary<string, List<string>>();
            Recursive_Batch(Dir, FilePaths);
            foreach (string Path in FilePaths[".p3d"])
            {
                TaskList.Add(ExportP3D(Path));
                LevelsLeft++;
                TotalLevels++;
            }
            foreach (string Path in FilePaths[".rsd"])
            {
                TaskList.Add(ExportRSD(Path));
                LevelsLeft++;
                TotalLevels++;
            }
            TotalFiles += TaskList.Count;
            FilesLeft += TaskList.Count;

            await Task.WhenAll(TaskList);
            TaskList.Clear();
            #endregion

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (PackingAssets)
            {
                Stage = ProcessStages.PackAssets;
                Console.WriteLine("Packing assets...");

                await PackAssets(DirPath);
            }

            Stage = ProcessStages.End;
            Console.WriteLine("Finishing up...");
            
            await Cleanup(DirPath);

            Console.WriteLine("Complete!");
            Exporting = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        void Recursive_Batch(DirectoryInfo dir, Dictionary<string, List<string>> paths)
        {
            foreach (DirectoryInfo di in dir.EnumerateDirectories())
            {
                Recursive_Batch(di, paths);
            }
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                string ext = file.Extension.ToLower().Replace(";1", "");
                if (!paths.ContainsKey(ext))
                {
                    paths.Add(ext, new List<string>() { file.FullName });
                }
                else
                {
                    paths[ext].Add(file.FullName);
                }
            }
        }

        async Task ExportRCF(string inName)
        {
            /*
            RCF_Manager rcf = null;
            await Task.Run(
                () =>
                {
                    rcf = new RCF_Manager(inName);
                }
                );
            await rcf.ExtractAsync(inName, OutputPath);
            */
            System.IO.File.Delete(inName);
            
            LevelsLeft--;
            FilesLeft--;
        }

        async Task ExportRSD(string inName)
        {
            await Task.Run(
                () =>
                {
                    RSD rsd = new RSD();
                    rsd.Load(inName);

                    string outPath = AudioBasePath + rsd.ShortName + ".res";
                    string dirPath = System.IO.Path.GetDirectoryName(outPath);
                    Directory.CreateDirectory(dirPath);

                    GodotBinaryAudioStreamWAV wav1 = new GodotBinaryAudioStreamWAV(rsd, false, 0);
                    wav1.WriteToFile(outPath);
                    uint tracks = (rsd.Channels / 2);
                    if (tracks > 1 && tracks < 32)
                    {
                        for (int t = 1; t < tracks; t++)
                        {
                            string name2 = outPath.Replace(".res", $"_{t}.res");
                            GodotBinaryAudioStreamWAV wav2 = new GodotBinaryAudioStreamWAV(rsd, false, t);
                            wav2.WriteToFile(name2);
                        }
                    }

                    LevelsLeft--;
                    FilesLeft--;
                }
                );
        }

        async Task ExportP3D(string inName)
        {
            await Task.Run(
                () =>
                {
                    Pure3D.File p3d = new Pure3D.File();
                    p3d.Load(inName);
                    ExportGodot.ExportP3D(p3d.RootChunk, OutputPath);

                    LevelsLeft--;
                    FilesLeft--;
                }
                );
        }

        async Task ExportLUA(string inName)
        {
            await Task.Run(
                () =>
                {
                    //System.IO.File.Copy(inName, OutName, true);
                    FilesLeft--;
                }
                );
        }
        async Task ExportGOD(string inName)
        {
            await Task.Run(
                () =>
                {
                    //System.IO.File.Copy(inName, OutName, true);
                    FilesLeft--;
                }
                );
        }

        async Task PackAssets(string IsoExtrPath)
        {
            await Task.Run(
                () =>
                {
                    if (isISO)
                    {
                        // Cleanup first
                        Directory.Delete(IsoExtrPath, true);
                    }
                    ZipFile.CreateFromDirectory(System.IO.Path.GetDirectoryName(OutputPath), ZipPath, CompressionLevel.Fastest, true);
                    //ZipFile.CreateFromDirectory(System.IO.Path.GetDirectoryName(OutputPath), ZipPath, CompressionLevel.NoCompression, true);
                }
                );
        }

        async Task Cleanup(string IsoExtrPath)
        {
            await Task.Run(
                () =>
                {
                    /*
                    if (isISO)
                    {
                        // Cleanup
                        Directory.Delete(IsoExtrPath, true);
                    }
                    */
                    if (PackingAssets)
                    {
                        Directory.Delete(System.IO.Path.GetDirectoryName(OutputPath), true);
                    }
                }
                );
        }

        void Recursive_ListFiles(DirectoryInfo di, string pathparent, ref Dictionary<string, string> Paths)
        {
            foreach (DirectoryInfo dir in di.EnumerateDirectories())
            {
                Directory.CreateDirectory(pathparent + dir.Name);
                string pathchild = pathparent + dir.Name + @"\";
                foreach (FileInfo file in dir.EnumerateFiles())
                {
                    Paths.Add(file.FullName, pathchild + file.Name);
                }
                Recursive_ListFiles(dir, pathchild, ref Paths);
            }
        }

        public bool DetectXBE(string inputPath)
        {
            bool Check = ISO.DetectXBE(inputPath);
            if (Check)
            {
                isPS2 = false;
                if (inputPath.ToLower().EndsWith(".iso"))
                {
                    isISO = true;
                    ISOpath = inputPath;
                }
            }
            else
            {
                isISO = false;
                isPS2 = false;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return Check;
        }

        public bool DetectPS2(string inputPath)
        {
            bool Check = ISO_PS2.DetectPS2(inputPath);
            if (Check)
            {
                isPS2 = true;
                if (inputPath.ToLower().EndsWith(".iso"))
                {
                    isISO = true;
                    ISOpath = inputPath;
                }
            }
            else
            {
                isISO = false;
                isPS2 = false;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return Check;
        }

        public void RunGame()
        {
            // todo     
        }

    }
}
