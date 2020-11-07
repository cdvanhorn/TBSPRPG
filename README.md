# TBSPRPG

This is a game engine for text based online single player role playing games (TBSPRPG).  This will be written in a Javascript framework, probably Angular.  This project will have everything you need to write your own TBSPRPG.  What will make the games unique is the data retrieved from the backend API.

This is the root project.  The backend has been divided up in to microservices, and front end is an Angular application.

## Backend Services
- [Public API](https://github.com/cdvanhorn/TBSPRPG_PAPI)
- [User API](https://github.com/cdvanhorn/TBSPRPG_UserAPI)
- [Adventure API](https://github.com/cdvanhorn/TBSPRPG_AdventureAPI)
- [Game API](https://github.com/cdvanhorn/TBSPRPG_GameAPI)

The backend services are written to be independent of each other, and will communicate via an event store.

## Frontend
- [UI](https://github.com/cdvanhorn/TBSPRPG_UI)

Like I said before the frontend is written using Angular.  I chose Angular because I didn't want have to build my own JavaScript stack like the other frameworks require.  I'm giving up speed for, hopefully, ease of development.

## Setup
For development everything can be run from docker containers.  There is a docker-compose file in this project, that if you have everything setup like I do, you'll be able to run the whole application.  I have all of the projects checked out in to one folder `~/Projects/TBSPRPG`.  I then copy the docker-compose file from this project to `~/Projects/TBSPRPG` and setup the environment file.  Finally, I run `docker-compose up frontend` to launch the application.

The environment file should be named `.env` be located in `~/Projects/TBSPRPG` and contain the following:
> `
JWTSECRET=xxx
DBPASSWORD=xxx
DBSALT=xxx
`

These values will be uniqe for your environment.  
- `JWTSECRET` is used to encrypt JWT tokens, used for authorization (64 character string letters and numbers)
- `DBPASSWORD` is the password for your mongo database
- `DBSALT` is used to encrypt user passwords (base64 encoded)
