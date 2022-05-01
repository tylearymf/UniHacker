using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace asardotnetasync
{

    public class AsarExtractor
    {

        public event EventHandler<AsarExtractEvent> FileExtracted;
        public event EventHandler<bool> Finished;



        public async Task<bool> Extract(AsarArchive archive, string path, string destination)
        {

            var pathArr = path.Split('/');

            var token = pathArr.Aggregate<string, JToken>(archive.Header.Json, (current, t) => current["files"][t]);

            var size = token.Value<int>("size");
            var offset = archive.BaseOffset + token.Value<int>("offset");

            var fileBytes = archive.Bytes.Skip(offset).Take(size).ToArray();

            //Write bytes to file TODO

            return true;
        }

        private List<AsarFile> _filesToExtract;
        private bool _emptyDir = false;

        public async Task<bool> ExtractAll(AsarArchive archive, string destination, bool emptyDir = false)
        {

            _filesToExtract = new List<AsarFile>();

            /* ENABLE FOR EMPTY FOLDERS (ONLY IF NEEDED) */
            _emptyDir = emptyDir;

            var jObject = archive.Header.Json;
            if (jObject.HasValues) TokenIterator(jObject.First);

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
                    var filePath = $"{destination}{asarFile.Path}";

                    await Utils.WriteToFile(fileBytes, filePath);

                    FileExtracted?.Invoke(this, new AsarExtractEvent(asarFile, progress, _filesToExtract.Count));
                }
                else
                {
                    if (_emptyDir)
                        $"{destination}{asarFile.Path}".CreateDir();
                }
            }

            Finished?.Invoke(this, true);

            return true;
        }

        private void TokenIterator(JToken token)
        {
            var property = token as JProperty;

            foreach (var jToken in property.Value.Children())
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
