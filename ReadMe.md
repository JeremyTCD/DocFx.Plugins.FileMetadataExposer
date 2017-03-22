**This ReadMe is incomplete (work in progress)**  
# DocFx FileMetadataExposer
[![Build status](https://ci.appveyor.com/api/projects/status/3gt21k5ah72ae31p?svg=true)](https://ci.appveyor.com/project/JeremyTCD/docfx-plugins-filemetadataexposer)
<!--- Add test status once badge with logo is available https://github.com/badges/shields/pull/812 --->

A [DocFx](https://dotnet.github.io/docfx/) plugin.

#### Table of Contents  
[Summary](#summary)  
[Getting FileMetadataExposer](#getting-filemetadataexposer)  
[Usage](#usage)  
[Detailed Explanation](#detailed-explanation)  
[Building](#building)  

## Summary
FileMetadataExposer exposes per file metadata to post processors.

## Getting FileMetadataExposer
1. [Get the latest version of MSBuild](https://docs.microsoft.com/en-us/visualstudio/msbuild/what-s-new-in-msbuild-15-0) and add its location to your *path* environment variable. 
2. Add the following MSBuild file to the root folder of your template. Name it *plugins.proj*.
	```XML
    <Project Sdk="Microsoft.NET.Sdk"
     DefaultTargets="Restore;ResolveReferences;RemoveUnecessaryReferences;_CopyFilesMarkedCopyLocal">
	    <PropertyGroup>
		    <TargetFramework>net452</TargetFramework>
		    <Configuration>release</Configuration>
		    <OutDir>../plugins</OutDir>
	    </PropertyGroup>

	    <ItemGroup>
		    <PackageReference Include="JeremyTCD.DocFx.Plugins.FileMetadataExposer" Version="0.1.2-beta" />
	    </ItemGroup>

	    <!-- Including DocAsCode assemblies will cause errors -->
	    <Target Name="RemoveUnecessaryReferences">
		    <ItemGroup>
			    <ReferenceCopyLocalPaths Remove="@(ReferenceCopyLocalPaths)"
               Condition="$([System.Text.RegularExpressions.Regex]::IsMatch(%(Filename), '.*(?:DocAsCode).*'))" />
		    </ItemGroup>
		    <Message Text="Included assemblies:%0a @(ReferenceCopyLocalPaths, '%0a')" />
	    </Target>
    </Project>
	```
    Your file structure should look like this:
	```
	|-- MyTemplate
		|-- plugins.proj 
		|-- ...
	```
    **Note**: If you already have a *plugins.proj* in your root folder, add `<PackageReference Include="JeremyTCD.DocFx.Plugins.FileMetadataExposer" Version="0.1.2-beta" />` to the 
    `ItemGroup` element. You don't need more than 1 *plugins.proj*.
3. Run `msbuild plugins.proj` in the root folder of your template. MSBuild
    - downloads the FileMetadataExposer nuget package
    - creates a *plugins* folder if one doesn't already exist
    - copies the FileMetadataExposer assembly along with its references to the *plugins* folder. 
    
	Your file structure should look like this:
	```
	|-- MyTemplate
		|-- plugins
			|-- JeremyTCD.DocFx.Plugins.FileMetadataExposer.dll
			|-- ...
		|-- plugins.proj 
		|-- ...
	```
4. Eventually, you might need to update FileMetadataExposer. To do so, update `Version` in *plugins.proj* and run `msbuild plugins.proj` again.

## Usage


## Detailed Explanation
DocFx generates in-memory [models](https://dotnet.github.io/docfx/tutorial/intro_overwrite_files.html#data-model-inside-docfx)
for input files. Model properties can be added or overwritten using "[overwrite files](https://dotnet.github.io/docfx/tutorial/intro_overwrite_files.html#introduction)".
These are simply markdown files with yaml blocks. Each yaml block in an overwrite file must be preceded and succeeded by `---`. For example:  

```YAML
---
uid: targetModelUid
customProperty: customValue
---

# Your Markdown Header
...
```  

## Building

## License
[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/JeremyTCD/JeremyTCD.github.io/dev/License.txt)  
