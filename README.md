# Neplan-Cloud-Connector-NCC
This is a dll to connect to the Neplan Cloud
All About the Neplan Cloud: http://www.neplan.ch/neplanproduct/de-neplan-360-cloud

The dll allows any other program to send neplan commands and inputs to Neplan Cloud and to get the results. In most programming languages dll's can be imported and used without much code.

## Requirements
* Neplan Cloud Login
* Neplan Cloud Webservices module has to be activated by Neplan

## Howto
```matlab
NCC_dll = NET.addAssembly('path/NCC.dll');

url =		"https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc";
username =	"username";
pasword =	"password";
project_name =	"Neplan_Project_X";

ws = NCC.Webservice(url,username,password);
ws.Open();
project = ws.GetProject(project_name, "", "", "");
```
