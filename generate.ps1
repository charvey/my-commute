cd .\src
dotnet build MyCommute.sln
cd .\Terminal\bin\Debug\netcoreapp2.1\
dotnet .\Terminal.dll
Copy-Item .\table.html .\..\..\..\..\..\docs\index.html
cd ..\..\..\..\..\
