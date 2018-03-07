using Microsoft.DocAsCode.Plugins;
using System.Collections.Generic;
using System.Composition;
using System.Collections.Immutable;
using Microsoft.DocAsCode.Build.ConceptualDocuments;
using Microsoft.DocAsCode.Common;
using System.IO;

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
                if (model.Content is IDictionary<string, object> content)
                {
                    // Add relative output path
                    FileAndType fileAndType = model.FileAndType;
                    // Copied from DocFx's LinkPhaseHandler.ExportManifest
                    string pageRelPath = Path.ChangeExtension((RelativePath)fileAndType.DestinationDir + (((RelativePath)fileAndType.File) - (RelativePath)fileAndType.SourceDir), null);
                    content.Add("mimo_pageRelPath", pageRelPath);

                    // Copy all options to manifest properties so they are accessible in post processors
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
