# FolderSync ![Peter J Kingston](https://peterjkingston.com/favicon-32x32.png) 
Folder Syncronization Windows Service app. Listens to a directory and copies the changes to another directory.

## Installation
* Build with VS2019
* From a shell app (Command Prompt or other), navigate to the coorisponding bin folder, or whereever you've put the build files.
* Enter the command ".\FolderSyncConsole.exe install start"
* If successful, an install message will run ending in "FolderSync service has started."

## Known Issues ![Issues](https://github.com/peterjkingston/FolderSync/blob/master/.github/action-issueTracker/file-plus.svg)
![IssueTracker](https://github.com/peterjkingston/FolderSync/workflows/IssueTracker/badge.svg)

    <script>
      let requestURL = 'https://api.github.com/repos/peterjkingston/FolderSync/contents/Issues.md'; 
      let request = new XMLHttpRequest(); 
      request.open('GET', requestURL); 
      request.responseType = 'json'; 
      request.send(); 

      request.onload = function(){ 
        const jsonObj = request.response; 
        var encoding = jsonObj.encoding; 
        var type = jsonObj.type; 
        var content = jsonObj.content; 
        var c = document.createElement('div'); 
        c.innerHTML = atob(content); 
        document.body.appendChild(c);
        }
    </script>
