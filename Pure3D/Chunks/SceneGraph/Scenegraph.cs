using System.Collections.Generic;
using System.IO;
using System.Text;
using Pure3D;
using Pure3D.Chunks;
using CircusSetup;
using System.Numerics;

namespace Pure3D.Chunks
{
    [ChunkType(0x120100)]
    public class Scenegraph : Named
    {
        public uint Version;

        public Scenegraph(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Scenegraph: {Name} Ver: {Version}";
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"Scenegraph_{Name}.tscn";
            if (System.IO.File.Exists(outName)) return;

            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name);

            var graph = GetChild<ScenegraphRoot>();
            GodotSceneFile.Node SceneNode = new($"Scenegraph_{Name}", ExportGodot.Node3D);
            SceneNode.KeyValues.Add("parent", ".");
            scene.Nodes.Add(SceneNode);
            foreach (var item in graph.Children)
            {
                NestedSceneGraph(scene, item, $"Scenegraph_{Name}");
            }
            
            scene.WriteToFile(outName);
        }

        public static void NestedSceneGraph(GodotSceneFileCircus scene, Chunk parent, string nodePath)
        {
            var node = (Named)parent;
            GodotSceneFile.Node ModelNode = new($"{node.Name}", ExportGodot.Node3D);
            if (parent is ScenegraphDrawable)
            {
                ModelNode.Type = string.Empty;
                GodotFileBase.ExternalResource ModelFileReference = new($"{node.Name}.tscn");
                var sceneChunk = parent.File.RootChunk.GetChildByName<Named>(node.Name);
                if (sceneChunk != null && sceneChunk is SkeletonCTTR)
                {
                    ModelFileReference.Path = "Rig_" + ModelFileReference.Path;
                }
                ModelFileReference.SetAsPackedScene();
                scene.ExternalResourceList.Add(ModelFileReference);
                ModelNode.InstanceID = scene.ExternalResourceList.Count;
            }
            else if (parent is ScenegraphTransform transform)
            {
                Matrix4x4.Decompose(transform.Matrix, out Vector3 scale, out Quaternion rot, out Vector3 pos);
                ModelNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({pos.X.ToText()},{pos.Y.ToText()},{pos.Z.ToText()})");
                ModelNode.Lines.Add($"rotation = Quaternion({rot.X.ToText()},{rot.Y.ToText()},{rot.Z.ToText()},{rot.W.ToText()})");
                ModelNode.Lines.Add($"scale = Vector3({scale.X.ToText()},{scale.Y.ToText()},{scale.Z.ToText()})");
            }
            ModelNode.KeyValues.Add("parent", nodePath);
            scene.Nodes.Add(ModelNode);

            foreach (var item in parent.Children)
            {
                NestedSceneGraph(scene, item, $"{nodePath}/{node.Name}");
            }
        }
    }
}