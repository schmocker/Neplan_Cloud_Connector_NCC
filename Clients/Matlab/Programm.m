clc
client.fnc = 'openWebservice';
client.input.username = 'christoph.hunziker@fhnw.ch';
client.input.password = 'nep360FH2017';
client.input.project = 'NeplanMatlab2';

client.send();

pause(5)

client.fnc = 'closeWebservice';
client.send();