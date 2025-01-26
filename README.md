

# CLI Files Bundler  

## Overview  
The **CLI files Bundler** is a lightweight and efficient command-line tool designed to simplify code management by combining multiple source files into a single, organized output file. It’s particularly useful for instructors,
developers, and anyone looking to streamline workflows by bundling files with minimal effort.  

With robust features like file sorting, line cleanup, and customizable comments,
the CLI Code Bundler is an essential tool forpackaging files for evaluation or deployment.  
---

## Features  
- **Multi-Language Support**: Bundle files from various programming languages (`C#`, `Python`, `Java`, etc.) or bundle all files in a directory.  
- **Customizable Output**: Define the output file path, sort files by name or type, and include optional notes like author names or file paths.  
- **Streamlined Cleanup**: Remove empty lines from source files with a simple flag.  
- **Efficient Sorting**: Organize files alphabetically or by their type for better readability.  

---

## Installation  
1. **Clone the Repository**:  
   Only with fork
     
2. **Build the Project**:  
   Use the .NET terminal to build the application:  
    ```cli
    dotnet build
    ```
   
3. **Add the Tool to Your Environment variables**:  
   Publish the project and add the executable to your Environment variables PATH for easy access from any folder:  
   in the origion folder of the project (the folder that the line project folder is there) open terminal and run: dotnet publish -o publish
   you get a folder called publish.
   copy the path of the publish folder.
   search in the computer "משתני סביבה" click on that and you will see a variable named "Path"
   click on it and add the path of the publish folder into the path variable
   and now you can run the command from every path in the cli.
    ```cli
    dotnet publish -o publish
    ```

---
### Basic Syntax  
- Basic:
  ```cli
  line bundle --language <languages> --output <bundle file's name> [options]
  ```
  
### Examples  
- Bundle all files in the directory: 
  ```cli
  line bundle --language all --output mergedCode
  ```    
- Bundle all files in a different directory: 
  ```cli
  line bundle --language all --output "C:\Users\HOME\Desktop\‏‏\תיקיה חדשהmergedCode"
  ```  
- Bundle specific languages (e.g., C# and Python) with cleanup and author details:  
  ```cli
  line bundle --language cs,py --output output --remove-empty-lines --author "John Doe"
  ```  

### Available Options  
| Option                 | Alias | Description                                                                                  |  
|------------------------|-------|----------------------------------------------------------------------------------------------|  
| `--language`           | `-l`  | List of programming languages to include (e.g., `cs`, `py`, `java`). Use `all` for all files.|  
| `--output`             | `-o`  | Output file name or path+name.                                                               |  
| `--note`               | `-n`  | Add source file paths as comments in the bundled file.                                       |  
| `--sort`               | `-s`  | Sorting order for files: `name` (default) or `type`.                                         |  
| `--remove-empty-lines` | `-r`  | Remove empty lines from source files before bundling.                                        |  
| `--author`             | `-a`  | Add the author’s name as a comment in the bundled file.                                      |  

---
notes:
* Languages must be written in lower case.
* Its preferable to write the type file like cs instead of c#.
* Every string input to the options has to be wraped with quotes in case of more than 1 word
* If you prefier to put the bundle file in a specific folder, in the option --output write the full path of the directory+the name you want for the bundle file
  without type, like: "C:\Users\HOME\Desktop\‏‏תיקיה חדשה\bundle" and not :"C:\Users\HOME\Desktop\‏‏תיקיה חדשה\bundle.txt" 
  the bundle file will be in pdf type.
