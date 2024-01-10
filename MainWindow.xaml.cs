using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pure3D;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using Pure3D.Chunks;
using RadcoreCementFile;
using CircusSetup.Script;

namespace CircusSetup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Pure3D.File P3D;
        RCF CementFile;
        Pure3D.RSD RSD;
        CircusSetup.Script.Script ScriptFile;
        string fileName;
        bool ModeRCF = false;
        bool ModeRSD = false;
        bool ModeScript = false;

        public static List<Animation> AnimCache = new List<Animation>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "P3D/RCF/RSD files|*.p3d;*.rcf;*.rsd";
            if (ofd.ShowDialog() == true)
            {
                fileName = ofd.FileName;
                if (fileName.ToUpper().Contains(".RCF"))
                {
                    ModeRCF = true;
                }
                else
                {
                    ModeRCF = false;
                }
                if (fileName.ToUpper().Contains(".RSD"))
                {
                    ModeRSD = true;
                }
                else
                {
                    ModeRSD = false;
                }
                if (fileName.ToUpper().Contains(".P3D"))
                {
                    ModeScript = false;
                }
                else
                {
                    ModeScript = true;
                }
                LoadFile();
            }
        }

        void LoadFile()
        {
            //LoadFileAction();
            try
            {
                LoadFileAction();
            }
            catch (Exception ex)
            {
                statusText.Text = $"Failed to load file: {ex.Message}";
            }
        }
        void LoadFileAction()
        {
            if (!ModeRCF)
            {
                if (!ModeRSD)
                {
                    if (!ModeScript)
                    {
                        P3D = new Pure3D.File();
                        P3D.Load(fileName);
                        LoadTree();
                    }
                    else
                    {
                        CircusSetup.Script.ScriptParser parser = new ScriptParser();
                        parser.Load(fileName);
                        ScriptFile = parser.script;
                        LoadTreeScript();
                    }
                }
                else
                {
                    RSD = new RSD();
                    RSD.Load(fileName);
                    LoadTreeRSD();
                }
            }
            else
            {
                CementFile = new RCF();
                CementFile.OpenRCF(fileName);
                LoadTreeRCF();
            }
            statusText.Text = $"P3D loaded.";
        }

        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                LoadFile();
            }
        }

        private void viewButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null) return;
            Chunk chunk = (Chunk)((TreeViewItem)treeView.SelectedItem).Tag;


        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (fileList.Length == 1)
            {
                fileName = fileList[0];
                if (fileName.ToUpper().Contains(".RCF"))
                {
                    ModeRCF = true;
                }
                else
                {
                    ModeRCF = false;
                }
                if (fileName.ToUpper().Contains(".RSD"))
                {
                    ModeRSD = true;
                }
                else
                {
                    ModeRSD = false;
                }
                if (fileName.ToUpper().Contains(".P3D"))
                {
                    ModeScript = false;
                }
                else
                {
                    ModeScript = true;
                }
                LoadFile();
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null) return;

            if (ModeRSD)
            {
                textBox.Text = ((TreeViewItem)e.NewValue).Tag.ToString();
                return;
            }
            if (ModeRCF)
            {

                return;
            }
            if (ModeScript)
            {
                StringBuilder slines = new StringBuilder();
                slines.Append(ScriptFile.ToDetails());
                textBox.Text = slines.ToString();
                return;
            }

            Chunk chunk = (Chunk)((TreeViewItem)e.NewValue).Tag;
            StringBuilder lines = new StringBuilder();
            lines.Append(chunk.ToDetails());
            textBox.Text = lines.ToString();

            try
            {
                byte[] imagedata = chunk.OnImagePreview();
                if (imagedata != null)
                {
                    using (System.IO.MemoryStream stream = new(imagedata)){
                        previewImage.Source = BitmapFrame.Create(stream,
                            BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                }
            }
            catch (Exception ex)
            {
                statusText.Text = $"Failed to load image: {ex.Message}";
            }
        }

        void LoadTree()
        {
            treeView.Items.Clear();
            TreeViewItem RootChunk = new TreeViewItem();
            RootChunk.Tag = P3D.RootChunk;
            RootChunk.Header = P3D.RootChunk.ToString();
            treeView.Items.Add(RootChunk);
            foreach (Chunk chunk in P3D.RootChunk.Children)
            {
                LoadTreeNode(RootChunk, chunk);
            }
            RootChunk.IsExpanded = true;
        }

        void LoadTreeNode(TreeViewItem Root, Chunk RootChunk)
        {
            TreeViewItem ChunkNode = new TreeViewItem();
            ChunkNode.Tag = RootChunk;
            ChunkNode.Header = RootChunk.ToString();
            if (RootChunk.FailedToLoad)
            {
                ChunkNode.Foreground = Brushes.Red;
            }
            Root.Items.Add(ChunkNode);
            foreach (Chunk chunk in RootChunk.Children)
            {
                LoadTreeNode(ChunkNode, chunk);
            }
        }

        private void textBox_Drop(object sender, DragEventArgs e)
        {
            Window_Drop(sender, e);
        }

        private void ScrollViewer_Drop(object sender, DragEventArgs e)
        {
            Window_Drop(sender, e);
        }

        private void textBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            //Window_Drop(sender, e);
            e.Handled = true;
        }

        private void textBox_PreviewDrop(object sender, DragEventArgs e)
        {
            Window_Drop(sender, e);
            e.Handled = true;
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {

            Stopwatch Timer = new Stopwatch();
            Timer.Start();

            if (ModeRSD)
            {
                if (treeView.SelectedItem == null) return;
                if (((TreeViewItem)treeView.SelectedItem).Tag is RSD)
                {
                    SaveFileDialog sfd2 = new SaveFileDialog();
                    if (Util.ExportToGodot)
                    {
                        sfd2.FileName = "sound.res";
                        sfd2.Filter = "RES files|*.res";
                    }
                    else
                    {
                        sfd2.FileName = "sound.wav";
                        sfd2.Filter = "WAV files|*.wav";
                    }
                    if (sfd2.ShowDialog() == true)
                    {
                        if (!Util.ExportToGodot)
                        {
                            byte[] SoundData = new byte[0];
                            string name1 = sfd2.FileName;
                            short channels = 1;
                            if (RSD.Channels > 1) channels = 2;
                            uint tracks = (RSD.Channels / 2);
                            switch (RSD.CodecString)
                            {
                                case "XADP": // XBOX IMA ADPCM
                                    SoundData = IMA_ADPCM.IMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, 0);
                                    break;
                                case "XMA ": // XBOX 360 XMA 
                                    SoundData = XMA_Audio.XMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, 0);
                                    break;
                                case "VAG ": // PS2/PSP VAG ADPCM
                                    if (RSD.Channels >= 4)
                                    {
                                        SoundData = ADPCM.ToPCMQuad(RSD.Data, RSD.Data.Length, (int)RSD.Interleave, 0, RSD.Channels);
                                    }
                                    else if (RSD.Channels == 2)
                                        SoundData = ADPCM.ToPCMStereo(RSD.Data, RSD.Data.Length, (int)RSD.Interleave);
                                    else if (RSD.Channels == 1)
                                        SoundData = ADPCM.ToPCMMono(RSD.Data, RSD.Data.Length);
                                    break;
                                case "AT3+": // PSP ATRAC3+
                                    SoundData = AT3Plus.AT3P_Decoder.Decode(RSD.Data, (int)RSD.Channels, 0);
                                    break;
                                case "RADP": // GCN/WII IMA ADPCM
                                    break;
                                case "WADP": // WII NGC DSP
                                    break;
                                default:
                                    break;
                            }
                            SoundData = RIFF.SaveRiff(SoundData, channels, (int)RSD.SampleRate);
                            FileStream file = new FileStream(name1, FileMode.Create, FileAccess.Write);
                            BinaryWriter writer = new BinaryWriter(file);
                            writer.Write(SoundData);
                            writer.Close();
                            
                            if (tracks > 1 && tracks < 32)
                            {
                                for (int t = 1; t < tracks; t++)
                                {
                                    string name2 = sfd2.FileName.Replace(".wav", $"_{t}.wav");
                                    switch (RSD.CodecString)
                                    {
                                        case "XADP": // XBOX IMA ADPCM
                                            SoundData = IMA_ADPCM.IMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, t);
                                            break;
                                        case "XMA ": // XBOX 360 XMA 
                                            SoundData = XMA_Audio.XMA_Decoder.Decode(RSD.Data, (int)RSD.Channels, t);
                                            break;
                                        case "VAG ": // PS2/PSP VAG ADPCM
                                            SoundData = ADPCM.ToPCMQuad(RSD.Data, RSD.Data.Length, (int)RSD.Interleave, t, RSD.Channels);
                                            break;
                                        case "AT3+": // PSP ATRAC3+
                                            SoundData = AT3Plus.AT3P_Decoder.Decode(RSD.Data, (int)RSD.Channels, t);
                                            break;
                                        case "RADP": // GCN/WII IMA ADPCM
                                            break;
                                        case "WADP": // WII NGC DSP
                                            break;
                                        default:
                                            break;
                                    }
                                    SoundData = RIFF.SaveRiff(SoundData, channels, (int)RSD.SampleRate);
                                    FileStream file2 = new FileStream(name2, FileMode.Create, FileAccess.Write);
                                    BinaryWriter writer2 = new BinaryWriter(file2);
                                    writer2.Write(SoundData);
                                    writer2.Close();
                                }
                            }
                        }
                        else
                        {
                            string outPath = System.IO.Path.GetDirectoryName(sfd2.FileName) + "\\Sounds\\";
                            outPath += RSD.ShortName + ".res";
                            string dirPath = System.IO.Path.GetDirectoryName(outPath);
                            Directory.CreateDirectory(dirPath);

                            GodotBinaryAudioStreamWAV wav1 = new GodotBinaryAudioStreamWAV(RSD, false, 0);
                            wav1.WriteToFile(outPath);

                            uint tracks = (RSD.Channels / 2);
                            if (tracks > 1 && tracks < 32)
                            {
                                for (int t = 1; t < tracks; t++)
                                {
                                    string name2 = outPath.Replace(".res", $"_{t}.res");
                                    GodotBinaryAudioStreamWAV wav2 = new GodotBinaryAudioStreamWAV(RSD, false, t);
                                    wav2.WriteToFile(name2);
                                }
                            }
                        }
                    }
                }
                return;
            }
            if (ModeRCF)
            {
                if (treeView.SelectedItem == null) return;
                if (((TreeViewItem)treeView.SelectedItem).Tag is RCF.RCF_HEADER)
                {
                    SaveFileDialog sfd2 = new SaveFileDialog();
                    sfd2.Filter = "All files|*.*";
                    if (sfd2.ShowDialog() == true)
                    {
                        //CementFile.ExtractRCF(sfd2.FileName);
                    }
                }
                else if (((TreeViewItem)treeView.SelectedItem).Tag is RCF.RCF_TABLE2 item)
                {
                    SaveFileDialog sfd2 = new SaveFileDialog();
                    sfd2.Filter = "All files|*.*";
                    sfd2.FileName = item.Name;
                    if (sfd2.ShowDialog() == true)
                    {
                        //CementFile.ExtractItem(0, sfd2.FileName);
                    }
                }
                return;
            }
            if (ModeScript)
            {
                SaveFileDialog sfd2 = new SaveFileDialog();
                sfd2.FileName = ScriptFile.FullName.Split('\\').Last().Replace(".lua",".lub");//"script.lub";
                sfd2.Filter = "LUB files|*.lub";
                if (sfd2.ShowDialog() == true)
                {
                    ScriptFile.Save(sfd2.FileName);
                }
                return;
            }
            if (treeView.SelectedItem == null) return;
            Chunk chunk = (Chunk)((TreeViewItem)treeView.SelectedItem).Tag;

            if (((TreeViewItem)treeView.SelectedItem).Tag is Pure3D.Chunks.Root)
            {
                SaveFileDialog sfd2 = new SaveFileDialog();
                sfd2.FileName = "GodotFiles.tscn";
                sfd2.Filter = "All files|*.*";
                if (sfd2.ShowDialog() == true)
                {
                    CircusSetup.ExportGodot.ExportP3D(P3D.RootChunk, sfd2.FileName);
                }
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "GodotFiles.tscn";
                sfd.Filter = "All files|*.*";
                if (sfd.ShowDialog() == true)
                {
                    if (Util.ExportToGodot)
                    {
                        chunk.OnGodotExport(sfd.FileName);
                    }
                    else
                    {
                        chunk.OnExport(sfd.FileName);
                    }
                }
            }

            Timer.Stop();
            statusText.Text = $"Export OK. Time: {Timer.Elapsed}";

            GC.Collect();
            GC.WaitForPendingFinalizers();
            
        }

        private void toggleWrapButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.TextWrapping == TextWrapping.NoWrap)
            {
                textBox.TextWrapping = TextWrapping.Wrap;
            }
            else
            {
                textBox.TextWrapping = TextWrapping.NoWrap;
            }
        }

        private void batchTestButton_Click(object sender, RoutedEventArgs e)
        {
            return;
            string path = "";

            List<string> paths = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (DirectoryInfo di in dir.EnumerateDirectories())
            {
                Recursive_Batch(di, paths);
            }
            List<string> errors = new List<string>();

            BatchTest(paths, errors);

            Console.WriteLine($"Batch testing done. Checked {paths.Count} files. Errors: {errors.Count}");
            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine(errors[i]);
            }
        }

        void Recursive_Batch(DirectoryInfo dir, List<string> paths)
        {
            foreach (DirectoryInfo di in dir.EnumerateDirectories())
            {
                Recursive_Batch(di, paths);
            }
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                if (file.Extension.ToLower().Contains("p3d"))
                {
                    paths.Add(file.FullName);
                }
            }
        }

        void BatchTest(List<string> paths, List<string> errors)
        {
            List<uint> UnkTypes = new List<uint>();
            List<string> UnkTypesFiles = new List<string>();
            for (int p = 0; p < paths.Count; p++)
            {
                //P3D = new Pure3D.File();
                //P3D.Load(paths[p]);
                
                try
                {
                    P3D = new Pure3D.File();
                    P3D.Load(paths[p]);
                }
                catch
                {
                    errors.Add(paths[p]);
                }
                
                Recursive_CheckUnk(P3D.RootChunk, ref UnkTypes, ref UnkTypesFiles, paths[p]);
            }
            for (int i = 0; i < UnkTypes.Count; i++)
            {
                Debug.WriteLine($"0x{UnkTypes[i]:X8} - {UnkTypesFiles[i]}");
            }
            P3D = null;
        }

        void Recursive_CheckUnk(Chunk root, ref List<uint> UnkTypes, ref List<string> UnkTypesFiles, string file)
        {
            foreach (Chunk item in root.Children)
            {
                /*
                if (item is Unknown && item.ToString().StartsWith("Unknown"))
                {
                    if (!UnkTypes.Contains(item.Type))
                    {
                        UnkTypes.Add(item.Type);
                        UnkTypesFiles.Add(file);
                    }
                }
                */
                if (item is Mesh || item is Skin)
                {
                    
                    foreach (var pitem in item.Children)
                    {
                        if (pitem is PrimitiveGroupCTTR prim)
                        {
                            var nat = prim.GetChild<NativeVertexList>();
                            if (nat != null && !UnkTypes.Contains(nat.PSP_MeshType))
                            {
                                UnkTypes.Add(nat.PSP_MeshType);
                                UnkTypesFiles.Add(file);
                            }
                        }
                    }
                    
                }
                Recursive_CheckUnk(item, ref UnkTypes, ref UnkTypesFiles, file);
            }
        }

        void LoadTreeRCF()
        {
            treeView.Items.Clear();
            TreeViewItem RootChunk = new TreeViewItem();
            RootChunk.Tag = CementFile.Header;
            RootChunk.Header = System.IO.Path.GetFileName(fileName);
            treeView.Items.Add(RootChunk);
            for (int i = 0; i < CementFile.Header.Files; i++)
            {
                LoadTreeNodeRCF(RootChunk, i);
            }
            RootChunk.IsExpanded = true;
        }

        void LoadTreeNodeRCF(TreeViewItem Root, int item)
        {
            TreeViewItem ChunkNode = new TreeViewItem();
            ChunkNode.Tag = CementFile.Header.T2File[item];
            ChunkNode.Header = CementFile.Header.T2File[item].Name;
            Root.Items.Add(ChunkNode);
        }

        private void addAnimButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem == null) return;
            if (((TreeViewItem)treeView.SelectedItem).Tag is Pure3D.Chunks.Animation anim)
            {
                AnimCache.Add(anim);
            }
            //addAnimButton.Header = $"Add To Anim Cache ({AnimCache.Count})";
        }

        private void clearAnimButton_Click(object sender, RoutedEventArgs e)
        {
            AnimCache.Clear();
            //addAnimButton.Header = $"Add To Anim Cache ({AnimCache.Count})";
        }

        void LoadTreeRSD()
        {
            treeView.Items.Clear();
            TreeViewItem RootChunk = new TreeViewItem();
            RootChunk.Tag = RSD;
            RootChunk.Header = System.IO.Path.GetFileName(fileName);
            treeView.Items.Add(RootChunk);
            RootChunk.IsExpanded = true;
        }

        private void exportToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Util.ExportToGodot = !Util.ExportToGodot;
            string text = Util.ExportToGodot ? "ON" : "OFF";
            exportToggleButton.Header = $"Export In Godot Format {text}";
        }

        void LoadTreeScript()
        {
            treeView.Items.Clear();
            TreeViewItem RootChunk = new TreeViewItem();
            RootChunk.Tag = ScriptFile;
            RootChunk.Header = System.IO.Path.GetFileName(fileName);
            treeView.Items.Add(RootChunk);
            RootChunk.IsExpanded = true;
        }

        private void demoModeToggleButton_Click(object sender, RoutedEventArgs e)
        {
            Util.IsDemo = !Util.IsDemo;
            string text = Util.IsDemo ? "ON" : "OFF";
            demoModeToggleButton.Header = $"Is Demo {text}";
        }

    }
}
