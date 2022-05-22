using System;
using System.Threading.Tasks;

namespace UniHacker
{
    internal class DefaultPatcher : Patcher
    {
        public DefaultPatcher(string filePath) : base(filePath)
        {
        }

        public override async Task<(bool success, string errorMsg)> ApplyPatch(Action<double> progress)
        {
            await Task.Yield();
            return (false, Language.GetString("Non_unity"));
        }
    }
}
