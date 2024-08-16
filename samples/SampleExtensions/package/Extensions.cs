using System;
using System.Collections.Generic;

namespace CriWare
{
    public static class NativeStructExtensions
    {
        /// <summary>
        /// PCMデータの取得
        /// </summary>
        /// <param name="arg">PCMデータ格納オブジェクト</param>
        /// <param name="channel">取得するチャンネル</param>
        /// <returns>PCMデータ</returns>
        public unsafe static Span<float> GetFloatPcm<T>(this T arg, int channel) where T : Interfaces.IPcmData {
            if(arg.format != CriAtom.PcmFormat.Float32) throw new InvalidOperationException();
            if(channel < 0) throw new ArgumentOutOfRangeException(nameof(channel));
            if(channel >= arg.numChannels) throw new ArgumentOutOfRangeException(nameof(channel));
            return new Span<float>(((float**)(IntPtr*)arg.data)[channel], arg.numSamples);
        }

        /// <inheritdoc cref="GetFloatPcm"/>
        public unsafe static Span<Int16> GetShortPcm<T>(this T arg, int channel) where T : Interfaces.IPcmData {
            if(arg.format != CriAtom.PcmFormat.Sint16) throw new InvalidOperationException();
            if(channel < 0) throw new ArgumentOutOfRangeException(nameof(channel));
            if(channel >= arg.numChannels) throw new ArgumentOutOfRangeException(nameof(channel));
            return new Span<Int16>(((Int16**)(IntPtr*)arg.data)[channel], arg.numSamples);
        }
    }
}
