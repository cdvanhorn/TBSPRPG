#!/bin/bash

sudo chown -R david:david ./*
cd TBSPRPG_PAPI
dotnet restore
cd ../TBSPRPG_UserAPI
dotnet restore
cd ../TBSPRPG_AdventureAPI
dotnet restore
cd ../TBSPRPG_GameAPI
dotnet restore
cd ../TBSPRPG_MapAPI
dotnet restore
cd ../TBSPRPG_ContentAPI
dotnet restore
cd ../TBSPRPG_GameSystemAPI
dotnet restore
