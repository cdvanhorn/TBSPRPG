curl -v -X POST -H "Content-Type: application/json" -d '{"Username": "test", "Password": "test"}'  http://localhost:8000/users/authenticate

curl -v -X GET http://localhost:8000/users
