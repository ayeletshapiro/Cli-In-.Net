CLI Files Bundler
Overview
The CLI Files Bundler is a lightweight and efficient command-line tool designed to simplify code management by combining multiple source files into a single, organized output file. It’s particularly useful for instructors, developers, and anyone looking to streamline workflows by bundling files with minimal effort.

With robust features like file sorting, line cleanup, and customizable comments, this tool is an essential solution for packaging files for evaluation or deployment.

Features
Multi-Language Support: Bundle files from various programming languages (e.g., C#, Python, Java) or bundle all files in a directory.
Customizable Output: Specify the output file path, sort files by name or type, and include optional notes such as author names or file paths.
Streamlined Cleanup: Remove empty lines from source files with a simple flag.
Efficient Sorting: Organize files alphabetically or by file type for better readability.
Installation
1. Clone the Repository
First, ensure you’ve forked the repository, then clone it to your local machine.

2. Build the Project
Use the .NET terminal to build the application:

bash
Copy
Edit
dotnet build
3. Add the Tool to Your Environment Variables
To use the tool from any directory:

Publish the project:
bash
Copy
Edit
dotnet publish -o publish
This will create a publish folder in the project directory.
Copy the path of the publish folder.
Search for "Environment Variables" (משתני סביבה) on your computer.
Locate the Path variable, edit it, and add the publish folder's path.
Once completed, you can use the tool from any terminal location!

Basic Syntax
bash
Copy
Edit
line bundle --language <languages> --output <bundle file's name> [options]
Examples
Bundle all files in the current directory:

bash
Copy
Edit
line bundle --language all --output mergedCode
Bundle all files from a specific directory:

bash
Copy
Edit
line bundle --language all --output "C:\Users\HOME\Desktop\NewFolder\mergedCode"
Bundle specific languages (e.g., C# and Python) with cleanup and author details:

bash
Copy
Edit
line bundle --language cs,py --output output --remove-empty-lines --author "John Doe"
Available Options
Option	Alias	Description
--language	-l	List of programming languages to include (e.g., cs, py, java). Use all to include all files.
--output	-o	Specify the output file name or path.
--note	-n	Add source file paths as comments in the bundled file.
--sort	-s	Specify sorting order: name (default) or type.
--remove-empty-lines	-r	Remove empty lines from source files before bundling.
--author	-a	Add the author’s name as a comment in the bundled file.
Notes
Language Formatting: Use lowercase language identifiers (e.g., cs, py) rather than symbols (e.g., c#).
String Inputs: Wrap strings containing spaces in quotes (e.g., "C:\My Path\file").
Output Path: For specific folders, provide the full directory path + file name (without extension). For example:
bash
Copy
Edit
"C:\Users\HOME\Desktop\NewFolder\bundle"
The tool will generate a .pdf file as the output.
