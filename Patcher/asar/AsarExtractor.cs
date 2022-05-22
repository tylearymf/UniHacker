using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace asardotnetasync
{

    public class AsarExtractor
    {
#pragma warning disable CS8618
        public event EventHandler<AsarExtractEvent> FileExtracted;
        public event EventHandler<bool> Finished;

        private List<AsarFile> _filesToExtract;
        private bool _emptyDir = false;
#pragma warning restore CS8618

        public async Task<bool> ExtractAll(AsarArchive archive, string destination, bool emptyDir = false)
        {

            _filesToExtract = new List<AsarFile>();

            /* ENABLE FOR EMPTY FOLDERS (ONLY IF NEEDED) */
            _emptyDir = emptyDir;

            var jObject = archive.Header.Json;
#pragma warning disable CS8604
            if (jObject.HasValues) TokenIterator(jObject.First);
#pragma warning restore CS8604

            var bytes = archive.Bytes;

            var progress = 0;

            foreach (var asarFile in _filesToExtract)
            {
                progress++;
                var size = asarFile.Size;
                var offset = archive.BaseOffset + asarFile.Offset;

                if (size > -1)
                {
                    var fileBytes = new byte[size];

                    Buffer.BlockCopy(bytes, offset, fileBytes, 0, size);
                    var filePath = Path.Combine(destination, asarFile.Path);

                    await Utils.WriteToFile(fileBytes, filePath);

                    FileExtracted?.Invoke(this, new AsarExtractEvent(asarFile, progress, _filesToExtract.Count));
                }
                else
                {
                    if (_emptyDir)
                        Path.Combine(destination, asarFile.Path).CreateDir();
                }
            }

            Finished?.Invoke(this, true);

            return true;
        }

        private void TokenIterator(JToken token)
        {
            var property = token as JProperty;

#pragma warning disable CS8602
            foreach (var jToken in property.Value.Children())
#pragma warning restore CS8602
            {
                var prop = (JProperty)jToken;
                var size = -1;
                var offset = -1;
                foreach (var jToken1 in prop.Value.Children())
                {
                    var nextProp = (JProperty)jToken1;
                    if (nextProp.Name == "files")
                    {
                        /* ENABLE FOR EMPTY FOLDERS (ONLY IF NEEDED) */
                        if (_emptyDir)
                        {
                            Console.WriteLine($"PROP PATH: {prop.Path}");
                            var afile = new AsarFile(prop.Path, "", size, offset);
                            _filesToExtract.Add(afile);
                        }

                        TokenIterator(nextProp);
                    }
                    else
                    {
                        if (nextProp.Name == "size") size = int.Parse(nextProp.Value.ToString());
                        else if (nextProp.Name == "offset") offset = int.Parse(nextProp.Value.ToString());
                    }
                }

                if (size <= -1 || offset <= -1) continue;
                {
                    var afile = new AsarFile(prop.Path, prop.Name, size, offset);
                    _filesToExtract.Add(afile);
                }
            }
        }

    }
}
