using System.Collections.Generic;
using System.IO;
namespace IMA_ADPCM
{
    public static class IMA_Decoder
    {
        class ImaAdpcmState
        {
            public int valprev;
            public int index;
        }

        static int[] StepTable = 
        {
            7, 8, 9, 10, 11, 12, 13, 14, 16, 17,
            19, 21, 23, 25, 28, 31, 34, 37, 41, 45,
            50, 55, 60, 66, 73, 80, 88, 97, 107, 118,
            130, 143, 157, 173, 190, 209, 230, 253, 279, 307,
            337, 371, 408, 449, 494, 544, 598, 658, 724, 796,
            876, 963, 1060, 1166, 1282, 1411, 1552, 1707, 1878, 2066,
            2272, 2499, 2749, 3024, 3327, 3660, 4026, 4428, 4871, 5358,
            5894, 6484, 7132, 7845, 8630, 9493, 10442, 11487, 12635, 13899,
            15289, 16818, 18500, 20350, 22385, 24623, 27086, 29794, 32767
        };

        static int[] IndexTable = 
        {
            -1, -1, -1, -1, 2, 4, 6, 8,
            -1, -1, -1, -1, 2, 4, 6, 8
        };

        // Credit to https://github.com/eurotools/es-xbox-adpcm-tool
        // Modified to support multichannel and fix quality issue
        public static byte[] Decode(byte[] ImaFileData, int channels, int channelpair)
        {
            byte[] outBuff;
            int sign;               /* Current adpcm sign bit */
            int delta;              /* Current adpcm output value */
            int step;               /* Stepsize */
            int valpred;            /* Predicted value */
            int vpdiff;             /* Current change to valpred */
            int index;              /* Current step change index */
            int inputbuffer;        /* place to keep next 4-bit value */

            int step2 = 0;
            int valpred2 = 0;
            int index2 = 0;

            int step3 = 0;
            int valpred3 = 0;
            int index3 = 0;

            int step4 = 0;
            int valpred4 = 0;
            int index4 = 0;

            int step5 = 0;
            int valpred5 = 0;
            int index5 = 0;

            int step6 = 0;
            int valpred6 = 0;
            int index6 = 0;

            int step7 = 0;
            int valpred7 = 0;
            int index7 = 0;
            
            int step8 = 0;
            int valpred8 = 0;
            int index8 = 0;

            using (BinaryReader BReader = new BinaryReader(new MemoryStream(ImaFileData)))
            using (MemoryStream pcmStream = new MemoryStream())
            using (BinaryWriter pcmWriter = new BinaryWriter(pcmStream))
            {
                ImaAdpcmState state = new ImaAdpcmState();
                ImaAdpcmState state2 = new ImaAdpcmState();
                ImaAdpcmState state3 = new ImaAdpcmState();
                ImaAdpcmState state4 = new ImaAdpcmState();
                ImaAdpcmState state5 = new ImaAdpcmState();
                ImaAdpcmState state6 = new ImaAdpcmState();
                ImaAdpcmState state7 = new ImaAdpcmState();
                ImaAdpcmState state8 = new ImaAdpcmState();

                while (BReader.BaseStream.Position < BReader.BaseStream.Length)
                {
                    valpred = BReader.ReadInt16();
                    index = BReader.ReadInt16();
                    step = StepTable[index];
                    List<short> vals1 = new List<short>();
                    List<short> vals2 = new List<short>();
                    List<short> vals3 = new List<short>();
                    List<short> vals4 = new List<short>();
                    List<short> vals5 = new List<short>();
                    List<short> vals6 = new List<short>();
                    List<short> vals7 = new List<short>();
                    List<short> vals8 = new List<short>();
                    if (channels >= 2)
                    {
                        valpred2 = BReader.ReadInt16();
                        index2 = BReader.ReadInt16();
                        step2 = StepTable[index2];
                    }
                    if (channels >= 4)
                    {
                        valpred3 = BReader.ReadInt16();
                        index3 = BReader.ReadInt16();
                        step3 = StepTable[index3];
                        valpred4 = BReader.ReadInt16();
                        index4 = BReader.ReadInt16();
                        step4 = StepTable[index4];
                    }
                    if (channels >= 6)
                    {
                        valpred5 = BReader.ReadInt16();
                        index5 = BReader.ReadInt16();
                        step5 = StepTable[index5];
                        valpred6 = BReader.ReadInt16();
                        index6 = BReader.ReadInt16();
                        step6 = StepTable[index6];
                    }
                    if (channels >= 8)
                    {
                        valpred7 = BReader.ReadInt16();
                        index7 = BReader.ReadInt16();
                        step7 = StepTable[index7];
                        valpred8 = BReader.ReadInt16();
                        index8 = BReader.ReadInt16();
                        step8 = StepTable[index8];
                    }

                    vals1.Add((short)valpred);
                    if (channels >= 2)
                    {
                        vals2.Add((short)valpred2);
                    }
                    if (channels >= 4)
                    {
                        vals3.Add((short)valpred3);
                        vals4.Add((short)valpred4);
                    }
                    if (channels >= 6)
                    {
                        vals5.Add((short)valpred5);
                        vals6.Add((short)valpred6);
                    }
                    if (channels >= 8)
                    {
                        vals7.Add((short)valpred7);
                        vals8.Add((short)valpred8);
                    }
                    for (int j = 0; j < 8; j++)
                    {
                        bool bufferstep = false;
                        for (int k = 0; k < 8; k++)
                        {
                            /* Step 1 - get the delta value */
                            inputbuffer = BReader.ReadByte();
                            BReader.BaseStream.Position -= 1;

                            if (bufferstep)
                            {
                                delta = (inputbuffer >> 4) & 0xf;
                                BReader.BaseStream.Position++;
                            }
                            else
                            {
                                delta = inputbuffer & 0xf;
                            }
                            bufferstep = !bufferstep;

                            /* Step 2 - Find new index value (for later) */
                            index += IndexTable[delta & 7];
                            if (index < 0) index = 0;
                            if (index > 88) index = 88;

                            /* Step 3 - Separate sign and magnitude */
                            sign = delta & 8;
                            delta = delta & 7;

                            /* Step 4 - Compute difference and new predicted value */
                            /*
                            ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                            ** in adpcm_coder.
                            */
                            vpdiff = step >> 3;
                            if ((delta & 4) != 0) vpdiff += step;
                            if ((delta & 2) != 0) vpdiff += step >> 1;
                            if ((delta & 1) != 0) vpdiff += step >> 2;

                            if (sign != 0)
                                valpred -= vpdiff;
                            else
                                valpred += vpdiff;

                            /* Step 5 - clamp output value */
                            if (valpred > short.MaxValue)
                                valpred = short.MaxValue;
                            else if (valpred < short.MinValue)
                                valpred = short.MinValue;

                            /* Step 6 - Update step value */
                            step = StepTable[index];

                            /* Step 7 - Output value */
                            //pcmWriter.Write((short)valpred);
                            if (j == 7 && k == 7)
                            {

                            }
                            else
                            {
                                vals1.Add((short)valpred);
                            }
                        }
                        state.valprev = valpred;
                        state.index = index;

                        if (channels >= 2)
                        {
                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index2 += IndexTable[delta & 7];
                                if (index2 < 0) index2 = 0;
                                if (index2 > 88) index2 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step2 >> 3;
                                if ((delta & 4) != 0) vpdiff += step2;
                                if ((delta & 2) != 0) vpdiff += step2 >> 1;
                                if ((delta & 1) != 0) vpdiff += step2 >> 2;

                                if (sign != 0)
                                    valpred2 -= vpdiff;
                                else
                                    valpred2 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred2 > short.MaxValue)
                                    valpred2 = short.MaxValue;
                                else if (valpred2 < short.MinValue)
                                    valpred2 = short.MinValue;

                                /* Step 6 - Update step value */
                                step2 = StepTable[index2];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals2.Add((short)valpred2);
                                }
                            }
                            state2.valprev = valpred2;
                            state2.index = index2;
                        }

                        if (channels >= 4)
                        {
                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index3 += IndexTable[delta & 7];
                                if (index3 < 0) index3 = 0;
                                if (index3 > 88) index3 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step3 >> 3;
                                if ((delta & 4) != 0) vpdiff += step3;
                                if ((delta & 2) != 0) vpdiff += step3 >> 1;
                                if ((delta & 1) != 0) vpdiff += step3 >> 2;

                                if (sign != 0)
                                    valpred3 -= vpdiff;
                                else
                                    valpred3 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred3 > short.MaxValue)
                                    valpred3 = short.MaxValue;
                                else if (valpred3 < short.MinValue)
                                    valpred3 = short.MinValue;

                                /* Step 6 - Update step value */
                                step3 = StepTable[index3];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals3.Add((short)valpred3);
                                }
                            }
                            state3.valprev = valpred3;
                            state3.index = index3;

                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index4 += IndexTable[delta & 7];
                                if (index4 < 0) index4 = 0;
                                if (index4 > 88) index4 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step4 >> 3;
                                if ((delta & 4) != 0) vpdiff += step4;
                                if ((delta & 2) != 0) vpdiff += step4 >> 1;
                                if ((delta & 1) != 0) vpdiff += step4 >> 2;

                                if (sign != 0)
                                    valpred4 -= vpdiff;
                                else
                                    valpred4 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred4 > short.MaxValue)
                                    valpred4 = short.MaxValue;
                                else if (valpred4 < short.MinValue)
                                    valpred4 = short.MinValue;

                                /* Step 6 - Update step value */
                                step4 = StepTable[index4];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals4.Add((short)valpred4);
                                }
                            }
                            state4.valprev = valpred4;
                            state4.index = index4;
                        }

                        if (channels >= 6)
                        {
                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index5 += IndexTable[delta & 7];
                                if (index5 < 0) index5 = 0;
                                if (index5 > 88) index5 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step5 >> 3;
                                if ((delta & 4) != 0) vpdiff += step5;
                                if ((delta & 2) != 0) vpdiff += step5 >> 1;
                                if ((delta & 1) != 0) vpdiff += step5 >> 2;

                                if (sign != 0)
                                    valpred5 -= vpdiff;
                                else
                                    valpred5 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred5 > short.MaxValue)
                                    valpred5 = short.MaxValue;
                                else if (valpred5 < short.MinValue)
                                    valpred5 = short.MinValue;

                                /* Step 6 - Update step value */
                                step5 = StepTable[index5];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals5.Add((short)valpred5);
                                }
                            }
                            state5.valprev = valpred5;
                            state5.index = index5;

                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index6 += IndexTable[delta & 7];
                                if (index6 < 0) index6 = 0;
                                if (index6 > 88) index6 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step6 >> 3;
                                if ((delta & 4) != 0) vpdiff += step6;
                                if ((delta & 2) != 0) vpdiff += step6 >> 1;
                                if ((delta & 1) != 0) vpdiff += step6 >> 2;

                                if (sign != 0)
                                    valpred6 -= vpdiff;
                                else
                                    valpred6 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred6 > short.MaxValue)
                                    valpred6 = short.MaxValue;
                                else if (valpred6 < short.MinValue)
                                    valpred6 = short.MinValue;

                                /* Step 6 - Update step value */
                                step6 = StepTable[index6];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals6.Add((short)valpred6);
                                }
                            }
                            state6.valprev = valpred6;
                            state6.index = index6;
                        }

                        if (channels >= 8)
                        {
                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index7 += IndexTable[delta & 7];
                                if (index7 < 0) index7 = 0;
                                if (index7 > 88) index7 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step7 >> 3;
                                if ((delta & 4) != 0) vpdiff += step7;
                                if ((delta & 2) != 0) vpdiff += step7 >> 1;
                                if ((delta & 1) != 0) vpdiff += step7 >> 2;

                                if (sign != 0)
                                    valpred7 -= vpdiff;
                                else
                                    valpred7 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred7 > short.MaxValue)
                                    valpred7 = short.MaxValue;
                                else if (valpred7 < short.MinValue)
                                    valpred7 = short.MinValue;

                                /* Step 6 - Update step value */
                                step7 = StepTable[index7];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals7.Add((short)valpred7);
                                }
                            }
                            state7.valprev = valpred7;
                            state7.index = index7;

                            bufferstep = false;
                            for (int k = 0; k < 8; k++)
                            {
                                /* Step 1 - get the delta value */
                                inputbuffer = BReader.ReadByte();
                                BReader.BaseStream.Position -= 1;

                                if (bufferstep)
                                {
                                    delta = (inputbuffer >> 4) & 0xf;
                                    BReader.BaseStream.Position++;
                                }
                                else
                                {
                                    delta = inputbuffer & 0xf;
                                }
                                bufferstep = !bufferstep;

                                /* Step 2 - Find new index value (for later) */
                                index8 += IndexTable[delta & 7];
                                if (index8 < 0) index8 = 0;
                                if (index8 > 88) index8 = 88;

                                /* Step 3 - Separate sign and magnitude */
                                sign = delta & 8;
                                delta = delta & 7;

                                /* Step 4 - Compute difference and new predicted value */
                                /*
                                ** Computes 'vpdiff = (delta+0.5)*step/4', but see comment
                                ** in adpcm_coder.
                                */
                                vpdiff = step8 >> 3;
                                if ((delta & 4) != 0) vpdiff += step8;
                                if ((delta & 2) != 0) vpdiff += step8 >> 1;
                                if ((delta & 1) != 0) vpdiff += step8 >> 2;

                                if (sign != 0)
                                    valpred8 -= vpdiff;
                                else
                                    valpred8 += vpdiff;

                                /* Step 5 - clamp output value */
                                if (valpred8 > short.MaxValue)
                                    valpred8 = short.MaxValue;
                                else if (valpred8 < short.MinValue)
                                    valpred8 = short.MinValue;

                                /* Step 6 - Update step value */
                                step8 = StepTable[index8];

                                /* Step 7 - Output value */
                                //pcmWriter.Write((short)valpred2);
                                if (j == 7 && k == 7)
                                {

                                }
                                else
                                {
                                    vals8.Add((short)valpred8);
                                }
                            }
                            state8.valprev = valpred8;
                            state8.index = index8;
                        }
                    }

                    if (channels < 4)
                    {
                        for (int i = 0; i < vals1.Count; i++)
                        {
                            pcmWriter.Write(vals1[i]);
                            if (vals2.Count != 0)
                            {
                                pcmWriter.Write(vals2[i]);
                            }
                            if (vals3.Count != 0)
                            {
                                pcmWriter.Write(vals3[i]);
                            }
                            if (vals4.Count != 0)
                            {
                                pcmWriter.Write(vals4[i]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < vals1.Count; i++)
                        {
                            if (channelpair == 1)
                            {
                                if (vals3.Count != 0)
                                {
                                    pcmWriter.Write(vals3[i]);
                                }
                                if (vals4.Count != 0)
                                {
                                    pcmWriter.Write(vals4[i]);
                                }
                            }
                            else if (channelpair == 2)
                            {
                                if (vals3.Count != 0)
                                {
                                    pcmWriter.Write(vals5[i]);
                                }
                                if (vals4.Count != 0)
                                {
                                    pcmWriter.Write(vals6[i]);
                                }
                            }
                            else if (channelpair == 3)
                            {
                                if (vals3.Count != 0)
                                {
                                    pcmWriter.Write(vals7[i]);
                                }
                                if (vals4.Count != 0)
                                {
                                    pcmWriter.Write(vals8[i]);
                                }
                            }
                            else
                            {
                                pcmWriter.Write(vals1[i]);
                                if (vals2.Count != 0)
                                {
                                    pcmWriter.Write(vals2[i]);
                                }
                            }
                        }
                    }

                }
                outBuff = pcmStream.ToArray();

                pcmWriter.Close();
                pcmStream.Close();
                BReader.Close();
            }
            return outBuff;
        }

    }
}