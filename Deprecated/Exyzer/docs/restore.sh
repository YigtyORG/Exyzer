#!/bin/bash

# ***********************************************************
# * Exyzer                                                  *
# * Copyright (C) 2020-2022 Yigty.ORG; all rights reserved. *
# * Copyright (C) 2020-2022 Takym.                          *
# *                                                         *
# * distributed under the MIT License.                      *
# ***********************************************************

echo begin: restore.sh
dotnet restore --packages=./packages
echo end: restore.sh
