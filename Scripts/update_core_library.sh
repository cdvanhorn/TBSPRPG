#!/bin/bash

#docker ps -a -q --filter ancestor=tbsprpg_publicapi:latest | xargs docker rm
#docker ps -a -q --filter ancestor=tbsprpg_gameapi:latest | xargs docker rm
#docker ps -a -q --filter ancestor=tbsprpg_mapapi:latest | xargs docker rm
#docker ps -a -q --filter ancestor=tbsprpg_adventureapi:latest | xargs docker rm
#docker ps -a -q --filter ancestor=tbsprpg_userapi:latest | xargs docker rm
#docker ps -a -q --filter ancestor=tbsprpg_gamesystemapi:latest | xargs docker rm
#docker image rm tbsprpg_publicapi:latest \
#	tbsprpg_gameapi:latest \
#	tbsprpg_mapapi:latest \
#	tbsprpg_adventureapi:latest \
#	tbsprpg_userapi:latest \
#	tbsprpg_gamesystemapi:latest \
#	tbsprpg_contentapi:latest

#delete images
docker-compose down --rmi local

#build a new version of the library
cd ./TBSPRPG/TbspRpgLib
dotnet pack --configuration Release

#copy the libraries to the individual projects
cd ../Scripts
./link_library.sh ../../TBSPRPG_PAPI
./link_library.sh ../../TBSPRPG_GameAPI
./link_library.sh ../../TBSPRPG_MapAPI
./link_library.sh ../../TBSPRPG_AdventureAPI
./link_library.sh ../../TBSPRPG_UserAPI
./link_library.sh ../../TBSPRPG_GameSystemAPI
./link_library.sh ../../TBSPRPG_ContentAPI
