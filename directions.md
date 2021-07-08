Windows Development Setup
- Docker Desktop
- Create a directory to hold all of the projects
- Checkout all of the projects
	- UserAPI
	- AdventureAPI
	- GameAPI
	- MapAPI
	- GameSystemAPI
	- ContentAPI
	- PAPI (PublicAPI)
- Setup an environment file
	- need to define
		- JWTSECRET - used in JWT tokens, 64 characters letters and numbers
		- POSTGRESPASSWORD - password for postgres database
		- POSTGRESUSER - username to use when connecting to postgres database
		- POSTGRESURL - address of postgres database
		- SECRET_KEY (optional) - key used in Django administration app
	- example
		```
		JWTSECRET=v5OFC4Z7bq6rCWEAoVWC8yFr7JB1F7OKkaFdmbzIeWVuVwKqk0ThR5hm1G2185n
		POSTGRESPASSWORD=tbsprpg
		POSTGRESUSER=postgres
		POSTGRESURL=localhost
		SECRET_KEY=h2pJPpKBfN2AYNVsc7Q6AZ1yCXIM1LUctsHlwCNTNlRu2dVjrl2ie7CYNjwVBcL
		```
	- You should change the JWTSECRET and SECRET_KEY to something unique for your instance.
	- https://www.grc.com/passwords.htm