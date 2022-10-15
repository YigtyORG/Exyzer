@echo off
@REM ***********************************************************
@REM * TakymLib                                                *
@REM * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved. *
@REM * Copyright (C) 2020-2022 Takym.                          *
@REM *                                                         *
@REM * distributed under the MIT License.                      *
@REM ***********************************************************
@echo restore.cmd: begin
@cd %~dp0
@call dotnet.exe restore --packages=./packages
@echo restore.cmd: end
