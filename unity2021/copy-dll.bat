
@echo off

REM !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!

md DocumentSee\Assets\3rd\fmp-xtc-documentsee

cd ..\vs2022
dotnet build -c Release

copy fmp-xtc-documentsee-lib-mvcs\bin\Release\netstandard2.1\*.dll ..\unity2021\DocumentSee\Assets\3rd\fmp-xtc-documentsee\