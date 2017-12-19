clc
clear all

% winopen('NPL-Demo-Ele.nepprj')

addpath(fullfile('NPL','NPL3','Release'))
NPL3

%%%
% Welcher Compiler wird benutzt
% mex -setup c++


 % ('NeplanProgrammingLibrary.lib','NeplanProgrammingLibrary.h')

%%%
% show all functions
% libfunctions('NPL3')
% libfunctions('NeplanProgrammingLibrary')

%%%
% call function
% calllib('libname','funcname',arg1,...,argN)