using System;
using Neo.IronLua;

namespace Breeze.Resources
{
    [Serializable]
    public class LuaScript : Resource
    {
        protected LuaChunk Chunk;

        public LuaScript(string filename)
            : base(filename)
        {
            Chunk = Core.LuaEngine.CompileChunk(filename, Core.LuaCompileOptions);
        }

        public LuaResult Execute(params object[] args)
        {
            return Core.LuaEnvironment.DoChunk(Chunk, args);
        }

        public override void Free()
        {
            // Do nothing
        }
    }
}

