@echo off

set TOOLS_PATH=..\packages\Grpc.Tools.1.4.1\tools\windows_x64

%TOOLS_PATH%\protoc.exe -I./ --csharp_out ./ Test.proto --grpc_out ./ --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe
