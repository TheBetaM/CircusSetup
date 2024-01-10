using System;
using System.Collections.Generic;
using Pure3D;

namespace CircusSetup
{
    public class GodotBinaryAudioStreamWAV : GodotBinaryResourceFile
    {

        public override string ResType => "AudioStreamWAV";

        public GodotBinaryAudioStreamWAV()
        {

        }

        public GodotBinaryAudioStreamWAV(RSD sfx, bool loop, int channelpair)
        {
            byte[] SoundData = new byte[0];
            switch (sfx.CodecString)
            {
                case "XADP": // XBOX IMA ADPCM
                    SoundData = IMA_ADPCM.IMA_Decoder.Decode(sfx.Data, (int)sfx.Channels, channelpair);
                    break;
                case "XMA ": // XBOX 360 XMA
                    SoundData = XMA_Audio.XMA_Decoder.Decode(sfx.Data, (int)sfx.Channels, channelpair);
                    break;
                case "VAG ": // PS2/PSP VAG ADPCM
                    if (sfx.Channels >= 4)
                        SoundData = ADPCM.ToPCMQuad(sfx.Data, sfx.Data.Length, (int)sfx.Interleave, channelpair, sfx.Channels);
                    if (sfx.Channels == 2)
                        SoundData = ADPCM.ToPCMStereo(sfx.Data, sfx.Data.Length, (int)sfx.Interleave);
                    else if (sfx.Channels == 1)
                        SoundData = ADPCM.ToPCMMono(sfx.Data, sfx.Data.Length);
                    break;
                case "AT3+": // PSP ATRAC3+
                    SoundData = AT3Plus.AT3P_Decoder.Decode(sfx.Data, (int)sfx.Channels, channelpair);
                    break;
                case "RADP": // GCN/WII IMA ADPCM
                    break;
                case "WADP": // WII NGC DSP
                    break;
                default:
                    break;
            }
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("data", SoundData);
            res.Add("format", 1);
            if (sfx.Channels >= 2)
            {
                res.Add("stereo", true);
            }
            if (loop)
            {
                res.Add("loop_mode", 1);
                if (sfx.Channels >= 2)
                {
                    res.Add("loop_begin", 16);
                    res.Add("loop_end", (int)(SoundData.Length / 4));
                }
                else
                {
                    res.Add("loop_begin", 32);
                    res.Add("loop_end", (int)(SoundData.Length / 2));
                }
            }
            res.Add("mix_rate", sfx.SampleRate);
            Resources.Add(res);
        }
        
    }
}