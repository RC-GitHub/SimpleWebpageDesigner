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
<img src="https://github.com/user-attachments/assets/32d3d2df-ce9d-4729-b24d-ddc4e985808c" alt="MainWindow" style="width: 500px;"/> 
* The MainWindow's buttons allow for:
   1.  **New project:** Creation of a new project. The user is asked whether they want the project to be saved in the default directory or in a specified place. After that, they are taken into the CreationWindow. <img src="https://github.com/user-attachments/assets/f81d1538-7597-4bfb-8544-dbce16be67b8" alt="MessageBox Dialog" style="width: 500px;"/> 
   
   3.  **Open project:** Opening of an existing project. The user is asked about the directory of their project. Once the folder with a correct naming scheme of `SWD-[Project name]` is selected, the user is taken into the ContentWindow;
   
   4.  **Delete existing:** Deletion of an existing project(s). The user is asked about the directory of their project. All folders with the correct naming scheme of `SWD-[Project name]` will then be deleted;
   
   5.  **Theme settings:** Change of the SWD app's theme. ThemeWindow is opened.


### CreationWindow:
* Inside of the CreationWindow the user can: 

   1. Set the metadata of his website; ![The CreationWindow](https://github.com/user-attachments/assets/d2ff1634-f8db-44a2-b473-a2f1d3510ad2)

   2. Add HTML Code before `<div class="grid-container">`'s contents, insert their own CSS styles and add their own JavaScript; ![CreationWindow's code-adding capabilities](https://github.com/user-attachments/assets/22088062-0c64-4dfb-9e99-e2258057e4d5)

   3. As well as set certain styles for `<header>`, `<body>`, `<div class="grid-container">` and `<footer>`. ![CW styling 1](https://github.com/user-attachments/assets/e8a11e09-1ada-4a62-a938-0bd50edb4a7a) ![CW styling 2](https://github.com/user-attachments/assets/4cce73ab-a791-4d4a-9d3a-56f334e2afaf) ![CW styling 3](https://github.com/user-attachments/assets/9bcde89c-d677-41ab-b1ce-c32d19796bac)




