FROM node:14-alpine
WORKDIR /app
COPY ./db.json /app
RUN npm install -g json-server
CMD json-server --watch /app/db.json --host 0.0.0.0
