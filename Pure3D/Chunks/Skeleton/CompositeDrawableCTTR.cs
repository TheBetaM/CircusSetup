using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x123000)]
    public class CompositeDrawableCTTR : CompositeDrawable
    {
        public uint UnkInt1;
        public uint UnkInt2;
        public CompositeDrawableCTTR(File file, uint type) : base(file, type)
        {

        }

        
        public override void ReadHeader(BinaryReader reader, long length)
        {
            UnkInt1 = reader.ReadUInt32();
            base.ReadHeader(reader, length);
            UnkInt2 = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            writer.Write(UnkInt1);
            base.WriteHeader(writer);
            writer.Write(UnkInt2);
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Composite Drawable CTTR: {Name}");
            Lines.AppendLine($"SkeletonName: {SkeletonName}");
            Lines.AppendLine($"Version: {UnkInt1}");
            Lines.AppendLine($"Primitive Count: {UnkInt2}");

            return Lines.ToString();
        }

        public override void OnExport(string path)
        {
            ExportGLTF.Export(this, path);
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"{Name}.tscn";
            if (System.IO.File.Exists(outName)) return;

            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name);

            var rig = File.RootChunk.GetChildByName<SkeletonCTTR>(SkeletonName);
            GodotSceneFile.InternalResource AnimLib = new ();
            GodotSceneFile.Node AnimNode = null;
            if (!string.IsNullOrEmpty(SkeletonName) && !string.IsNullOrEmpty(rig.Name))
            {
                rig.OnGodotExport(path);

                GodotFileBase.ExternalResource ModelFileReference = new($"Rig_{rig.Name}.tscn");
                ModelFileReference.SetAsPackedScene();
                scene.ExternalResourceList.Add(ModelFileReference);

                GodotSceneFile.Node ModelNode = new($"{rig.Name}");
                ModelNode.InstanceID = scene.ExternalResourceList.Count;
                ModelNode.KeyValues.Add("parent", ".");
                scene.Nodes.Add(ModelNode);

                GodotSceneFile.ExternalResource ResetAnimRef = new($"Rig_{rig.Name}_RESET.res");
                ResetAnimRef.SetAsAnimation();
                scene.ExternalResourceList.Add(ResetAnimRef);
                int ResetAnimRefID = scene.ExternalResourceList.Count;

                AnimLib.Type = "AnimationLibrary";
                AnimLib.Lines.Add("_data = {");
                AnimLib.Lines.Add($"\"RESET\": ExtResource({ResetAnimRefID}),");
                AnimLib.Lines.Add("}");
                scene.InternalResourceList.Add(AnimLib);

                AnimNode = new($"AnimationPlayer", "AnimationPlayer");
                AnimNode.KeyValues.Add("parent", $"{rig.Name}");
                AnimNode.Lines.Add("libraries = {");
                AnimNode.Lines.Add($"\"\": SubResource({scene.InternalResourceList.Count})");
                AnimNode.Lines.Add("}");
                scene.Nodes.Add(AnimNode);
            }

            Dictionary<uint, GodotSceneFile.Node> AttachNodes = new();
            foreach (var prim in GetChildren<CompositeDrawablePrimitive>())
            {
                var item = File.RootChunk.GetChildByName<Named>(prim.Name);
                if (item == null || string.IsNullOrEmpty(item.Name)) continue;
                item.OnGodotExport(path);
                
                GodotFileBase.ExternalResource ModelFileReference = new($"{item.Name}.tscn");
                ModelFileReference.SetAsPackedScene();
                scene.ExternalResourceList.Add(ModelFileReference);

                GodotSceneFile.Node ModelNode = new($"{item.Name}");
                ModelNode.InstanceID = scene.ExternalResourceList.Count;
                if (!string.IsNullOrEmpty(SkeletonName))
                {
                    if (prim.UnkInt4 != 0)
                    {
                        uint jointID = prim.UnkInt4;
                        if (!AttachNodes.ContainsKey(jointID))
                        {
                            var bone = (Named)rig.Children[(int)jointID];
                            var attach = new GodotSceneFile.Node($"attach{jointID}", "BoneAttachment3D");
                            attach.KeyValues.Add("parent", SkeletonName);
                            attach.Lines.Add($"bone_name=\"{bone.Name}\"");
                            attach.Lines.Add($"bone_idx={jointID}");
                            AttachNodes.Add(jointID, attach);
                            scene.Nodes.Add(attach);
                        }
                        ModelNode.KeyValues.Add("parent", $"{SkeletonName}/attach{jointID}");
                    }
                    else
                    {
                        ModelNode.KeyValues.Add("parent", SkeletonName);
                    }
                }
                else
                {
                    ModelNode.KeyValues.Add("parent", ".");
                }
                scene.Nodes.Add(ModelNode);
            }

            bool firstAnim = false;
            foreach (var frame in GetChildren<FrameController>())
            {
                var anim = File.RootChunk.GetChildByName<Animation>(frame.AnimName);
                if (!string.IsNullOrEmpty(anim.Name))
                {
                    anim.OnGodotExport(path);
                }

                GodotSceneFile.ExternalResource ResetAnimRef = new($"{frame.AnimName}.res");
                ResetAnimRef.SetAsAnimation();
                scene.ExternalResourceList.Add(ResetAnimRef);
                int AnimRefID = scene.ExternalResourceList.Count;
                AnimLib.Lines.Insert(AnimLib.Lines.Count - 1, $"\"{frame.AnimName}\": ExtResource({AnimRefID}),");
                if (!firstAnim)
                {
                    AnimNode.Lines.Add($"autoplay = \"{frame.AnimName}\"");
                    firstAnim = true; // todo frame controller can contain multiple animations
                }
            }

            scene.WriteToFile(outName);
        }

    }
}