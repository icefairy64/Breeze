using System;
using System.Collections.Generic;

namespace Breeze
{
    public struct ChunkAddress
    {
        public int X;
        public int Y;
    }

    public abstract class ChunkCollection<T>
    {
        public int ChunkSize { get; protected set; }
        protected Dictionary<ChunkAddress, LinkedList<T>> Chunks;

        protected ChunkCollection(int chunkSize)
        {
            ChunkSize = chunkSize;
            Chunks = new Dictionary<ChunkAddress, LinkedList<T>>();
        }

        public LinkedList<T> GetChunkAt(int x, int y)
        {
            ChunkAddress addr = new ChunkAddress() { X = x / ChunkSize, Y = y / ChunkSize };

            return Chunks.ContainsKey(addr) ? Chunks[addr] : null;
        }

        public abstract void ValidateChunkAt(int x, int y);

        public void InsertAt(T item, int x, int y)
        {
            ChunkAddress addr = new ChunkAddress() { X = x / ChunkSize, Y = y / ChunkSize };

            if (!Chunks.ContainsKey(addr))
                Chunks.Add(addr, new LinkedList<T>());

            Chunks[addr].AddLast(item);
        }
    }
}