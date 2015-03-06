using System;
using System.Text;

namespace Net.Common.Text
{
	public static class StringCrc32
	{
		static StringCrc32()
		{
			Crc32HashTable = InitializeHashTable();
		}

		public static uint CalculateHash(string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value);

			var hash = uint.MaxValue;
			foreach (var num in bytes)
			{
				var tableIndex = (byte)(hash ^ num);
				hash = Crc32HashTable[tableIndex] ^ ((hash >> SizeOfByteInBits) & LowThreeBytesMask);
			}

			var hashFinal = BitConverter.GetBytes(~hash);
			if (!BitConverter.IsLittleEndian)
			{
				Array.Reverse(hashFinal);
			}

			return BitConverter.ToUInt32(hashFinal, 0);
		}

		private static uint[] InitializeHashTable()
		{
			var hashTable = new uint[256];
			for (uint index = 0; index < hashTable.Length; index++)
			{
				var value = index;
				for (var bit = 0; bit < SizeOfByteInBits; bit++)
				{
					if ((value & 1) == 1)
					{
						value = (value >> 1) ^ DefaultPolynomial;
					}
					else
					{
						value >>= 1;
					}
				}

				hashTable[index] = value;
			}

			return hashTable;
		}

		private static readonly uint[] Crc32HashTable;
		private const uint DefaultPolynomial = 0xedb88320;
		private const int SizeOfByteInBits = 8;
		private const uint LowThreeBytesMask = 0xffffff;
	}
}
