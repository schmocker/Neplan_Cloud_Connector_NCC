clc
fnc = 'StartNeplanClient';
input.username = 'christoph.hunziker@fhnw.ch';
input.password = 'nep360FH2017';
input.project = 'NeplanMatlab2';
output = client.do(fnc,input);
input = struct;


%%

fnc = 'AnalyseVariant';
input.analysisModule = "LoadFlow";
input.calcNameID = "Default";
input.analysisMethode = "";
input.conditions = "";
input.analysisLoadOptionXML = "";
output = client.do(fnc,input);
input = struct;

%%

fnc = 'GetListResultSummary';
input.analysisType = "LoadFlow";
input.networkTypeGroup = 0;
input.networkTypeGroupID = " ";
output = client.do(fnc,input);
input = struct;

%%

fnc = 'GetResultElementByName';
input.elementName = "Netz";
input.elementTypeName = "ExternalGrid";
input.portNr = 0;
input.analysisType = "LoadFlow";
output = client.do(fnc,input);
input = struct;

%%

fnc = 'StopNeplanClient';
output = client.do(fnc,input);