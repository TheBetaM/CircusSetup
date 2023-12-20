namespace Pure3D
{

    public struct ByteColour
    {
        public ByteColour(byte x, byte y, byte z, byte w)
        {
            R = x;
            G = y;
            B = z;
            A = w;
        }

        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public override string ToString()
        {
            return $"R: {R}; G: {G}; B: {B}; A: {A}";
        }
    }

    public struct Topology
    {
        public ushort V0;
        public ushort V1;
        public ushort V2;
        public ushort N0;
        public ushort N1;
        public ushort N2;

        public Topology(ushort v0, ushort v1, ushort v2, ushort n0, ushort n1, ushort n2)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            N0 = n0;
            N1 = n1;
            N2 = n2;
        }

        public override string ToString()
        {
            return $"{V0}; {V1}; {V2}; {N0}; {N1}; {N2}; ";
        }
    }
}
