using System;
using System.Collections.Generic;
using Pure3D.Chunks;
using System.Numerics;
using System.Linq;

namespace CircusSetup
{
    public class GodotBinaryAnimation : GodotBinaryResourceFile
    {

        public override string ResType => "Animation";

        public GodotBinaryAnimation()
        {

        }

        public GodotBinaryAnimation(Animation Anim)
        {
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("resource_name", Anim.Name);
            float FrameStep = 1f / Anim.FrameRate;
            uint AllFrames = (uint)Anim.NumberOfFrames;
            if (AllFrames == 0) return;
            res.Add("length", FrameStep * AllFrames);
            res.Add("step", FrameStep);
            if (Anim.Looping != 0)
            {
                res.Add("loop_mode", 1);
            }

            var groupList = Anim.GetChild<AnimationGroupList>();
            int track = 0;
            foreach (var item in groupList.Children)
            {
                if (item is AnimationGroup group)
                {
                    NodePath node = new NodePath(".", group.Name);

                    foreach (var chan in group.Children)
                    {
                        if (chan is Vector3Channel vec3chan)
                        {
                            List<float> frames = new List<float>();
                            for (int i = 0; i < vec3chan.NumberOfFrames; i++)
                            {
                                frames.Add(vec3chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(vec3chan.Values[i].X);
                                frames.Add(vec3chan.Values[i].Y);
                                frames.Add(vec3chan.Values[i].Z);
                            }

                            if (vec3chan.Parameter == "TRAN")
                            {
                                res.Add($"tracks/{track}/type", "position_3d");
                                res.Add($"tracks/{track}/path", node);
                                res.Add($"tracks/{track}/keys", frames.ToArray());
                                track++;
                            }
                            else if (vec3chan.Parameter.StartsWith("SCL"))
                            {
                                res.Add($"tracks/{track}/type", "scale_3d");
                                res.Add($"tracks/{track}/path", node);
                                res.Add($"tracks/{track}/keys", frames.ToArray());
                                track++;
                            }
                        }
                        else if (chan is Vector2Channel vec2chan)
                        {
                            List<float> frames = new List<float>();
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
                                frames.Add(vec2chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(frame.X);
                                frames.Add(frame.Y);
                                frames.Add(frame.Z);
                            }

                            if (vec2chan.Parameter == "TRAN")
                            {
                                res.Add($"tracks/{track}/type", "position_3d");
                                res.Add($"tracks/{track}/path", node);
                                res.Add($"tracks/{track}/keys", frames.ToArray());
                                track++;
                            }
                            else if (vec2chan.Parameter.StartsWith("SCL"))
                            {
                                res.Add($"tracks/{track}/type", "scale_3d");
                                res.Add($"tracks/{track}/path", node);
                                res.Add($"tracks/{track}/keys", frames.ToArray());
                                track++;
                            }
                        }
                        else if (chan is Vector1Channel vec1chan)
                        {
                            List<float> frames = new List<float>();
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
                                frames.Add(vec1chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(frame.X);
                                frames.Add(frame.Y);
                                frames.Add(frame.Z);
                            }

                            if (vec1chan.Parameter == "TRAN")
                            {
                                res.Add($"tracks/{track}/type", "position_3d");
                                res.Add($"tracks/{track}/path", node);
                                res.Add($"tracks/{track}/keys", frames.ToArray());
                                track++;
                            }
                            else if (vec1chan.Parameter.StartsWith("SCL"))
                            {
                                res.Add($"tracks/{track}/type", "scale_3d");
                                res.Add($"tracks/{track}/path", node);
                                res.Add($"tracks/{track}/keys", frames.ToArray());
                                track++;
                            }
                        }
                        else if (chan is QuaternionChannel2 quat2chan && quat2chan.Parameter == "ROT")
                        {
                            List<float> frames = new List<float>();
                            for (int i = 0; i < quat2chan.NumberOfFrames; i++)
                            {
                                /*
                                Quaternion quat = Quaternion.CreateFromYawPitchRoll(
                                    ((quat2chan.Values[i].Y / 32767f) - 1f) * (float)Math.PI,
                                    ((quat2chan.Values[i].X / 32767f) - 1f) * (float)Math.PI,
                                    ((quat2chan.Values[i].Z / 32767f) - 1f) * (float)Math.PI);
                                
                                Matrix4x4 matX = Matrix4x4.CreateRotationX(((quat2chan.Values[i].X / 32767f)) * (float)Math.PI);
                                Matrix4x4 matY = Matrix4x4.CreateRotationY(((quat2chan.Values[i].Y / 32767f)) * (float)Math.PI);
                                Matrix4x4 matZ = Matrix4x4.CreateRotationZ(((quat2chan.Values[i].Z / 32767f)) * (float)Math.PI);
                                */
                                /*
                                float angX = quat2chan.Values[i].X / (float)(ushort.MaxValue + 1) * (float)Math.PI * 2;
                                float angY = quat2chan.Values[i].Y / (float)(ushort.MaxValue + 1) * (float)Math.PI * 2;
                                float angZ = quat2chan.Values[i].Z / (float)(ushort.MaxValue + 1) * (float)Math.PI * 2;
                                Matrix4x4 matX = Matrix4x4.CreateRotationX(angX);
                                Matrix4x4 matY = Matrix4x4.CreateRotationY(angY);
                                Matrix4x4 matZ = Matrix4x4.CreateRotationZ(angZ);
                                Quaternion quat = Quaternion.CreateFromRotationMatrix(matX * matY * matZ);
                                float angX = (float)Math.PI * 0.5f * (quat2chan.Values[i, 0] / 32767f);
                                float angY = (float)Math.PI * 0.5f * (quat2chan.Values[i, 1] / 32767f);
                                float angZ = (float)Math.PI * 0.5f * (quat2chan.Values[i, 2] / 32767f);
                                */
                                //float angX = (4f * quat2chan.Values[i, 0] / 65535f) - 0.25f;
                                //float angY = (4f * quat2chan.Values[i, 1] / 65535f) - 0.25f;
                                //float angZ = (4f * quat2chan.Values[i, 2] / 65535f) - 0.25f;
                                //float angX = (float)Math.PI * 2f * (quat2chan.Values[i, 0] / 65535f);
                                //float angY = (float)Math.PI * 2f * (quat2chan.Values[i, 1] / 65535f);
                                //float angZ = (float)Math.PI * 2f * (quat2chan.Values[i, 2] / 65535f);
                                //Quaternion quat = Quaternion.CreateFromYawPitchRoll(angY, angX, angZ);
                                //Matrix4x4.Decompose(bone.LocalMatrix, out Vector3 sc, out Quaternion rot, out Vector3 tr);
                                //quat += rot;
                                //float angX = (float)Math.PI * -1f * ((quat2chan.Values[i, 0] / 32767f) + 1f);
                                //float angY = (float)Math.PI * 1f * ((quat2chan.Values[i, 1] / 32767f) + 1f);
                                //float angZ = (float)Math.PI * 1f * ((quat2chan.Values[i, 2] / 32767f) + 1f);
                                //float angX = (float)Math.PI * 1f * (quat2chan.Values[i, 0] / 32767f);
                                //float angY = (float)Math.PI * 1f * (quat2chan.Values[i, 1] / 32767f);
                                //float angZ = (float)Math.PI * 1f * (quat2chan.Values[i, 2] / 32767f);

                                //float angX = (float)Math.PI * 0.5f  * (quat2chan.Values[i, 0] / 32767f);
                                //float angY = (float)Math.PI * 0.5f  * (quat2chan.Values[i, 1] / 32767f);
                                //float angZ = (float)Math.PI * 0.5f  * (quat2chan.Values[i, 2] / 32767f);
                                //Quaternion quat = new Quaternion(angX, angY, angZ, 1f);

                                float angX = (float)Math.PI * 2f * (quat2chan.Values[i, 0] / 65535f);
                                float angY = (float)Math.PI * 2f * (quat2chan.Values[i, 1] / 65535f);
                                float angZ = (float)Math.PI * 2f * (quat2chan.Values[i, 2] / 65535f);
                                Quaternion quat = Quaternion.CreateFromYawPitchRoll(angY, angX, angZ);
                                //Quaternion quat = new Quaternion(angX, angY, angZ, 1f);
                                quat = Quaternion.Normalize(quat);
                                
                                //Matrix4x4 matX = Matrix4x4.CreateRotationX(angX);
                                //Matrix4x4 matY = Matrix4x4.CreateRotationY(angY);
                                //Matrix4x4 matZ = Matrix4x4.CreateRotationZ(angZ);
                                //Quaternion quat = Quaternion.CreateFromRotationMatrix(matX * matY * matZ);
                                //Matrix4x4.Decompose(bone.LocalMatrix, out var scale, out var rot, out var pos);

                                frames.Add(quat2chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(quat.X);
                                frames.Add(quat.Y);
                                frames.Add(quat.Z);
                                frames.Add(quat.W);
                            }

                            res.Add($"tracks/{track}/type", "rotation_3d");
                            res.Add($"tracks/{track}/path", node);
                            res.Add($"tracks/{track}/keys", frames.ToArray());
                            track++;
                        }
                        else if (chan is QuaternionChannel quat1chan && quat1chan.Parameter == "ROT")
                        {
                            List<float> frames = new List<float>();
                            for (int i = 0; i < quat1chan.NumberOfFrames; i++)
                            {
                                frames.Add(quat1chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(quat1chan.Values[i].X);
                                frames.Add(quat1chan.Values[i].Y);
                                frames.Add(quat1chan.Values[i].Z);
                                frames.Add(quat1chan.Values[i].W);
                            }

                            res.Add($"tracks/{track}/type", "rotation_3d");
                            res.Add($"tracks/{track}/path", node);
                            res.Add($"tracks/{track}/keys", frames.ToArray());
                            track++;
                        }
                        else if (chan is QuaternionChannel3 quat3chan && quat3chan.Parameter == "ROT")
                        {
                            List<float> frames = new List<float>();
                            for (int i = 0; i < quat3chan.NumberOfFrames; i++)
                            {
                                frames.Add(quat3chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(quat3chan.Values1[i] / 127f);
                                frames.Add(quat3chan.Values2[i] / 127f);
                                frames.Add(quat3chan.Values3[i] / 127f);
                                frames.Add(quat3chan.Values4[i] / 127f);
                            }

                            //res.Add($"tracks/{track}/type", "rotation_3d");
                            //res.Add($"tracks/{track}/path", node);
                            //res.Add($"tracks/{track}/keys", frames.ToArray());
                            //track++;
                        }
                        else if (chan is QuaternionChannel4 quat4chan && quat4chan.Parameter == "ROT")
                        {
                            List<float> frames = new List<float>();
                            for (int i = 0; i < quat4chan.NumberOfFrames; i++)
                            {
                                float angX = (float)Math.PI * 2f * (quat4chan.Values1[i] / 127f);
                                float angY = (float)Math.PI * 2f * (quat4chan.Values2[i] / 127f);
                                float angZ = (float)Math.PI * 2f * (quat4chan.Values3[i] / 127f);
                                Quaternion quat = Quaternion.CreateFromYawPitchRoll(angY, angX, angZ);
                                quat = Quaternion.Normalize(quat);

                                frames.Add(quat4chan.Frames[i] * FrameStep);
                                frames.Add(1f);
                                frames.Add(quat.X);
                                frames.Add(quat.Y);
                                frames.Add(quat.Z);
                                frames.Add(quat.W);
                            }

                            //res.Add($"tracks/{track}/type", "rotation_3d");
                            //res.Add($"tracks/{track}/path", node);
                            //res.Add($"tracks/{track}/keys", frames.ToArray());
                            //track++;
                        }
                    }
                    
                }
            }

            Resources.Add(res);
        }

        public GodotBinaryAnimation(SkeletonCTTR Skeleton)
        {
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("resource_name", $"{Skeleton.Name}_RESET");
            res.Add("length", 0.01f);

            var jnts = Skeleton.GetChildren<SkeletonJointCTTR>();
            
            int track = 0;
            for (int i = 0; i < jnts.Length; i++)
            {
                var item = jnts[i];
                NodePath node = new NodePath(".", item.Name);
                System.Numerics.Matrix4x4.Decompose(item.RestPose, out System.Numerics.Vector3 scale, out System.Numerics.Quaternion rot, out System.Numerics.Vector3 pos);
                List<float> frames1 = new List<float>();
                List<float> frames2 = new List<float>();
                List<float> frames3 = new List<float>();
                frames1.Add(0f);
                frames1.Add(1f);
                frames1.Add(pos.X);
                frames1.Add(pos.Y);
                frames1.Add(pos.Z);
                frames2.Add(0f);
                frames2.Add(1f);
                frames2.Add(rot.X);
                frames2.Add(rot.Y);
                frames2.Add(rot.Z);
                frames2.Add(rot.W);
                frames3.Add(0f);
                frames3.Add(1f);
                frames3.Add(scale.X);
                frames3.Add(scale.Y);
                frames3.Add(scale.Z);

                res.Add($"tracks/{track}/type", "position_3d");
                res.Add($"tracks/{track}/path", node);
                res.Add($"tracks/{track}/keys", frames1.ToArray());
                track++;
                res.Add($"tracks/{track}/type", "rotation_3d");
                res.Add($"tracks/{track}/path", node);
                res.Add($"tracks/{track}/keys", frames2.ToArray());
                track++;
                res.Add($"tracks/{track}/type", "scale_3d");
                res.Add($"tracks/{track}/path", node);
                res.Add($"tracks/{track}/keys", frames3.ToArray());
                track++;
            }

            Resources.Add(res);
        }


    }
}