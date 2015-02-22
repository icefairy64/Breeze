using System;
using System.Collections.Generic;

namespace Breeze.Graphics
{
    public class DrawableChunkCollection : ChunkCollection<Drawable>
    {
        public DrawableChunkCollection(int chunkSize)
            : base(chunkSize)
        {
        }

        public override void ValidateChunkAt(int x, int y)
        {
            ChunkAddress addr = new ChunkAddress() { X = x / ChunkSize, Y = y / ChunkSize };

            if (!Chunks.ContainsKey(addr))
                return;

            var toRemove = new LinkedList<Drawable>();
            var chunk = Chunks[addr];

            foreach (Drawable e in chunk)
            {
                if (!addr.Equals(new ChunkAddress() { X = (int)e.X / ChunkSize, Y = (int)e.Y / ChunkSize }))
                {
                    Console.WriteLine("Addresses are not equal: {0}:{1} and {2}:{3}", addr.X, addr.Y, (int)e.X / ChunkSize, (int)e.Y / ChunkSize);
                    toRemove.AddLast(e);
                }
            }

            foreach (Drawable e in toRemove)
            {
                Console.WriteLine("Moving drawable");
                chunk.Remove(e);
                InsertAt(e, (int)e.X, (int)e.Y);
            }
        }
    }
}

