using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using Pure3D;
using Pure3D.Chunks;

namespace CircusSetup
{
    public static class ExportGodot
    {
        public static void ExportP3D(Root RootChunk, string path)
        {
            Stopwatch Timer = new Stopwatch();
            Timer.Start();

            foreach (var item in RootChunk.Children)
            {
                item.OnGodotExport(path);
            }

            Console.WriteLine($"END: {Timer.Elapsed}");
        }


        #region Constants

        public const bool ExportModelsAsResource = true; // set to false to export COLLADA
        public const bool ExportTexturesAsResource = true; // textures as resource can be loaded at runtime without compression or pre-processing, but loads longer
        public const bool ExportSoundsAsResource = true; // sounds as resource can be loaded at runtime without compression or pre-processing, but loads longer

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

        public static uint GetSequenceHashCode(this List<string> sequence)
        {
            Crc32 crc = new Crc32();
            const uint seed = 487;
            const uint modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current * modifier) + crc.Get(Encoding.ASCII.GetBytes(item)));
            }
        }

        public static uint GetSequenceHashCode(this List<Color> sequence)
        {
            Crc32 crc = new Crc32();
            const uint seed = 487;
            const uint modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current * modifier) + crc.Get(new byte[4] {item.R, item.G, item.B, item.A}) );
            }
        }

        public static uint GetSequenceHashCode(this byte[] sequence)
        {
            Crc32 crc = new Crc32();
            return crc.Get(sequence);
        }
#endregion

    }

#region CRC32
    /// <summary>
    /// Performs 32-bit reversed cyclic redundancy checks.
    /// </summary>
    public class Crc32
    {
#region Constants
        /// <summary>
        /// Generator polynomial (modulo 2) for the reversed CRC32 algorithm. 
        /// </summary>
        private const UInt32 s_generator = 0xEDB88320;
#endregion

#region Constructors
        /// <summary>
        /// Creates a new instance of the Crc32 class.
        /// </summary>
        public Crc32()
        {
            // Constructs the checksum lookup table. Used to optimize the checksum.
            m_checksumTable = Enumerable.Range(0, 256).Select(i =>
            {
                var tableEntry = (uint)i;
                for (var j = 0; j < 8; ++j)
                {
                    tableEntry = ((tableEntry & 1) != 0)
                        ? (s_generator ^ (tableEntry >> 1))
                        : (tableEntry >> 1);
                }
                return tableEntry;
            }).ToArray();
        }
#endregion

#region Methods
        /// <summary>
        /// Calculates the checksum of the byte stream.
        /// </summary>
        /// <param name="byteStream">The byte stream to calculate the checksum for.</param>
        /// <returns>A 32-bit reversed checksum.</returns>
        public UInt32 Get<T>(IEnumerable<T> byteStream)
        {
            try
            {
                // Initialize checksumRegister to 0xFFFFFFFF and calculate the checksum.
                return ~byteStream.Aggregate(0xFFFFFFFF, (checksumRegister, currentByte) =>
                          (m_checksumTable[(checksumRegister & 0xFF) ^ Convert.ToByte(currentByte)] ^ (checksumRegister >> 8)));
            }
            catch (FormatException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
            catch (InvalidCastException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
            catch (OverflowException e)
            {
                throw new Exception("Could not read the stream out as bytes.", e);
            }
        }
#endregion

#region Fields
        /// <summary>
        /// Contains a cache of calculated checksum chunks.
        /// </summary>
        private readonly UInt32[] m_checksumTable;

#endregion
    }
#endregion
}
