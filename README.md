# SimpleWebpageDesigner
is a relatively simple **Windows** application made in C# + WPF in which one can design their own webpage. 

The styles specified by the user are saved and preserved in JSON files and later converted into HTML, CSS and JS files.

## Download and Installation Instructions
To install the application, please follow these steps:

### Windows:
1. **Download the Application**: Click the link below to download the application:
   - [Download Application](https://github.com/RC-GitHub/SimpleWebpageDesigner/releases)

2. **Run the Application**: Locate the `SWD.application` file and double-click it to start the installation. Once the application is installed it should open instantly.

3. **Handling Errors**: If you encounter an error while trying to run the application, you may need to unblock the file. To do this, follow these steps:

   - Open PowerShell as an administrator.
   - Run the following command, replacing `[path]` with the actual path to the directory where the `SWD.application` file is located:

     ```powershell
     Get-ChildItem -Recurse "[path]" | Unblock-File
     ```

   - After running the command, try running the `SWD.application` file again.

## Inner Workings of an App
### MainWindow:
* After the SWD App is installed, you should be greeted by the MainWindow.
![MainWindow](https://github.com/user-attachments/assets/32d3d2df-ce9d-4729-b24d-ddc4e985808c)
* The MainWindow's buttons allow for:
  1. **New project:** Creation of a new project. The user is asked whether they want the project to be saved in the default directory or in a specified place. ![MessageBox Dialog](https://github.com/user-attachments/assets/f81d1538-7597-4bfb-8544-dbce16be67b8) After that they are taken into the CreationWindow.
  2. **Open project:** Opening of an existing project. The user is asked about the directory of their project. Once the folder with a correct naming scheme of `SWD-[Project name]` is selected, the user is taken into the ContentWindow.
  3. **Delete existing:** Deletion of an existing project(s). The user is asked about the directory of their project. All folders with the correct naming schene of `SWD-[Project name]` will then be deleted.
  4. **Theme settings:** Change of the SWD app's theme. ThemeWindow is opened.

### CreationWindow:
