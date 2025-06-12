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
* After the SWD App is installed, you should be greeted by the MainWindow. <img src="https://github.com/user-attachments/assets/32d3d2df-ce9d-4729-b24d-ddc4e985808c" alt="MainWindow" style="width: 500px;"/> 
* The MainWindow's buttons allow for:
   1.  **New project:** Creation of a new project. The user is asked whether they want the project to be saved in the default directory or in a specified place. After that, they are taken into the CreationWindow. <img src="https://github.com/user-attachments/assets/f81d1538-7597-4bfb-8544-dbce16be67b8" alt="MessageBox Dialog" style="width: 500px;"/> 
   
   3.  **Open project:** Opening of an existing project. The user is asked about the directory of their project. Once the folder with a correct naming scheme of `SWD-[Project name]` is selected, the user is taken into the ContentWindow;
   
   4.  **Delete existing:** Deletion of an existing project(s). The user is asked about the directory of their project. All folders with the correct naming scheme of `SWD-[Project name]` will then be deleted;
   
   5.  **Theme settings:** Change of the SWD app's theme. ThemeWindow is opened.

### CreationWindow:
* Inside of the CreationWindow the user can: 

   1. Set the metadata of his website; 
   <img src="https://github.com/user-attachments/assets/d2ff1634-f8db-44a2-b473-a2f1d3510ad2" alt="The CreationWindow" style="width: 500px;"/>

   2. Add HTML Code before `<div class="grid-container">`'s contents, insert their own CSS styles and add their own JavaScript; 
   <img src="https://github.com/user-attachments/assets/22088062-0c64-4dfb-9e99-e2258057e4d5" alt="CreationWindow's code-adding capabilities" style="width: 500px;"/>

   3. As well as set certain styles for `<header>`, `<body>`, `<div class="grid-container">` and `<footer>`. 
   <img src="https://github.com/user-attachments/assets/e8a11e09-1ada-4a62-a938-0bd50edb4a7a" alt="CW styling 1" style="width: 300px;"/>
   <img src="https://github.com/user-attachments/assets/4cce73ab-a791-4d4a-9d3a-56f334e2afaf" alt="CW styling 2" style="width: 300px;"/>
   <img src="https://github.com/user-attachments/assets/9bcde89c-d677-41ab-b1ce-c32d19796bac" alt="CW styling 3" style="width: 300px;"/>
   
* After the Submit button is clicked, the ContentWindow will be opened.

### ContentWindow:
* The ContentWindow is the place where one can design their own [grid layout](https://www.w3schools.com/css/css_grid.asp) which later on will be represented on a page.
<img src="https://github.com/user-attachments/assets/d349a746-4024-469f-9833-135290a5d9dc" alt="The ContentWindow" style="width: 500px;"/>

* Inside of the ContentWindow the user can:
   1. Add rows and (a max of 12) columns via the Row and Column modification sections.
   <img src="https://github.com/user-attachments/assets/256fdd28-4b2a-469b-9969-d2468c72d773" alt="Row and Column modification" style="width: 500px;"/>

   2. Create their own Component. The user has to select a group of DataGrid cells and then right-click to display a context menu. After choosing one of 4 built-in Component types, the InputDialog will open asking for a name of the Component. After that the user has to choose a color for their Component in order to differentiate them on the DataGrid.
   <img src="https://github.com/user-attachments/assets/64291c35-3779-4249-9441-e190192ce9c1" alt="Component creation step 1" style="width: 250px;"/>
   <img src="https://github.com/user-attachments/assets/f65d517a-b9f6-4a3c-8b64-e87d722dff18" alt="Component creation step 2" style="width: 250px;"/>
   <img src="https://github.com/user-attachments/assets/94b9e65e-7374-40e2-ae39-3665f6bc13ab" alt="Component creation step 3" style="width: 250px;"/>
   <img src="https://github.com/user-attachments/assets/5435a914-6d01-4deb-b87c-0782e4288362" alt="Component creation step 4" style="width: 250px;"/>
   <img src="https://github.com/user-attachments/assets/958ecfdc-9d5f-49cc-a51d-48dcf806f09b" alt="Component creation step 5" style="width: 250px;"/>
   <img src="https://github.com/user-attachments/assets/fa89f8e4-57bf-4f8d-b0fa-59df83fa6a9f" alt="Component creation step 6" style="width: 250px;"/>
   <img src="https://github.com/user-attachments/assets/7a24754d-f230-460a-b9cf-a62276b0910f" alt="Component creation step 7" style="width: 250px;"/>

   3. Resize and change the position of a Component via the Component modification section.
   <img src="https://github.com/user-attachments/assets/7742c24c-9005-40d1-b773-39dd38d07fe6" alt="Component modification step 1" style="width: 400px;"/>
   <img src="https://github.com/user-attachments/assets/5afc9292-47d4-4456-ba75-c1d66030e0e5" alt="Component modification step 2" style="width: 400px;"/>

   4. Add and move subfolders via the "File and folder control" section or add subfolders and files with the "New" toolbar context menu.
   <img src="https://github.com/user-attachments/assets/461f57e5-0d31-4d7e-89dd-73a5a8f05a02" alt="File and folder control step 2" style="width: 300px;"/>
   <img src="https://github.com/user-attachments/assets/c38fcf00-8688-42fb-8f16-10fe1fc4fee9" alt="File and folder control step 1" style="width: 300px;"/>
   <img src="https://github.com/user-attachments/assets/14b5dee2-10c8-48e4-94bb-620a64d3980a" alt="Edit file step 1" style="width: 300px;"/>

   6. Edit a different file with the "Files" section.
   <img src="https://github.com/user-attachments/assets/f2eda915-a094-41cf-b537-645c1f4bbfc1" alt="New file step 2" style="width: 300px;"/>
   <img src="https://github.com/user-attachments/assets/a8250530-e25f-4f00-942c-42fea561f05b" alt="Edit file step 2" style="width: 300px;"/>

   6. Modify or delete a component with the "Edit" and "Delete" buttons inside of the context menu available after right-clicking the selected Component.
   <img src="https://github.com/user-attachments/assets/dedc3ff5-9d55-486f-b428-c21b8e7a76af" alt="Modify or delete component" style="width: 500px;"/>

   7. Save a file (turn the selected styles and settings into a JSON file) with the "Save" button inside of the toolbar.
   <img src="https://github.com/user-attachments/assets/f54f6d72-940b-4bce-8913-2b4a723b04e5" alt="Save file" style="width: 500px;"/>

   8. Recreate the folder structure and convert the JSON files into the HTML, CSS, and JS files with the "Build" button inside of the toolbar.
   <img src="https://github.com/user-attachments/assets/eb818972-732f-454d-81d4-4bda575d9803" alt="Build files" style="width: 500px;"/>









