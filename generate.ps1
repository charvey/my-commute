cd .\src\Terminal\bin\Debug\netcoreapp2.0\
dotnet .\Terminal.dll
cd ..\..\..\..\..\
Copy-Item .\src\Terminal\bin\Debug\netcoreapp2.0\table.html docs\index.html