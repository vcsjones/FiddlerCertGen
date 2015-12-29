using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;

namespace VCSJones.FiddlerCertGen
{
    public static class IA5StringEncoding
    {
        internal const byte IDENTIFIER = 0x16;
        public static byte[] Encode(string str)
        {
            var length = EncodeLengthTo7Bit(str.Length);
            var data = System.Text.Encoding.ASCII.GetBytes(str);
            var buffer = new byte[1 + length.Length + data.Length];
            buffer[0] = IDENTIFIER;
            Buffer.BlockCopy(length, 0, buffer, 1, length.Length);
            Buffer.BlockCopy(data, 0, buffer, length.Length + 1, data.Length);
            return buffer;
        }

        public static string Decode(byte[] data)
        {
            if (data.Length < 2 || data[0] != IDENTIFIER)
            {
                throw new ArgumentException("Data does not appear to be a string.", nameof(data));
            }
            var length = data[1];
            var multiByteLength = (length & 0x80) == 0x80;
            if (multiByteLength)
            {
                var byteCount = length & ~0x80;
                Console.WriteLine(byteCount);
                var lengthBuffer = new byte[byteCount];
                Buffer.BlockCopy(data, 2, lengthBuffer, 0, byteCount);
                Array.Reverse(lengthBuffer);
                var lengthValue = 0;
                for (var i = 0; i < byteCount; i++)
                {
                    lengthValue |= lengthBuffer[i] << (i * 8);
                }
                return System.Text.Encoding.ASCII.GetString(data, 2 + byteCount, lengthValue);
            }
            return System.Text.Encoding.ASCII.GetString(data, 2, length);
        }

        private static byte[] EncodeLengthTo7Bit(int length)
        {
            if (length <= 0x7F)
            {
                return new[] {(byte) length};
            }
            var octets = new List<byte>();
            while (length > 0)
            {
                octets.Insert(0, (byte) (length & 0xFF));
                length >>= 8;
            }
            var result = new byte[1 + octets.Count];
            result[0] = (byte) (0x80 | octets.Count);
            for (var i = 0; i < octets.Count; i++)
            {
                result[i + 1] = octets[i];
            }
            return result;
        }
    }
}