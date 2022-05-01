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
            return (false, Language.GetString("non_unity"));
        }
    }
}
