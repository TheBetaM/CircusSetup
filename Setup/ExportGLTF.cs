using System;
using System.Collections.Generic;
using System.Text;
using Pure3D.Chunks;
using System.IO;
using System.Drawing;

using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SharpGLTF.Memory;
using SharpGLTF.Transforms;
using SharpGLTF.Animations;
using SharpGLTF.Scenes;
using System.Numerics;
using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPosition;
using VERTEXC = SharpGLTF.Geometry.VertexTypes.VertexColor1Texture1;
using VERTEXT = SharpGLTF.Geometry.VertexTypes.VertexTexture1;
using VERTEXJ = SharpGLTF.Geometry.VertexTypes.VertexJoints4;
using VERTEXN = SharpGLTF.Geometry.VertexTypes.VertexPositionNormal;
using VERTEXNT = SharpGLTF.Geometry.VertexTypes.VertexPositionNormalTangent;
using System.Linq;
using SharpGLTF;

namespace CircusSetup
{
    public static class ExportGLTF
    {

        public static void Export(Pure3D.Chunks.Skin model, string path)
        {
            var scene = new SceneBuilder();
            LoadSkin(scene, path, model);
            var outScene = scene.ToGltf2();
            Export(outScene, path);
        }

        public static void Export(Pure3D.Chunks.Mesh model, string path)
        {
            var scene = new SceneBuilder();
            LoadModel(scene, path, model);
            var outScene = scene.ToGltf2();
            Export(outScene, path);
        }

        public static void Export(Pure3D.Chunks.CompositeDrawableCTTR model, string path)
        {
            var scene = new SceneBuilder();
            LoadDrawable(scene, path, model);
            var outScene = scene.ToGltf2();
            Export(outScene, path);
        }


        static void Export(ModelRoot model, string path, bool ExportGLB = true, bool ExportGLTF = false)
        {
            if (ExportGLB)
            {
                model.SaveGLB(path);
            }
            else if (ExportGLTF)
            {
                model.SaveGLTF(path);
            }
            else
            {
                model.SaveAsWavefront(path);
            }
        }

        static void LoadSkin(SceneBuilder scene, string path, 
            Pure3D.Chunks.Skin Model, List<NodeBuilder> bindings = null)
        {
            if (bindings == null)
            {
                var skeleton = Model.File.RootChunk.GetChildByName<SkeletonCTTR>(Model.SkeletonName);
                bindings = LoadSkeleton(scene, path, skeleton);
            }

            List<Shader> Materials = new List<Shader>();
            List<int> ShaderCounts = new List<int>();
            int TotalShaders = 0;
            List<string> MaterialDupeCheck = new List<string>();
            List<int> MaterialIDs = new List<int>();

            List<PrimitiveGroup> prims = new List<PrimitiveGroup>();
            foreach (var item in Model.Children)
            {
                if (item is PrimitiveGroup prim)
                {
                    prims.Add(prim);
                }
            }

            for (int i = 0; i < Model.NumPrimGroups; i++)
            {
                var prim = prims[i];
                Shader Mat = Model.File.RootChunk.GetChildByName<Shader>(prim.ShaderName);
                ShaderCounts.Add(1);
                if (!MaterialDupeCheck.Contains(prim.ShaderName))
                {
                    Materials.Add(Mat);
                    TotalShaders += 1;
                    MaterialDupeCheck.Add(prim.ShaderName);
                    MaterialIDs.Add(Materials.Count - 1);
                }
                else
                {
                    MaterialIDs.Add(MaterialDupeCheck.IndexOf(prim.ShaderName));
                }
            }

            bool ExportInternalMaterials = true;
            List<MaterialBuilder> BMaterials = new();
            if (ExportInternalMaterials)
            {
                BMaterials = BuildMaterials(Materials, Model.File.RootChunk, path);
            }
            else
            {
                for (int i = 0; i < Materials.Count; i++)
                {
                    BMaterials.Add(new MaterialBuilder($"Mat{i}"));
                }
            }

            
            for (int i = 0; i < Model.NumPrimGroups; i++)
            {
                PrimitiveGroup Sub = prims[i];
                var primType = Sub.PrimitiveType;
                var inds = Sub.GetChild<IndexList>();
                var poslist = Sub.GetChild<PositionList>();
                var norlist = Sub.GetChild<NormalList>();
                var uvlist = Sub.GetChild<UVList>();
                var collist = Sub.GetChild<ColourList>();
                var matlist = Sub.GetChild<MatrixList>();
                var matpal = Sub.GetChild<MatrixPalette>();
                var wgtlist = Sub.GetChild<WeightList>();
                var tanlist = Sub.GetChild<TangentList>();
                var isPlain = poslist != null;
                var hasColors = collist != null;
                var hasTangents = tanlist != null;

                int primCount = 3;
                if (primType == PrimitiveGroup.PrimitiveTypes.Points)
                {
                    primCount = 1;
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.LineList || primType == PrimitiveGroup.PrimitiveTypes.LineStrip)
                {
                    primCount = 2;
                }

                List<VertexData> verts = new List<VertexData>();
                for (int v = 0; v < Sub.NumVertices; v++)
                {
                    VertexData vd = new VertexData();
                    if (poslist != null)
                    {
                        vd.POS = poslist.Positions[v];
                    }
                    if (norlist != null)
                    {
                        vd.NOR = norlist.Normals[v];
                    }
                    if (tanlist != null)
                    {
                        vd.TAN = tanlist.Tangents[v];
                    }
                    if (uvlist != null)
                    {
                        vd.UV = uvlist.UVs[v];
                    }
                    if (collist != null)
                    {
                        vd.COL = new Vector4(collist.Colours[v].R / 255f, collist.Colours[v].G / 255f, collist.Colours[v].B / 255f, collist.Colours[v].A / 255f);
                    }
                    if (wgtlist != null)
                    {
                        vd.WGT = wgtlist.Weights[v];
                    }
                    if (matlist != null && matpal != null)
                    {
                        var ind1 = matlist.Matrices[v][3];
                        var ind2 = matlist.Matrices[v][2];
                        var ind3 = matlist.Matrices[v][1];
                        var ind4 = matlist.Matrices[v][0];
                        vd.JNT[0] = matpal.Matrices[ind1];
                        vd.JNT[1] = matpal.Matrices[ind2];
                        vd.JNT[2] = matpal.Matrices[ind3];
                        vd.JNT[3] = matpal.Matrices[ind4];
                    }
                    verts.Add(vd);
                }

                var vert = new List<IVertexBuilder>();
                IPrimitiveBuilder prim = null;
                IMeshBuilder<MaterialBuilder> mesh = null;
                if (hasColors)
                {
                    mesh = new MeshBuilder<VERTEXN, VERTEXC, VERTEXJ>($"mesh{i}");
                    prim = mesh.UsePrimitive(BMaterials[MaterialIDs[i]], primCount);
                    for (int d = 0; d < verts.Count; d++)
                    {
                        vert.Add(BuildVertexNCJ(verts[d]));
                    }
                }
                else
                {
                    mesh = new MeshBuilder<VERTEXN, VERTEXT, VERTEXJ>($"mesh{i}");
                    prim = mesh.UsePrimitive(BMaterials[MaterialIDs[i]], primCount);
                    for (int d = 0; d < verts.Count; d++)
                    {
                        vert.Add(BuildVertexNTJ(verts[d]));
                    }
                }

                if (primType == PrimitiveGroup.PrimitiveTypes.TriangleList)
                {
                    for (int d = 0; d < inds.Indices.Length - 2; d++)
                    {
                        var v1b = vert[(int)inds.Indices[d]];
                        d++;
                        var v2b = vert[(int)inds.Indices[d]];
                        d++;
                        var v3b = vert[(int)inds.Indices[d]];

                        prim.AddTriangle(v1b, v2b, v3b);
                    }
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.Points)
                {
                    // untested
                    for (int d = 0; d < inds.Indices.Length; d++)
                    {
                        prim.AddPoint(vert[(int)inds.Indices[d]]);
                    }
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.LineList)
                {
                    // untested
                    for (int d = 0; d < inds.Indices.Length - 1; d++)
                    {
                        var v1b = vert[(int)inds.Indices[d]];
                        d++;
                        var v2b = vert[(int)inds.Indices[d]];
                        prim.AddLine(v1b, v2b);
                    }
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.LineStrip)
                {
                    // untested, probably wrong
                    for (int d = 0; d < inds.Indices.Length - 1; d++)
                    {
                        var v1b = vert[(int)inds.Indices[d]];
                        d++;
                        var v2b = vert[(int)inds.Indices[d]];
                        prim.AddLine(v1b, v2b);
                    }
                }
                else
                {
                    //prim.AddTriangle(v[2], v[1], v[0]);
                    //prim.AddTriangle(v[1], v[2], v[3]);
                    for (int d = 1; d < inds.Indices.Length - 2; d++) // d = 3
                    {
                        prim.AddTriangle(vert[(int)inds.Indices[d + 0]], vert[(int)inds.Indices[d - 1]], vert[(int)inds.Indices[d + 1]]);
                        prim.AddTriangle(vert[(int)inds.Indices[d + 0]], vert[(int)inds.Indices[d + 1]], vert[(int)inds.Indices[d + 2]]);
                        d++;
                    }
                }

                if (bindings != null)
                {
                    scene.AddSkinnedMesh(mesh, Matrix4x4.Identity, bindings.ToArray());
                }
                else
                {
                    scene.AddSkinnedMesh(mesh, Matrix4x4.Identity);
                }

            }

            if (bindings != null)
            {
                
                foreach (var anim in Model.File.RootChunk.GetChildren<Pure3D.Chunks.Animation>())
                {
                    LoadAnim(scene, bindings, anim);
                }
                
                
                foreach (var anim in MainWindow.AnimCache)
                {
                    LoadAnim(scene, bindings, anim);
                }
                
            }
        }

        static void LoadModel(SceneBuilder scene, string path, Pure3D.Chunks.Mesh Model, NodeBuilder targetNode = null)
        {
            List<Shader> Materials = new List<Shader>();
            List<int> ShaderCounts = new List<int>();
            int TotalShaders = 0;
            List<string> MaterialDupeCheck = new List<string>();
            List<int> MaterialIDs = new List<int>();

            List<PrimitiveGroup> prims = new List<PrimitiveGroup>();
            foreach (var item in Model.Children)
            {
                if (item is PrimitiveGroup prim)
                {
                    prims.Add(prim);
                }
            }

            for (int i = 0; i < Model.NumPrimGroups; i++)
            {
                var prim = prims[i];
                Shader Mat = Model.File.RootChunk.GetChildByName<Shader>(prim.ShaderName);
                ShaderCounts.Add(1);
                if (!MaterialDupeCheck.Contains(prim.ShaderName))
                {
                    Materials.Add(Mat);
                    TotalShaders += 1;
                    MaterialDupeCheck.Add(prim.ShaderName);
                    MaterialIDs.Add(Materials.Count - 1);
                }
                else
                {
                    MaterialIDs.Add(MaterialDupeCheck.IndexOf(prim.ShaderName));
                }
            }

            bool ExportInternalMaterials = true;
            List<MaterialBuilder> BMaterials = new();
            if (ExportInternalMaterials)
            {
                BMaterials = BuildMaterials(Materials, Model.File.RootChunk, path);
            }
            else
            {
                for (int i = 0; i < Materials.Count; i++)
                {
                    BMaterials.Add(new MaterialBuilder($"Mat{i}"));
                }
            }

            
            for (int i = 0; i < Model.NumPrimGroups; i++)
            {
                PrimitiveGroup Sub = prims[i];
                var primType = Sub.PrimitiveType;
                var inds = Sub.GetChild<IndexList>();
                var poslist = Sub.GetChild<PositionList>();
                var norlist = Sub.GetChild<NormalList>();
                var uvlist = Sub.GetChild<UVList>();
                var collist = Sub.GetChild<ColourList>();
                //var matlist = Sub.GetChild<MatrixList>();
                //var matpal = Sub.GetChild<MatrixPalette>();
                //var wgtlist = Sub.GetChild<WeightList>();
                var tanlist = Sub.GetChild<TangentList>();
                var isPlain = poslist != null;
                var hasColors = collist != null;
                var hasTangents = tanlist != null;

                int primCount = 3;
                if (primType == PrimitiveGroup.PrimitiveTypes.Points)
                {
                    primCount = 1;
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.LineList || primType == PrimitiveGroup.PrimitiveTypes.LineStrip)
                {
                    primCount = 2;
                }

                List<VertexData> verts = new List<VertexData>();
                for (int v = 0; v < Sub.NumVertices; v++)
                {
                    VertexData vd = new VertexData();
                    if (poslist != null)
                    {
                        vd.POS = poslist.Positions[v];
                    }
                    if (norlist != null)
                    {
                        vd.NOR = norlist.Normals[v];
                    }
                    if (tanlist != null)
                    {
                        vd.TAN = tanlist.Tangents[v];
                    }
                    if (uvlist != null)
                    {
                        vd.UV = uvlist.UVs[v];
                    }
                    if (collist != null)
                    {
                        vd.COL = new Vector4(collist.Colours[v].R / 255f, collist.Colours[v].G / 255f, collist.Colours[v].B / 255f, collist.Colours[v].A / 255f);
                    }
                    verts.Add(vd);
                }

                var vert = new List<IVertexBuilder>();
                IPrimitiveBuilder prim = null;
                IMeshBuilder<MaterialBuilder> mesh = null;
                if (hasColors)
                {
                    mesh = new MeshBuilder<VERTEXN, VERTEXC, VertexEmpty>($"mesh{i}");
                    prim = mesh.UsePrimitive(BMaterials[MaterialIDs[i]], primCount);
                    for (int d = 0; d < verts.Count; d++)
                    {
                        vert.Add(BuildVertexNC(verts[d]));
                    }
                }
                else
                {
                    mesh = new MeshBuilder<VERTEXN, VERTEXT, VertexEmpty>($"mesh{i}");
                    prim = mesh.UsePrimitive(BMaterials[MaterialIDs[i]], primCount);
                    for (int d = 0; d < verts.Count; d++)
                    {
                        vert.Add(BuildVertexNT(verts[d]));
                    }
                }

                if (primType == PrimitiveGroup.PrimitiveTypes.TriangleList)
                {
                    for (int d = 0; d < inds.Indices.Length - 2; d++)
                    {
                        var v1b = vert[(int)inds.Indices[d]];
                        d++;
                        var v2b = vert[(int)inds.Indices[d]];
                        d++;
                        var v3b = vert[(int)inds.Indices[d]];

                        prim.AddTriangle(v1b, v2b, v3b);
                    }
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.Points)
                {
                    // untested
                    for (int d = 0; d < inds.Indices.Length; d++)
                    {
                        prim.AddPoint(vert[(int)inds.Indices[d]]);
                    }
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.LineList)
                {
                    // untested
                    for (int d = 0; d < inds.Indices.Length - 1; d++)
                    {
                        var v1b = vert[(int)inds.Indices[d]];
                        d++;
                        var v2b = vert[(int)inds.Indices[d]];
                        prim.AddLine(v1b, v2b);
                    }
                }
                else if (primType == PrimitiveGroup.PrimitiveTypes.LineStrip)
                {
                    // untested, probably wrong
                    for (int d = 0; d < inds.Indices.Length - 1; d++)
                    {
                        var v1b = vert[(int)inds.Indices[d]];
                        d++;
                        var v2b = vert[(int)inds.Indices[d]];
                        prim.AddLine(v1b, v2b);
                    }
                }
                else
                {
                    //prim.AddTriangle(v[2], v[1], v[0]);
                    //prim.AddTriangle(v[1], v[2], v[3]);
                    for (int d = 1; d < inds.Indices.Length - 2; d++) // d = 3
                    {
                        prim.AddTriangle(vert[(int)inds.Indices[d + 0]], vert[(int)inds.Indices[d - 1]], vert[(int)inds.Indices[d + 1]]);
                        prim.AddTriangle(vert[(int)inds.Indices[d + 0]], vert[(int)inds.Indices[d + 1]], vert[(int)inds.Indices[d + 2]]);
                        d++;
                    }
                }

                if (targetNode != null)
                {
                    scene.AddRigidMesh(mesh, targetNode, Matrix4x4.Identity);
                }
                else
                {
                    scene.AddRigidMesh(mesh, Matrix4x4.Identity);
                }

            }

        }

        static void LoadDrawable(SceneBuilder scene, string path, Pure3D.Chunks.CompositeDrawableCTTR Model)
        {
            var skeleton = Model.File.RootChunk.GetChildByName<SkeletonCTTR>(Model.SkeletonName);
            var bindings = LoadSkeleton(scene, path, skeleton);

            List<CompositeDrawablePrimitive> prims = new List<CompositeDrawablePrimitive>();
            List<FrameController> frames = new List<FrameController>();
            foreach (var item in Model.Children)
            {
                if (item is CompositeDrawablePrimitive prim)
                {
                    prims.Add(prim);
                }
                else if (item is FrameController frame)
                {
                    frames.Add(frame);
                }
            }

            foreach (var prim in prims)
            {
                object chunk = null;
                foreach (var item in prim.File.RootChunk.Children)
                {
                    if (item is Named namedChunk && namedChunk.Name == prim.Name)
                    {
                        chunk = item;
                        break;
                    }
                }
                if (chunk == null) continue;
                if (chunk is Pure3D.Chunks.Skin skin)
                {
                    LoadSkin(scene, path, skin, bindings);
                }
                else if (chunk is Pure3D.Chunks.Mesh mesh)
                {
                    LoadModel(scene, path, mesh, bindings[(int)prim.UnkInt4]);
                }
                else
                {
                    // particles, etc.
                }
            }

            if (bindings != null)
            {
                
                foreach (var anim in Model.File.RootChunk.GetChildren<Pure3D.Chunks.Animation>())
                {
                    LoadAnim(scene, bindings, anim);
                }
                
                
                foreach (var anim in MainWindow.AnimCache)
                {
                    LoadAnim(scene, bindings, anim);
                }
                
                foreach (var frame in frames)
                {
                    var anim = Model.File.RootChunk.GetChildByName<Pure3D.Chunks.Animation>(frame.AnimName);
                    if (anim != null)
                    {
                        LoadAnim(scene, bindings, anim);
                    }
                }
            }
        }

        static List<NodeBuilder> LoadSkeleton(SceneBuilder scene, string path, Pure3D.Chunks.SkeletonCTTR skeleton)
        {
            var bindings = new List<NodeBuilder>();
            var skelnode = new NodeBuilder();
            if (skeleton != null)
            {
                var jnts = skeleton.GetChildren<SkeletonJointCTTR>();
                skelnode.Name = "Armature";
                skelnode.Name = jnts[0].Name;
                skelnode.LocalMatrix = jnts[0].RestPose;
                //skelnode.WithLocalScale(new Vector3(-1f, 1f, 1f));
                bindings.Add(skelnode);
                var skelroot = scene.AddNode(skelnode);
                if (skeleton.NumJoints == 1) return bindings;
                for (int i = 1; i < skeleton.NumJoints; i++)
                {
                    var item = jnts[i];
                    var bone = new NodeBuilder();
                    bone.Name = item.Name;
                    bone.LocalMatrix = item.RestPose;
                    bindings.Add(bone);
                }
                for (int i = 1; i < skeleton.NumJoints; i++)
                {
                    var item = jnts[i];
                    var bone = bindings[i];
                    if (item.SkeletonParent == 0)
                    {
                        skelnode.AddNode(bone);
                    }
                    else
                    {
                        bindings[(int)item.SkeletonParent].AddNode(bone);
                    }
                }
            }

            return bindings;
        }

        static void LoadAnim(SceneBuilder scene, List<NodeBuilder> bindings, Pure3D.Chunks.Animation animChunk)
        {
            float FrameStep = 1f / animChunk.FrameRate;
            AnimationGroupList groupList = animChunk.GetChild<AnimationGroupList>();
            string GroupName = animChunk.Name; // animation clip name

            foreach (var item in groupList.Children)
            {
                if (item is AnimationGroup group)
                {
                    var bone = bindings.Find(e => e.Name == group.Name);
                    if (bone == null) continue;

                    foreach (var chan in group.Children)
                    {
                        if (chan is Vector3Channel vec3chan)
                        {
                            List<(float, Vector3)> frames = new List<(float, Vector3)>();
                            for (int i = 0; i < vec3chan.NumberOfFrames; i++)
                            {
                                frames.Add((vec3chan.Frames[i] * FrameStep, vec3chan.Values[i]));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            if (vec3chan.Parameter == "TRAN")
                            {
                                bone.SetTranslationTrack(GroupName, sampler);
                            }
                            else if (vec3chan.Parameter.StartsWith("SCL"))
                            {
                                bone.SetScaleTrack(GroupName, sampler);
                            }
                        }
                        else if (chan is Vector2Channel vec2chan)
                        {
                            List<(float, Vector3)> frames = new List<(float, Vector3)>();
                            Vector3 frameConst = vec2chan.Constants;
                            for (int i = 0; i < vec2chan.NumberOfFrames; i++)
                            {
                                Vector3 frame = new Vector3(frameConst.X, frameConst.Y, frameConst.Z);
                                if (vec2chan.Mapping == 0)
                                {
                                    frame.Y = vec2chan.Values[i].X;
                                    frame.Z = vec2chan.Values[i].Y;
                                }
                                else if (vec2chan.Mapping == 1)
                                {
                                    frame.X = vec2chan.Values[i].X;
                                    frame.Z = vec2chan.Values[i].Y;
                                }
                                else
                                {
                                    frame.X = vec2chan.Values[i].X;
                                    frame.Y = vec2chan.Values[i].Y;
                                }
                                frames.Add((vec2chan.Frames[i] * FrameStep, frame));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            if (vec2chan.Parameter == "TRAN")
                            {
                                bone.SetTranslationTrack(GroupName, sampler);
                            }
                            else if (vec2chan.Parameter.StartsWith("SCL"))
                            {
                                bone.SetScaleTrack(GroupName, sampler);
                            }
                        }
                        else if (chan is Vector1Channel vec1chan)
                        {
                            List<(float, Vector3)> frames = new List<(float, Vector3)>();
                            Vector3 frameConst = vec1chan.Constants;
                            for (int i = 0; i < vec1chan.NumberOfFrames; i++)
                            {
                                Vector3 frame = new Vector3(frameConst.X, frameConst.Y, frameConst.Z);
                                if (vec1chan.Mapping == 0)
                                {
                                    frame.X = vec1chan.Values[i];
                                }
                                else if (vec1chan.Mapping == 1)
                                {
                                    frame.Y = vec1chan.Values[i];
                                }
                                else
                                {
                                    frame.Z = vec1chan.Values[i];
                                }
                                frames.Add((vec1chan.Frames[i] * FrameStep, frame));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            if (vec1chan.Parameter == "TRAN")
                            {
                                bone.SetTranslationTrack(GroupName, sampler);
                            }
                            else if (vec1chan.Parameter.StartsWith("SCL"))
                            {
                                bone.SetScaleTrack(GroupName, sampler);
                            }
                        }
                        else if (chan is QuaternionChannel2 quat2chan && quat2chan.Parameter == "ROT")
                        {
                            List<(float, Quaternion)> frames = new List<(float, Quaternion)>();
                            for (int i = 0; i < quat2chan.NumberOfFrames; i++)
                            {
                                float angX = (float)quat2chan.Values[i, 0] / short.MaxValue;
                                float angY = (float)quat2chan.Values[i, 1] / short.MaxValue;
                                float angZ = (float)quat2chan.Values[i, 2] / short.MaxValue;
                                float angW = (float)Math.Sqrt(1 - (angX * angX + angY * angY + angZ * angZ));
                                Quaternion quat = new Quaternion(angX, angY, angZ, angW);

                                frames.Add((quat2chan.Frames[i] * FrameStep, quat));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            bone.SetRotationTrack(GroupName, sampler);
                        }
                        else if (chan is QuaternionChannel quat1chan && quat1chan.Parameter == "ROT")
                        {
                            List<(float, Quaternion)> frames = new List<(float, Quaternion)>();
                            for (int i = 0; i < quat1chan.NumberOfFrames; i++)
                            {
                                frames.Add((quat1chan.Frames[i] * FrameStep, quat1chan.Values[i]));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            bone.SetRotationTrack(GroupName, sampler);
                        }
                        else if (chan is QuaternionChannel3 quat3chan && quat3chan.Parameter == "ROT")
                        {
                            List<(float, Quaternion)> frames = new List<(float, Quaternion)>();
                            for (int i = 0; i < quat3chan.NumberOfFrames; i++)
                            {
                                float angX = (float)quat3chan.Values[i, 0] / sbyte.MaxValue;
                                float angY = (float)quat3chan.Values[i, 1] / sbyte.MaxValue;
                                float angZ = (float)quat3chan.Values[i, 2] / sbyte.MaxValue;
                                float angW = (float)quat3chan.Values[i, 3] / sbyte.MaxValue;
                                Quaternion quat = new Quaternion(angX, angY, angZ, angW);

                                frames.Add((quat3chan.Frames[i] * FrameStep, quat));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            bone.SetRotationTrack(GroupName, sampler);
                        }
                        else if (chan is QuaternionChannel3 quat4chan && quat4chan.Parameter == "ROT")
                        {
                            List<(float, Quaternion)> frames = new List<(float, Quaternion)>();
                            for (int i = 0; i < quat4chan.NumberOfFrames; i++)
                            {
                                float angX = (float)quat4chan.Values[i, 0] / sbyte.MaxValue;
                                float angY = (float)quat4chan.Values[i, 1] / sbyte.MaxValue;
                                float angZ = (float)quat4chan.Values[i, 2] / sbyte.MaxValue;
                                float angW = (float)Math.Sqrt(1 - (angX * angX + angY * angY + angZ * angZ));
                                Quaternion quat = new Quaternion(angX, angY, angZ, angW);

                                frames.Add((quat4chan.Frames[i] * FrameStep, quat));
                            }
                            var sampler = CurveSampler.CreateSampler(frames);
                            bone.SetRotationTrack(GroupName, sampler);
                        }
                    }
                    
                }
            }

        }

        static VertexBuilder<VERTEXN, VERTEXC, VERTEXJ> BuildVertexNCJ(VertexData v)
        {
            VERTEXN vt = new VERTEXN(new Vector3(v.POS.X, v.POS.Y, v.POS.Z), new Vector3(v.NOR.X, v.NOR.Y, v.NOR.Z));
            VERTEXC vc = new VERTEXC(new Vector4(v.COL.X, v.COL.Y, v.COL.Z, v.COL.W), new Vector2(v.UV.X, -v.UV.Y));
            SparseWeight8 weights = SparseWeight8.Create(new Vector4(v.JNT[0], v.JNT[1], v.JNT[2], v.JNT[3]), new Vector4(v.WGT.X, v.WGT.Y, v.WGT.Z, 0));
            VERTEXJ vj = new VERTEXJ(weights);
            return new VertexBuilder<VERTEXN, VERTEXC, VERTEXJ>
                        (vt, vc, vj);
        }
        static VertexBuilder<VERTEXN, VERTEXT, VERTEXJ> BuildVertexNTJ(VertexData v)
        {
            VERTEXN vt = new VERTEXN(new Vector3(v.POS.X, v.POS.Y, v.POS.Z), new Vector3(v.NOR.X, v.NOR.Y, v.NOR.Z));
            VERTEXT vc = new VERTEXT(new Vector2(v.UV.X, -v.UV.Y));
            SparseWeight8 weights = SparseWeight8.Create(new Vector4(v.JNT[0], v.JNT[1], v.JNT[2], v.JNT[3]), new Vector4(v.WGT.X, v.WGT.Y, v.WGT.Z, 0));
            VERTEXJ vj = new VERTEXJ(weights);
            return new VertexBuilder<VERTEXN, VERTEXT, VERTEXJ>
                        (vt, vc, vj);
        }
        static VertexBuilder<VERTEXN, VERTEXC, VertexEmpty> BuildVertexNC(VertexData v)
        {
            VERTEXN vt = new VERTEXN(new Vector3(v.POS.X, v.POS.Y, v.POS.Z), new Vector3(v.NOR.X, v.NOR.Y, v.NOR.Z));
            return new VertexBuilder<VERTEXN, VERTEXC, VertexEmpty>
                        (vt, new VERTEXC(new Vector4(v.COL.X, v.COL.Y, v.COL.Z, v.COL.W), new Vector2(v.UV.X, -v.UV.Y)));
        }
        static VertexBuilder<VERTEXN, VERTEXT, VertexEmpty> BuildVertexNT(VertexData v)
        {
            VERTEXN vt = new VERTEXN(new Vector3(v.POS.X, v.POS.Y, v.POS.Z), new Vector3(v.NOR.X, v.NOR.Y, v.NOR.Z));
            return new VertexBuilder<VERTEXN, VERTEXT, VertexEmpty>
                        (vt, new VERTEXT(new Vector2(v.UV.X, -v.UV.Y)));
        }


        public static List<MaterialBuilder> BuildMaterials(List<Shader> Materials, Pure3D.Chunk root, string path)
        {
            List<MaterialBuilder> BMaterials = new();
            for (int i = 0; i < Materials.Count; i++)
            {
                var mat = Materials[i];
                var matbuild = new MaterialBuilder($"Mat_{i}");
                matbuild.WithMetallicRoughness(0f, 1f);
                matbuild.WithDoubleSide(true);

                foreach (var item in mat.Children)
                {
                    if (item is ShaderTextureParam texp)
                    {
                        Pure3D.Chunks.Texture tex = root.GetChildByName<Pure3D.Chunks.Texture>(texp.Value);
                        if (tex == null) continue; // todo external texture ref instead of internal
                        byte[] Colors = tex.OnImagePreview();
                        //System.IO.File.WriteAllBytes(outPath, Colors);
                        if (texp.Param == "TEX")
                        {
                            matbuild.WithChannelImage(KnownChannel.BaseColor, new MemoryImage(Colors));
                        }
                        else if (texp.Param == "REFL")
                        {
                            //matbuild.WithChannelImage(KnownChannel.Transmission, new MemoryImage(Colors));
                        }
                    }
                    else if (item is ShaderIntParam intp)
                    {
                        if (intp.Param == "ATST" && intp.Value != 0)
                        {
                            matbuild.WithAlpha(SharpGLTF.Materials.AlphaMode.MASK);
                        }
                        else if (intp.Param == "BLMD" && intp.Value != 0)
                        {
                            matbuild.WithAlpha(SharpGLTF.Materials.AlphaMode.BLEND);
                        }
                        else if (intp.Param == "2SID" && intp.Value != 0)
                        {
                            //matbuild.WithDoubleSide(true);
                        }
                        else if (intp.Param == "LIT" && intp.Value == 0)
                        {
                            matbuild.WithUnlitShader();
                        }
                    }
                    else if (item is ShaderColourParam colp)
                    {
                        if (colp.Param == "DIFF")
                        {
                            matbuild.WithBaseColor(new Vector4(colp.Color.R / 255f, colp.Color.G / 255f, colp.Color.B / 255f, colp.Color.A / 255f));
                        }
                    }
                }

                BMaterials.Add(matbuild);
            }
            return BMaterials;
        }

        public static Vector3 CalcNormal(Vector3 Vert1, Vector3 Vert2, Vector3 Vert3)
        {
            Vector3 u = Vert2 - Vert1;
            Vector3 v = Vert3 - Vert1;
            float nx = u.Y * v.Z - u.Z * v.Y;
            float ny = u.Z * v.X - u.X * v.Z;
            float nz = u.X * v.Y - u.Y * v.X;
            return new Vector3(nx, ny, nz);
        }

        public class VertexData
        {
            public Vector3 POS = new Vector3();
            public Vector3 NOR = new Vector3();
            public Vector3 TAN = new Vector3();
            public Vector2 UV = new Vector2();
            public Vector4 COL = new Vector4();
            public uint[] JNT = new uint[4];
            public Vector3 WGT = new Vector3();
        }
    }
}