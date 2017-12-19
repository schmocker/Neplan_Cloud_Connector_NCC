%[status,cmdout] = system("cd C:\Program Files (x86)\NEPLAN-V558\Bin\")
%[status,cmdout] = system("start Neplan.exe")

current_folder = cd;
cd 'C:\Program Files (x86)\NEPLAN-V558\Bin\'
system("start Neplan.exe")
pause(2)
cd(current_folder)
system("NPL-Demo-Ele.nepprj")
dos('NPL-Demo-Ele.nepprj');

