﻿using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Helper;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Controls;
using PdfSharp.Pdf;
using PdfSharp.Xps.UnitTests.Helpers;
using PdfSharp.Xps.XpsModel;
using PdfSharp.Xps.Rendering;
using IOPath = System.IO.Path;
using Path = System.IO.Path;

namespace PdfSharp.Xps.UnitTests.Typography
{
  using PdfSharp.Internal;

  /// <summary>
  /// Summary description for TestExample
  /// </summary>
  [GotoWorkDirectory]
  public class RenderingTypography : TestBase
  {

    public TestContext TestContext => TestContext.CurrentContext;

    [SetUp]
    public void TestInitialize()
    {
    }

    [TearDown]
    public void TestCleanup()
    {
    }

    [Test]
    [DeploymentItemFrom("@testing/SampleXpsDocuments_1_0", "SampleXpsDocuments_1_0")]
    public void TestRenderingTypographySamples()
    {
#if true
      string path = "SampleXpsDocuments_1_0/MXDW";
      string dir = (path);
      if (dir == null)
      {
        Assert.Inconclusive("Path not found: " + path + ". Follow instructions in ../../../SampleXpsDocuments_1_0/!readme.txt to download samples from the Internet.");
        return;
      }
      if (!Directory.Exists(dir))
      {
        Assert.Inconclusive("Path not found: " + path + ". Follow instructions in ../../../SampleXpsDocuments_1_0/!readme.txt to download samples from the Internet.");
        return;
      }

      string[] files = Directory.GetFiles(dir, "*Poster.xps", SearchOption.TopDirectoryOnly);

      if (files.Length == 0)
      {
        Assert.Inconclusive("No sample file found.");
        return;
      }

      foreach (string filename in files)
      {
        //if (!filename.EndsWith("CalibriPoster.xps"))
        //  continue;

        Debug.WriteLine(filename);
        try
        {
          using (XpsDocument xpsDoc = XpsDocument.Open(filename))
          {
            int docIndex = 0;
            foreach (FixedDocument doc in xpsDoc.Documents)
            {
              PdfDocument pdfDocument = new PdfDocument();
              //PdfRenderer renderer = new PdfRenderer();
              XpsConverter converter = new XpsConverter(pdfDocument, xpsDoc);

              int pageIndex = 0;
              foreach (FixedPage page in doc.Pages)
              {
                Debug.WriteLine(String.Format("  doc={0}, page={1}", docIndex, pageIndex));

                // HACK: API is senseless
                PdfPage pdfPage = converter.CreatePage(pageIndex);
                converter.RenderPage(pdfPage, pageIndex);
                pageIndex++;
              }

              string pdfFilename = IOPath.GetFileNameWithoutExtension(filename);
              if (docIndex != 0)
                pdfFilename += docIndex.ToString();
              pdfFilename += ".pdf";
              pdfFilename = IOPath.Combine(IOPath.GetDirectoryName(filename), pdfFilename);

              pdfDocument.Save(pdfFilename);
              docIndex++;
            }
          }
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.Message);
          GetType();
        }
      }
#else
      //string data = "2.11592697149066,169.466971230985 7.31945717924454E-09,161.067961604689";
      //TokenizerHelper helper = new TokenizerHelper(data);
      //string t = helper.NextTokenRequired();
      //t = helper.NextTokenRequired();
      //t = helper.NextTokenRequired();
      //t = helper.NextTokenRequired();

      string[] files = Directory.GetFiles("../../../../../testing/PdfSharp.Xps.UnitTests/Typography", "*.xps", SearchOption.AllDirectories);

      //files = Directory.GetFiles(@"G:\!StLa\PDFsharp-1.10\OpenSource\PDFsharp\WPF\SampleXpsDocuments_1_0\FontPoster", "*.xps", SearchOption.AllDirectories);

      if (files.Length == 0)
        throw new Exception("No sample file found.");

      foreach (string filename in files)
      {
        // No negative tests here
        if (filename.Contains("\\ConformanceViolations\\"))
          continue;

        //if (!filename.EndsWith("CalibriPoster.xps"))
        //  continue;

        Debug.WriteLine(filename);
        try
        {
#if true

          XpsDocument xpsDoc = XpsDocument.Open(filename);

          int docIndex = 0;
          foreach (FixedDocument doc in xpsDoc.Documents)
          {
            PdfDocument pdfDocument = new PdfDocument();
            //PdfRenderer renderer = new PdfRenderer();
            XpsConverter converter = new XpsConverter(pdfDocument, xpsDoc);

            int pageIndex = 0;
            foreach (FixedPage page in doc.Pages)
            {
              Debug.WriteLine(String.Format("  doc={0}, page={1}", docIndex, pageIndex));

              // HACK: API is senseless
              PdfPage pdfPage = converter.CreatePage(pageIndex);
              converter.RenderPage(pdfPage, pageIndex);
              pageIndex++;
            }

            string pdfFilename = IOPath.GetFileNameWithoutExtension(filename);
            if (docIndex != 0)
              pdfFilename += docIndex.ToString();
            pdfFilename += ".pdf";
            pdfFilename = IOPath.Combine(IOPath.GetDirectoryName(filename), pdfFilename);

            pdfDocument.Save(pdfFilename);
            docIndex++;
          }

#else
          int docIndex = 0;
          XpsDocument xpsDoc = XpsDocument.Open(filename);
          foreach (FixedDocument doc in xpsDoc.Documents)
          {
            PdfDocument pdfDoc = new PdfDocument();
            PdfRenderer renderer = new PdfRenderer();

            int pageIndex = 0;
            foreach (FixedPage page in doc.Pages)
            {
              Debug.WriteLine(String.Format("  doc={0}, page={1}", docIndex, pageIndex));
              PdfPage pdfPage = renderer.CreatePage(pdfDoc, page);
              renderer.RenderPage(pdfPage, page);
              pageIndex++;
            }

            string pdfFilename = IOPath.GetFileNameWithoutExtension(filename);
            if (docIndex != 0)
              pdfFilename += docIndex.ToString();
            pdfFilename += ".pdf";
            pdfFilename = IOPath.Combine(IOPath.GetDirectoryName(filename), pdfFilename);

            pdfDoc.Save(pdfFilename);
            docIndex++;
          }
#endif
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.Message);
          GetType();
        }
      }
#endif
    }
  }
}