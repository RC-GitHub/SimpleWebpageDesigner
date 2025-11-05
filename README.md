# SimpleWebpageDesigner
is a relatively simple **Windows** application made in C# + WPF in which one can design their own webpage. 

The styles specified by the user are saved and preserved in JSON files and later converted into HTML, CSS and JS files.

## License [![License: CC BY-NC 4.0](https://img.shields.io/badge/License-CC%20BY--NC%204.0-lightgrey.svg)](https://creativecommons.org/licenses/by-nc/4.0/)
This project is licensed under the [Creative Commons Attribution–NonCommercial 4.0 International (CC BY-NC 4.0)](https://creativecommons.org/licenses/by-nc/4.0/) license.
You are free to use, modify, and share this work for non-commercial purposes, as long as you credit the author.
Use it, remix it, share it — just don’t sell it.

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
* After the SWD App is installed, you should be greeted by the _MainWindow_.
      <img src="https://github.com/user-attachments/assets/32d3d2df-ce9d-4729-b24d-ddc4e985808c" alt="MainWindow" style="width: 500px;"/> 
* The _MainWindow_'s buttons allow for:
   1. **New project:** Creation of a new project. The user is asked whether they want the project to be saved in the default directory or in a specified place. After that, they are taken into the _CreationWindow_. 
      <img src="https://github.com/user-attachments/assets/f81d1538-7597-4bfb-8544-dbce16be67b8" alt="MessageBox Dialog" style="width: 500px;"/> 
   
   2. **Open project:** Opening of an existing project. The user is asked about the directory of their project. Once the folder with a correct naming scheme of `SWD-[Project name]` is selected, the user is taken into the _ContentWindow_;
   
   3. **Delete existing:** Deletion of an existing project(s). The user is asked about the directory of their project. All folders with the correct naming scheme of `SWD-[Project name]` will then be deleted;
   
   4. **Theme settings:** Change of the SWD app's theme. _ThemeWindow_ is opened.

### CreationWindow:
* The _CreationWindow_ is the place where the user can set the primary settings of the SWD Project.
* Mainly: 

   1. Set the metadata of their website (Author's name, Keywords, Description) as well as the name of the Project.
      <img src="https://github.com/user-attachments/assets/d2ff1634-f8db-44a2-b473-a2f1d3510ad2" alt="The CreationWindow" style="width: 500px;"/>

   3. Add HTML Code before `<div class="grid-container">`'s contents, insert their own CSS styles and add their own JavaScript.<br>  
      <img src="https://github.com/user-attachments/assets/22088062-0c64-4dfb-9e99-e2258057e4d5" alt="CreationWindow's code-adding capabilities" style="width: 500px;"/>

   4. As well as set certain styles for `<header>`, `<body>`, `<div class="grid-container">` and `<footer>`.<br>  
      <img src="https://github.com/user-attachments/assets/e8a11e09-1ada-4a62-a938-0bd50edb4a7a" alt="CW styling 1" style="width: 300px;"/>
      <img src="https://github.com/user-attachments/assets/4cce73ab-a791-4d4a-9d3a-56f334e2afaf" alt="CW styling 2" style="width: 300px;"/>
      <img src="https://github.com/user-attachments/assets/9bcde89c-d677-41ab-b1ce-c32d19796bac" alt="CW styling 3" style="width: 300px;"/>
      
* After the "Submit" button is clicked, the _ContentWindow_ will be opened while the _MainWindow_ and _CreationWindow_ will close.

### ContentWindow:
* The _ContentWindow_ is the place where the user can design their own [grid layout](https://www.w3schools.com/css/css_grid.asp) which later on will be represented on a page.<br>
   <img src="https://github.com/user-attachments/assets/d349a746-4024-469f-9833-135290a5d9dc" alt="The ContentWindow" style="width: 600px;"/>

* Inside of the _ContentWindow_ the user can:
   1. Add rows and (a max of 12) columns via the Row and Column modification sections.<br> 
      <img src="https://github.com/user-attachments/assets/256fdd28-4b2a-469b-9969-d2468c72d773" alt="Row and Column modification" style="width: 500px;"/>

   2. Create their own Component. The user has to select a group of DataGrid cells and then right-click to display a context menu. After choosing one of 4 built-in Component types, the _InputDialog_ will open asking for a name of the Component. After that the user has to choose a color for their Component in order to differentiate them on the DataGrid.<br> 
      <img src="https://github.com/user-attachments/assets/64291c35-3779-4249-9441-e190192ce9c1" alt="Component creation step 1" style="width: 500px;"/><br> 
      <img src="https://github.com/user-attachments/assets/f65d517a-b9f6-4a3c-8b64-e87d722dff18" alt="Component creation step 2" style="width: 250px;"/>
      <img src="https://github.com/user-attachments/assets/94b9e65e-7374-40e2-ae39-3665f6bc13ab" alt="Component creation step 3" style="width: 250px;"/>
      <img src="https://github.com/user-attachments/assets/5435a914-6d01-4deb-b87c-0782e4288362" alt="Component creation step 4" style="width: 250px;"/>
      <img src="https://github.com/user-attachments/assets/958ecfdc-9d5f-49cc-a51d-48dcf806f09b" alt="Component creation step 5" style="width: 250px;"/>
      <img src="https://github.com/user-attachments/assets/fa89f8e4-57bf-4f8d-b0fa-59df83fa6a9f" alt="Component creation step 6" style="width: 250px;"/>
      <img src="https://github.com/user-attachments/assets/7a24754d-f230-460a-b9cf-a62276b0910f" alt="Component creation step 7" style="width: 250px;"/>

   3. Resize and change the position of a Component via the Component modification section.<br> 
      <img src="https://github.com/user-attachments/assets/7742c24c-9005-40d1-b773-39dd38d07fe6" alt="Component modification step 1" style="width: 400px;"/>
      <img src="https://github.com/user-attachments/assets/5afc9292-47d4-4456-ba75-c1d66030e0e5" alt="Component modification step 2" style="width: 400px;"/>

   4. Add and move subfolders via the "File and folder control" section.<br> 
      <img src="https://github.com/user-attachments/assets/461f57e5-0d31-4d7e-89dd-73a5a8f05a02" alt="File and folder control step 2" style="width: 500px;"/>
      <img src="https://github.com/user-attachments/assets/c38fcf00-8688-42fb-8f16-10fe1fc4fee9" alt="File and folder control step 1" style="width: 300px;"/><br> 
      <img src="https://github.com/user-attachments/assets/14b5dee2-10c8-48e4-94bb-620a64d3980a" alt="Edit file step 1" style="width: 300px;"/>

   5. Edit a different file with the "Files" section.<br> 
      <img src="https://github.com/user-attachments/assets/f2eda915-a094-41cf-b537-645c1f4bbfc1" alt="New file step 2" style="width: 500px;"/><br> 
      <img src="https://github.com/user-attachments/assets/a8250530-e25f-4f00-942c-42fea561f05b" alt="Edit file step 2" style="width: 300px;"/>

   6. Modify or delete a component with the "Edit" and "Delete" buttons inside of the context menu available after right-clicking the selected Component. Clicking the "Edit" button will open the _ComponentWindow_.<br> 
      <img src="https://github.com/user-attachments/assets/dedc3ff5-9d55-486f-b428-c21b8e7a76af" alt="Modify or delete component" style="width: 500px;"/>

   7. Create a new SWD Project or add subfolders and files with the "New" toolbar context menu.

   8. Import (add) files and folders from different SWD Projects into the current one with the "Import" toolbar context menu.

   9. Open the _CreationWindow_ or _ThemeWindow_ with the "Edit" toolbar context menu to tweak the current settings.

   10. Save a file (turn the selected styles and settings into a JSON file) with the "Save" button inside of the toolbar.<br> 
      <img src="https://github.com/user-attachments/assets/f54f6d72-940b-4bce-8913-2b4a723b04e5" alt="Save file" style="width: 500px;"/>

   11. Close a current project with the "Close" button inside of the toolbar.

   12. Recreate the folder structure and convert the JSON files into the HTML, CSS, and JS files with the "Build" button inside of the toolbar.<br> 
      <img src="https://github.com/user-attachments/assets/eb818972-732f-454d-81d4-4bda575d9803" alt="Build files" style="width: 600px;"/>

   13. Open the currently edited file inside of the browser (if the projects is built) with the "Reveal" button inside of the toolbar.<br> 
       <img src="https://github.com/user-attachments/assets/5ca34eb3-52e9-44c1-8368-9aab4aa2a184" alt="Reveal file" style="width: 700px;"/>
 
### ComponentWindow and Component Pages:
* The _ComponentWindow_ is the place where the user can set the primary settings and stylings for their Component (both for the purposes of the SWD application and for the purposes of the website).
* Four pages can be accessed via the Component Type ComboBox!<br> 
   ![ComponentWindow](https://github.com/user-attachments/assets/29802e21-430a-4327-97a6-d2c46b0fce10)

  1. TextSimple Page:
     ![TextSimple](https://github.com/user-attachments/assets/9aba9d1a-5d5d-4c27-a86c-c498eac46e63)

  2. ImageSimple Page:
     ![ImageSimple](https://github.com/user-attachments/assets/fc1b1dc4-d96c-4477-a08d-14f22dd8f48e)

  3. ButtonSimple Page:
     ![ButtonSimple](https://github.com/user-attachments/assets/f66153f0-e6dd-48e4-9f92-c3ad93706f59)

  4. CodeSimple Page:
     ![CodeSimple](https://github.com/user-attachments/assets/8318ebd4-caa3-4f09-81de-6f714c41d2fe)

### ThemeWindow:
* The _ThemeWindow_ is the place where the user can change the colors, alignment or add images to make the SWD application more pleasing to the eye.
* The application has two themes built in, which will regenerate once they're deleted.
   1. Light Theme:<br> 
      ![Light Theme](https://github.com/user-attachments/assets/f54afc2a-35ac-4f77-b63b-19fcf3d491e9)
   2. Dark Theme:<br> 
      ![Dark Theme](https://github.com/user-attachments/assets/fa275b7f-e484-497c-9178-a8c39cb74732)
      
   ![Theme Change](https://github.com/user-attachments/assets/44d05924-eff7-4f57-9f99-5fae9ee86e38)










     








