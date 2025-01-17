using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Numerics;
using CircusSetup;

namespace Pure3D.Chunks
{
    [ChunkType(0x3F00020)]
    public class WorldDef : Named
    {
        public ushort Version; // 22 in demo/preview, 23 in final
        public uint UnkPlatformSpecific; // the only value that differs between consoles
        ushort UnkCount01;
        ushort TriggerCount;
        ushort UnkCount03;
        ushort CollisionPointCount;
        ushort PickupCount;
        ushort UnkCount06; // ignored by the game?
        ushort PropCount;
        ushort UnkCount08;
        ushort UnkCount09;
        ushort UnkCount10;
        ushort UnkCount11;
        ushort UnkCount12;
        ushort UnkCount13; // ignored by the game?
        ushort UnkCount14;
        ushort UnkCount15;
        ushort UnkCount16;
        short UnkShort2; // 0/1/-1

        public Vector3 SpawnPoint;
        public Vector3 SpawnRot;
        public float UnkFloat1; // SpawnRot W?
        public Vector3 BoundingBoxMin;
        public Vector3 BoundingBoxMax;

        public List<Vector4> UnkCount03List1 = new List<Vector4>();
        public List<byte> UnkCount03List2 = new List<byte>();

        public List<uint> UnkCount01List1 = new List<uint>();
        public List<short> UnkCount01List2 = new List<short>();
        public List<short> UnkCount01List3 = new List<short>();
        public List<Vector3> UnkCount01Vec3Low = new List<Vector3>();
        public List<Vector3> UnkCount01Vec3Hi = new List<Vector3>();

        public List<Trigger> Triggers = new List<Trigger>();

        public List<Vector3> CollisionPoints = new List<Vector3>();

        public List<Pickup> Pickups = new List<Pickup>();

        public List<Prop> Props = new List<Prop>();

        public List<Vector3> UnkCount08List1 = new List<Vector3>(); // not coordinates

        public List<ushort> UnkCount09List1 = new List<ushort>();

        public List<ushort> UnkCount10List1 = new List<ushort>();

        public List<float> UnkCount11List1 = new List<float>();
        public List<short> UnkCount11List2 = new List<short>();
        public List<short> UnkCount11List2_a = new List<short>();
        public List<short> UnkCount11List3 = new List<short>();
        public List<Vector3> UnkCount11List4 = new List<Vector3>();
        public List<Vector4> UnkCount11List5 = new List<Vector4>();

        public List<short> UnkCount12List1 = new List<short>();
        public List<uint> UnkCount12List2 = new List<uint>();
        public List<Vector3> UnkCount12List3 = new List<Vector3>();
        public List<uint> UnkCount12List4 = new List<uint>();
        public List<uint> UnkCount12List5 = new List<uint>();

        public List<uint> UnkCount14List1 = new List<uint>();
        public List<ushort> UnkCount14List2 = new List<ushort>();

        public List<ushort> UnkCount15List1 = new List<ushort>();

        public List<ushort> UnkCount16List1 = new List<ushort>();

        public WorldDef(File file, uint type) : base(file, type)
        {

        }

        public override void ReadHeader(BinaryReader reader, long length)
        {
            long endPos = reader.BaseStream.Position + length;
            base.ReadHeader(reader, length);
            Version = reader.ReadUInt16();
            UnkPlatformSpecific = reader.ReadUInt32();
            UnkCount01 = reader.ReadUInt16();
            TriggerCount = reader.ReadUInt16();
            UnkCount03 = reader.ReadUInt16();
            CollisionPointCount = reader.ReadUInt16();
            PickupCount = reader.ReadUInt16();
            UnkCount06 = reader.ReadUInt16();
            PropCount = reader.ReadUInt16();
            UnkCount08 = reader.ReadUInt16();
            UnkCount09 = reader.ReadUInt16();
            UnkCount10 = reader.ReadUInt16();
            UnkCount11 = reader.ReadUInt16();
            UnkCount12 = reader.ReadUInt16();
            UnkCount13 = reader.ReadUInt16();
            UnkCount14 = reader.ReadUInt16();
            UnkCount15 = reader.ReadUInt16();
            UnkCount16 = reader.ReadUInt16();
            UnkShort2 = reader.ReadInt16();
            
            SpawnPoint = Util.ReadVector3(reader);
            SpawnRot = Util.ReadVector3(reader);
            UnkFloat1 = reader.ReadSingle();
            BoundingBoxMin = Util.ReadVector3(reader);
            BoundingBoxMax = Util.ReadVector3(reader);
            for (int i = 0; i < UnkCount03; i++)
            {
                UnkCount03List1.Add(Util.ReadVector4(reader));
            }
            for (int i = 0; i < UnkCount03; i++)
            {
                UnkCount03List2.Add(reader.ReadByte());
            }
            for (int i = 0; i < UnkCount01; i++)
            {
                UnkCount01List1.Add(reader.ReadUInt32());
            }
            for (int i = 0; i < UnkCount01; i++)
            {
                UnkCount01List2.Add(reader.ReadInt16());
            }
            for (int i = 0; i < UnkCount01; i++)
            {
                UnkCount01List3.Add(reader.ReadInt16());
            }
            for (int i = 0; i < UnkCount01; i++)
            {
                UnkCount01Vec3Low.Add(Util.ReadVector3(reader));
                UnkCount01Vec3Hi.Add(Util.ReadVector3(reader));
            }
            for (int i = 0; i < TriggerCount; i++)
            {
                Trigger t = new();
                t.BoxMin = Util.ReadVector3(reader);
                t.BoxMax = Util.ReadVector3(reader);
                Triggers.Add(t);
            }
            for (int i = 0; i < TriggerCount; i++)
            {
                Triggers[i].UnkShort = reader.ReadInt16();
            }
            for (int i = 0; i < CollisionPointCount; i++)
            {
                CollisionPoints.Add(Util.ReadVector3(reader));
            }
            for (int i = 0; i < PickupCount; i++)
            {
                Pickup p = new();
                p.Position = Util.ReadVector3(reader);
                Pickups.Add(p);
            }
            for (int i = 0; i < PickupCount; i++)
            {
                Pickups[i].Type = reader.ReadUInt16();
                Pickups[i].UnkShort1 = reader.ReadUInt16();
            }
            for (int i = 0; i < PickupCount; i++)
            {
                Pickups[i].UnkShort2 = reader.ReadInt16();
            }
            for (int i = 0; i < PropCount; i++)
            {
                Prop p = new();
                p.UnkInt1 = reader.ReadUInt32();
                p.UnkInt2 = reader.ReadUInt32();
                Props.Add(p);
            }
            for (int i = 0; i < PropCount; i++)
            {
                Props[i].Position = Util.ReadVector3(reader);
            }
            for (int i = 0; i < PropCount; i++)
            {
                Props[i].Rotation = Util.ReadVector4(reader);
            }
            for (int i = 0; i < PropCount; i++)
            {
                Props[i].UnkShort = reader.ReadInt16();
            }
            if (Version >= 23)
            {
                for (int i = 0; i < PropCount; i++)
                {
                    Props[i].UnkInt3 = reader.ReadUInt32();
                }
            }
            for (int i = 0; i < UnkCount08; i++)
            {
                UnkCount08List1.Add(Util.ReadVector3(reader));
            }
            for (int i = 0; i < UnkCount09; i++)
            {
                UnkCount09List1.Add(reader.ReadUInt16());
            }
            for (int i = 0; i < UnkCount10; i++)
            {
                UnkCount10List1.Add(reader.ReadUInt16());
            }
            for (int i = 0; i < UnkCount11; i++)
            {
                UnkCount11List1.Add(reader.ReadSingle());
            }
            for (int i = 0; i < UnkCount11; i++)
            {
                UnkCount11List2.Add(reader.ReadInt16());
                UnkCount11List2_a.Add(reader.ReadInt16());
            }
            for (int i = 0; i < UnkCount11; i++)
            {
                UnkCount11List3.Add(reader.ReadInt16());
            }
            for (int i = 0; i < UnkCount11; i++)
            {
                UnkCount11List4.Add(Util.ReadVector3(reader));
            }
            for (int i = 0; i < UnkCount11; i++)
            {
                UnkCount11List5.Add(Util.ReadVector4(reader));
            }
            for (int i = 0; i < UnkCount12; i++)
            {
                UnkCount12List1.Add(reader.ReadInt16());
            }
            for (int i = 0; i < UnkCount12; i++)
            {
                UnkCount12List2.Add(reader.ReadUInt32());
            }
            for (int i = 0; i < UnkCount12; i++)
            {
                UnkCount12List3.Add(Util.ReadVector3(reader));
            }
            for (int i = 0; i < UnkCount12; i++)
            {
                UnkCount12List4.Add(reader.ReadUInt32());
                UnkCount12List5.Add(reader.ReadUInt32());
            }
            for (int i = 0; i < UnkCount14; i++)
            {
                UnkCount14List1.Add(reader.ReadUInt32());
            }
            for (int i = 0; i < UnkCount14; i++)
            {
                UnkCount14List2.Add(reader.ReadUInt16());
            }
            for (int i = 0; i < UnkCount15; i++)
            {
                UnkCount15List1.Add(reader.ReadUInt16());
            }
            for (int i = 0; i < UnkCount16; i++)
            {
                UnkCount16List1.Add(reader.ReadUInt16());
            }
        }

        public override void WriteHeader(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public override string? ToDetails()
        {
            StringBuilder Lines = new();

            Lines.AppendLine($"WorldDef Name: {Name}");
            Lines.AppendLine($"Version: {Version}");
            Lines.AppendLine($"UnkCount01: {UnkCount01}");
            Lines.AppendLine($"TriggerCount: {TriggerCount}");
            Lines.AppendLine($"UnkCount03: {UnkCount03}");
            Lines.AppendLine($"CollisionPointCount: {CollisionPointCount}");
            Lines.AppendLine($"PickupCount: {PickupCount}");
            //Lines.AppendLine($"UnkCount06 (unused?): {UnkCount06}");
            Lines.AppendLine($"PropCount: {PropCount}");
            Lines.AppendLine($"UnkCount08: {UnkCount08}");
            Lines.AppendLine($"UnkCount09: {UnkCount09}");
            Lines.AppendLine($"UnkCount10: {UnkCount10}");
            Lines.AppendLine($"UnkCount11: {UnkCount11}");
            Lines.AppendLine($"UnkCount12: {UnkCount12}");
            //Lines.AppendLine($"UnkCount13 (unused?): {UnkCount13}");
            Lines.AppendLine($"UnkCount14: {UnkCount14}");
            Lines.AppendLine($"UnkCount15: {UnkCount15}");
            Lines.AppendLine($"UnkCount16: {UnkCount16}");
            Lines.AppendLine($"UnkShort: {UnkShort2}");
            Lines.AppendLine($"UnkPlatformSpecific 0x{UnkPlatformSpecific:X8}");

            Lines.AppendLine($"SpawnPoint: {SpawnPoint}");
            Lines.AppendLine($"SpawnRot: {SpawnRot}");
            Lines.AppendLine($"UnkFloat1 (SpawnRotW?): {UnkFloat1}");
            Lines.AppendLine($"BoundingBoxMin: {BoundingBoxMin}");
            Lines.AppendLine($"BoundingBoxMax: {BoundingBoxMax}");
            
            /*
            Lines.AppendLine($"UnkCount03:");
            for (int i = 0; i < UnkCount03; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount03List1[i]} {UnkCount03List2[i]}");
            }
            */
            

            /*
            Lines.AppendLine($"UnkCount01:");
            for (int i = 0; i < UnkCount01; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount01List1[i]} {UnkCount01List2[i]} {UnkCount01List3[i]} {UnkCount01Vec3Low[i]} {UnkCount01Vec3Hi[i]}");
            }
            */

            
            Lines.AppendLine($"Triggers:");
            for (int i = 0; i < TriggerCount; i++)
            {
                var t = Triggers[i];
                Lines.AppendLine($"#{i}: {t.BoxMin} {t.BoxMax} {t.UnkShort}");
            }
            

            /*
            Lines.AppendLine($"CollisionPoints:");
            for (int i = 0; i < CollisionPointCount; i++)
            {
                Lines.AppendLine($"#{i}: {CollisionPoints[i]}");
            }
            */
            

            
            Lines.AppendLine($"Pickups:");
            for (int i = 0; i < PickupCount; i++)
            {
                var p = Pickups[i];
                Lines.AppendLine($"#{i}: {p.Position} {(Pickup.PType)p.Type} {p.UnkShort1} {p.UnkShort2}");
            }
            
            
            Lines.AppendLine($"Props:");
            if (Version >= 23)
            {
                for (int i = 0; i < PropCount; i++)
                {
                    var p = Props[i];
                    Lines.AppendLine($"#{i}: {p.UnkInt1} {p.UnkInt2} {p.Position} {p.Rotation} {p.UnkShort} {p.UnkInt3}");
                }
            }
            else
            {
                for (int i = 0; i < PropCount; i++)
                {
                    var p = Props[i];
                    Lines.AppendLine($"#{i}: {p.UnkInt1} {p.UnkInt2} {p.Position} {p.Rotation} {p.UnkShort}");
                }
            }
            
            

            /*
            Lines.AppendLine($"UnkCount08:");
            for (int i = 0; i < UnkCount08; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount08List1[i]}");
            }
            */
            

            /*
            ushort max1 = 0;
            Lines.AppendLine($"UnkCount09:");
            for (int i = 0; i < UnkCount09; i++)
            {
                //Lines.AppendLine($"#{i}: {UnkCount09List1[i]}");
                //Lines.AppendLine($"#{i}: {UnkCount09List1[i]}: {UnkCount10List1[UnkCount09List1[i]]}");
                if (UnkCount09List1[i] > max1)
                {
                    max1 = UnkCount09List1[i];
                }
            }
            Lines.AppendLine($"Max: {max1}");
            */

            /*
            Lines.AppendLine($"UnkCount10:");
            for (int i = 0; i < UnkCount10; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount10List1[i]}");
            }
            */
            

            
            Lines.AppendLine($"UnkCount11:");
            for (int i = 0; i < UnkCount11; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount11List1[i]} {UnkCount11List2[i]} {UnkCount11List2_a[i]} {UnkCount11List3[i]} {UnkCount11List4[i]} {UnkCount11List5[i]}");
            }
            

            
            Lines.AppendLine($"UnkCount12:");
            for (int i = 0; i < UnkCount12; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount12List1[i]} {UnkCount12List2[i]} {UnkCount12List3[i]} {UnkCount12List4[i]} {UnkCount12List5[i]}");
            }
            

            
            Lines.AppendLine($"UnkCount14:");
            for (int i = 0; i < UnkCount14; i++)
            {
                Lines.AppendLine($"#{i}: {UnkCount14List1[i]} {UnkCount14List2[i]}");
            }
            
            
            
            /*
            uint max1 = 0;
            Lines.AppendLine($"UnkCount15:");
            for (int i = 0; i < UnkCount15; i++)
            {
                //Lines.AppendLine($"#{i}: {UnkCount15List1[i]}");
                //if (i != UnkCount15 - 1)
                //    Lines.AppendLine($"#{i}: {UnkCount15List1[i]}: {UnkCount16List1[UnkCount15List1[i]]}");
                if (UnkCount15List1[i] > max1)
                {
                    max1 = UnkCount15List1[i];
                }
            }
            Lines.AppendLine($"Max: {max1}");
            */
            

            /*
            max1 = 0;
            Lines.AppendLine($"UnkCount16:");
            for (int i = 0; i < UnkCount16; i++)
            {
                //Lines.AppendLine($"#{i}: {UnkCount16List1[i]}");
                if (UnkCount16List1[i] > max1)
                {
                    max1 = UnkCount16List1[i];
                }
            }
            Lines.AppendLine($"Max: {max1}");
            */

            return Lines.ToString();
        }

        public override void OnGodotExport(string path)
        {
            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"WorldDef.tscn";
            if (System.IO.File.Exists(outName)) return;

            GodotSceneFileCircus scene = GodotSceneFileCircus.Create(Name);

            string shapeOutName = pathDir + $"Collision_{Name}_shape.res";
            GodotBinaryCollisionShape shape = new(this);
            shape.WriteToFile(shapeOutName);

            GodotFileBase.ExternalResource ModelFileReference = new($"Collision_{Name}_shape.res");
            ModelFileReference.Type = shape.ResType;
            scene.ExternalResourceList.Add(ModelFileReference);

            GodotSceneFile.Node ShapeNode = new("CollisionShape", ExportGodot.CollisionShape3D);
            ShapeNode.KeyValues.Add("parent", $".");
            ShapeNode.Lines.Add($"shape=ExtResource(1)");
            scene.Nodes.Add(ShapeNode);

            
            GodotSceneFile.Node Count1Node = new($"Count1", ExportGodot.Node3D);
            Count1Node.KeyValues.Add("parent", ".");
            scene.Nodes.Add(Count1Node);
            for (int i = 0; i < UnkCount01; i++)
            {
                float BoxSizeX = (UnkCount01Vec3Hi[i].X - UnkCount01Vec3Low[i].X) / 2f;
                float BoxSizeY = (UnkCount01Vec3Hi[i].Y - UnkCount01Vec3Low[i].Y) / 2f;
                float BoxSizeZ = (UnkCount01Vec3Hi[i].Z - UnkCount01Vec3Low[i].Z) / 2f;
                float BoxPosX = (UnkCount01Vec3Hi[i].X - BoxSizeX);
                float BoxPosY = (UnkCount01Vec3Hi[i].Y - BoxSizeY);
                float BoxPosZ = (UnkCount01Vec3Hi[i].Z - BoxSizeZ);
                GodotSceneFile.InternalResource BoxShapeData = new GodotSceneFile.InternalResource();
                BoxShapeData.CreateBoxShape(BoxSizeX, BoxSizeY, BoxSizeZ);
                scene.InternalResourceList.Add(BoxShapeData);

                GodotSceneFile.Node BoxNode = new($"Count1_{i}", ExportGodot.CollisionShape3D);
                BoxNode.KeyValues.Add("parent", "Count1");
                BoxNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({BoxPosX.ToText()},{BoxPosY.ToText()},{BoxPosZ.ToText()})");
                BoxNode.Lines.Add($"shape=SubResource({scene.InternalResourceList.Count})");
                scene.Nodes.Add(BoxNode);
            }

            GodotSceneFile.Node TriggersNode = new($"Triggers", ExportGodot.Node3D);
            TriggersNode.KeyValues.Add("parent", ".");
            scene.Nodes.Add(TriggersNode);
            for (int i = 0; i < TriggerCount; i++)
            {
                var t = Triggers[i];
                float BoxSizeX = (t.BoxMax.X - t.BoxMin.X) / 2f;
                float BoxSizeY = (t.BoxMax.Y - t.BoxMin.Y) / 2f;
                float BoxSizeZ = (t.BoxMax.Z - t.BoxMin.Z) / 2f;
                float BoxPosX = (t.BoxMax.X - BoxSizeX);
                float BoxPosY = (t.BoxMax.Y - BoxSizeY);
                float BoxPosZ = (t.BoxMax.Z - BoxSizeZ);
                GodotSceneFile.InternalResource BoxShapeData = new GodotSceneFile.InternalResource();
                BoxShapeData.CreateBoxShape(BoxSizeX, BoxSizeY, BoxSizeZ);
                scene.InternalResourceList.Add(BoxShapeData);

                GodotSceneFile.Node BoxNode = new($"Trigger_{i}", ExportGodot.CollisionShape3D);
                BoxNode.KeyValues.Add("parent", "Triggers");
                BoxNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({BoxPosX.ToText()},{BoxPosY.ToText()},{BoxPosZ.ToText()})");
                BoxNode.Lines.Add($"shape=SubResource({scene.InternalResourceList.Count})");
                scene.Nodes.Add(BoxNode);
            }
            

            GodotSceneFile.Node PickupsNode = new($"Pickups", ExportGodot.Node3D);
            PickupsNode.KeyValues.Add("parent", ".");
            scene.Nodes.Add(PickupsNode);
            for (int i = 0; i < Pickups.Count; i++)
            {
                var p = Pickups[i];
                string name = $"Pickup_{i}";
                switch (p.Type)
                {
                    default: break;
                    case 0: name = $"Crystal_{i}"; break;
                    case 1: name = $"WumpaWhip_{i}"; break;
                    case 2: name = $"Coin_{i}"; break;
                    case 9: name = $"Interactable_{i}"; break;
                }
                GodotSceneFile.Node PickupNode = new(name, ExportGodot.Marker3D);
                PickupNode.KeyValues.Add("parent", "Pickups");
                PickupNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({p.Position.X.ToText()},{p.Position.Y.ToText()},{p.Position.Z.ToText()})");
                scene.Nodes.Add(PickupNode);
            }

            GodotSceneFile.Node PropsNode = new($"Props", ExportGodot.Node3D);
            PropsNode.KeyValues.Add("parent", ".");
            scene.Nodes.Add(PropsNode);
            for (int i = 0; i < PropCount; i++)
            {
                var p = Props[i];
                GodotSceneFile.Node PropNode = new($"Prop_{i}", ExportGodot.Marker3D);
                PropNode.KeyValues.Add("parent", "Props");
                PropNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({p.Position.X.ToText()},{p.Position.Y.ToText()},{p.Position.Z.ToText()})");
                PropNode.Lines.Add($"quaternion = Quaternion({p.Rotation.X.ToText()},{p.Rotation.Y.ToText()},{p.Rotation.Z.ToText()},{p.Rotation.W.ToText()})");
                scene.Nodes.Add(PropNode);
            }

            /*
            GodotSceneFile.Node Count8Node = new($"Count8", ExportGodot.Node3D);
            Count8Node.KeyValues.Add("parent", ".");
            scene.Nodes.Add(Count8Node);
            for (int i = 0; i < UnkCount08List1.Count; i++)
            {
                GodotSceneFile.Node Count8Node1 = new($"Count8_{i}", ExportGodot.Marker3D);
                Count8Node1.KeyValues.Add("parent", "Count8");
                Count8Node1.Lines.Add($"{ExportGodot.transformPosition} = Vector3({UnkCount08List1[i].X.ToText()},{UnkCount08List1[i].Y.ToText()},{UnkCount08List1[i].Z.ToText()})");
                scene.Nodes.Add(Count8Node1);
            }
            */

            GodotSceneFile.Node Count11Node = new($"Count11", ExportGodot.Node3D);
            Count11Node.KeyValues.Add("parent", ".");
            scene.Nodes.Add(Count11Node);
            for (int i = 0; i < UnkCount11List1.Count; i++)
            {
                GodotSceneFile.Node Count11Node1 = new($"Count11_{i}", ExportGodot.Marker3D);
                Count11Node1.KeyValues.Add("parent", "Count11");
                Count11Node1.Lines.Add($"{ExportGodot.transformPosition} = Vector3({UnkCount11List4[i].X.ToText()},{UnkCount11List4[i].Y.ToText()},{UnkCount11List4[i].Z.ToText()})");
                Count11Node1.Lines.Add($"quaternion = Quaternion({UnkCount11List5[i].X.ToText()},{UnkCount11List5[i].Y.ToText()},{UnkCount11List5[i].Z.ToText()},{UnkCount11List5[i].W.ToText()})");
                scene.Nodes.Add(Count11Node1);
            }

            GodotSceneFile.Node Count12Node = new($"Count12", ExportGodot.Node3D);
            Count12Node.KeyValues.Add("parent", ".");
            scene.Nodes.Add(Count12Node);
            for (int i = 0; i < UnkCount12List1.Count; i++)
            {
                GodotSceneFile.Node Count12Node1 = new($"Count12_{i}", ExportGodot.Marker3D);
                Count12Node1.KeyValues.Add("parent", "Count12");
                Count12Node1.Lines.Add($"{ExportGodot.transformPosition} = Vector3({UnkCount12List3[i].X.ToText()},{UnkCount12List3[i].Y.ToText()},{UnkCount12List3[i].Z.ToText()})");
                scene.Nodes.Add(Count12Node1);
            }

            GodotSceneFile.Node SpawnPointNode = new($"SpawnPoint", ExportGodot.Node3D);
            SpawnPointNode.KeyValues.Add("parent", ".");
            SpawnPointNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({SpawnPoint.X.ToText()},{SpawnPoint.Y.ToText()},{SpawnPoint.Z.ToText()})");
            SpawnPointNode.Lines.Add($"quaternion = Quaternion({SpawnRot.X.ToText()},{SpawnRot.Y.ToText()},{SpawnRot.Z.ToText()},{UnkFloat1.ToText()})");
            scene.Nodes.Add(SpawnPointNode);

            float BBoxSizeX = (BoundingBoxMax.X - BoundingBoxMin.X) / 2f;
            float BBoxSizeY = (BoundingBoxMax.Y - BoundingBoxMin.Y) / 2f;
            float BBoxSizeZ = (BoundingBoxMax.Z - BoundingBoxMin.Z) / 2f;
            float BBoxPosX = (BoundingBoxMax.X - BBoxSizeX);
            float BBoxPosY = (BoundingBoxMax.Y - BBoxSizeY);
            float BBoxPosZ = (BoundingBoxMax.Z - BBoxSizeZ);
            GodotSceneFile.InternalResource BBoxShapeData = new GodotSceneFile.InternalResource();
            BBoxShapeData.CreateBoxShape(BBoxSizeX, BBoxSizeY, BBoxSizeZ);
            scene.InternalResourceList.Add(BBoxShapeData);

            GodotSceneFile.Node BBoxNode = new($"BoundingBox", ExportGodot.CollisionShape3D);
            BBoxNode.KeyValues.Add("parent", ".");
            BBoxNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({BBoxPosX.ToText()},{BBoxPosY.ToText()},{BBoxPosZ.ToText()})");
            BBoxNode.Lines.Add($"shape=SubResource({scene.InternalResourceList.Count})");
            scene.Nodes.Add(BBoxNode);
            
            scene.WriteToFile(outName);
        }

        public class Pickup
        {
            public Vector3 Position;
            public ushort Type;
            public ushort UnkShort1; // ID?
            public short UnkShort2; // Target?

            public enum PType
            {
                Crystal = 0,
                WumpaWhip = 1,
                Coin = 2,
                Interactable = 9,
            }
        }

        public class Trigger
        {
            public Vector3 BoxMin;
            public Vector3 BoxMax;
            public short UnkShort;
        }

        public class Prop
        {
            public uint UnkInt1; // Model related 1
            public uint UnkInt2; // Model related 2
            public Vector3 Position;
            public Vector4 Rotation;
            public short UnkShort; // Movement path ID?
            public uint UnkInt3; // 0/1/2, Version >= 23
        }
    }
}
