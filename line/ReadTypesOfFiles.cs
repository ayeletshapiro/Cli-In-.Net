using System.Text;
using Aspose.Html;
using IronWord;
using IronWord.Models;

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

        public static bool IsFullPath(string path)
        {
            // בודק אם המחרוזת מכילה תו ניתוב
            return path.Contains("\\") || path.Contains("/");
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
