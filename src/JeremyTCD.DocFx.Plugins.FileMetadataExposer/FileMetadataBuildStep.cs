using Microsoft.DocAsCode.Plugins;
using System.Collections.Generic;
using System.Composition;
using System.Collections.Immutable;
using Microsoft.DocAsCode.Build.ConceptualDocuments;

namespace JeremyTCD.DocFx.Plugins.FileMetadataExposer
{
    [Export(nameof(ConceptualDocumentProcessor), typeof(IDocumentBuildStep))]
    public class FileMetadataExposer : IDocumentBuildStep
    {
        public int BuildOrder => 1000;

        public string Name => nameof(FileMetadataExposer);

        public void Build(FileModel model, IHostService host)
        {
            // Do nothing
        }

        public void Postbuild(ImmutableList<FileModel> models, IHostService host)
        {
            foreach (FileModel model in models)
            {
                IDictionary<string, object> content = model.Content as IDictionary<string, object>;
                if (content != null)
                {
                    IDictionary<string, object> manifestProperties = model.ManifestProperties as IDictionary<string, object>;

                    foreach (KeyValuePair<string, object> pair in content)
                    {
                        manifestProperties.Add(pair);
                    }
                }
            }

            return;
        }

        public IEnumerable<FileModel> Prebuild(ImmutableList<FileModel> models, IHostService host)
        {
            // Do nothing
            return models;
        }
    }
}
