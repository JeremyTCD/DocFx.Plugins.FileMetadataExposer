using Microsoft.DocAsCode.Plugins;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace JeremyTCD.DocFx.Plugins.FileMetadataExposer.Tests
{
    public class FileMetadataBuildStepTests
    {
        [Fact]
        public void PostBuild_CopiesFileModelContentToFileModelManifestProperties()
        {
            // Arrange
            const string metadata = "metadata";
            const string value = "value";

            FileAndType dummyFileAndType = new FileAndType("/dummyBaseDir", "dummyFile", DocumentType.Article);
            FileModel fileModel = new FileModel(dummyFileAndType, null)
            {
                Content = new Dictionary<string, object>
                {
                    { metadata, value }
                }
            };
            ImmutableList<FileModel> models = ImmutableList.Create(new FileModel[]{
                fileModel
            });

            FileMetadataExposer fileMetadataExposer = new FileMetadataExposer();

            // Act
            fileMetadataExposer.Postbuild(models, null);

            // Assert
            Assert.True((models[0].ManifestProperties as IDictionary<string, object>).ContainsKey(metadata));
            Assert.Equal(value, (models[0].ManifestProperties as IDictionary<string, object>)[metadata]);
        }

        [Fact]
        public void PostBuild_IgnoresFilesModelsWithNonDictionaryContent()
        {
            // Arrange
            FileAndType dummyFileAndType = new FileAndType("/dummyBaseDir", "dummyFile", DocumentType.Article);
            FileModel fileModel = new FileModel(dummyFileAndType, null)
            {
                Content = null
            };
            ImmutableList<FileModel> models = ImmutableList.Create(new FileModel[]{
                fileModel
            });

            FileMetadataExposer fileMetadataExposer = new FileMetadataExposer();

            // Act
            fileMetadataExposer.Postbuild(models, null);

            // Assert
            Assert.Equal(0, (models[0].ManifestProperties as IDictionary<string, object>).Keys.Count);
        }
    }
}