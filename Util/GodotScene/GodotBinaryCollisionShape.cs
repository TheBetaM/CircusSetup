using System;
using System.Collections.Generic;
using Pure3D.Chunks;
using System.Numerics;
using System.Linq;

namespace CircusSetup
{
    public class GodotBinaryCollisionShape : GodotBinaryResourceFile
    {

        public override string ResType => "ConcavePolygonShape3D";

        public GodotBinaryCollisionShape()
        {

        }

        public GodotBinaryCollisionShape(FenceHeader fence)
        {
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            var poslist = fence.GetChild<FencePositionList>();
            var posind = fence.GetChild<FencePositionPalette>();
            var posArray = new List<Vector3>();
            for (int i = 0; i < posind.Indices.Count; i++)
            {
                posArray.Add(poslist.Positions[posind.Indices[i]]);
            }
            res.Add("data", posArray.ToArray());
            res.Add("backface_collision", true);
            Resources.Add(res);
        }
    }
}