clc
client.fnc = 'openWebservice';
client.input.username = 'christoph.hunziker@fhnw.ch';
client.input.password = 'nep360FH2017';
client.input.project = 'NeplanMatlab2';

client.send()

%%

client.fnc = 'AnalyseVariant';
client.input.analysisModule = "LoadFlow";
client.input.calcNameID = "Default";
client.input.analysisMethode = "";
client.input.conditions = "";
client.input.analysisLoadOptionXML = "";
client.send()

%%

client.fnc = 'GetListResultSummary';
client.input.analysisType = "LoadFlow";
client.input.networkTypeGroup = 0;
client.input.networkTypeGroupID = "";
result = client.send()
a = xmlreadstring(result);
%%

client.fnc = 'closeWebservice';
client.send()