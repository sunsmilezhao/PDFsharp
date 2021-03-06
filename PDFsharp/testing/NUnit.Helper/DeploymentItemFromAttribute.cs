﻿using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NUnit.Helper
{
  [AttributeUsage(AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = false)]
  public class DeploymentItemFromAttribute : Attribute, IApplyToContext
  {
    private readonly string path;
    private readonly string outputDirectory;
    private readonly bool flattenOutput;

    public DeploymentItemFromAttribute(string path, string outputDirectory = null, bool flattenOutput = false)
    {
      this.path = path.Replace('/', '\\');
      this.outputDirectory = outputDirectory;
      this.flattenOutput = flattenOutput;
    }

    public void ApplyToContext(TestExecutionContext context)
    {
      var inputDir = TestContext.CurrentContext.TestDirectory;
      if (!path.StartsWith("@"))
      {
        throw new ArgumentException(path);
      }
      var inputAddionalPath = path.Substring(1);
      var firstMatch = inputAddionalPath.Split('\\')[0];
      while (true)
      {
        if (Path.GetFileName(inputDir) == firstMatch)
        {
          inputDir = Path.Combine(Path.GetDirectoryName(inputDir), inputAddionalPath);
          break;
        }
        else
        {
          inputDir = Path.GetDirectoryName(inputDir);
        }
      }
      var outputDir = TestContext.CurrentContext.WorkDirectory;
      if (outputDirectory != null)
      {
        outputDir = Path.Combine(outputDir, outputDirectory);
      }

      RecursiveCopy(inputDir, outputDir);
    }

    private void RecursiveCopy(string inputOne, string outputDir)
    {
      Directory.CreateDirectory(outputDir);
      if (File.Exists(inputOne))
      {
        File.Copy(
          inputOne,
          Path.Combine(outputDir, Path.GetFileName(inputOne)),
          true
        );
      }
      else if (Directory.Exists(inputOne))
      {
        foreach (var subOne in Directory.GetDirectories(inputOne))
        {
          RecursiveCopy(subOne, flattenOutput ? outputDir : Path.Combine(outputDir, Path.GetFileName(subOne)));
        }
        foreach (var subOne in Directory.GetFiles(inputOne))
        {
          RecursiveCopy(subOne, outputDir);
        }
      }
    }
  }
}
