// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.SemanticKernel.Connectors.AI.OpenAI.Tokenizers2.Settings;

public static class EmbeddedResource
{
    // This is usually the assembly name, if the project follows the naming conventions about namespaces and assembly names
    private const string PrefixToIgnore = "Microsoft.SemanticKernel.Connectors.AI.OpenAI";

    private static readonly string s_namespace = typeof(EmbeddedResource).Namespace;

    public static Dictionary<byte[], int> LoadTokenBytePairEncoding(string dataSourceName)
    {
        var contents = ReadEmbeddedResourceAsLines(dataSourceName)
            .Where(line => !string.IsNullOrEmpty(line))
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToArray();

        return contents.ToDictionary(
            splitLine => Convert.FromBase64String(splitLine[0]),
            splitLine => int.Parse(splitLine[1], CultureInfo.InvariantCulture)
        );
    }

    private static IList<string> ReadEmbeddedResourceAsLines(string resourceName)
    {
        var content = ReadFile(resourceName);
        return content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    /// <summary>
    /// Read a content file embedded in the project. Files are read from disk,
    /// not from the assembly, to avoid inflating the assembly size.
    /// </summary>
    /// <param name="fileName">Filename to read</param>
    /// <returns>File content</returns>
    /// <exception cref="FileNotFoundException">Error in case the file doesn't exist</exception>
    private static string ReadFile(string fileName)
    {
        // Assume the class namespace matches the directory structure
        var currentClassDir = s_namespace
            .Replace(PrefixToIgnore, string.Empty)
            .Trim('.')
            .Replace('.', Path.DirectorySeparatorChar);

        // Check the execution assembly directory first
        var assembly1 = Assembly.GetExecutingAssembly();
        var assembly1Dir = Path.GetDirectoryName(Path.GetFullPath(assembly1.Location));

        // Concatenate assembly location with class namespace with file name
        var filePath1 = Path.Combine(assembly1Dir!, currentClassDir, fileName);
        if (File.Exists(filePath1))
        {
            return File.ReadAllText(filePath1);
        }

        // Check the current assembly, in case that's a different file on a different directory
        Assembly? assembly2 = Assembly.GetAssembly(typeof(EmbeddedResource));
        if (assembly2 == null)
        {
            throw new FileNotFoundException($"{fileName} not found, path: '{filePath1}'");
        }

        // Path where the assembly is
        var assembly2Dir = Path.GetDirectoryName(Path.GetFullPath(assembly2.Location));

        // No need to continue if the path is the same
        if (assembly2Dir == assembly1Dir)
        {
            throw new FileNotFoundException($"{fileName} not found, path: '{filePath1}'");
        }

        // Concatenate assembly location with class namespace with file name
        var filePath2 = Path.Combine(assembly2Dir!, currentClassDir, fileName);
        if (File.Exists(filePath2))
        {
            return File.ReadAllText(filePath2);
        }

        throw new FileNotFoundException($"{fileName} not found, paths: '{filePath1}', '{filePath2}'");
    }
}
