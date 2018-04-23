# Neplan-Cloud-Connector-NCC
This is a dll to connect to the Neplan Cloud.
All about the Neplan Cloud: http://www.neplan.ch/neplanproduct/de-neplan-360-cloud

The dll allows any other program to send neplan commands and inputs to Neplan Cloud and to get the results. The NCC.dll is developed for Matlab but it might also work in other programming languages.

## Requirements
* Neplan Cloud account
* The Neplan Cloud "Webservices module" has to be activated by Neplan for your account

## Howto
Here is an example on how to use the dll in Matlab (tested in Matlab R2018a)
```Matlab
% settings
url =		"https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc";
username =	"username";
pasword =	"password";
project_name =	"Neplan_Project_X";

% load libraries
NCC_lib = NET.addAssembly([pwd,'\NCC.dll']);
import System.*
import DateTime.*

% creat and open webservice 
ws = NCC.Webservice(url,username,password);
ws.Open();

% get project
project = ws.GetProject(project_name, "", "", "");

% send command to do a LoadFlow analysis
guid = System.Guid.NewGuid();
guid = guid.ToString();
res = ws.AnalyseVariant(project, guid, "LoadFlow", "Default", "", "", "");

% get results from the LoadFlow analysis
res = ws.GetListResultSummary(project, "LoadFlow", DateTime(), 0, "");
res = xml2struct(char(res(1)));

% close websocket
ws.Close()
```
