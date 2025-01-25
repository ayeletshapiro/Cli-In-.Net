using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Html;
using IronWord;
using IronWord.Models;
using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace line
{
    public static class ReadTypesOfFiles
    {
        public static StringBuilder ReadHtmlFile(string file, bool clearLines)
        {
            var htmlDocument = new HTMLDocument(file);
            StringBuilder html = new StringBuilder();
            html.Append(htmlDocument.DocumentElement.OuterHTML);
            if (clearLines)
            {
                html = RemoveEmptyLinesFromText(html.ToString());
            }

            return html;

        }
        public static StringBuilder ReadDocxFile(string file, bool clearLines)
        {
            StringBuilder fileDocxContent = new StringBuilder();
            WordDocument doc = new WordDocument(file);
            foreach (IronWord.Models.Paragraph paragraph in doc.Paragraphs)
            {
                foreach (Text textRun in paragraph.Texts)
                {
                    fileDocxContent.Append(textRun.Text);
                }
                fileDocxContent.AppendLine();
            }

            if (clearLines)
            {
                fileDocxContent = RemoveEmptyLinesFromText(fileDocxContent.ToString());
            }
            return fileDocxContent;
        }
        public static StringBuilder ReadPdfFile(string file, bool clearLines)
        {
            using (iText.Kernel.Pdf.PdfReader reader = new iText.Kernel.Pdf.PdfReader(file))
            using (iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(reader))
            {
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    text.Append(iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)) + Environment.NewLine);
                }
                if (clearLines)
                {
                    text = RemoveEmptyLinesFromText(text.ToString());
                }
                return text;
            }
        }
        public static StringBuilder ReadRegularFile(string file, bool clearLines)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(File.ReadAllText(file, Encoding.UTF8));

            if (clearLines)
            {
                sb = RemoveEmptyLinesFromText(sb.ToString());
            }
            return sb;
        }

        public static void WriteRegularFile(string word)
        {
            using (StreamWriter writer = new StreamWriter("C:\\Users\\HOME\\Desktop\\לפרקטיקוד\\מסמך 2.txt"))
            {
                writer.WriteLine(word); // כותב את המילה לקובץ
                writer.Write(word);
                writer.Close();
            }
        }


        public static bool IsFullPath(string path)
        {
            // בודק אם המחרוזת מכילה תו ניתוב
            return path.Contains("\\") || path.Contains("/");
        }


        public static string GetRelativePath(string bundleFilePath, string filePath)
        {
            if (!System.IO.Path.IsPathRooted(bundleFilePath))
            {
                bundleFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), bundleFilePath);
            }

            if (!System.IO.Path.IsPathRooted(filePath))
            {
                filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), filePath);
            }

            // בדיקת קיום הקבצים
            if (!File.Exists(bundleFilePath))
            {
                throw new FileNotFoundException($"The bundle file '{bundleFilePath}' does not exist.");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The source file '{filePath}' does not exist.");
            }

            // חישוב הנתיב היחסי
            string relativePath = System.IO.Path.GetRelativePath(bundleFilePath, filePath);

            return relativePath;
        }
        // remove empty lines
        public static StringBuilder RemoveEmptyLinesFromText(string text)
        {
            string[] lines = text.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            StringBuilder result = new StringBuilder();

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    result.AppendLine(line);
                }
            }
            Console.WriteLine(result);
            return result;
        }


    }
}
