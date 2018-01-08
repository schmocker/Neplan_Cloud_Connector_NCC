


path = '\\Mac\Home\Desktop\NeplanWebservices\NeplanWebservices\bin\Release\NeplanWebservices.dll';

asm = NET.addAssembly(path);

import('NeplanWebservices.Project');

P=NeplanWebservices.Project

b = P.Square(3)
