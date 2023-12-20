using System;
using System.Collections.Generic;
using Pure3D;
using Twinsanity;

namespace CircusSetup
{
    public class GodotBinaryAudioStreamWAV : GodotBinaryResourceFile
    {

        public override string ResType => "AudioStreamWAV";

        public GodotBinaryAudioStreamWAV()
        {

        }

        public GodotBinaryAudioStreamWAV(RSD sfx, bool loop)
        {
            byte[] SoundData = new byte[0];
            switch (sfx.CodecString)
            {
                case "XADP": // XBOX IMA ADPCM
                    SoundData = IMA_ADPCM.IMA_Decoder.Decode(sfx.Data, (int)sfx.Channels);
                    break;
                case "XMA ": // XBOX 360 XMA todo
                    break;
                case "VAG ": // PS2/PSP VAG ADPCM
                    if (sfx.Channels == 4) // broken
                        SoundData = ADPCM.ToPCMQuad(sfx.Data, sfx.Data.Length, (int)sfx.Interleave);
                    if (sfx.Channels == 2)
                        SoundData = ADPCM.ToPCMStereo(sfx.Data, sfx.Data.Length, (int)sfx.Interleave);
                    else if (sfx.Channels == 1)
                        SoundData = ADPCM.ToPCMMono(sfx.Data, sfx.Data.Length);
                    break;
                case "AT3+": // PSP ATRAC3+ todo
                    break;
                default:
                    break;
            }
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("data", SoundData);
            res.Add("format", 1);
            if (sfx.Channels == 2)
            {
                res.Add("stereo", true);
            }
            if (loop)
            {
                res.Add("loop_mode", 1);
                if (sfx.Channels == 2)
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

        /*
        public GodotBinaryAudioStreamWAV(XWB.Sound sfx, bool loop)
        {
            byte[] SoundData = IMA_ADPCM.IMA_Decoder.Decode(sfx.SoundData, sfx.Channels);
            //byte[] SoundData = RIFF.SaveRiff(data, sfx.Channels, (int)sfx.SampleRate);
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("data", SoundData);
            res.Add("format", 1);
            if (sfx.Channels == 2)
            {
                res.Add("stereo", true);
            }
            if (loop)
            {
                res.Add("loop_mode", 1);
                if (sfx.Channels == 2)
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

        public GodotBinaryAudioStreamWAV(MusicHash.Track sfx, bool loop)
        {
            byte[] SoundData;
            if (sfx.Type == 1)
            {
                SoundData = ADPCM.ToPCMStereo(sfx.SoundData, sfx.SoundData.Length, sfx.inter);
            }
            else
            {
                SoundData = ADPCM.ToPCMMono(sfx.SoundData, sfx.SoundData.Length);
            }

            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("data", SoundData);
            res.Add("format", 1);
            if (sfx.Type == 1)
            {
                res.Add("stereo", true);
            }
            if (loop)
            {
                res.Add("loop_mode", 1);
                if (sfx.Type == 1)
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

        public GodotBinaryAudioStreamWAV(SoundEffectX sfx)
        {
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("data", sfx.SoundData);
            res.Add("format", 1);
            res.Add("mix_rate", sfx.Freq);
            Resources.Add(res);
        }

        public GodotBinaryAudioStreamWAV(SoundEffect sfx)
        {
            byte[] RawData = new byte[sfx.SoundSize];
            Array.Copy(sfx.Parent.ExtraData, sfx.SoundOffset, RawData, 0, sfx.SoundSize);
            byte[] SoundData = RIFF.SaveRiff(ADPCM.ToPCMMono(RawData, (int)sfx.SoundSize), 1, sfx.Freq);
            var res = new Resource(ResType, $"local://{ResType}_aaaaa");
            res.Add("data", SoundData);
            res.Add("format", 1);
            res.Add("mix_rate", sfx.Freq);
            Resources.Add(res);
        }
        */

    }
}