using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DiscUtils.Iso9660;

namespace CircusSetup
{
    public class ISO9660
    {
        public bool CNF_Only = false;
        public bool ExtractFiles = false;
        public string ExtractPath;
        public string[] CNF_Buffer;
        public GameVersion Version = GameVersion.PS2_USA;

        public bool DetectPS2(string filePath)
        {
            bool isISO = false;
            string dirPath = string.Empty;
            FileInfo? xbe = new FileInfo(filePath);
            if (xbe == null) return false;

            CNF_Only = true;
            ExtractFiles = false;

            if (xbe.Extension.ToLower() == ".iso")
            {
                isISO = true;
                ExtractPath = AppDomain.CurrentDomain.BaseDirectory;
                using (FileStream file = new FileStream(filePath, FileMode.Open))
                {
                    CDReader cd;

                    if (!CDReader.Detect(file))
                    {
                        return false;
                    }
                    else
                    {
                        cd = new CDReader(file, true);
                    }

                    if (cd.FileExists(@"SYSTEM.CNF"))
                    {
                        using (StreamReader sr = new StreamReader(cd.OpenFile(@"SYSTEM.CNF", FileMode.Open)))
                        {
                            CNF_Buffer = new string[3];
                            CNF_Buffer[0] = sr.ReadLine();
                            CNF_Buffer[1] = sr.ReadLine();
                            CNF_Buffer[2] = sr.ReadLine();
                        }
                    }
                    if (cd.FileExists(@"UMD_DATA.BIN"))
                    {
                        using (StreamReader sr = new StreamReader(cd.OpenFile(@"UMD_DATA.BIN", FileMode.Open)))
                        {
                            CNF_Buffer = new string[1];
                            CNF_Buffer[0] = sr.ReadLine().Substring(0, 10);
                        }
                    }
                    cd.Dispose();
                    cd = null;
                }
            }
            else
            {
                CNF_Buffer = System.IO.File.ReadAllLines(filePath);
            }

            Version = GameVersion.Unknown;
            foreach (var pair in TitleIDs)
            {
                if (CNF_Buffer[0].Contains(pair.Key))
                {
                    Version = pair.Value;
                    break;
                }
            }
            if (Version == GameVersion.Unknown) return false;
            return true;
        }

        public async Task ExportISO(string inputPath, string outputPath)
        {
            CNF_Only = false;
            ExtractFiles = true;
            ExtractPath = outputPath;
            Directory.CreateDirectory(ExtractPath);

            IList<Task> extractTaskList = new List<Task>();
            Dictionary<string, string> Paths = new Dictionary<string, string>();
            
            using (FileStream extract_isoStream = System.IO.File.Open(inputPath, FileMode.Open))
            {
                using (CDReader extract_reader = new CDReader(extract_isoStream, true))
                {
                    Recursive_MakePaths(extract_reader, "", ref Paths);
                }
            }

            foreach (KeyValuePair<string, string> Path in Paths)
            {
                extractTaskList.Add(ISO_ExtractAsync(inputPath, Path.Key, Path.Value));
            }

            await Task.WhenAll(extractTaskList);

            extractTaskList.Clear();
        }


        private void Recursive_MakePaths(CDReader cd, string dir, ref Dictionary<string, string> Paths)
        {
            if (cd.GetDirectoryInfo(dir).GetFiles().Length > 0)
            {
                foreach (string file in cd.GetFiles(dir))
                {
                    string filename = ExtractPath.TrimEnd('\\') + file;
                    filename = filename.Replace(";1", string.Empty);
                    Paths.Add(file, filename);
                }
            }
            if (cd.GetDirectories(dir).Length > 0)
            {
                foreach (string directory in cd.GetDirectories(dir))
                {
                    Recursive_MakePaths(cd, directory, ref Paths);
                }
            }
        }

        private async Task ISO_ExtractAsync(string input, string file, string path)
        {
            // CDReader doesn't work in async, so this is the workaround
            using (FileStream iso = new FileStream(input, FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, System.IO.FileOptions.SequentialScan))
            {
                using (CDReader cd = new CDReader(iso, true))
                {
                    using (Stream fileStreamFrom = cd.OpenFile(file, FileMode.Open))
                    {
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                        using (Stream fileStreamTo = System.IO.File.Open(path, FileMode.OpenOrCreate))
                        {
                            await fileStreamFrom.CopyToAsync(fileStreamTo);
                            //fileStreamFrom.CopyTo(fileStreamTo);
                        }
                    }
                }
            }
        }

        public enum GameVersion
        {
            Unknown = -1,
            PS2_USA = 0,
            PS2_EUR = 2,
            PS2_JPN = 3,
            PSP_USA = 4,
            PSP_EUR = 5,
            PSP_JPN = 6,
        }

        Dictionary<string, GameVersion> TitleIDs = new Dictionary<string, GameVersion>(){
            ["SLUS_211.91"] = GameVersion.PS2_USA,
            ["SLES_534.39"] = GameVersion.PS2_EUR,
            ["SLPM_660.90"] = GameVersion.PS2_JPN,
            ["ULUS-10044"] = GameVersion.PSP_USA,
            ["ULES-00168"] = GameVersion.PSP_EUR,
            ["ULJM-05036"] = GameVersion.PSP_JPN,
        };
    }
}