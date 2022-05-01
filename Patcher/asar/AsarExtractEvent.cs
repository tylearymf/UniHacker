using System;

namespace asardotnetasync
{

    public class AsarExtractEvent : EventArgs
    {
        public AsarFile File { get; }
        public double Index { get; }
        public double Total { get; }
        public double Progress { get; }

        public AsarExtractEvent(AsarFile file, double index, double total)
        {
            File = file;
            Index = index;
            Total = total;

            Progress = Math.Round(index / total, 2);
        }
    }
}
