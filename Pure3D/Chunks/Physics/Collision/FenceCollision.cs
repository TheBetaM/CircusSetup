using System.Collections.Generic;
using System.IO;
using System.Text;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x7010025)]
    public class FenceCollision : Named
    {
        public uint Unk;

        public FenceCollision(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Unk = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"Fence Collision: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Fence Collision: {Name}");
            Lines.AppendLine($"Unk: {Unk}");

            return Lines.ToString();
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"Collision_{Name}.tscn";
            if (System.IO.File.Exists(outName)) return;

            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name, -1, ExportGodot.StaticBody3D);

            var volume = GetChild<CollisionVolume>();
            string shapeOutName = pathDir + $"Collision_{Name}_shape.res";
            GodotBinaryCollisionShape shape = new(GetChild<CollisionVolume>().GetChild<FenceHeader>());
            shape.WriteToFile(shapeOutName);

            GodotFileBase.ExternalResource ModelFileReference = new($"Collision_{Name}_shape.res");
            ModelFileReference.Type = shape.ResType;
            scene.ExternalResourceList.Add(ModelFileReference);

            GodotSceneFile.Node ShapeNode = new("CollisionShape", ExportGodot.CollisionShape3D);
            ShapeNode.KeyValues.Add("parent", $".");
            ShapeNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({volume.vector.X.ToText()},{volume.vector.Y.ToText()},{volume.vector.Z.ToText()})");
            ShapeNode.Lines.Add($"shape=ExtResource(1)");
            scene.Nodes.Add(ShapeNode);
            
            scene.WriteToFile(outName);
        }
    }
}