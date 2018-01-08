figure;
textH = text(.5, .5, 'Edit me and click away');
set(textH,'Editing','on', 'BackgroundColor',[1 1 1]);
disp('This prints immediately.');
drawnow
waitfor(textH,'Editing','off');
set(textH,'BackgroundColor',[1 1 0]);
disp('This prints after text editing is complete.');