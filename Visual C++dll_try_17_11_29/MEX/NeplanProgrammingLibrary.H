#pragma once

//#include <windows.h>
#include "BCPProgrammingLibrary.h"
#include <string>
#include <vector>
//#include "atlstr.h"

extern HINSTANCE g_hMainProg;

//class CString;

enum{EXCITER, PSS, U_EXITER_LIMIT, SVC, TRAFO, TRAFO_FI, CAPACITOR,
     GOVERNOR, FIELD_LIM, STATOR_I_LIM,
     RESERVED_1, RESERVED_2,
     HVDC_CR, HVDC_COR, HVDC_VDCOL, HVDC_GR,
     HVDC_CC, HVDC_DCR, HVDC_FPD,
	   HVDC_N_CFC, HVDC_N_CCA, HVDC_N_COL, HVDC_N_MC, HVDC_N_VCR,PWM_MA,PWM_FI,PWM_RE_IM,ASM_ROTOR,GEN_VREF,GEN_PREF, VCOMP,
	   HVDC_MASTER_CTRL=31, HVDC_CONV_CTRL=32, HVDC_VSC_CTRL=33,
	   WINDCTRL=34,
     CCS_IP=35, CCS_IQ=36,
     TURBINE=70, MISC=90
    };


#define  EDT_NEPLAN_FILE 0
#define  NDT_NEPLAN_FILE 1
#define  NDB_NEPLAN_FILE 2
#define  ZDB_NEPLAN_FILE 3
#define  UCTE_NEPLAN_FILE 5
#define  GIS_NEPLAN_FILE 6
#define  PSSE_NEPLAN_FILE 8
#define  WET_NEPLAN_FILE 10
#define  WKT_NEPLAN_FILE 11
#define  GRT_NEPLAN_FILE 12
#define  MCB_NEPLAN_FILE 13


//creates a log handler. One will automatically created on execution of an NPL
BOOL CreateLoggingHandler(TCHAR* wcFilePathName);
//deletes a log handler. One will automatically created on execution of an NPL
BOOL DeleteLoggingHandler();
//Defines, if the note messages should be written to the log file. Default is yes.
BOOL ShowNotesInLogFile(BOOL bShow);
//Defines, if the warning messages should be written to the log file. Default is yes.
BOOL ShowWarningsInLogFile(BOOL bShow);
//Defines, if the error messages should be written to the log file. Default is yes.
BOOL ShowErrorsInLogFile(BOOL bShow);
//shows the report of a NEPLAN NPL run
BOOL ShowReport();
//closes the report log file of a NEPLAN NPL run
BOOL CloseLogFile();
//Gets the name of the log file
BOOL GetLogFileName(LPTSTR lpLogFileName);
//close NEPLAN(exit NEPLAN application)
void Close_NEPLAN(BOOL bAskForSave=FALSE);
//hide NEPLAN (run NEPLAN in background mode)
void Hide_NEPLAN();
//show NEPLAN (
void Show_NEPLAN();
//get the data from a variant element set
void UseTempVariantSetOfElements(bool val);
bool GetUseTempVariantSetOfElements();
void RemoveTempVariantSetOfElements();

//open, close save projects
BOOL OpenNeplanProject(TCHAR* sFileName);
BOOL CloseCurrentProject();
BOOL SaveCurrentProject(TCHAR* wcFileName=NULL);
BOOL IsAnyProjectOpen();

//set and get element parameter data
BOOL GetParameterDouble(unsigned long lID,TCHAR* sParameter, double& dValue);
BOOL SetParameterDouble(unsigned long lID,TCHAR* sParameter, double dValue);
BOOL GetParameterLong(unsigned long lID,TCHAR* sParameter, long& lValue);
BOOL SetParameterLong(unsigned long lID,TCHAR* sParameter, long lValue);
BOOL GetParameterInt(unsigned long lID,TCHAR* wcParameter, int& nValue);
BOOL SetParameterInt(unsigned long lID,TCHAR* wcParameter, int nValue);
BOOL GetParameterBool(unsigned long lID,TCHAR* wcParameter, bool& bValue);
BOOL SetParameterBool(unsigned long lID,TCHAR* wcParameter, bool bValue);
BOOL GetParameterString(unsigned long lID,TCHAR* wcParameter, LPTSTR lpValue);
BOOL SetParameterString(unsigned long lID,TCHAR* wcParameter, TCHAR* wcValue);

//Ask if user data extists
BOOL AskForUserDataDouble(unsigned long lID,TCHAR* sParameter);
BOOL AskForUserDataInt(unsigned long lID,TCHAR* wcParameter);
BOOL AskForUserDataBool(unsigned long lID,TCHAR* wcParameter);
BOOL AskForUserDataString(unsigned long lID,TCHAR* wcParameter);

//set and get user data
BOOL GetUserDataDouble(unsigned long lID,TCHAR* sParameter, double& dValue);
BOOL SetUserDataDouble(unsigned long lID,TCHAR* sParameter, double dValue);
BOOL GetUserDataInt(unsigned long lID,TCHAR* wcParameter, int& nValue);
BOOL SetUserDataInt(unsigned long lID,TCHAR* wcParameter, int nValue);
BOOL GetUserDataBool(unsigned long lID,TCHAR* wcParameter, bool& bValue);
BOOL SetUserDataBool(unsigned long lID,TCHAR* wcParameter, bool bValue);
BOOL GetUserDataString(unsigned long lID,TCHAR* wcParameter, LPTSTR lpValue);
BOOL SetUserDataString(unsigned long lID,TCHAR* wcParameter, TCHAR* wcValue);

//set and get element parameter data for user defined models LF_DYN_SC: 0=LoadFlow, 1=DynamicModel, 2=ShortCircuit
BOOL GetUserDefinedParameterDouble(unsigned long lID,TCHAR* wcParameter, int LF_DYN_SC, double& dValue);
BOOL SetUserDefinedParameterDouble(unsigned long lID,TCHAR* wcParameter, int LF_DYN_SC, double dValue);
//set and get calculation parameters
BOOL SetCalcParameterDouble(TCHAR* sCalcParameter, TCHAR* sParameter, double dValue);
BOOL GetCalcParameterDouble(TCHAR* sCalcParameter, TCHAR* sParameter, double& dValue);
BOOL SetCalcParameterInt(TCHAR* sCalcParameter, TCHAR* sParameter, int nValue);
BOOL GetCalcParameterInt(TCHAR* sCalcParameter, TCHAR* sParameter, int& nValue);
BOOL SetCalcParameterBool(TCHAR* sCalcParameter, TCHAR* sParameter, bool bValue);
BOOL GetCalcParameterBool(TCHAR* sCalcParameter, TCHAR* sParameter, bool& bValue);
BOOL SetCalcParameterString(TCHAR* sCalcParameter, TCHAR* sParameter, LPTSTR lpValue);
BOOL GetCalcParameterString(TCHAR* sCalcParameter, TCHAR* sParameter, TCHAR** sValue);
//set Variant Parameters
BOOL GetVariantParameterString(TCHAR* wcParameter, LPTSTR lpValue);
BOOL SetVariantParameterString(TCHAR* wcParameter, TCHAR* wcValue);
BOOL GetVariantParameterBool(TCHAR* wcParameter, bool& bValue);
BOOL SetVariantParameterBool(TCHAR* wcParameter, bool bValue);

//set and remove faulted node and fault on a line
BOOL SetFaultedNode(unsigned long lID);
BOOL RemoveFaultedNode(unsigned long lID);
BOOL SetFaultOnLine(unsigned long lID, double dDistance);
BOOL RemoveFaultOnLine(unsigned long lID);

//remove Load flow control areas transactions and imports
BOOL RemoveAllControlAreaImports();
BOOL RemoveAllControlAreaTransactions();
//add conprol area import for load flow
BOOL AddControlAreaImport(TCHAR* wcName, TCHAR* wcAreaName, double dP, double dToler, BOOL bActive, BOOL bIsArea, TCHAR* wcCimID);
//gets the data of a control area import item defined for load flow
BOOL GetAreaControl2Item(int nItem, LPTSTR lpName, LPTSTR lpAreaName, long& lAreaID, double& dP, double& dToler, bool& bActive, LPTSTR lpCimID);
//get number of control area imports defined for load flow
int GetNumAreaControl2Items();
//Get an element ID by name
void GetElementByName(TCHAR* wcType,TCHAR* wcName,unsigned long& lElementID, BOOL bWithErrorMsg = TRUE);
//Get an element ID by name. all names must be unique in the NEPLAN  project!
void GetElementByUniqueName(TCHAR* wcName,unsigned long& lElementID);
//Get all elements of a certain type
void GetElements(TCHAR* sType,int& nCount,unsigned long*& pElementIDs);
//Free the element list, which was get by the "GetElements" function
void FreeElements(unsigned long*& pElementIDs);
//Gets all elements, which are connected at node with ID=lNodeID
//0= get all elements, 1...Switched On elements, 2...Switched Off element
//Important: the pElementIDs must be freed  withe 'FreeElements' after use
BOOL GetConnectedElementsAtNode(unsigned long lNodeID, int nAllOnOff, int& nCount, unsigned long*& pElementIDs);
//Gets a node from an element, nNodeNr=0-4 -> 0=1st node, 2=2nd node,.. etc.
BOOL GetNodeFromElement(unsigned long& lNodeID, unsigned long lElementID, int nNodeNr=0);
//Gets the protected element of a protection device
BOOL GetProtectedElement(unsigned long& lElementID, unsigned long lProtectionID);
//Gets a element from a Node, nElementNr
BOOL GetElementFromNode(unsigned long& lElementID, unsigned long lNodeID, int nElementNr);

//Assigns User Defined field to element if not exist and the values: wcVariableType = "BOOLEAN", "INTEGER", "DOUBLE", "STRING"
BOOL AssignUserDefData(unsigned long& lElementID, TCHAR* wcVariableName, TCHAR* wcVariableType, TCHAR* wcVariableValue, BOOL bAssignValue, BOOL bDisplayValue, BOOL bDisplayDecription);

//delete an element from project
BOOL DeleteElement(unsigned long lID);
//delete a node with connected elements from project
BOOL DeleteNodeWithElements(unsigned long lID);

//add an element to project
BOOL AddElement(unsigned long& lElementID, TCHAR* wcElementType, TCHAR* wcElementName,
                TCHAR* wcNodeName1=NULL, TCHAR* wcNodeName2=NULL, TCHAR* wcNodeName3=NULL, TCHAR* wcNodeName4=NULL);
//adds an element which is on an element (e.g. protection devices, like fuses) to a project
BOOL AddOnElement(unsigned long& lElementID, TCHAR* wcElementType,
                                        TCHAR* wcElementName, TCHAR* wcOnElementName,
                                        TCHAR* wcOnNodeName, TCHAR* wcOnElementType);
BOOL AddOnElementUsingParentID(unsigned long& lElementID, TCHAR* wcElemType, long lOnElementID);
BOOL AddRegulator(unsigned long& lElementID, TCHAR* wcElementType, TCHAR* wcElementName,
                  int nRegulatorType, TCHAR* wcRegulatorName);
//switch on/of an element (logical switches and logical sliders)
BOOL SwitchElement(unsigned long lID, BOOL bSwitch);
//sets the phase connection of an element L1L2L3N (nPhase: 0), L1N (nPhase: 1), L2N (nPhase: 2), L3N (nPhase: 3), L1L2N (nPhase: 4), L1L3N (nPhase: 5), L2L3N (nPhase: 6), L1L2L3N_AS (nPhase: 7)
BOOL SetPhaseOfElement(unsigned long lID, int nPhase);
//switch on/of an element at a defined side/node
BOOL SwitchElementAtNode(unsigned long lElemID, unsigned long lNodeID, BOOL bSwitch);
//gets the logical switch state at port number 'nPortNum'
BOOL GetSwitchOfElement(unsigned long lElemID, int nPortNum, bool& bSwitch);
//get elemnt results
BOOL GetResultDouble(unsigned long lID,TCHAR* sParameter, double& dValue, BOOL bShowErrorMsg=TRUE);
//get summary results
BOOL GetResultSummaryDouble(TCHAR* sParameter, double& dValue, BOOL bShowErrorMsg=TRUE);
//get summary results
BOOL GetResultSummaryInt(TCHAR* sParameter, int& nValue, BOOL bShowErrorMsg=TRUE);
// Get protection device trip time
BOOL GetTripTime(unsigned long lID, double UL1, double UL1ang, double UL2, double UL2ang, double UL3, double UL3ang, double IL1, double IL1ang, double IL2, double IL2ang, double IL3, double IL3ang, double& tripTime);
//run current analysis
void RunSelectedAnalysis();
void RunAnalysis(int nCalculationModul);
//run gas, water and district heating analysis
BOOL RunAnalysisGWTimeSim();
BOOL RunAnalysisGW();
//run reliability analysis
BOOL RunAnalysisRE();
//run load flow analysis
BOOL RunAnalysisLF(BOOL bStopIfNC = TRUE);

// Run load flow analysis, advanced mode for current iteration method: the process is split up; results must be read by special functions
// nStep=0: Setup load flow
// nStep=1: Read network data
// nStep=2: Run first load flow
// nStep=3: Run subsequent load flow
// nStep=4: Delete load flow
BOOL RunAnalysisLFAdvanced(int nStep, BOOL bCalcSelectedFeeders);
BOOL GetNodeResultLFAdvanced(unsigned long lID, double& UL1, double& UL2, double& UL3, double& UangL1, double& UangL2, double& UangL3);
BOOL GetElementResultLFAdvanced(unsigned long lID, int nSide, double P[3], double Q[3], double Imag[3], double& dLoading);
BOOL SetLoadLFAdvanced(unsigned long lID, double dP, double dQ);
BOOL SwitchElementLFAdvanced(unsigned long lID, BOOL bSwitch);

//run load flow with profile at a selected time
BOOL RunAnalysisLFWithProfile(TCHAR* wcTimeStamp);
//run load flow analysis of a certain network feeder defined by the node identifier
BOOL RunAnalysisFeederLF(unsigned long lNodeID);
//run load flow analysis of a certain partial network defined by the node identifier
BOOL RunAnalysisPartnetLF(unsigned long lNodeID, BOOL bStopIfNC = TRUE);
//Has the last LF converged?
BOOL GetLoadflowConverged(bool& bValue);
//Has the last LF with Profile converged?
BOOL GetLoadflowWithProfileConverged(bool& bValue);
//Has the last OPF converged?
BOOL GetOptimalPowerFlowConverged(bool& bValue);
//run short circuit analysis
BOOL RunAnalysisSC();
//run transient stability analysis
void RunAnalysisTS(TCHAR* wcResFileName);
void RunAnalysisTSWithAllDisturbances(TCHAR* wcResFileName);
BOOL RunAnalysisDynWithAllDisturbances(TCHAR* wcResFileName=NULL);
BOOL RunAnalysisTD(double dTEND=1.0, TCHAR* wcDisturbanceFilePathName=NULL,TCHAR* wcPlotFilePathName=NULL, TCHAR* wcResFileName=NULL, TCHAR* wcDynamicDataFileName=NULL);

//Export to SQL Database
BOOL ExportToSQLDB(TCHAR* sConnect);
//Import from SQL Database
BOOL ImportFromSQLDB(TCHAR* sConnect, TCHAR* sNetworkName);
//Print the active diagram in "zoom all" mode
BOOL PrintZoomAll();
//Print the active diagram as it is shown
BOOL PrintDirect();
//Print all diagrams in the project in "zoom all" mode
BOOL PrintAllDiagrams();
//Select a user defined view setting of the active diagram
BOOL SelectViewSetting(TCHAR* wcViewName);
//Activate a diagram
BOOL ActivateDiagramByName(TCHAR* wcViewName);
//Sets the diagrm size in mm
void SetDiagramSizeInMM(int nX_mm, int nY_mm);
//get number of existing diagrams
int GetNumberOfDiagrams();
//gets the name of the ith diagram
BOOL GetDiagramName(int iDiagram, LPTSTR wcName);
//gets the ID of the ith diagram
BOOL GetDiagramID(int iDiagram, unsigned long& lID);
//sets the name of the ith diagram
BOOL SetDiagramName(int iDiagram, TCHAR* wcName);
//bring diagram model to front
BOOL BringDiagramModelToFront(TCHAR* wcViewName);
//check if diagram exists return valuses: -1=Error, 0=not exits 1=exist
int CheckIfDiagramExists(TCHAR* wcViewName);
//Close the diagram View
BOOL CloseDiagramViewByName(TCHAR* wcViewName);
//Import ZDB, NDB, EDT, NDT, CDE and UCTE files
BOOL ImportFileElectrical(TCHAR* wcFileName, int nFileType, TCHAR* wcProtectionLibName=NULL, TCHAR* wcNeplanLibName=NULL);
//Export ZDB, NDB, EDT, NDT, CDE and UCTE files
BOOL ExportFileElectrical(TCHAR* wcFileName, int nFileType);
//Import dynamic part of SCADA file
BOOL ImportSCADAFile(TCHAR* wcFileName, TCHAR* wcDate, TCHAR* wcTime);
//Import ZDB, NDB, WET, GET, WKT, GKT, GRT and CDE
BOOL ImportFileGasWater(TCHAR* wcFileName, int nFileType, TCHAR* wcNeplanLibName=NULL);
//Export ZDB, NDB, WET, GET, WKT, GKT, GRT and CDE
BOOL ExportFileGasWater(TCHAR* wcFileName, int nFileType);
//Join two projects
BOOL JoinProject(TCHAR* wcFileName);
//Updates Partial Network ID
BOOL UpdatePartialNetworkID();
//redraw all open diagrams
BOOL RedrawDiagrams();
// Performs database consistency check
// nReturnCode = 0: No problems found
// nReturnCode = 1: Problems found
// nReturnCode = 2: Problems found and they could not be resolved
// nReturnCode = 3: Problems found and they have been resolved
BOOL CheckDatabaseConsistency(BOOL bTryToResolveProblems, int& nReturnCode);
//refresh actual open diagrams
BOOL RefreshScreen();
BOOL RunDACFAnalysisExpNEP();
BOOL RunDACFAnalysisExpUCT();

//shows a message to the NEPLAN message window.
//The nMessageWnd variable can be used to display to the different message windows
//nMessageWnd = 0 -> Messages
//nMessageWnd = 1 -> Error
//nMessageWnd = 2 -> Analyisis
void ShowMessage(TCHAR* wcMessage, int nMessageWnd=0);
//same as 'ShowMessage' but no new line will be added to the message.
void ShowMessageNoNewLine(TCHAR* wcMessage, int nMessageWnd=0);
//Updates all elements with the data from an element library (*.neplib) and a protection library (*.sd3)
BOOL UpdateAllElementsWithLibraryData(TCHAR* wcElementLibFileName, TCHAR* wcProtectionLibFileName = NULL);
//Updates the element data with the data from the actual element library
BOOL UpdateElementWithLibraryData(unsigned long lElementID, TCHAR* wcElementLibFileName, TCHAR* wcLibName=NULL);
//Assignes the Frequency Dependence Type to an element or Updates the one.
BOOL SetFreqDependenceWithLibraryData(unsigned long lElementID, TCHAR* wcFreqDepTypeName,TCHAR* wcElementLibFileName, TCHAR* wcLibName/*=NULL*/);
//Shows th objective function dialog of the load restoration module (re-supply module)
BOOL RestorationShowObjectiveFunctionDlg();
//Shows the plan selection dialog (after analysis with different objective functions)
//of the load restoration module (re-supply module)
BOOL RestorationShowSelectPlanDlg();
//Shows state selection dialog of the load restoration module (re-supply module)
BOOL RestorationShowSelectStateDlg();
//Connect NEPLAN to a TCP/IP bus either as server or as client
BOOL ConnectToNetwork(TCHAR* wcIpAdress, int portNr, BOOL bServer);
//Defines, if the message boxes (e.g. for error messages) will appear. Set to FALSE in batch mode operation
BOOL SetShowMessageBox(BOOL bShow);
//Runs a NPL Dll program
BOOL RunNPLDll(TCHAR* wcDllFileName);
//Defines, if the error message shall be saved in for later retrival
BOOL AllowToAddErrorMessages(BOOL bShow);
//Adds an error message to the error message stack. Message may be retrieved with "GetSavedErrorMessageAt"
void AddErrorMessage(TCHAR* wcMessage);
//Clears all saved error messages, if "AllowToAddErrorMessages" was set to TRUE
BOOL ClearAllErrorMessages();
//Gets the number of saved error messages
BOOL GetNumOfSavedErrorMessages(int& nValue);
//Gets the string of the saved error message at the position "nAt". The first message has position nAt=0;
BOOL GetSavedErrorMessageAt(int nAt, LPTSTR lpValue, int& nErrorNum, int& nSeverity);
//Gets the coordinate of an elemenet symbol
BOOL GetCoordinatesOfElement(unsigned long lID, double& xLeft, double& yBottom, double& xRight, double& yTop);
//Search and Show Element in active Diagram
BOOL SearchAndShowElementOnActiveDiagram(unsigned long lID);
//Gets the coordinate of a port an elemenet symbol
BOOL GetCoordinatesOfPortOfElement(unsigned long lID, int nPortNum, double& xPort, double& yPort);
//adds graphic links to an element
BOOL AddLinksToSymbol(unsigned long lElementID);
//adds graphic coordinates to an element x,y in 1/10 mm 0-point is upper left corner
BOOL AddSymbol(unsigned long lID, double x, double y, TCHAR* wcSymbolName, double dAngle=0.0);
//adds graphic coordinates to an element, point x/y is center point
BOOL AddSymbolCentered(unsigned long lID, double x, double y, TCHAR* wcSymbolName, double dAngle);
//adds graphic coordinates to an element which is on an element
BOOL AddOnElementSymbol(unsigned long lID, TCHAR* wcSymbolName);
//adds graphic coordinates to a node x,y in mm 0-point is upper left corner
BOOL AddGraphicToNode(unsigned long lID, double x, double y);
//adds graphic coordinates to a busbar x1,y1, x2,y2 in mm 0-point is upper left corner
BOOL AddGraphicToBusbar(unsigned long lID, double x1, double y1, double x2, double y2);
//adds graphic coordinates to a busbar with more that 2 points numPoints=number of points-> in pCoordinates = 2*numPoints values
// x,y in mm
BOOL AddMultiPointBusbar(unsigned long lID, int numPoints, double* pCoordinates);
//adds graphic link to a line
BOOL AddGraphicToLine(unsigned long lID);
//adds graphic link to a line with more that 2 points numPoints=number of points-> in pCoordinates = 2*numPoints values
// x,y in mm
BOOL AddMultiPointGraphicToLine(unsigned long lID, int numPoints, double* pCoordinates);
//adds graphic to a link with more that 2 points numPoints=number of points-> in pCoordinates = 2*numPoints values
// x,y in mm
BOOL AddMultiPointGraphicToLink(unsigned long lID, int numPoints, double* pCoordinates);
//adds graphic link to all lines without graphic
BOOL AddGraphicsToAllLines(BOOL bOnlyLinesWithoutGraphic=TRUE);
//adds graphic link between to elements or element-node, x1,y1, x2,y2 in mm 0-point is upper left corner
BOOL AddGraphicToLink(unsigned long lFromElemID, unsigned long lToElemID, double x1, double y1, double x2, double y2);
//adds graphic link to all symbols without graphic
BOOL AddGraphicsToAllLinks();
//adds one symbol near to a node or busbar
BOOL AddSymbolAutoLayout(unsigned long lID, int nDownUpLeftRight, TCHAR* wcSymbolName);
//adds all symbols near to nodes or busbars
BOOL AddAllSymbolAutoLayout(int nDownUpLeftRight, TCHAR* wcElementType, TCHAR* wcSymbolName);
//adds all exciters and turbines symbols near to th generator
BOOL AddRegulatorSymbolsToGeneratorsAutoLayout(int nDownUpLeftRight, double dDist, TCHAR* wcSymbolName);
//scale, flip a component
BOOL ScaleGraphicOfElement(unsigned long lElementID, float fScaleX, float fScaleY, int nControlPoint=8); //8=OD_CENTER
//adds a line section to a line
BOOL AddLineSection(unsigned long lLineID, TCHAR* wcSectionDescription, TCHAR* wcSectionType, double length,  int nYear=1900, TCHAR* wcElementLibFileName=NULL);
//remove line section
BOOL RemoveLineSection(unsigned long lLineID, int sectionIndex, BOOL bAddErrorMsg=TRUE);
//adds a Feeder definition to the network
BOOL CreateVariantFeeder(unsigned long lNodeID, unsigned long lElementID, TCHAR* wcFeederName, int colR=0, int colG=180, int colB=55);
//adds a network zone definition to the network
BOOL CreateVariantZone(TCHAR* wcName, int colR=40, int colG=180, int colB=55, double dLoadFactor=1.0);
//adds a network area definition to the network
BOOL CreateVariantArea(TCHAR* wcName, int colR=100, int colG=50, int colB=55);
//adds a element group definition to the network
BOOL CreateElementGroup(TCHAR* wcName, TCHAR* wcDescr=_T(""), int colR=255, int colG=100, int colB=100);
//deletes a network zone
BOOL DeleteVariantZone(TCHAR* wcName);
//deletes a network area
BOOL DeleteVariantArea(TCHAR* wcName);
//gets number of arae and zones
BOOL GetNumAreaAndZones(int &nNum);
//gets data of zone and area
BOOL GetAreaOrZone(int nItem, LPTSTR lpAreaName, long& lAreaID, double& dloadP, double& dloadQ, int &isZone); //Zone = 1
// Read the color of an area or zone
BOOL GetAreaOrZoneColor(int nItem, int& colR, int& colG, int& colB);
// Set the color of an area or zone
BOOL SetAreaOrZoneColor(int nItem, int colR, int colG, int colB);
// Set the factors a zone
BOOL SetZoneFaktors(int nItem, double pLoad, double qLoad, double pGen, double qGen, double shuntL, double shuntC);
//deletes a network feeder
BOOL DeleteVariantFeeder(TCHAR* wcName);
//deletes a element groupr
BOOL DeleteElementGroup(TCHAR* wcName);
//sends a message through TCP/IP to the client
int SendNeplanMessageToClient(TCHAR* wcMessage, BOOL bAddMsgLengthInfo);
//loads a customer defined dll, which may define TCP-IP commands for NEPLAN
BOOL LoadTcpIpCommandDll(TCHAR* wcDllCmdFileName);
//Set command Instance handler
BOOL SetHInstanceCmdDll(HINSTANCE hInstanceCmdDll, TCHAR* wcDllCmdFileName);
//Delete all load profile scaling factors
BOOL DeleteAllScalingFactorTypes();
//Adds a new profile scaling year factor.
//If the type or the year already exists the the factor will be over written
BOOL CreateScalingFactorLongTerm(TCHAR* wcType, int nYear, double dFactor);
//Adds a new profile scaling day factor.
//If the type or the time already exists the the factor will be over written
BOOL CreateScalingFactorDay(TCHAR* wcType, int nHour, int nMinute, double dFactor);
//Adds a new profile scaling type.
BOOL CreateScalingFactorType(TCHAR* wcType,  double dCosPhi, double dVelFactor1, double dVelFactor2, int nNoSeasons);
//assigns a day scaling factor type to a scaling factor
BOOL AssignSeasonDayToScalingFactorType(TCHAR* wcFactorType, int nSeason, int nMonSatSun, TCHAR* wcDayType);
//assigns a year scaling factor type to a scaling factor
BOOL AssignSeasonYearToScalingFactorType(TCHAR* wcFactorType, TCHAR* wcYearType);
//assigns a load profile type with portion to an element (lLoad, generator, line load)
BOOL AssignLoadProfileTypeToElement(unsigned long lElemID, TCHAR* wcProfileType, double dPortion);
//define the dates of a season
BOOL DefineSeason(int nSeasonNo, int nMonth, int nDay);
BOOL AreResultsReseted(int nResultType);
BOOL ResetResult(int nResultType);
BOOL ResetResults();
BOOL RemoveResults();
BOOL DeleteAllElementOfType(TCHAR* wcElemType);
int  DeleteUnusedPartialNetwork();
int  DeleteUnusedNetworkFeeders();
int  DeleteUnusedNetworkZones();
void SetZoomTo(double dZoomValue);
void SetZoomToFit();
//Get the number of line sections of a line
BOOL GetNumLineSections(unsigned long lineID ,int &nCount);
//Get and set the line section parametres
BOOL SetLineSectionParameterDouble(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, double dValue);
BOOL GetLineSectionParameterDouble(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, double& dValue);
BOOL SetLineSectionParameterInt(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, int nValue);
BOOL GetLineSectionParameterInt(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, int& nValue);
BOOL SetLineSectionParameterBool(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, bool bValue);
BOOL GetLineSectionParameterBool(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, bool& bValue);
BOOL SetLineSectionParameterString(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter,  TCHAR* wcValue);
BOOL GetLineSectionParameterString(unsigned long lLineID, int sectionIndex, TCHAR* wcParameter, LPTSTR lpValue);
//Adds a switcht shunt item to shunt table
BOOL AddSwitchtShuntItemIntoShuntTable(unsigned long lID, int nSteps, double dQ1, double dQ0);
//Deletes the last switcht shunt item from a shunt table
BOOL DeleteSwitchtShuntItemFromShuntTable(unsigned long lID);
//Gets the number of switchted shuntsWithLibraryData
int GetNumSwitchtShunts(unsigned long lID);
//Gets the the data of a switchted shunt item
BOOL GetSwitchtShuntItem(unsigned long lID, int nItem, int &nStep, double &dQ1, double &dQ0);
//Deletes all switched shunts from the tabel
BOOL DeleteAllSwitchShuntItemsFromShuntTable(unsigned long lID);
//Add a new cability item to the generators capability curve table
BOOL AddNewCapabilityCurveItem(unsigned long lID, double dP, double dQmax, double dQmin, TCHAR* cCurveID);
//remove all cability items from the generators capability curve table
BOOL RemoveAllCapabilityCurveItem(unsigned long lID);
//gets the number of items in the capability curve of the generator
int GetNumCapabilityCurveItems(unsigned long lID);
//gets the data of an item in the capability curve of the generator
BOOL GetCapabilityCurveItem(unsigned long lID, int nItem, double &dP, double &dQmin, double &dQmax, LPTSTR cCruveID);
//Adds a relaibility type to the project
BOOL AddReliabilityType(int nRelType,  TCHAR*  wcRelModelType);  //nRelType= 1...5: Comp, Line, Switch, Gen, Load
//Get and set the reliability parametres
BOOL SetReliabilityParameterDouble(int nRelType, TCHAR*  wcRelModelType, TCHAR* wcParameter, double dValue);
BOOL GetReliabilityParameterDouble(int nRelType, TCHAR*  wcRelModelType, TCHAR* wcParameter, double& dValue);
//Set protection data
BOOL SetProtectionData(unsigned long lID, int nProtIdx, double dIset, double dTset);
//Create protection data
BOOL CreateProtectionData(unsigned long lID, TCHAR* wcModelType, TCHAR* wcLibraryName, double dIr_kA=0.0);
//Add disturbance Entry for the dynamic simulator
BOOL AddDisturbance(unsigned long lID, TCHAR* wcParam, double time, double value, TCHAR* wcDescription, BOOL active=TRUE);
//Change adisturbance entry with the same description for the dynamic simulator
BOOL ChangeDisturbance(unsigned long lID, TCHAR* wcParam, double time, double value, TCHAR* wcDescription, BOOL active=TRUE);
//Removes the last Ddsturbances for the dynamic simulator
BOOL RemoveLastDisturbanceSet();
//Adds a plot variable to the result list for the dynamic simulator
BOOL AddSimulatorPlotVariable(unsigned long lID, TCHAR* wcParam, int nScreenPos=1, BOOL toFile=TRUE);
//Removes the last plot ot variable of the result list for the dynamic simulator
BOOL RemoveLastSimulatorPlotVariable();
BOOL AddAuxilaryText(TCHAR* wcText, double dX, double dY, int nFontSize=10, TCHAR* wcFontName=_T("Arial"), double dAngle=0.0, TCHAR* wcColor=_T("0X000000"), BOOL bUseModelOffset=FALSE);
BOOL AddAuxilarySymbol(TCHAR* wcSymbolName, double dX, double dY, double dSize=1.0,  double dAngle=0.0, TCHAR* wcSymbolType=_T("DRAW_SYMBOL"), BOOL bUseModelOffset=FALSE);
BOOL AddDiagramModel(TCHAR* wcName, TCHAR* wcDescription);
BOOL AddDiagramModelWithCimID(TCHAR* wcName, TCHAR* wcDescription, TCHAR* wcCimID);
BOOL SetDiagramSize(double cmX, double cmY, double cmXOffset, double cmYOffset);
BOOL MoveDiagram(double cmX, double cmY);
int GetModifiedStateOfElement(unsigned long lID);
int IsElementDifferentFromParentVariant(unsigned long lID);
// Sets unit generator flags if the corresponding unit transformer flags are set
BOOL UpdateUnitGenerators();

//nMsgWndNum=-1 all windows; 0=Message  1=Error   2=Analysis
void ClearWindowMessages(int nMsgWndNum=-1);

//Get a list of all Network names in a SQL Database
//--BOOL GetListOfNetworksInSQLDB(CArray<CString, CString>& listOfNetworkNames, TCHAR* wcConnect);

//write messages to a log file
BOOL WriteMessageToLogFile(TCHAR* wcFileName, int nNoteWarningError=0);

BOOL SetAllCABranchLimits(double dP);

// Get and set transformer parameters that are dependent on transformer taps (deltaU, impedance)
// sParameter: "DeltaU1", "DeltaU2", "Uk" or "Uk0"
BOOL GetTransformerTapDependentParameter(unsigned long lID, TCHAR* sParameter, int tap1, int tap2, double& dValue, LPTSTR TapChangerTablePointCimID);
BOOL SetTransformerTapDependentParameter(unsigned long lID, TCHAR* sParameter, int tap1, int tap2, double dValue, TCHAR* TapChangerTablePointCimID);

//CIM iport/export
BOOL RunCIMImportExport(TCHAR* wcOptionFileName, BOOL import, BOOL withDialog);
//make unique cim ids for export to CIM
BOOL MakeUniqueCimIDs();
//get all CIM IDs of the elements. The vectors needs to be allocated and freed in the calling routine
BOOL GetElementsCimIDs(int& nCount, std::vector<unsigned long>*& elementIDs, std::vector<std::basic_string<TCHAR>>*& elemTypes, std::vector<std::basic_string<TCHAR>>*& cimIDs);
//get all CIM IDs defined for an elament. The vectors needs to be allocated and freed in the calling routine
BOOL GetAllCimIDs(int& nCount, std::vector<unsigned long>*& elementIDs, std::vector<std::basic_string<TCHAR>>*& elemTypes, std::vector<std::basic_string<TCHAR>>*& cimIDs);
//initialze NEPAN for Cim export (e.g. MakeUniqueCimIDs)
BOOL InitializeCimExport();
//Gets loading limit data of an element (PATL, TATl, etc.)
BOOL GetElementLoadingLimits(unsigned long lElementID, int nPort, int& nCount, std::vector<std::basic_string<TCHAR>>*& limitValueCimIDs, std::vector<std::basic_string<TCHAR>>*& limitDefinitionCimIDs, std::vector<double>*& limitValues, std::vector<std::basic_string<TCHAR>>*& CimNames, std::vector<std::basic_string<TCHAR>>*& CimShortNames);
//Gets the loading limits (PATL, TATL, etc.) defined in the respective area
BOOL GetAreaLoadingLimits(TCHAR* wcAreaName, int& nCount, std::vector<int>*& types, std::vector<double>*& durations, std::vector<double>*& percentages, std::vector<std::basic_string<TCHAR>>*& cimIDs, std::vector<std::basic_string<TCHAR>>*& names);
//Sets loading limit data of an element (PATL, TATl, etc.)
BOOL SetElementLoadingLimits(unsigned long lElementID, int nPort, int nCount, std::vector<std::basic_string<TCHAR>>*& limitValueCimIDs, std::vector<std::basic_string<TCHAR>>*& limitDefinitionCimIDs, std::vector<double>*& limitValues, std::vector<std::basic_string<TCHAR>>*& CimNames, std::vector<std::basic_string<TCHAR>>*& CimShortNames);
//Sets the loading limits (PATL, TATL, etc.) defined in the respective area
BOOL SetAreaLoadingLimits(TCHAR* wcAreaName, int nCount, std::vector<int>*& types, std::vector<double>*& durations, std::vector<double>*& percentages, std::vector<std::basic_string<TCHAR>>*& cimIDs, std::vector<std::basic_string<TCHAR>>*& names);
//get all bays in a substation
BOOL GetAllBaysInSubstations(int substationID, int& nCount, std::vector<std::basic_string<TCHAR>>*& cimIDs, std::vector<std::basic_string<TCHAR>>*& names, std::vector<std::basic_string<TCHAR>>*& aliasNames, std::vector<std::basic_string<TCHAR>>*& shortNames, std::vector<std::basic_string<TCHAR>>*& descriptions, std::vector<std::basic_string<TCHAR>>*& cimVoltagelevelIDs, std::vector<unsigned long>*& bayIDs);
//delete/remove all bays in a substation
BOOL RemoveAllBaysInSubstations(int substationID);
//add one bay to substation
BOOL AddOneBaysToSubstations(unsigned long& newBayID, int substationID, TCHAR* tcCimID, TCHAR* tcName, TCHAR* tcAliasName, TCHAR* tcShortName, TCHAR* tcDescription, TCHAR* tcVoltageLevelID);
// is the actual variant a root variant
BOOL IsRootVariant();
//add geopgraphical position to element
BOOL AddGeographicalPositions(unsigned long lID, int numPoints, double* pCoordinates, std::vector<std::basic_string<TCHAR>>*& pwcCoordinatesCimIDs);
BOOL RemoveGeographicalPositions(unsigned long lID);
BOOL GetDeletedOrChangedElementsFromRoot(int& nCount, std::vector<unsigned long>*& elementIDs, std::vector<int>*& changeNewDel, std::vector<std::basic_string<TCHAR>>*&  elemTypes, std::vector<std::basic_string<TCHAR>>*& cimIDs, std::vector<std::basic_string<TCHAR>>*& areaNames);
BOOL IsNewElementInVariant(unsigned long lElementID);
BOOL GetBusbarGraphicByDiagramID(unsigned long lID, unsigned long lDiagramID, int nMaxSize, int& numPoints, double* pCoordinates);
BOOL GetLineGraphicByDiagramID(unsigned long lID, unsigned long lDiagramID, int nMaxSize, int& numPoints, double* pCoordinates);
BOOL GetLinkGraphicByDiagramID(unsigned long lEdgeID, unsigned long lDiagramID, BOOL bSortFromElemToNode, int nMaxSize, int& numPoints, double* pCoordinates);
BOOL GetSymbolGraphicByDiagramID(unsigned long lID, unsigned long lDiagramID, double& dSymbolX, double& dSymbolY, double& dScaleX, double& dScaleY, double& dAngle, TCHAR* tcSymbolName);
BOOL GetSelectedElementInfo(unsigned long& lID, LPTSTR lpElementType, LPTSTR lpName, double& xPos, double& yPos);
