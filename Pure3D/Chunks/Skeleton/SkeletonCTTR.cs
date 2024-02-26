using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using Pure3D;
using Pure3D.Chunks;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x23000)]
    public class SkeletonCTTR : Named
    {
        public uint Version;
        public uint NumJoints;
        public uint PartitionCount;
        public uint LimbCount;

        public SkeletonCTTR(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt32();
            NumJoints = reader.ReadUInt32();
            PartitionCount = reader.ReadUInt32();
            LimbCount = reader.ReadUInt32();
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            base.WriteHeader(writer);
            writer.Write(Version);
            writer.Write(NumJoints);
            writer.Write(PartitionCount);
            writer.Write(LimbCount);
        }

        public override string ToString()
        {
            return $"Skeleton: {Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"Skeleton CTTR: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"NumJoints: {NumJoints}");
            Lines.AppendLine($"PartitionCount: {PartitionCount}");
            Lines.AppendLine($"LimbCount: {LimbCount}");

            return Lines.ToString();
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"Rig_{Name}.tscn";
            if (System.IO.File.Exists(outName)) return;
            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name);
            scene.Nodes[0].Type = "Skeleton";
            var RootNode = scene.Nodes[0];
            var jnts = GetChildren<SkeletonJointCTTR>();
            
            for (int i = 0; i < NumJoints; i++)
            {
                var item = jnts[i];
                string RestPoseString = MatrixToTransform(item.RestPose);
                System.Numerics.Matrix4x4.Decompose(item.RestPose, out System.Numerics.Vector3 scale, out System.Numerics.Quaternion rot, out System.Numerics.Vector3 pos);
                RootNode.Lines.Add($"bones/{i}/name=\"{item.Name}\"");
                if (i == 0) {
                    RootNode.Lines.Add($"bones/{i}/parent=-1");
                }
                else{
                    RootNode.Lines.Add($"bones/{i}/parent={(int)(item.SkeletonParent)}");
                }
                RootNode.Lines.Add($"bones/{i}/rest={RestPoseString}");
                RootNode.Lines.Add($"bones/{i}/position=Vector3({pos.X.ToText()},{pos.Y.ToText()},{pos.Z.ToText()})");
                RootNode.Lines.Add($"bones/{i}/rotation=Quaternion({rot.X.ToText()},{rot.Y.ToText()},{rot.Z.ToText()},{rot.W.ToText()})");
                RootNode.Lines.Add($"bones/{i}/scale=Vector3({scale.X.ToText()},{scale.Y.ToText()},{scale.Z.ToText()})");
            }

            scene.WriteToFile(outName);

            GodotBinaryAnimation resetAnim = new GodotBinaryAnimation(this);
            resetAnim.WriteToFile(pathDir + $"Rig_{Name}_RESET.res");
        }

        public static string MatrixToTransform(System.Numerics.Matrix4x4 Matrix)
        {
            //Transform( 1, 0, 0 | 0, 1, 0 | 0, 0, 1 | 0, 0, 0 )");
            // godot doesn't like strings like 5,960464E-08, has to be 5.960464e-08
            List<string> Values = new List<string>();
            Values.Add((Matrix.M11).ToText());
            Values.Add((Matrix.M21).ToText());
            Values.Add((Matrix.M31).ToText());

            Values.Add((Matrix.M12).ToText());
            Values.Add((Matrix.M22).ToText());
            Values.Add((Matrix.M32).ToText());

            Values.Add((Matrix.M13).ToText());
            Values.Add((Matrix.M23).ToText());
            Values.Add((Matrix.M33).ToText());

            Values.Add(Matrix.M41.ToText());
            Values.Add(Matrix.M42.ToText());
            Values.Add(Matrix.M43.ToText());

            string outMatrix = $"{ExportGodot.Transform3D}(";
            for (int i = 0; i < Values.Count - 1; i++)
            {
                outMatrix += $"{Values[i]},";
            }
            outMatrix += $"{Values[Values.Count - 1]})";
            return outMatrix;
        }
    }
}