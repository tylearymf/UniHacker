/*
 *  asar.net Async Copyright (c) 2015 Jiiks | http://jiiks.net
 * 
 *  https://github.com/Jiiks/asar.net
 * 
 *  For: https://github.com/atom/asar
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 * */

using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace asardotnetasync
{
    public class AsarArchive
    {

        private const int SIZE_UINT = 4;
        private const int SIZE_LONG = 2 * SIZE_UINT;
        private const int SIZE_INFO = 2 * SIZE_LONG;

        public int BaseOffset { get; }
        private readonly byte[] _bytes;
        public byte[] Bytes => _bytes;
        public string FilePath { get; }
        public AsarHeader Header { get; }

        public AsarArchive(string filepath)
        {
            FilePath = filepath;

            if (!File.Exists(FilePath)) throw new AsarException(EAsarException.ASAR_FILE_CANT_FIND);

            try
            {
                _bytes = File.ReadAllBytes(FilePath);
            }
            catch (Exception ex)
            {
                throw new AsarException(EAsarException.ASAR_FILE_CANT_READ, ex.ToString());
            }

            try
            {
                Header = ReadHeader(ref _bytes);
                BaseOffset = Header.Length;
            }
            catch (Exception _ex)
            {
#pragma warning disable CA2200
                throw _ex;
#pragma warning restore CA2200
            }
        }

        private static AsarHeader ReadHeader(ref byte[] bytes)
        {

            var headerInfo = bytes.Take(SIZE_INFO).ToArray();

            if (headerInfo.Length < SIZE_INFO) throw new AsarException(EAsarException.ASAR_INVALID_FILE_SIZE);

            var asarFileDescriptor = headerInfo.Take(SIZE_LONG).ToArray();
            var asarPayloadSize = asarFileDescriptor.Take(SIZE_UINT).ToArray();

            var payloadSize = BitConverter.ToInt32(asarPayloadSize, 0);
            var payloadOffset = asarFileDescriptor.Length - payloadSize;

            if (payloadSize != SIZE_UINT && payloadSize != SIZE_LONG) throw new AsarException(EAsarException.ASAR_INVALID_DESCRIPTOR);

            var asarHeaderLength = asarFileDescriptor.Skip(payloadOffset).Take(SIZE_UINT).ToArray();

            var headerLength = BitConverter.ToInt32(asarHeaderLength, 0);

            var asarFileHeader = headerInfo.Skip(SIZE_LONG).Take(SIZE_LONG).ToArray();
            var asarHeaderPayloadSize = asarFileHeader.Take(SIZE_UINT).ToArray();

            var headerPayloadSize = BitConverter.ToInt32(asarHeaderPayloadSize, 0);
            var headerPayloadOffset = headerLength - headerPayloadSize;

            var dataTableLength = asarFileHeader.Skip(headerPayloadOffset).Take(SIZE_UINT).ToArray();
            var dataTableSize = BitConverter.ToInt32(dataTableLength, 0);

            var hdata = bytes.Skip(SIZE_INFO).Take(dataTableSize).ToArray();

            if (hdata.Length != dataTableSize) throw new AsarException(EAsarException.ASAR_INVALID_FILE_SIZE);

            var asarDataOffset = asarFileDescriptor.Length + headerLength;

            var jObject = JObject.Parse(System.Text.Encoding.Default.GetString(hdata));

            return new AsarHeader(headerInfo, asarDataOffset, hdata, jObject);
        }
    }
}
