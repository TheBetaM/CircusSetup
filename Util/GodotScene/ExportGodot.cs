using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Numerics;
using Pure3D;
using Pure3D.Chunks;

namespace CircusSetup
{
    public static class ExportGodot
    {
        public static void ExportP3D(Root RootChunk, string path)
        {
            //Stopwatch Timer = new Stopwatch();
            //Timer.Start();

            string pathDir = System.IO.Path.GetDirectoryName(path) + "\\";
            string outName = pathDir + $"P3D.tscn";
            GodotSceneFileCircus scene = GodotSceneFileCircus.Create("P3D");
            bool HasDrawables = false;
            bool HasScenegraph = false;

            //List<string> DrawablesUsed = new List<string>();
            List<string> ModelsUsed = new List<string>();
            foreach (var item in RootChunk.Children)
            {
                item.OnGodotExport(path);
                if (item is CompositeDrawableCTTR draw)
                {
                    AddNamedScene(scene, item);
                    foreach (var comp in item.Children)
                    {
                        if (comp is CompositeDrawablePrimitive prim)
                        {
                            ModelsUsed.Add(prim.Name);
                        }
                    }
                }
                else if (item is Scenegraph graph)
                {
                    HasScenegraph = true;
                    AddNamedScene(scene, item);
                    //NestedSceneGraph(graph, ref DrawablesUsed);
                }
                else if (item is Locator loc)
                {
                    GodotSceneFile.Node LocNode = new($"{loc.Name}", ExportGodot.Node3D);
                    LocNode.KeyValues.Add("parent", ".");
                    LocNode.Lines.Add($"{ExportGodot.transformPosition} = Vector3({loc.Position.X.ToText()},{loc.Position.Y.ToText()},{loc.Position.Z.ToText()})");
                    scene.Nodes.Add(LocNode);
                }
                else if (item is FenceCollision)
                {
                    AddNamedScene(scene, item);
                }
            }
            foreach (var item in RootChunk.Children)
            {
                if (item is Mesh ||
                    item is Skin ||
                    item is ShadowMesh)
                {
                    var aitem = (Named)item;
                    if (!ModelsUsed.Contains(aitem.Name))
                    {
                        AddNamedScene(scene, item);
                    }
                }
            }

            scene.WriteToFile(outName);
            //Console.WriteLine($"END: {Timer.Elapsed}");
        }

        public static void AddNamedScene(GodotSceneFileCircus scene, Chunk bchunk)
        {
            var chunk = (Named)bchunk;
            GodotFileBase.ExternalResource ModelFileReference = new($"{chunk.Name}.tscn");
            if (bchunk is Scenegraph)
            {
                ModelFileReference.Path = "Scenegraph_" + ModelFileReference.Path;
            }
            else if (bchunk is FenceCollision)
            {
                ModelFileReference.Path = "Collision_" + ModelFileReference.Path;
            }
            ModelFileReference.SetAsPackedScene();
            scene.ExternalResourceList.Add(ModelFileReference);

            GodotSceneFile.Node ModelNode = new($"{chunk.Name}");
            if (bchunk is Scenegraph)
            {
                ModelNode.Name = "Scenegraph_" + ModelNode.Name;
            }
            else if (bchunk is FenceCollision)
            {
                ModelNode.Name = "Collision_" + ModelNode.Name;
            }
            ModelNode.InstanceID = scene.ExternalResourceList.Count;
            ModelNode.KeyValues.Add("parent", ".");
            scene.Nodes.Add(ModelNode);
        }

        static void NestedSceneGraph(Chunk parent, ref List<string> Drawables)
        {
            if (parent is ScenegraphDrawable draw)
            {
                if (!Drawables.Contains(draw.Name))
                {
                    Drawables.Add(draw.Name);
                }
            }
            foreach (var item in parent.Children)
            {
                NestedSceneGraph(item, ref Drawables);
            }
        }


        #region Constants
        public const uint Format = 3;
        public const string Node3D = "Node3D";
        public const string StandardMaterial3D = "StandardMaterial3D";
        public const string ShaderMaterial = "ShaderMaterial";
        public const string MeshInstance3D = "MeshInstance3D";
        public const string ConvexPolygonShape3D = "ConvexPolygonShape3D";
        public const string ConcavePolygonShape3D = "ConcavePolygonShape3D";
        public const string RigidBody3D = "RigidBody3D";
        public const string StaticBody3D = "StaticBody3D";
        public const string CollisionShape3D = "CollisionShape3D";
        public const string BoxShape3D = "BoxShape3D";
        public const string Area3D = "Area3D";
        public const string CharacterBody3D = "CharacterBody3D";
        public const string Transform3D = "Transform3D";
        public const string Marker3D = "Marker3D";
        public const string materialOverride = "surface_material_override";
        public const string materialCullMode = "cull_mode";
        public const string materialBlendMode = "blend_mode";
        public const string materialTransparency = "transparency = 4"; // depth pre-pass
        public const string materialDepthDrawMode = "";
        public const string Texture2D = "Texture2D";
        public const string Path3D = "Path3D";
        public const string ambientLightSource = "ambient_light_source = 2";
        public const string transformPosition = "position";
        #endregion

        #region Helpers
        public static string ToText(this float f)
        {
            return f.ToString().ToLower().Replace(',', '.');
        }
        #endregion

    }
}
