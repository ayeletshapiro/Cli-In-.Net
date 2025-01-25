//line bundle
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.CommandLine;
using System.Text;
using Syncfusion.OfficeChart.Implementation;
using System.Linq;
using line;
using System.IO;

//Register the encoding provider at the start of your Main method
//System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider);

using SixLabors.Fonts;
using System.Text.Unicode;
using System;
using Google.Protobuf.WellKnownTypes;
using BitMiracle.LibTiff.Classic;
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
IronWord.License.LicenseKey = "YOUR-KEY-HERE";

var bundleLangOption = new Option<string>("--language", "list of languages of files to bundle")//option --language
{
    IsRequired = true
};
bundleLangOption.AddAlias("-l");
var bundleOutputOption = new Option<string>(new[] { "--output", "-o" }, "the name of the bundle file of full path");//option --output
var bundleNoteOption = new Option<bool>(new[] { "--note", "-n" }, "if to indicate the name of the origion file in the bundle file");//option --output
var bundleSortOption = new Option<string>(new[] { "--sort", "-s" }, "sort the files in the bundle file acording to what the user prefiers");//option --output
var bundleAuthorOption = new Option<string>(new[] { "--author", "-a" }, "name to write in the up of the bundle file");//option --output
var bundleCleanLinesOption = new Option<bool>(new[] { "--remove-empty-lines", "-rel" }, "removes empty lines from each file before copy it to the bundle");//option --output
var bundleCommand = new Command("bundle", "bundle all code files into a single file");//the bundle command
bundleCommand.AddOption(bundleLangOption);
bundleCommand.AddOption(bundleOutputOption);
bundleCommand.AddOption(bundleNoteOption);
bundleCommand.AddOption(bundleSortOption);
bundleCommand.AddOption(bundleAuthorOption);
bundleCommand.AddOption(bundleCleanLinesOption);
bundleCommand.SetHandler((languagesFromUser, name, note, sort, author, cleanLines) =>
{
    if (name == null)
    {
        name = "bundled file";
    }
    string lowerLanguagesFromUser = languagesFromUser.ToLower();//low the letters in the languages input
    List<string> languages = new List<string>();
    languages = lowerLanguagesFromUser.Split(',').ToList();//make a list of languages from the languages input
    StringBuilder content = new StringBuilder();//Variable from  the content of the files
    try
    {
        string fileToRelate = "";// for the note option i create a temp file for the relative path
        bool deleteTempFile = false;
        string currentDirectory = Directory.GetCurrentDirectory();
        string folderName = Path.GetFileName(Directory.GetCurrentDirectory());
        if (folderName == "bin" || folderName == "Debug")
        {
            Console.WriteLine("this directory is not allowed to read files from");
            return;
        }
        string[] files = Directory.GetFiles(currentDirectory);

        if (files.Length == 0)//רק אם יש קבצים לעבור עליהם
        {
            Console.WriteLine("the folder that was given is empty of files...");
            return;
        }
        if (note)
        {
            if (ReadTypesOfFiles.IsFullPath(name))
            {
                Console.WriteLine();
                Console.WriteLine("The string is a full path.");
                if (Path.Exists(Path.GetDirectoryName(name)))
                {

                    string directoryBundledFile = Path.GetDirectoryName(name);
                    Console.WriteLine(directoryBundledFile);
                    string tFile = "tempFile.txt"; // Specify the file path
                    string contentTempFile = "Hello, this is a new temp text file."; // Content to write
                    // Create and write to the file
                    File.WriteAllText(Path.Combine(directoryBundledFile, tFile), contentTempFile);
                    fileToRelate = Path.Combine(directoryBundledFile, tFile);//the temp file for the note option
                    Console.WriteLine(fileToRelate);
                    deleteTempFile = true;
                }
            }
            else
            {
                fileToRelate = files[0];
            }
        }

        if (sort != null && sort.Equals("type"))
        {
            Array.Sort(files, (file1, file2) =>
                String.Compare(System.IO.Path.GetExtension(file1), System.IO.Path.GetExtension(file2)));
        }
        else
        { Array.Sort(files); }
        if (lowerLanguagesFromUser.ToLower().Equals("all"))
        {
            foreach (var file in files)
            {
                string fileType = Path.GetExtension(file);//Thy type of the file
               

                if (fileType == ".docx" || fileType == ".doc")
                {
                    if (note)
                    {
                        content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                    }
                    content.Append(ReadTypesOfFiles.ReadDocxFile(file, cleanLines));
                }

                else if (fileType == ".xlsx")
                {
                    if (note)
                    {
                        content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                    }
                    content.Append(File.ReadAllText(file, Encoding.UTF8));// Read the content of excel file and append it to the content variable
                }
                else if (fileType == ".pdf")
                {
                    if (note)
                    {
                        content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                    }
                    content.Append(ReadTypesOfFiles.ReadPdfFile(file, cleanLines));// Read the content of pdf file and append it to the content variable
                }
                else if (fileType == ".html")
                {
                    if (note)
                    {
                        content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                    }
                    content.Append(ReadTypesOfFiles.ReadHtmlFile(file, cleanLines));
                }
                else
                {
                    if (note)
                    {
                        content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                    }
                    content.Append(ReadTypesOfFiles.ReadRegularFile(file, cleanLines));// Read the content of any other file and append it to the content variable
                }
                content.AppendLine();
            }
            Console.WriteLine(content);
        }
        else
        {

            foreach (var file in files)
            {
                bool con = false;
                string fileType = Path.GetExtension(file);
                if (languages.Contains("c#") && fileType == ".cs")
                {
                    con = true;
                }
                if ((languages.Contains("angularjs") || languages.Contains("reactjs")) && fileType == ".js") { con = true; }
                if ((languages.Contains("angularts") || languages.Contains("reactts")) && fileType == ".ts") { con = true; }
                if (languages.Contains("word") && fileType == ".docx") { con = true; }

                if ((con == true) || (languages.Contains(fileType.Substring(1))))
                {
                    if (fileType == ".html")
                    {
                        if (note)
                        {
                            content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                        }
                        content.Append(ReadTypesOfFiles.ReadHtmlFile(file,cleanLines));
                        
                    }
                    else if (fileType == ".docx")
                    {
                        if (note)
                        {
                            content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                        }
                        content.Append(ReadTypesOfFiles.ReadDocxFile(file ,cleanLines));
                    }
                    else
                    {
                        if (note)
                        {
                            content.Append($"\t the file origion:{Relative(fileToRelate, file)}\n");
                        }
                        content.Append(ReadTypesOfFiles.ReadRegularFile(file,cleanLines));
                    }
                    content.AppendLine();

                }


            }
        }

        if (deleteTempFile) { File.Delete(fileToRelate); }//if we created a temp file for the note option we need to delete it!

        if (content.Length > 0)
        {
            Document document = new Document();
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "David.ttf");
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream($"{name}.pdf", FileMode.Create));
            document.Open();
            BaseFont titleFontBase = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font titleFont = new iTextSharp.text.Font(titleFontBase, 18f, iTextSharp.text.Font.BOLD); // גודל פונט 18
            BaseFont besadFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font fontBesad = new iTextSharp.text.Font(besadFont, 12f, iTextSharp.text.Font.BOLDITALIC);
            BaseFont basedFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(basedFont);

            // הוספת כותרת
            string title = $"Name: {author}";
            string besad = "Besiata Dishmaya";
            iTextSharp.text.Paragraph titleParagraph = new iTextSharp.text.Paragraph(title, titleFont)
            {
                Alignment = Element.ALIGN_CENTER // מיקום במרכז
            };
            iTextSharp.text.Paragraph besadParagraph = new iTextSharp.text.Paragraph(besad, fontBesad)
            {
                Alignment = Element.ALIGN_TOP // מיקום למעלה
            };
            // הוספת הכותרת למסמך
            document.Add(besadParagraph);
            if (author != null)//if the user want a name in the top of the bundle file
            {
                document.Add(titleParagraph);
            }

            // הגדרת פונט לתוכן

            // הוספת תוכן
            iTextSharp.text.Paragraph paragraphPage = new iTextSharp.text.Paragraph(content.ToString(), font);
            document.Add(paragraphPage);
            document.Close();
            Console.WriteLine("The Bundled File Created Successfully!!");
        }
        else
        {
            Console.WriteLine($"File of type {languages[0]} isn't found");
        }
    }

    catch (System.IO.DirectoryNotFoundException)
    {
        Console.WriteLine("path is not exist!");

    }
    catch (System.IO.IOException)
    {
        Console.WriteLine("the document is empty you cant open it");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
    finally
    {

    }

}, bundleLangOption, bundleOutputOption, bundleNoteOption, bundleSortOption, bundleAuthorOption, bundleCleanLinesOption);
var rootCommand = new RootCommand("root command for a bundler cli file");
rootCommand.AddCommand(bundleCommand);
rootCommand.InvokeAsync(args);

static string Relative(string path1, string path2)
{
    return System.IO.Path.GetRelativePath(path1, path2)=="."?$"..\\{Path.GetFileName(path2)}": System.IO.Path.GetRelativePath(path1, path2);
}
#region Hebrew Reverse

//static string ReverseHebrewText(string input)
//{
//    // Define the Unicode ranges for Hebrew characters
//    Regex regex = new Regex(@"\p{IsHebrew}");

//    // Find all Hebrew text in the input string
//    MatchCollection matches = regex.Matches(input);

//    // Reverse each Hebrew text found in the input string
//    foreach (Match match in matches)
//    {
//        char[] charArray = match.Value.ToCharArray();
//        Array.Reverse(charArray);
//        string reversedHebrew = new string(charArray);

//        // Replace the original Hebrew text with the reversed Hebrew text in the input string
//        input = input.Replace(match.Value, reversedHebrew);
//    }

//    return input;
//}
#endregion Hebrew Reverse

#region Hebrew Reverse

//static string ReverseHebrewText(string input)
//{
//    // Define the Unicode ranges for Hebrew characters
//    Regex regex = new Regex(@"\p{IsHebrew}");

//    // Find all Hebrew text in the input string
//    MatchCollection matches = regex.Matches(input);

//    // Reverse each Hebrew text found in the input string
//    foreach (Match match in matches)
//    {
//        char[] charArray = match.Value.ToCharArray();
//        Array.Reverse(charArray);
//        string reversedHebrew = new string(charArray);

//        // Replace the original Hebrew text with the reversed Hebrew text in the input string
//        input = input.Replace(match.Value, reversedHebrew);
//    }

//    return input;
//}
#endregion Hebrew Reverse












