[![Build Status](https://dev.azure.com/allors/Allors2/_apis/build/status/Allors.allors2?branchName=master)](https://dev.azure.com/allors/Allors2/_build/latest?definitionId=4&branchName=master)



## **Setup & Usage:**

**For Windows:**

Steps to setup

1. Fork & Clone the repository
2. Use the "**UpdateAllorsFromUpstreamRemote.cmd**" to pull updates from upstream remote or make your own
3. Copy the "config" directory in the root directory of your project location. For example if you project is anywhere in "C" drive, place the "config" folder in "C:\" (assuming you're using windows)
4. Create database(s). Best to use the same name used in the configuration files provided.
5. Type in "nuke" in Command Prompt or Power Shell to build and wait patiently until the process is completed
6. Run "Commands.cmd"
7. Type in "populate" to migrate
8. If you want to populate with some demo data, type in "populate -d"
9. Now your project is almost ready to run

Steps to run

1. Run "Server.cmd" to start the backend
2. Run "CodeIntranet.cmd" to open the project in your default IDE/Editor (assuming you have Visual Studio Code setup and set as the default)
3. Type "npm start" in the terminal and wait until the process is completed
4. Go to the url shown in the terminal/console and voila!
5. Now in the login screen, use "administrator" as username and keep password empty
