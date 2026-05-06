using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeSamples.CSharp
{
    public class ByteTools
    {
        /// <summary>
        /// Clones byte array
        /// </summary>
        public static byte[] Clone(byte[] array)
        {
            byte[] res = new byte[array.Length];
            Buffer.BlockCopy(array, 0, res, 0, array.Length);

            return res;
        }

        /// <summary>
        /// Concatenates n byte arrays. Null entries are skipped.
        /// </summary>
        public static byte[] Concatenate(params byte[][] arrays)
        {
            if (arrays == null || arrays.All(x => x == null))
                return null;

            byte[] res = new byte[arrays.Where(x => x != null).Sum(x => x.Length)];

            int offset = 0;
            foreach (byte[] array in arrays)
            {
                if (array == null)
                    continue;

                Buffer.BlockCopy(array, 0, res, offset, array.Length);
                offset += array.Length;
            }

            return res;
        }

        /// <summary>
        /// Converts a 16-bit int to a byte array in the requested byte order.
        /// </summary>
        public static byte[] ConvertIntToBytes(int n, bool littleEndian = true)
        {
            byte[] bytes = BitConverter.GetBytes((ushort)n);

            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        /// <summary>
        /// Converts a 32-bit int to a byte array in the requested byte order.
        /// </summary>
        public static byte[] ConvertBigIntToBytes(int n, bool littleEndian = true)
        {
            byte[] bytes = BitConverter.GetBytes(n);

            if (BitConverter.IsLittleEndian != littleEndian)
                Array.Reverse(bytes);

            return bytes;
        }

        /// <summary>
        /// Converts a byte array (in the specified byte order) to an int.
        /// </summary>
        public static int ConvertBytesToInt(byte[] array, bool littleEndian = true)
        {
            if (array == null)
                return 0;

            byte[] bytes;
            if (BitConverter.IsLittleEndian != littleEndian)
            {
                bytes = new byte[array.Length];
                array.CopyTo(bytes, 0);
                Array.Reverse(bytes);
            }
            else
            {
                bytes = array;
            }

            switch (bytes.Length)
            {
                case 1:
                    return bytes[0];
                case 2:
                    return BitConverter.ToInt16(bytes, 0);
                case 4:
                    return BitConverter.ToInt32(bytes, 0);
                default:
                    throw new ArgumentException("Size " + bytes.Length + " not handled for conversion to int.");
            }
        }

        /// <summary>
        /// Converts a byte array to an image
        /// </summary>
        public static Image ConvertBytesToImage(byte[] array)
        {
            try
            {
                using (var ms = new MemoryStream(array))
                {
                    return Image.FromStream(ms);
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Converts an image to a byte array
        /// </summary>
        public static byte[] ConvertImageToBytes(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Copies a range of bytes from the source array starting at the specified offset to the end.
        /// </summary>
        /// <param name="src">The source byte array.</param>
        /// <param name="offset">The zero-based offset at which to start copying.</param>
        /// <returns>A new byte array containing the copied range.</returns>
        public static byte[] CopyRange(byte[] src, int offset)
        {
            int length = src.Length - offset;
            byte[] dest = new byte[length];
            Buffer.BlockCopy(src, offset, dest, 0, length);
            return dest;
        }

        /// <summary>
        /// Copies a specified range of bytes from the source array.
        /// </summary>
        /// <param name="src">The source byte array.</param>
        /// <param name="start">The zero-based starting position in the source array.</param>
        /// <param name="end">The position in the source array at which to stop copying (exclusive).</param>
        /// <returns>A new byte array containing the specified range.</returns>
        public static byte[] CopyRange(byte[] src, int start, int end)
        {
            int length = end - start;
            byte[] dest = new byte[length];
            Buffer.BlockCopy(src, start, dest, 0, length);
            return dest;
        }

        /// <summary>
        /// Converts a byte to its unsigned integer representation.
        /// </summary>
        /// <param name="b">The byte to convert.</param>
        /// <returns>An integer representing the unsigned value of the byte.</returns>
        public static int UnsignedToByte(byte b)
        {
            return ((int)b) & 0xFF;
        }


        /// <summary>
        /// Returns true iff. the arrays are strictly equal, false otherwise
        /// </summary>
        public static bool AreEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
                return true;
            if (array1 == null || array2 == null)
                return false;

            return array1.Length == array2.Length && array1.SequenceEqual(array2);
        }

        /// <summary>
        /// Converts a byte array to an ASCII string (each byte mapped 1:1 to a char).
        /// </summary>
        /// <param name="array">The byte array to convert.</param>
        /// <returns>A string representation of the byte array, or null if the input is null.</returns>
        public static string ConvertByteArrayToString(byte[] array)
        {
            if (array == null)
                return null;

            return Encoding.ASCII.GetString(array);
        }

        /// <summary>
        /// Converts a string to an ASCII byte array (each char mapped 1:1 to a byte).
        /// </summary>
        /// <param name="text">The string to convert.</param>
        /// <returns>A byte array representing the string, or null if the input is null.</returns>
        public static byte[] ConvertStringToByteArray(string text)
        {
            if (text == null)
                return null;

            return Encoding.ASCII.GetBytes(text);
        }

        /// <summary>
        /// Trims null bytes from the end of a byte array.
        /// </summary>
        /// <param name="array">The byte array to trim.</param>
        /// <param name="addLastZero">If true, ensures the result ends with a zero byte.</param>
        /// <returns>A new byte array with trailing null bytes removed.</returns>
        public static byte[] TrimEndByteArray(byte[] array, bool addLastZero = false)
        {
            int lastNonZeroIndex = Array.FindLastIndex(array, b => b != 0);

            // all bytes are zero (or array is empty)
            if (lastNonZeroIndex < 0)
                return addLastZero ? new byte[1] : new byte[0];

            // already ends with non-zero — appending a zero is the only way to "include last zero"
            if (lastNonZeroIndex == array.Length - 1)
                addLastZero = false;

            if (addLastZero)
                Array.Resize(ref array, lastNonZeroIndex + 2); // keep one trailing zero
            else
                Array.Resize(ref array, lastNonZeroIndex + 1);

            return array;
        }

        /// <summary>
        /// Trims a byte array to the specified length.
        /// </summary>
        /// <param name="array">The byte array to trim.</param>
        /// <param name="index">The index at which to trim the array.</param>
        /// <returns>A new byte array with the specified length.</returns>
        public static byte[] TrimByteArrayToIndex(byte[] array, int index)
        {
            Array.Resize(ref array, index);

            return array;
        }
    }
}
