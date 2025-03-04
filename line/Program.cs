﻿//line bundle
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.CommandLine;
using System.Text;
using line;
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
IronWord.License.LicenseKey = "YOUR-KEY-HERE";
//Create the options
var bundleLangOption = new Option<string>("--language", "list of languages of files to bundle")//Option --language
{
    IsRequired = true
};
bundleLangOption.AddAlias("-l");
var bundleOutputOption = new Option<string>(new[] { "--output", "-o" }, "the name of the bundle file of full path");//Option --output
var bundleNoteOption = new Option<bool>(new[] { "--note", "-n" }, "if to indicate the name of the origion file in the bundle file");//Option --note
var bundleSortOption = new Option<string>(new[] { "--sort", "-s" }, "sort the files in the bundle file acording to what the user prefiers");//Option --sort
var bundleAuthorOption = new Option<string>(new[] { "--author", "-a" }, "name to write in the up of the bundle file");//Option --author
var bundleCleanLinesOption = new Option<bool>(new[] { "--remove-empty-lines", "-rel" }, "removes empty lines from each file before copy it to the bundle");//Option --remove-empty-lines
//Create the root command
var bundleCommand = new Command("bundle", "bundle all code files into a single file");//the bundle command
bundleCommand.AddOption(bundleLangOption);
bundleCommand.AddOption(bundleOutputOption);
bundleCommand.AddOption(bundleNoteOption);
bundleCommand.AddOption(bundleSortOption);
bundleCommand.AddOption(bundleAuthorOption);
bundleCommand.AddOption(bundleCleanLinesOption);
bundleCommand.SetHandler((languagesFromUser, name, note, sort, author, cleanLines) =>
{
    if (name == null) // The option name was not inserted
    {
        name = "bundled file";
    }
    string lowerLanguagesFromUser = languagesFromUser.ToLower();//Low the letters in the languages option input
    List<string> languages = new List<string>();
    languages = lowerLanguagesFromUser.Split(',').ToList();//Make a list of languages from the languages input
    StringBuilder content = new StringBuilder();//Variable from  the content of the files
    try
    {
        string fileToRelate = "";// For the note option i create a temp file for the relative path
        bool deleteTempFile = false;
        string currentDirectory = Directory.GetCurrentDirectory();
        string folderName = Path.GetFileName(Directory.GetCurrentDirectory());
        if (folderName == "bin" || folderName == "Debug")
        {
            Console.WriteLine("this directory is not allowed to read files from");
            return;
        }
        string[] files = Directory.GetFiles(currentDirectory);

        if (files.Length == 0) // Only if the folder doesnwt have files to go over them
        {
            Console.WriteLine("the folder that was given is empty of files...");
            return;
        }
        if (note) // If the user is interested that the file origion will be written above the file content
        {
            if (ReadTypesOfFiles.IsFullPath(name)) // If the user chose a certain path for the bundle file
            {
                if (Path.Exists(Path.GetDirectoryName(name)))
                {
                    //Creating a temp file in the directory of the bundle file in order to compare the relative path
                    string directoryBundledFile = Path.GetDirectoryName(name);
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
        { Array.Sort(files); } // Sort the files by their names
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
                List<string> notFoundLang = new List<string>();// List for languages that weren't in the folder
                bool con = false;
                string fileType = Path.GetExtension(file);
                if (languages.Contains("c#") && fileType == ".cs")
                {
                    con = true;
                }
                if ((languages.Contains("angularjs") || languages.Contains("reactjs")) && fileType == ".js") { con = true; }
                if ((languages.Contains("angularts") || languages.Contains("reactts")) && fileType == ".ts") { con = true; }
                if (languages.Contains("word") && fileType == ".docx") { con = true; }
                if (languages.Contains("c++") && fileType == ".cpp") { con = true; }
                if (languages.Contains("python") && fileType == ".py") { con = true; }
                if (languages.Contains("javascript") && fileType == ".js") { con = true; }
                if (languages.Contains("typescript") && fileType == ".ts") { con = true; }

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

        if (deleteTempFile) { File.Delete(fileToRelate); }//If we created a temp file for the note option we need to delete it!

        if (content.Length > 0)
        {
            Document document = new Document(); //Create the bundle file
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
