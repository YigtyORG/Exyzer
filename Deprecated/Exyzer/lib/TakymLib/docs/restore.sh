#!/bin/bash
# ***********************************************************
# * TakymLib                                                *
# * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved. *
# * Copyright (C) 2020-2022 Takym.                          *
# *                                                         *
# * distributed under the MIT License.                      *
# ***********************************************************
echo restore.sh: begin
cd `dirname $0`
dotnet restore --packages=./packages
echo restore.sh: end
