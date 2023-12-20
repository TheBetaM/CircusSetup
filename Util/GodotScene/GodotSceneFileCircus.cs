using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace CircusSetup
{
    public class GodotSceneFileCircus : GodotSceneFile
    {
        public static GodotSceneFileCircus Create(string Name, int InstanceID = -1, string targetType = ExportGodot.Node3D)
        {
            GodotSceneFileCircus ModelScene = new GodotSceneFileCircus();
            Node RootNode = new Node(Name, targetType);
            RootNode.InstanceID = InstanceID;
            ModelScene.Nodes.Add(RootNode);
            return ModelScene;
        }

        
    }
}
