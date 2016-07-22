
Arebis MAX Mapper Extensions for VS.NET 2015
============================================


Q&A : Visual Studio does not show the project and item templates of MAX in the Add Project/Add Item dialogs
----------------------------------------------------------------------------------------------------------


If Visual Studio does not detect the presence of the MAX Mapper Extensions, run

   devenv.exe /updateconfiguration 

See: https://msdn.microsoft.com/en-us/library/mt718283.aspx



Alternatively, "touch" the following file (change it's file date/time, edit the file with whatever content):

"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\extensions.configurationchanged"


