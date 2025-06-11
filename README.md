# SimpleWebpageDesigner
Ideally I want to make a relatively simple application in C# + WPF in which one can design their own webpage. It will then convert into HTML/CSS and/or JS files (or at least I hope so).
^^

## Download and Installation Instructions

To install the application, please follow these steps:

1. **Download the Application**: Click the link below to download the application:
   - [Download Application](https://github.com/RC-GitHub/SimpleWebpageDesigner/releases)

2. **Run the Application**: Locate the downloaded `.SWD.application` file and double-click it to start the installation.

3. **Handling Errors**: If you encounter an error while trying to run the application, you may need to unblock the file. To do this, follow these steps:

   - Open PowerShell as an administrator.
   - Run the following command, replacing `"path"` with the actual path to the directory where the `.SWD.application` file is located:

     ```powershell
     Get-ChildItem -Recurse "path" | Unblock-File
     ```

   - After running the command, try running the `.SWD.application` file again.
