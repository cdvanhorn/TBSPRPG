curl -v -X POST -H "Content-Type: application/json" -d '{"Username": "test", "Password": "test"}'  http://localhost:8000/users/authenticate

curl -v -X GET http://localhost:8000/users
curl -v -X GET -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2MDIyMDEyMzgsImV4cCI6MTYwMjgwNjAzOCwiaWF0IjoxNjAyMjAxMjM4fQ.GIchynzZ_Eq8SCcosESEkwNUspzTM-alfEn6YbuNn58" http://localhost:8000/users
