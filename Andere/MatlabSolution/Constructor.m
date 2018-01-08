clc
clear all

jdk = 'C:\Program Files\Java\jdk1.7.0_75';
matlab.wsdl.setWSDLToolPath('JDK',jdk);
cxf = 'C:\apache-cxf-2.7.10';
matlab.wsdl.setWSDLToolPath('CXF',cxf);
matlab.wsdl.setWSDLToolPath

wsdlURL = 'https://demo.neplan.ch/NEPLAN360_Demo/Services/External/NeplanService.svc?singleWsdl';
matlab.wsdl.createWSDLClient(wsdlURL)